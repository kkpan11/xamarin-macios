//
// debug.m: Debugging code for MonoTouch
// 
// Authors:
//   Geoff Norton
//   Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2009 Novell, Inc.
// Copyright 2011-2013 Xamarin Inc. 
//

#ifdef DEBUG

//#define LOG_HTTP(...) do { NSLog (@ __VA_ARGS__); } while (0);
#define LOG_HTTP(...)

#include <TargetConditionals.h>

#include "frameworks.h"
#if HAVE_UIKIT
#include <UIKit/UIKit.h>
#else
#include <AppKit/AppKit.h>
#endif

#include <zlib.h>

#include <stdlib.h>
#include <string.h>
#include <arpa/inet.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <sys/select.h>
#include <sys/time.h>
#include <netinet/in.h>
#include <netinet/tcp.h>
#include <unistd.h>
#include <fcntl.h>
#include <errno.h>
#include <ctype.h>
#include <pthread.h>
#include <objc/objc.h>
#include <objc/runtime.h>
#include <sys/shm.h>
#include <stdatomic.h>

#include "xamarin/xamarin.h"
#include "runtime-internal.h"
#include "monotouch-debug.h"
#include "product.h"

// permanent connection variables
static long monodevelop_port = -1;
static int sdb_fd = -1;
static long sdb_timeout_time = -1;
static int heapshot_fd = -1; // this is the socket to write 'heapshot' to to requests heapshots from the profiler
static long heapshot_port = -1;
static char *profiler_description = NULL;
// old variables
static char *debug_host = NULL;

enum DebuggingMode
{
	DebuggingModeNone,
	DebuggingModeUsb,
	DebuggingModeWifi,
	DebuggingModeHttp,
};

static pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;
static pthread_cond_t cond = PTHREAD_COND_INITIALIZER;
static bool debugging_configured = false;
static bool profiler_configured = false;
static bool config_timedout = false;
static bool connection_failed = false;
static DebuggingMode debugging_mode = DebuggingModeWifi;
static const char *connection_mode = "default"; // this is set from the cmd line, can be either 'usb', 'wifi', 'http' or 'none'

extern "C" {
void monotouch_connect_usb ();
void monotouch_connect_wifi (NSMutableArray *hosts);
void xamarin_connect_http (NSMutableArray *hosts);
int monotouch_debug_connect (NSMutableArray *hosts, int debug_port, int output_port);
void monotouch_configure_debugging ();
void monotouch_load_profiler ();
void monotouch_load_debugger ();
bool monotouch_process_connection (int fd);
void monotouch_dump_objc_api (Class klass);
}

static struct timeval wait_tv;
static struct timespec wait_ts;

void
monotouch_set_connection_mode (const char *mode)
{
	connection_mode = mode;
}

void
monotouch_set_monodevelop_port (int port)
{
	monodevelop_port = (long) port;
}

void
monotouch_start_debugging ()
{
	bool debug_enabled = strcmp (connection_mode, "none");
	if (xamarin_debug_mode) {
		if (debug_enabled) {
			// wait for debug configuration to finish
			gettimeofday(&wait_tv, NULL);

			int timeout;
			int iterations = 1;
			timeout = 2;
			wait_ts.tv_sec = wait_tv.tv_sec + timeout;
			wait_ts.tv_nsec = wait_tv.tv_usec * 1000;
			
			pthread_mutex_lock (&mutex);
			while (!debugging_configured && !config_timedout && !connection_failed) {
				if (pthread_cond_timedwait (&cond, &mutex, &wait_ts) == ETIMEDOUT) {
					iterations--;
					if (iterations <= 0) {
						config_timedout = true;
					} else {
						LOG (PRODUCT ": Waiting for connection to the IDE...")
						// Try again
						gettimeofday(&wait_tv, NULL);
						wait_ts.tv_sec = wait_tv.tv_sec + timeout;
						wait_ts.tv_nsec = wait_tv.tv_usec * 1000;
					}
				}
			}
			pthread_mutex_unlock (&mutex);
			
			if (connection_failed) {
				LOG (PRODUCT ": Debugger not loaded (failed to connect to the IDE).\n");
			} else if (config_timedout) {
				LOG (PRODUCT ": Debugger not loaded (timed out while trying to connect to the IDE).\n");
			} else {
				monotouch_load_debugger ();
			}
		} else {
			LOG (PRODUCT ": Not connecting to the IDE, debug has been disabled\n");
		}
	
#if !defined (CORECLR_RUNTIME)
		char *trace = getenv ("MONO_TRACE");
		if (trace && *trace) {
			if (!strncmp (trace, "--trace=", 8))
				trace += 8;

			mono_jit_set_trace_options (trace);
		}
#endif // !defined (CORECLR_RUNTIME)
	}
}

void
monotouch_start_profiling ()
{
	bool debug_enabled = strcmp (connection_mode, "none");
	if (xamarin_debug_mode && debug_enabled) {
		// wait for profiler configuration to finish
		pthread_mutex_lock (&mutex);
		while (!profiler_configured && !config_timedout) {
			if (pthread_cond_timedwait (&cond, &mutex, &wait_ts) == ETIMEDOUT)
				config_timedout = true;
		}
		pthread_mutex_unlock (&mutex);
	
		if (!config_timedout)
			monotouch_load_profiler ();
	}
}

static NSString *
get_preference (NSArray *preferences, NSUserDefaults *defaults, NSString *lookupKey)
{
	NSDictionary *dict;
 
	// Apple appears to return nil if the user has never opened the Settings, so we
	// manually parse it here.  This has the added benefits that if people don't open
	// settings we can control the default from MD

	// User Preferences have the highest precedence
	for (dict in preferences) {
		NSString *key = [dict objectForKey:@"Key"];
		if (![key isEqualToString:lookupKey])
			continue;
		
		return [dict objectForKey:@"DefaultValue"];
	}

	// Global Defaults have the second highest precedence
	return defaults ? [defaults stringForKey:lookupKey] : nil;
}

static uint64_t xamarin_launch_timestamp = 0;
void monotouch_start_launch_timer ()
{
	const char *action = getenv ("XAMARIN_TRACK_LAUNCH_DURATION");
	if (!action || !*action) {
		// do nothing (null or empty string)
	} else if (strcmp (action, "disable") == 0) {
		// do nothing
	} else if (strcmp (action, "enable") == 0) {
		xamarin_launch_timestamp = clock_gettime_nsec_np (CLOCK_UPTIME_RAW);
		PRINT (PRODUCT ": Started launch timer");
	} else {
		PRINT (PRODUCT ": Unknown value for XAMARIN_TRACK_LAUNCH_DURATION: %s", action);
	}
}

static void
xamarin_track_finished_launching ()
{
	if (xamarin_launch_timestamp == 0)
		return;

#if HAVE_UIKIT
	NSNotificationName didFinishLaunchingNotification = UIApplicationDidFinishLaunchingNotification;
#else
	NSNotificationName didFinishLaunchingNotification = NSApplicationDidFinishLaunchingNotification;
#endif
	NSNotificationCenter *center = NSNotificationCenter.defaultCenter;
	id token = nil;
	token = [center addObserverForName: didFinishLaunchingNotification
			object: nil
			queue: nil
			usingBlock: ^(NSNotification *note) {
				const uint64_t nsPerSecond = 1000000000ULL;
				const uint64_t nsPerMillisecond = 1000000ULL;

				uint64_t endTime = clock_gettime_nsec_np (CLOCK_UPTIME_RAW);
				uint64_t nsDuration = endTime - xamarin_launch_timestamp;
				uint64_t seconds = nsDuration / nsPerSecond;
				uint64_t milliseconds = (nsDuration - seconds * nsPerSecond) / nsPerMillisecond;
				PRINT (PRODUCT ": Did finish launching in %llu.%3llu s", seconds, milliseconds);
				[center removeObserver: token];
			}];
}

void monotouch_configure_debugging ()
{
	xamarin_track_finished_launching ();

	// This method is invoked on a separate thread
	NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
	NSString *bundle_path = [NSString stringWithUTF8String:xamarin_get_bundle_path ()];
	NSString *settings_path = [bundle_path stringByAppendingPathComponent:@"Settings.bundle"]; 
	NSString *root_plist = [settings_path stringByAppendingPathComponent:@"Root.plist"];
	NSDictionary *settings = [NSDictionary dictionaryWithContentsOfFile: root_plist];
	NSArray *preferences = [settings objectForKey:@"PreferenceSpecifiers"];
	NSMutableArray *hosts = [NSMutableArray array];
	bool debug_enabled = true;
	NSString *monodevelop_host;

	if (!strcmp (connection_mode, "default")) {
		char *evar = getenv ("__XAMARIN_DEBUG_MODE__");
		if (evar && *evar) {
			connection_mode = evar;
			LOG (PRODUCT ": Found debug mode %s in environment variables\n", connection_mode);
			unsetenv ("__XAMARIN_DEBUG_MODE__");
		}
	}
	
	if (!strcmp (connection_mode, "none")) {
		// nothing to do
		return;
	}
 
 #if !(TARGET_OS_MACCATALYST || TARGET_OS_OSX)
	NSString *monotouch_debug_enabled;
	// If debugging is enabled (only check for mobile builds - for macOS / Mac Catalyst debugging is always enabled in debug versions of libxamarin)
	monotouch_debug_enabled = get_preference (preferences, NULL, @"__monotouch_debug_enabled"); 
	if (monotouch_debug_enabled != nil) {
		debug_enabled = [monotouch_debug_enabled isEqualToString:@"1"];
	} else {
		debug_enabled = [defaults boolForKey:@"__monotouch_debug_enabled"];
	}
#endif // !(TARGET_OS_MACCATALYST || TARGET_OS_OSX)

	//        We get the IPs of the dev machine + one port (monodevelop_port).
	//        We start up a thread (using the same thread that we have to start up
	//        anyway to initialize cocoa threading) and then establishes several
	//        connections to MD (for usb we listen for connections and for wifi we
	//        connect to MD using any of the IPs we got). MD then sends instructions
	//        on those connections telling us what to do with them. We never stop
	//        processing connections and commands from that thread - this way MD can
	//        send an exit request when MD wants us to exit.
	monodevelop_host = get_preference (preferences, defaults, @"__monodevelop_host");
	if (monodevelop_host != nil && ![monodevelop_host isEqualToString:@"automatic"]) {
		[hosts addObject:monodevelop_host];
		LOG (PRODUCT ": Added host from settings to look for the IDE: %s\n", [monodevelop_host UTF8String]);
	}

	char *evar = getenv ("__XAMARIN_DEBUG_PORT__");
	if (evar && *evar) {
		if (monodevelop_port == -1) {
			monodevelop_port = strtol (evar, NULL, 10);
			LOG (PRODUCT ": Found port %i in environment variables\n", monodevelop_port);
		}
		unsetenv ("__XAMARIN_DEBUG_PORT__");
	}

	evar = getenv ("__XAMARIN_DEBUG_HOSTS__");
	if (evar && *evar) {
		NSArray *ips = [[NSString stringWithUTF8String:evar] componentsSeparatedByString:@";"];
		for (unsigned int i = 0; i < [ips count]; i++) {
			NSString *ip = [ips objectAtIndex:i];
			if (![hosts containsObject:ip]) {
				[hosts addObject:ip];
				LOG (PRODUCT ": Found host %s in environment variables\n", [ip UTF8String]);
			}
		}
		unsetenv ("__XAMARIN_DEBUG_HOSTS__");
	}

	evar = getenv ("__XAMARIN_DEBUG_CONNECT_TIMEOUT__");
	if (evar && *evar) {
		if (sdb_timeout_time == -1) {
			sdb_timeout_time = strtol (evar, NULL, 10);
			LOG (PRODUCT ": Found connect timeout %i in environment variables\n", sdb_timeout_time);
		}
		unsetenv ("__XAMARIN_DEBUG_CONNECT_TIMEOUT__");
	}

#if MONOTOUCH && defined (__x86_64__)
	// Try to read shared memory as well
	key_t shmkey;
	if (xamarin_launch_mode == XamarinLaunchModeApp) {
		// Don't read shared memory in normal apps, because we're always able to pass
		// the debug data (host/port) using either command-line arguments or environment variables
	} else if ((shmkey = ftok ("/Library/Frameworks/Xamarin.iOS.framework/Versions/Current/bin/mtouch", 0)) == -1) {
		LOG (PRODUCT ": Could not create shared memory key: %s\n", strerror (errno));
	} else {
		size_t shmsize = 1024;
		int shmid = shmget (shmkey, shmsize, 0);
		if (shmid == -1) {
			LOG (PRODUCT ": Could not get shared memory id: %s\n", strerror (errno));
		} else {
			void *ptr = shmat (shmid, NULL, SHM_RDONLY);
			if (ptr == NULL || ptr == (void *) -1) {
				LOG (PRODUCT ": Could not map shared memory: %s\n", strerror (errno));
			} else {
				LOG (PRODUCT ": Read %i bytes from shared memory: %p with key %i and id %i\n", shmsize, ptr, shmkey, shmid);
				// Make a local copy of the shared memory, so that it doesn't change while we're parsing it.
				char *data = strndup ((const char *) ptr, shmsize); // strndup will null-terminate
				char *line = data;
				// Parse!
				while (*line) {
					char *nline = line;
					// find the end of the line, null-terminate the line and make 'nptr' to the next line. 
					do {
						if (*nline == '\n') {
							*nline = 0;
							nline++;
							break;
						}
					} while (*++nline);

					if (!strncmp (line, "__XAMARIN_DEBUG_PORT__=", 23)) {
						long shm_monodevelop_port = strtol (line + 23, NULL, 10);
						if (monodevelop_port == -1) {
							monodevelop_port = shm_monodevelop_port;
							LOG (PRODUCT ": Found port %i in shared memory\n", monodevelop_port);
						} else  {
							LOG (PRODUCT ": Found port %i in shared memory, but not overriding existing port %i\n", shm_monodevelop_port, monodevelop_port);
						}
					} else {
						LOG (PRODUCT ": Unknown data found in shared memory: %s\n", line);
					}
					line = nline;
				}
				free (data);
				shmdt (ptr);
			}
		}
	}
#endif

	// Finally, fall back to loading values from MonoTouchDebugConfiguration.txt
	NSString *conf_path = [bundle_path stringByAppendingPathComponent:@"MonoTouchDebugConfiguration.txt"];
	FILE *debug_conf = fopen ([conf_path UTF8String], "r");
	if (debug_conf != NULL) { 
		bool add_hosts = [hosts count] == 0;
		char line [128];
		int i;

		while (!feof (debug_conf)) {
			if (fgets (line, sizeof (line), debug_conf) != NULL) {
				// Remove trailing newline
				for (i = 0; line[i]; i++) {
					if (line [i] == '\n' || line [i] == '\r') {
						line [i] = 0;
						break;
					}
				}
				
				if (!strncmp ("IP: ", line, 4)) {
					if (add_hosts) {
						NSString *ip;

						ip = [NSString stringWithUTF8String:line + 4];
						if (![hosts containsObject:ip]) {
							[hosts addObject:ip];
							LOG (PRODUCT ": Added IP to look for the IDE: %s\n", [ip UTF8String]);
						}
					}
				} else if (!strncmp ("USB Debugging: ", line, 15) && (connection_mode == NULL || !strcmp (connection_mode, "default"))) {
#if defined(__aarch64__)
					debugging_mode = !strncmp ("USB Debugging: 1", line, 16) ? DebuggingModeUsb : DebuggingModeWifi;
#endif
				} else if (!strncmp ("Port: ", line, 6) && monodevelop_port == -1) {
					monodevelop_port = strtol (line + 6, NULL, 10);
				} else  if (!strncmp ("Connect Timeout: ", line, 17) && sdb_timeout_time == -1) {
					sdb_timeout_time = strtol (line + 17, NULL, 10);
				}
			}
		}
		
		fclose (debug_conf);
	}

	if (debug_enabled) {
		// connection_mode is set from the command line, and will override any other setting
		if (connection_mode != NULL) {
			if (!strcmp (connection_mode, "usb")) {
				debugging_mode = DebuggingModeUsb;
			} else if (!strcmp (connection_mode, "wifi")) {
				debugging_mode = DebuggingModeWifi;
			} else if (!strcmp (connection_mode, "http")) {
				debugging_mode = DebuggingModeHttp;
			}
		}

		if (monodevelop_port <= 0) {
			LOG (PRODUCT ": Invalid IDE Port: %i\n", monodevelop_port);
		} else {
			LOG (PRODUCT ": IDE Port: %i Transport: %s Connect Timeout: %i\n", monodevelop_port, debugging_mode == DebuggingModeHttp ? "HTTP" : (debugging_mode == DebuggingModeUsb ? "USB" : "WiFi"), sdb_timeout_time);
			if (debugging_mode == DebuggingModeUsb) {
				monotouch_connect_usb ();
			} else if (debugging_mode == DebuggingModeWifi) {
				monotouch_connect_wifi (hosts);
			}
		}
	}

	profiler_configured = true;
	debugging_configured = true;
	pthread_mutex_lock (&mutex);
	pthread_cond_signal (&cond);
	pthread_mutex_unlock (&mutex);
}

#if !defined (CORECLR_RUNTIME)
static void sdb_connect (const char *address)
{
	gboolean shaked;

	shaked = mono_debugger_agent_transport_handshake ();
	
	if (!shaked)
		PRINT (PRODUCT ": Handshake error with IDE.");

	return;
}

static void sdb_close1 (void)
{
	shutdown (sdb_fd, SHUT_RD);
}

static void sdb_close2 (void)
{
	shutdown (sdb_fd, SHUT_RDWR);
}
#endif // !defined (CORECLR_RUNTIME)

static gboolean send_uninterrupted (int fd, const void *buf, size_t len)
{
	ssize_t res;

	do {
		res = send (fd, buf, len, 0);
	} while (res == -1 && errno == EINTR);

	return (size_t) res == len;
}

static ssize_t recv_uninterrupted (int fd, void *buf, size_t len)
{
	ssize_t res;
	ssize_t total = 0;
	int flags = 0;

	do { 
		res = recv (fd, (char *) buf + total, len - (size_t) total, flags); 
		if (res > 0)
			total += res;
	} while ((res > 0 && (size_t) total < len) || (res == -1 && errno == EINTR));

	return total;
}

#if !defined (CORECLR_RUNTIME)
static gboolean sdb_send (void *buf, size_t len)
{
	gboolean rv;

	if (debugging_configured) {
		rv = send_uninterrupted (sdb_fd, buf, len);
	} else {
		rv = send_uninterrupted (sdb_fd, buf, len);
	}

	return rv;
}


static ssize_t sdb_recv (void *buf, size_t len)
{
	ssize_t rv;

	if (debugging_configured) {
		rv = recv_uninterrupted (sdb_fd, buf, len);
	} else {
		rv = recv_uninterrupted (sdb_fd, buf, len);
	}

	return rv;
}
#endif // !defined (CORECLR_RUNTIME)

void
monotouch_connect_wifi (NSMutableArray *ips)
{
	long listen_port = monodevelop_port;
	unsigned char sockaddr[sizeof (struct sockaddr_in6)];
	struct sockaddr_in6 *sin6 = (struct sockaddr_in6 *) sockaddr;
	struct sockaddr_in *sin = (struct sockaddr_in *) sockaddr;
	int family, waiting, len, rv;
	unsigned long i, connection_port;
	bool connected;
	unsigned long ip_count = [ips count];
	const char *family_str;
	const char *ip;
	int *sockets;
	long flags;
	
	if (ip_count == 0) {
		PRINT (PRODUCT ": No IPs to connect to.");
		return;
	}
	
	sockets = (int *) calloc (sizeof (int), ip_count);
	for (i = 0; i < ip_count; i++)
		sockets[i] = -2;
	
	// Open a socket and try to establish a connection for each IP
	do {
		waiting = 0;
		connected = false;
		for (i = 0; i < ip_count; i++) {
			if (sockets [i] == -1)
				continue;

			ip = [[ips objectAtIndex:i] UTF8String];
			
			memset (sockaddr, 0, sizeof (sockaddr));
			
			// Parse the host IP, assuming IPv4 and falling back to IPv6
			if ((rv = inet_pton (AF_INET, ip, &sin->sin_addr)) == 1) {
				len = sin->sin_len = sizeof (struct sockaddr_in);
				family = sin->sin_family = AF_INET;
				sin->sin_port = htons (listen_port);
				family_str = "IPv4";
			} else if (rv == 0 && (rv = inet_pton (AF_INET6, ip, &sin6->sin6_addr)) == 1) {
				len = sin6->sin6_len = sizeof (struct sockaddr_in6);
				family = sin6->sin6_family = AF_INET6;
				sin6->sin6_port = htons (listen_port);
				family_str = "IPv6";
			} else {
				PRINT (PRODUCT ": Error parsing '%s': %s", ip, errno ? strerror (errno) : "unsupported address type");
				sockets[i] = -1;
				continue;
			}
			
			if ((sockets[i] = socket (family, SOCK_STREAM, IPPROTO_TCP)) == -1) {
				PRINT (PRODUCT ": Failed to create %s socket: %s", family_str, strerror (errno));
				continue;
			}
			
			// Make the socket non-blocking
			flags = fcntl (sockets[i], F_GETFL, NULL);
			fcntl (sockets[i], F_SETFL, flags | O_NONBLOCK);
			
			// Connect to the host
			if ((rv = connect (sockets[i], (struct sockaddr *) sockaddr, (socklen_t) len)) == 0) {
				// connection completed, this is our man.
				connection_port = i;
				connected = true;
				break;
			}
			
			if (rv < 0 && errno != EINPROGRESS) {
				PRINT (PRODUCT ": Failed to connect to %s on port %d: %s", ip, listen_port, strerror (errno));
				close (sockets[i]);
				sockets[i] = -1;
				continue;
			}
			
			// asynchronous connect
			waiting++;
		}
	
		// Wait for async socket connections to become available
		while (!connected && waiting > 0) {
			socklen_t optlen = sizeof (int);
			fd_set rset, wset, xset;
			struct timeval tv;
			int max_fd = -1;
			int error;
			
			tv.tv_sec = 2;
			tv.tv_usec = 0;
			
			FD_ZERO (&rset);
			FD_ZERO (&wset);
			FD_ZERO (&xset);
			
			for (i = 0; i < ip_count; i++) {
				if (sockets[i] < 0)
					continue;
				
				max_fd = MAX (max_fd, sockets[i]);
				FD_SET (sockets[i], &rset);
				FD_SET (sockets[i], &wset);
				FD_SET (sockets[i], &xset);
			}
			
			if ((rv = select (max_fd + 1, &rset, &wset, &xset, &tv)) == 0) {
				// timeout hit, no connections available.
				free (sockets);
				return;
			}
			
			if (rv < 0) {
				if (errno == EINTR || errno == EAGAIN)
					continue;
				
				// irrecoverable error
				PRINT (PRODUCT ": Error while waiting for connections: %s", strerror (errno));
				free (sockets);
				return;
			}
			
			for (i = 0; i < ip_count; i++) {
				if (sockets[i] < 0)
					continue;
				
				if (FD_ISSET (sockets[i], &xset)) {
					// exception on this socket
					close (sockets[i]);
					sockets[i] = -1;
					waiting--;
					continue;
				}
				
				if (!FD_ISSET (sockets[i], &rset) && !FD_ISSET (sockets[i], &wset)) {
					// still waiting...
					continue;
				}
				
				// okay, this socket is ready for reading or writing...
				if (getsockopt (sockets[i], SOL_SOCKET, SO_ERROR, &error, &optlen) < 0) {
					PRINT (PRODUCT ": Error while trying to get socket options for %s: %s", [[ips objectAtIndex:i] UTF8String], strerror (errno));
					close (sockets[i]);
					sockets[i] = -1;
					waiting--;
					continue;
				}
				
				if (error != 0) {
					PRINT (PRODUCT ": Socket error while connecting to IDE on %s:%d: %s", [[ips objectAtIndex:i] UTF8String], listen_port, strerror (error));
					close (sockets[i]);
					sockets[i] = -1;
					waiting--;
					continue;
				}
				
				// success!
				connected = true;
				connection_port = i;
				break;
			}
		}
	
		if (!connected) {
			free (sockets);
			return;
		}
	
		// close the remaining sockets
		for (i = 0; i < ip_count; i++) {
			if (i == connection_port || sockets[i] < 0)
				continue;
			
			close (sockets[i]);
			sockets[i] = -1;
		}
	
		LOG (PRODUCT ": Established connection with the IDE (fd: %i)\n", sockets [connection_port]);
	} while (monotouch_process_connection (sockets [connection_port]));

	free (sockets);

	return;
}

void
monotouch_connect_usb ()
{
	long listen_port = monodevelop_port;
	struct sockaddr_in listen_addr;
	int listen_socket = -1;
	int fd;
	socklen_t len;
	int rv;
	fd_set rset;
	struct timeval tv;
	struct timeval start;
	struct timeval now;
	int flags;
	
	// Create the listen socket and set it up
	listen_socket = socket (PF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (listen_socket == -1) {
		PRINT (PRODUCT ": Could not create socket for the IDE to connect to: %s", strerror (errno));
		return;
	}
	
	flags = 1;
	if (setsockopt (listen_socket, SOL_SOCKET, SO_REUSEADDR, &flags, sizeof (flags)) == -1) {
		PRINT (PRODUCT ": Could not set SO_REUSEADDR on the listening socket (%s)", strerror (errno));
		// not a fatal failure
	}
	
	// Bind
	memset (&listen_addr, 0, sizeof (listen_addr));
	listen_addr.sin_family = AF_INET;
	listen_addr.sin_port = htons (listen_port);
	listen_addr.sin_addr.s_addr = INADDR_ANY;
	rv = bind (listen_socket, (struct sockaddr *) &listen_addr, sizeof (listen_addr));
	if (rv == -1) {
		PRINT (PRODUCT ": Could not bind to address: %s", strerror (errno));
		goto cleanup;
	}

	// Make the socket non-blocking
	flags = fcntl (listen_socket, F_GETFL, NULL);
	flags |= O_NONBLOCK;
	fcntl (listen_socket, F_SETFL, flags);

	rv = listen (listen_socket, 1);
	if (rv == -1) {
		PRINT (PRODUCT ": Could not listen for the IDE: %s", strerror (errno));
		goto cleanup;
	}

	// Wait for connections
	start.tv_sec = 0;
	start.tv_usec = 0;
	do {
		FD_ZERO (&rset);
		FD_SET (listen_socket, &rset);

		do {
			// Calculate how long we can wait if we can only work for 2s since we started
			gettimeofday (&now, NULL);
			if (start.tv_sec == 0) {
				start.tv_sec = now.tv_sec;
				start.tv_usec = now.tv_usec;
				tv.tv_sec = 2;
				tv.tv_usec = 0;
			} else if ((start.tv_sec + 2 == now.tv_sec && start.tv_usec < now.tv_usec) || start.tv_sec + 2 < now.tv_sec) {
				// timeout
			} else {
				tv.tv_sec = start.tv_sec + 2 - now.tv_sec;
				if (start.tv_usec > now.tv_usec) {
					tv.tv_usec = start.tv_usec - now.tv_usec;
				} else {
					tv.tv_sec--;
					tv.tv_usec = 1000000 + start.tv_usec - now.tv_usec;
				}
			}

			// LOG (PRODUCT ": Waiting for connections from the IDE, sec: %i usec: %i\n", (int) tv.tv_sec, (int) tv.tv_usec);

			if ((rv = select (listen_socket + 1, &rset, NULL, NULL, &tv)) == 0) {
				// timeout hit, no connections available.
				LOG (PRODUCT ": Listened for connections from the IDE for 2 seconds, nobody connected.\n");
				goto cleanup;
			}
		} while (rv == -1 && errno == EINTR);

		if (rv == -1) {
			PRINT (PRODUCT ": Failed while waiting for the IDE to connect: %s", strerror (errno));
			goto cleanup;
		}

		len = sizeof (struct sockaddr_in);
		fd = accept (listen_socket, (struct sockaddr *) &listen_addr, &len);
		if (fd == -1) {
			PRINT (PRODUCT ": Failed to accept connection from the IDE: %s", strerror (errno));
			goto cleanup;
		}

		flags = 1;
		if (setsockopt (fd, IPPROTO_TCP, TCP_NODELAY, (char *) &flags, sizeof (flags)) < 0) {
			PRINT (PRODUCT ": Could not set TCP_NODELAY on socket (%s)", strerror (errno));
			// not a fatal failure
		}

		LOG (PRODUCT ": Successfully received USB connection from the IDE on port %i, fd: %i\n", listen_port, fd);
	} while (monotouch_process_connection (fd));

	LOG (PRODUCT ": Successfully talked to the IDE. Will continue startup now.\n");

cleanup:
	close (listen_socket);
	return;
}

void
monotouch_dump_objc_api (Class klass)
{
	unsigned int c;
	Ivar *vars;
	Method *methods;
	objc_property_t *props;
	
	printf ("Dumping class %p = %s\n", klass, class_getName (klass));
	
	vars = class_copyIvarList (klass, &c);
	printf ("\t%i instance variables:\n", c);
	for (unsigned int i = 0; i < c; i++)
		printf ("\t\t#%i: %s\n", i + 1, ivar_getName (vars [i]));
	free (vars);
	
	methods = class_copyMethodList (klass, &c);
	printf ("\t%i instance methods:\n", c);
	for (unsigned int i = 0; i < c; i++)
		printf ("\t\t#%i: %s\n", i + 1, sel_getName (method_getName (methods [i])));
	free (methods);
	
	props = class_copyPropertyList (klass, &c);
	printf ("\t%i instance properties:\n", c);
	for (unsigned int i = 0; i < c; i++)
		printf ("\t\t#%i: %s\n", i + 1, property_getName (props [i]));
	free (props);
	
	fflush (stdout);
}

void
monotouch_load_debugger ()
{
	// main thread only 
	if (sdb_fd != -1) {
#if !defined (CORECLR_RUNTIME)
		DebuggerTransport transport;
		transport.name = "custom_transport";
		transport.connect = sdb_connect;
		transport.close1 = sdb_close1;
		transport.close2 = sdb_close2;
		transport.send = sdb_send;
		transport.recv = sdb_recv;

		mono_debugger_agent_register_transport (&transport);

		char *options;
		if (sdb_timeout_time != -1) {
			options = xamarin_strdup_printf ("transport=custom_transport,address=dummy,embedding=1,timeout=%d", sdb_timeout_time);
		} else {
			options = xamarin_strdup_printf ("transport=custom_transport,address=dummy,embedding=1");
		}
		mono_debugger_agent_parse_options (options);
		// Can't free the 'options' variable, because mono_debugger_agent_parse_option stores the pointer instead of creating a copy :/

		LOG (PRODUCT ": Debugger loaded with custom transport (fd: %i)\n", sdb_fd);
#else
		LOG (PRODUCT ": Debugger not loaded (debugger loading not implemented for CoreCLR).\n");
#endif
	} else {
		LOG (PRODUCT ": Debugger not loaded (disabled).\n");
	}
}

void
monotouch_load_profiler ()
{
	// TODO: make this generic enough for other profilers to work too
	// Main thread only
	if (profiler_description != NULL) {
#if !defined (CORECLR_RUNTIME)
		mono_profiler_load (profiler_description);

		LOG (PRODUCT ": Profiler loaded: %s\n", profiler_description);
#else
		LOG (PRODUCT ": Profiler not loaded (profiler loading not implemented for CoreCLR): %s\n", profiler_description);
#endif
		free (profiler_description);
		profiler_description = NULL;
	} else {
		LOG (PRODUCT ": Profiler not loaded (disabled)\n");
	}
}

// returns true if it's necessary to create more
// connections to process more data.
bool
monotouch_process_connection (int fd)
{
	// make sure the fd/socket blocks on reads/writes
	fcntl (fd, F_SETFL, fcntl (fd, F_GETFL, NULL) & ~O_NONBLOCK);

	while (true) {
		char command [257];
		ssize_t rv;
		unsigned char cmd_len;

		rv = recv_uninterrupted (fd, &cmd_len, 1);
		if (rv <= 0) {
			LOG (PRODUCT ": Error while receiving command from the IDE (%s)\n", strerror (errno));
			return false;
		}

		rv = recv_uninterrupted (fd, command, cmd_len);
		if (rv <= 0) {
			LOG (PRODUCT ": Error while receiving command from the IDE (%s)\n", strerror (errno));
			return false;
		}
		
		// null-terminate
		command [cmd_len] = 0;

		LOG (PRODUCT ": Processing: '%s'\n", command);
		
		if (!strcmp (command, "connect output")) {
			dup2 (fd, 1);
			dup2 (fd, 2);
			return true; 
		} else if (!strcmp (command, "connect stdout")) {
			dup2 (fd, 1);
			return true;
		} else if (!strcmp (command, "connect stderr")) {
			dup2 (fd, 2);
			return true;
		} else if (!strcmp (command, "discard")) {
			return true;
		} else if (!strcmp (command, "ping")) {
			if (!send_uninterrupted (fd, "pong", 5))
				LOG (PRODUCT ": Got keepalive request from the IDE, but could not send response back (%s)\n", strerror (errno));
		} else if (!strcmp (command, "exit process")) {
			LOG (PRODUCT ": The IDE requested an exit, will exit immediately.\n");
			fflush (stderr);
			exit (0);
		} else if (!strncmp (command, "start debugger: ", 16)) {
			const char *debugger = command + 16;
			bool use_fd = false;
			if (!strcmp (debugger, "no")) {
				/* disabled */
			} else if (!strcmp (debugger, "sdb")) {
				sdb_fd = fd;
				use_fd = true;
			}
			debugging_configured = true;
			pthread_mutex_lock (&mutex);
			pthread_cond_signal (&cond);
			pthread_mutex_unlock (&mutex);
			if (use_fd)
				return true;
		} else if (!strncmp (command, "start profiler: ", 16)) {
			// initialize the log profiler if we're debugging
			const char *prof = command + 16;
			bool use_fd = false;
			
			if (!strcmp (prof, "no")) {
				/* disabled */
			} else if (!strncmp (prof, "log:", 4)) {
#if defined (__x86_64__)
				profiler_description = strdup (prof);
#else
				use_fd = true;
				profiler_description = xamarin_strdup_printf ("%s,output=#%i", prof, fd);
#endif
				xamarin_set_gc_pump_enabled (false);
			} else {
				LOG (PRODUCT ": Unknown profiler, expect unexpected behavior (%s)\n", prof);
				profiler_description = strdup (prof);
			}
			profiler_configured = true;
			pthread_mutex_lock (&mutex);
			pthread_cond_signal (&cond);
			pthread_mutex_unlock (&mutex);
			if (use_fd)
				return true;
		} else if (!strncmp (command, "set heapshot port: ", 19)) {
			heapshot_port = strtol (command + 19, NULL, 0);
			LOG (PRODUCT ": HeapShot port is now: %i\n", heapshot_port);
		} else if (!strcmp (command, "heapshot")) {
			if (heapshot_fd == -1) {
				struct sockaddr_in heapshot_addr;

				memset (&heapshot_addr, 0, sizeof (heapshot_addr));
				heapshot_addr.sin_len = sizeof (heapshot_addr);
				heapshot_addr.sin_port = htons (heapshot_port);
				heapshot_addr.sin_addr.s_addr = htonl (INADDR_LOOPBACK);
				heapshot_addr.sin_family = AF_INET;

				if ((heapshot_fd = socket (AF_INET, SOCK_STREAM, IPPROTO_TCP)) == -1) {
					LOG (PRODUCT ": Failed to create socket to connect to profiler: %s\n", strerror (errno));
				} else if (connect (heapshot_fd, (struct sockaddr *) &heapshot_addr, sizeof (heapshot_addr)) != 0) {
					LOG (PRODUCT ": Failed to connect to profiler to request a heapshot: %s\n", strerror (errno));
					close (heapshot_fd);
					heapshot_fd = -1;
				} else {
					// Success!
				}
			}
			if (heapshot_fd != -1) {
				if (!send_uninterrupted (heapshot_fd, "heapshot\n", 9))
					LOG (PRODUCT ": Failed to request heapshot: %s\n", strerror (errno));
			}
		} else {
			LOG (PRODUCT ": Unknown command received from the IDE: '%s'\n", command);
		}
	}
}

// SUCCESS = 0
// FAILURE > 0
int monotouch_debug_connect (NSMutableArray *ips, int debug_port, int output_port)
{
	unsigned char sockaddr[sizeof (struct sockaddr_in6)];
	struct sockaddr_in6 *sin6 = (struct sockaddr_in6 *) sockaddr;
	struct sockaddr_in *sin = (struct sockaddr_in *) sockaddr;
	int family, waiting, len, rv;
	unsigned long i;
	unsigned long ip_count = [ips count];
	const char *family_str;
	bool connected = false;
	unsigned long connection_port;
	const char *ip;
	int *sockets;
	long flags;
	
	if (ip_count == 0) {
		PRINT (PRODUCT ": No IPs to connect to.");
		return 2;
	}
	
	sockets = (int *) calloc (sizeof (int), ip_count);
	for (i = 0; i < ip_count; i++)
		sockets[i] = -1;
	
	// Open a socket and try to establish a connection for each IP
	waiting = 0;
	for (i = 0; i < ip_count; i++) {
		ip = [[ips objectAtIndex:i] UTF8String];
		
		memset (sockaddr, 0, sizeof (sockaddr));
		
		// Parse the host IP, assuming IPv4 and falling back to IPv6
		if ((rv = inet_pton (AF_INET, ip, &sin->sin_addr)) == 1) {
			len = sin->sin_len = sizeof (struct sockaddr_in);
			family = sin->sin_family = AF_INET;
			sin->sin_port = htons (output_port);
			family_str = "IPv4";
		} else if (rv == 0 && (rv = inet_pton (AF_INET6, ip, &sin6->sin6_addr)) == 1) {
			len = sin6->sin6_len = sizeof (struct sockaddr_in6);
			family = sin6->sin6_family = AF_INET6;
			sin6->sin6_port = htons (output_port);
			family_str = "IPv6";
		} else {
			PRINT (PRODUCT ": Error parsing '%s': %s", ip, errno ? strerror (errno) : "unsupported address type");
			sockets[i] = -1;
			continue;
		}
		
		if ((sockets[i] = socket (family, SOCK_STREAM, IPPROTO_TCP)) == -1) {
			PRINT (PRODUCT ": Failed to create %s socket: %s", family_str, strerror (errno));
			continue;
		}
		
		// Make the socket non-blocking
		flags = fcntl (sockets[i], F_GETFL, NULL);
		fcntl (sockets[i], F_SETFL, flags | O_NONBLOCK);
		
		// Connect to the host
		if ((rv = connect (sockets[i], (struct sockaddr *) sockaddr, (socklen_t) len)) == 0) {
			// connection completed, this is our man.
			connected = true;
			connection_port = i;
			break;
		}
		
		if (rv < 0 && errno != EINPROGRESS) {
			PRINT (PRODUCT ": Failed to connect to %s on port %d: %s", ip, output_port, strerror (errno));
			close (sockets[i]);
			sockets[i] = -1;
			continue;
		}
		
		// asynchronous connect
		waiting++;
	}
	
	// Wait for async socket connections to become available
	while (!connected && waiting > 0) {
		socklen_t optlen = sizeof (int);
		fd_set rset, wset, xset;
		struct timeval tv;
		int max_fd = -1;
		int error;
		
		tv.tv_sec = 2;
		tv.tv_usec = 0;
		
		FD_ZERO (&rset);
		FD_ZERO (&wset);
		FD_ZERO (&xset);
		
		for (i = 0; i < ip_count; i++) {
			if (sockets[i] < 0)
				continue;
			
			max_fd = MAX (max_fd, sockets[i]);
			FD_SET (sockets[i], &rset);
			FD_SET (sockets[i], &wset);
			FD_SET (sockets[i], &xset);
		}
		
		if ((rv = select (max_fd + 1, &rset, &wset, &xset, &tv)) == 0) {
			// timeout hit, no connections available.
			free (sockets);
			return 1;
		}
		
		if (rv < 0) {
			if (errno == EINTR || errno == EAGAIN)
				continue;
			
			// irrecoverable error
			PRINT (PRODUCT ": Error while waiting for connections: %s", strerror (errno));
			free (sockets);
			return 1;
		}
		
		for (i = 0; i < ip_count; i++) {
			if (sockets[i] < 0)
				continue;
			
			if (FD_ISSET (sockets[i], &xset)) {
				// exception on this socket
				close (sockets[i]);
				sockets[i] = -1;
				waiting--;
				continue;
			}
			
			if (!FD_ISSET (sockets[i], &rset) && !FD_ISSET (sockets[i], &wset)) {
				// still waiting...
				continue;
			}
			
			// okay, this socket is ready for reading or writing...
			if (getsockopt (sockets[i], SOL_SOCKET, SO_ERROR, &error, &optlen) < 0) {
				PRINT (PRODUCT ": Error while trying to get socket options for %s: %s", [[ips objectAtIndex:i] UTF8String], strerror (errno));
				close (sockets[i]);
				sockets[i] = -1;
				waiting--;
				continue;
			}
			
			if (error != 0) {
				PRINT (PRODUCT ": Socket error while connecting to the IDE on %s:%d: %s", [[ips objectAtIndex:i] UTF8String], output_port, strerror (error));
				close (sockets[i]);
				sockets[i] = -1;
				waiting--;
				continue;
			}
			
			// success!
			connected = true;
			connection_port = i;
			break;
		}
	}
	
	if (!connected) {
		free (sockets);
		return 1;
	}
	
	// make the socket block on reads/writes
	flags = fcntl (sockets[connection_port], F_GETFL, NULL);
	fcntl (sockets[connection_port], F_SETFL, flags & ~O_NONBLOCK);
 
	LOG (PRODUCT ": Connected output to the IDE on %s:%d\n", [[ips objectAtIndex:i] UTF8String], output_port);

	dup2 (sockets[connection_port], 1);
	dup2 (sockets[connection_port], 2);

	debug_host = strdup ([[ips objectAtIndex:connection_port] UTF8String]);
	
	// close the remaining sockets
	for (i = 0; i < ip_count; i++) {
		if (i == connection_port || sockets[i] < 0)
			continue;
		
		close (sockets[i]);
		sockets[i] = -1;
	}
	
	free (sockets);
	
	return 0;
}

#else
int xamarin_fix_ranlib_warning_about_no_symbols_v2;
#endif /* DEBUG */
