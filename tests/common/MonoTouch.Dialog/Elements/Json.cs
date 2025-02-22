// 
// Json.cs: MonoTouch.Dialog support for creating UIs from Json description files
// 
// Author:
//   Miguel de Icaza
//
// See the JSON.md file for documentation
//
// TODO: Json to load entire view controllers
//

#if !NET // This code needs to be migrated to use System.Text.Json in .NET
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Json;
using System.Net;
using System.Reflection;

using Foundation;
using UIKit;
using CoreGraphics;

using NSAction = global::System.Action;

namespace MonoTouch.Dialog {

	public class JsonElement : RootElement {
		JsonElement jsonParent;
		Dictionary<string, Element> map;
		const int CSIZE = 16;
		const int SPINNER_TAG = 1000;
		public string Url;
		bool loading;

		public static DateTimeKind DateKind { get; set; } = DateTimeKind.Unspecified;

		UIActivityIndicatorView StartSpinner (UITableViewCell cell)
		{
			var cvb = cell.ContentView.Bounds;

			var spinner = new UIActivityIndicatorView (new CGRect (cvb.Width - CSIZE / 2, (cvb.Height - CSIZE) / 2, CSIZE, CSIZE)) {
				Tag = SPINNER_TAG,
				ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray,
			};
			cell.ContentView.AddSubview (spinner);
			spinner.StartAnimating ();
			cell.Accessory = UITableViewCellAccessory.None;

			return spinner;
		}

		void RemoveSpinner (UITableViewCell cell, UIActivityIndicatorView spinner)
		{
			spinner.StopAnimating ();
			spinner.RemoveFromSuperview ();
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = base.GetCell (tv);
			if (Url is null)
				return cell;

			var spinner = cell.ViewWithTag (SPINNER_TAG) as UIActivityIndicatorView;
			if (loading) {
				if (spinner is null)
					StartSpinner (cell);
				else
					if (spinner is not null)
					RemoveSpinner (cell, spinner);
			}
			return cell;
		}

#if __UNIFIED__
		class ConnectionDelegate : NSUrlConnectionDataDelegate {
#else
		class ConnectionDelegate : NSUrlConnectionDelegate {
#endif
			Action<Stream, NSError> callback;
			NSMutableData buffer;

			public ConnectionDelegate (Action<Stream, NSError> callback)
			{
				this.callback = callback;
				buffer = new NSMutableData ();
			}

			public override void ReceivedResponse (NSUrlConnection connection, NSUrlResponse response)
			{
				buffer.Length = 0;
			}

			public override void FailedWithError (NSUrlConnection connection, NSError error)
			{
				callback (null, error);
			}

			public override void ReceivedData (NSUrlConnection connection, NSData data)
			{
				buffer.AppendData (data);
			}

			public override void FinishedLoading (NSUrlConnection connection)
			{
				callback (buffer.AsStream (), null);
			}
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			if (Url is null) {
				base.Selected (dvc, tableView, path);
				return;
			}

			tableView.DeselectRow (path, false);
			if (loading)
				return;
			var cell = GetActiveCell ();
			var spinner = StartSpinner (cell);
			loading = true;

			var request = new NSUrlRequest (new NSUrl (Url), NSUrlRequestCachePolicy.UseProtocolCachePolicy, 60);
			/*var connection = */
			new NSUrlConnection (request, new ConnectionDelegate ((data, error) => {
				loading = false;
				spinner.StopAnimating ();
				spinner.RemoveFromSuperview ();
				if (error is null) {
					try {
						var obj = JsonValue.Load (new StreamReader (data)) as JsonObject;
						if (obj is not null) {
							var root = JsonElement.FromJson (obj);
							var newDvc = new DialogViewController (root, true) {
								Autorotate = true
							};
							PrepareDialogViewController (newDvc);
							dvc.ActivateController (newDvc);
							return;
						}
					} catch (Exception ee) {
						Console.WriteLine (ee);
					}
				}
				var alertController = UIAlertController.Create ("Error", "Unable to download data", UIAlertControllerStyle.Alert);
				alertController.AddAction (UIAlertAction.Create ("Ok", UIAlertActionStyle.Default, (obj) => { }));
				UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController (alertController, true, () => { });
			}));
		}

		public JsonElement (string caption, string url) : base (caption)
		{
			Url = url;
		}

		public JsonElement (string caption, int section, int element, string url) : base (caption, section, element)
		{
			Url = url;
		}

		public JsonElement (string caption, Group group, string url) : base (caption, group)
		{
			Url = url;
		}

		public static JsonElement FromFile (string file, object arg)
		{
			using (var reader = File.OpenRead (file))
				return FromJson (JsonObject.Load (reader) as JsonObject, arg);
		}

		public static JsonElement FromFile (string file)
		{
			return FromFile (file, null);
		}

		public static JsonElement FromJson (JsonObject json)
		{
			return FromJson (null, json, null);
		}

		public static JsonElement FromJson (JsonObject json, object data)
		{
			return FromJson (null, json, data);
		}

		public static JsonElement FromJson (JsonElement parent, JsonObject json, object data)
		{
			if (json is null)
				return null;

			var title = GetString (json, "title") ?? "";

			var group = GetString (json, "group");
			var url = GetString (json, "url");
			var radioSelected = GetString (json, "radioselected");
			JsonElement root;
			if (group is null) {
				if (radioSelected is null)
					root = new JsonElement (title, url);
				else
					root = new JsonElement (title, new RadioGroup (int.Parse (radioSelected)), url);
			} else {
				if (radioSelected is null)
					root = new JsonElement (title, new Group (group), url);
				else {
					// It does not seem that we group elements together, notice when I add
					// the return, and then change my mind, I have to undo *twice* instead of once.

					root = new JsonElement (title, new RadioGroup (group, int.Parse (radioSelected)), url);
				}
			}
			root.jsonParent = parent;
			root.LoadSections (GetArray (json, "sections"), data);
			return root;
		}

		void AddMapping (string id, Element element)
		{
			if (jsonParent is not null) {
				jsonParent.AddMapping (id, element);
				return;
			}
			if (map is null)
				map = new Dictionary<string, Element> ();
			map.Add (id, element);
		}

		//
		// Retrieves the element name "key"
		//
		public Element this [string key] {
			get {
				if (jsonParent is not null)
					return jsonParent [key];
				if (map is null)
					return null;
				Element res;
				if (map.TryGetValue (key, out res))
					return res;
				return null;
			}
		}

		static void Error (string msg)
		{
			Console.WriteLine (msg);
		}

		static void Error (string fmt, params object [] args)
		{
			Error (String.Format (fmt, args));
		}

		static string GetString (JsonValue obj, string key)
		{
			if (obj.ContainsKey (key))
				if (obj [key].JsonType == JsonType.String)
					return (string) obj [key];
			return null;
		}

		static JsonArray GetArray (JsonObject obj, string key)
		{
			if (obj.ContainsKey (key))
				if (obj [key].JsonType == JsonType.Array)
					return (JsonArray) obj [key];
			return null;
		}

		static bool GetBoolean (JsonObject obj, string key)
		{
			try {
				return (bool) obj [key];
			} catch {
				return false;
			}
		}

		void LoadSections (JsonArray array, object data)
		{
			if (array is null)
				return;
			int n = array.Count;
			for (int i = 0; i < n; i++) {
				var jsonSection = array [i];
				var header = GetString (jsonSection, "header");
				var footer = GetString (jsonSection, "footer");
				var id = GetString (jsonSection, "id");

				var section = new Section (header, footer);
				if (jsonSection.ContainsKey ("elements"))
					LoadSectionElements (section, jsonSection ["elements"] as JsonArray, data);
				Add (section);
				if (id is not null)
					AddMapping (id, section);
			}
		}

		static string bundlePath;

		static string ExpandPath (string path)
		{
			if (path is not null && path.Length > 1 && path [0] == '~' && path [1] == '/') {
				if (bundlePath is null)
					bundlePath = NSBundle.MainBundle.BundlePath;

				return Path.Combine (bundlePath, path.Substring (2));
			}
			return path;
		}

		static Element LoadBoolean (JsonObject json)
		{
			var caption = GetString (json, "caption");
			bool bvalue = GetBoolean (json, "value");
			var group = GetString (json, "group");
			var onImagePath = ExpandPath (GetString (json, "on"));
			var offImagePath = ExpandPath (GetString (json, "off"));

			if (onImagePath is not null && offImagePath is not null) {
				var onImage = UIImage.FromFile (onImagePath);
				var offImage = UIImage.FromFile (offImagePath);

				return new BooleanImageElement (caption, bvalue, onImage, offImage);
			} else
				return new BooleanElement (caption, bvalue, group);
		}

		static UIKeyboardType ToKeyboardType (string kbdType)
		{
			switch (kbdType) {
			case "numbers": return UIKeyboardType.NumberPad;
			case "default": return UIKeyboardType.Default;
			case "ascii": return UIKeyboardType.ASCIICapable;
			case "numbers-and-punctuation": return UIKeyboardType.NumbersAndPunctuation;
			case "decimal": return UIKeyboardType.DecimalPad;
			case "email": return UIKeyboardType.EmailAddress;
			case "name": return UIKeyboardType.NamePhonePad;
			case "twitter": return UIKeyboardType.Twitter;
			case "url": return UIKeyboardType.Url;
			default:
				Console.WriteLine ("Unknown keyboard type: {0}, valid values are numbers, default, ascii, numbers-and-punctuation, decimal, email, name, twitter and url", kbdType);
				break;
			}
			return UIKeyboardType.Default;
		}

		static UIReturnKeyType ToReturnKeyType (string returnKeyType)
		{
			switch (returnKeyType) {
			case "default": return UIReturnKeyType.Default;
			case "done": return UIReturnKeyType.Done;
			case "emergencycall": return UIReturnKeyType.EmergencyCall;
			case "go": return UIReturnKeyType.Go;
			case "google": return UIReturnKeyType.Google;
			case "join": return UIReturnKeyType.Join;
			case "next": return UIReturnKeyType.Next;
			case "route": return UIReturnKeyType.Route;
			case "search": return UIReturnKeyType.Search;
			case "send": return UIReturnKeyType.Send;
			case "yahoo": return UIReturnKeyType.Yahoo;
			default:
				Console.WriteLine ("Unknown return key type `{0}', valid values are default, done, emergencycall, go, google, join, next, route, search, send and yahoo");
				break;
			}
			return UIReturnKeyType.Default;
		}

		static UITextAutocapitalizationType ToAutocapitalization (string auto)
		{
			switch (auto) {
			case "sentences": return UITextAutocapitalizationType.Sentences;
			case "none": return UITextAutocapitalizationType.None;
			case "words": return UITextAutocapitalizationType.Words;
			case "all": return UITextAutocapitalizationType.AllCharacters;
			default:
				Console.WriteLine ("Unknown autocapitalization value: `{0}', allowed values are sentences, none, words and all");
				break;
			}
			return UITextAutocapitalizationType.Sentences;
		}

		static UITextAutocorrectionType ToAutocorrect (JsonValue value)
		{
			if (value.JsonType == JsonType.Boolean)
				return ((bool) value) ? UITextAutocorrectionType.Yes : UITextAutocorrectionType.No;
			if (value.JsonType == JsonType.String) {
				var s = ((string) value);
				if (s == "yes")
					return UITextAutocorrectionType.Yes;
				return UITextAutocorrectionType.No;
			}
			return UITextAutocorrectionType.Default;
		}

		static Element LoadEntry (JsonObject json, bool isPassword)
		{
			var caption = GetString (json, "caption");
			var value = GetString (json, "value");
			var placeholder = GetString (json, "placeholder");

			var element = new EntryElement (caption, placeholder, value, isPassword);

			if (json.ContainsKey ("keyboard"))
				element.KeyboardType = ToKeyboardType (GetString (json, "keyboard"));
			if (json.ContainsKey ("return-key"))
				element.ReturnKeyType = ToReturnKeyType (GetString (json, "return-key"));
			if (json.ContainsKey ("capitalization"))
				element.AutocapitalizationType = ToAutocapitalization (GetString (json, "capitalization"));
			if (json.ContainsKey ("autocorrect"))
				element.AutocorrectionType = ToAutocorrect (json ["autocorrect"]);

			return element;
		}

		static UITableViewCellAccessory ToAccessory (string accesory)
		{
			switch (accesory) {
			case "checkmark": return UITableViewCellAccessory.Checkmark;
			case "detail-disclosure": return UITableViewCellAccessory.DetailDisclosureButton;
			case "disclosure-indicator": return UITableViewCellAccessory.DisclosureIndicator;
			}
			return UITableViewCellAccessory.None;
		}

		static int FromHex (char c)
		{
			if (c >= '0' && c <= '9')
				return c - '0';
			if (c >= 'a' && c <= 'f')
				return c - 'a' + 10;
			if (c >= 'A' && c <= 'F')
				return c - 'A' + 10;
			Console.WriteLine ("Unexpected `{0}' in hex value for color", c);
			return 0;
		}

		static void ColorError (string text)
		{
			Console.WriteLine ("Unknown color specification {0}, expecting #rgb, #rgba, #rrggbb or #rrggbbaa formats", text);
		}

		static UIColor ParseColor (string text)
		{
			int tl = text.Length;

			if (tl > 1 && text [0] == '#') {
				int r, g, b, a;

				if (tl == 4 || tl == 5) {
					r = FromHex (text [1]);
					g = FromHex (text [2]);
					b = FromHex (text [3]);
					a = tl == 5 ? FromHex (text [4]) : 15;

					r = r << 4 | r;
					g = g << 4 | g;
					b = b << 4 | b;
					a = a << 4 | a;
				} else if (tl == 7 || tl == 9) {
					r = FromHex (text [1]) << 4 | FromHex (text [2]);
					g = FromHex (text [3]) << 4 | FromHex (text [4]);
					b = FromHex (text [5]) << 4 | FromHex (text [6]);
					a = tl == 9 ? FromHex (text [7]) << 4 | FromHex (text [8]) : 255;
				} else {
					ColorError (text);
					return UIColor.Black;
				}
				return UIColor.FromRGBA (r, g, b, a);
			}
			ColorError (text);
			return UIColor.Black;
		}

		static UILineBreakMode ToLinebreakMode (string mode)
		{
			switch (mode) {
			case "character-wrap": return UILineBreakMode.CharacterWrap;
			case "clip": return UILineBreakMode.Clip;
			case "head-truncation": return UILineBreakMode.HeadTruncation;
			case "middle-truncation": return UILineBreakMode.MiddleTruncation;
			case "tail-truncation": return UILineBreakMode.TailTruncation;
			case "word-wrap": return UILineBreakMode.WordWrap;
			default:
				Console.WriteLine ("Unexpeted linebreak mode `{0}', valid values include: character-wrap, clip, head-truncation, middle-truncation, tail-truncation and word-wrap", mode);
				return UILineBreakMode.Clip;
			}
		}

		// Parses a font in the format:
		// Name[-SIZE]
		// if -SIZE is omitted, then the value is SystemFontSize
		//
		static UIFont ToFont (string kvalue)
		{
			int q = kvalue.LastIndexOf ("-");
			string fname = kvalue;
			nfloat fsize = 0;

			if (q != -1) {
				nfloat.TryParse (kvalue.Substring (q + 1), out fsize);
				fname = kvalue.Substring (0, q);
			}
			if (fsize <= 0) {
#if __TVOS__
				fsize = UIFont.SystemFontOfSize (12).PointSize;
#else
				fsize = UIFont.SystemFontSize;
#endif // __TVOS__
			}

			var f = UIFont.FromName (fname, fsize);
			if (f is null)
				return UIFont.SystemFontOfSize (12);
			return f;
		}

		static UITableViewCellStyle ToCellStyle (string style)
		{
			switch (style) {
			case "default": return UITableViewCellStyle.Default;
			case "subtitle": return UITableViewCellStyle.Subtitle;
			case "value1": return UITableViewCellStyle.Value1;
			case "value2": return UITableViewCellStyle.Value2;
			default:
				Console.WriteLine ("unknown cell style `{0}', valid values are default, subtitle, value1 and value2", style);
				break;
			}
			return UITableViewCellStyle.Default;
		}

		static UITextAlignment ToAlignment (string align)
		{
			switch (align) {
			case "center": return UITextAlignment.Center;
			case "left": return UITextAlignment.Left;
			case "right": return UITextAlignment.Right;
			default:
				Console.WriteLine ("Unknown alignment `{0}'. valid values are left, center, right", align);
				return UITextAlignment.Left;
			}
		}

		//
		// Creates one of the various StringElement classes, based on the
		// properties set.   It tries to load the most memory efficient one
		// StringElement, if not, it fallsback to MultilineStringElement or
		// StyledStringElement
		//
		static Element LoadString (JsonObject json, object data)
		{
			string value = null;
			string caption = value;
			string background = null;
			NSAction ontap = null;
			NSAction onaccessorytap = null;
			int? lines = null;
			UITableViewCellAccessory? accessory = null;
			UILineBreakMode? linebreakmode = null;
			UITextAlignment? alignment = null;
			UIColor textcolor = null, detailcolor = null;
			UIFont font = null;
			UIFont detailfont = null;
			UITableViewCellStyle style = UITableViewCellStyle.Value1;

			foreach (var kv in json) {
				string kvalue = (string) kv.Value;
				switch (kv.Key) {
				case "caption":
					caption = kvalue;
					break;
				case "value":
					value = kvalue;
					break;
				case "background":
					background = kvalue;
					break;
				case "style":
					style = ToCellStyle (kvalue);
					break;
				case "ontap":
				case "onaccessorytap":
					string sontap = kvalue;
					int p = sontap.LastIndexOf ('.');
					if (p == -1)
						break;
					NSAction d = delegate
					{
						string cname = sontap.Substring (0, p);
						string mname = sontap.Substring (p + 1);
						foreach (var a in AppDomain.CurrentDomain.GetAssemblies ()) {
							Type type = a.GetType (cname);

							if (type is not null) {
								var mi = type.GetMethod (mname, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
								if (mi is not null)
									mi.Invoke (null, new object [] { data });
								break;
							}
						}
					};
					if (kv.Key == "ontap")
						ontap = d;
					else
						onaccessorytap = d;
					break;
				case "lines":
					int res;
					if (int.TryParse (kvalue, out res))
						lines = res;
					break;
				case "accessory":
					accessory = ToAccessory (kvalue);
					break;
				case "textcolor":
					textcolor = ParseColor (kvalue);
					break;
				case "linebreak":
					linebreakmode = ToLinebreakMode (kvalue);
					break;
				case "font":
					font = ToFont (kvalue);
					break;
				case "subtitle":
					value = kvalue;
					style = UITableViewCellStyle.Subtitle;
					break;
				case "detailfont":
					detailfont = ToFont (kvalue);
					break;
				case "alignment":
					alignment = ToAlignment (kvalue);
					break;
				case "detailcolor":
					detailcolor = ParseColor (kvalue);
					break;
				case "type":
					break;
				default:
					Console.WriteLine ("Unknown attribute: '{0}'", kv.Key);
					break;
				}
			}
			if (caption is null)
				caption = "";
			if (font is not null || style != UITableViewCellStyle.Value1 || detailfont is not null || linebreakmode.HasValue || textcolor is not null || accessory.HasValue || onaccessorytap is not null || background is not null || detailcolor is not null) {
				StyledStringElement styled;

				if (lines.HasValue) {
					styled = new StyledMultilineElement (caption, value, style);
					styled.Lines = lines.Value;
				} else {
					styled = new StyledStringElement (caption, value, style);
				}
				if (ontap is not null)
					styled.Tapped += ontap;
				if (onaccessorytap is not null)
					styled.AccessoryTapped += onaccessorytap;
				if (font is not null)
					styled.Font = font;
				if (detailfont is not null)
					styled.SubtitleFont = detailfont;
				if (detailcolor is not null)
					styled.DetailColor = detailcolor;
				if (textcolor is not null)
					styled.TextColor = textcolor;
				if (accessory.HasValue)
					styled.Accessory = accessory.Value;
				if (linebreakmode.HasValue)
					styled.LineBreakMode = linebreakmode.Value;
				if (background is not null) {
					if (background.Length > 1 && background [0] == '#')
						styled.BackgroundColor = ParseColor (background);
					else
						styled.BackgroundUri = new Uri (background);
				}
				if (alignment.HasValue)
					styled.Alignment = alignment.Value;
				return styled;
			} else {
				StringElement se;
				if (lines == 0)
					se = new MultilineElement (caption, value);
				else
					se = new StringElement (caption, value);
				if (alignment.HasValue)
					se.Alignment = alignment.Value;
				if (ontap is not null)
					se.Tapped += ontap;
				return se;
			}
		}

		static Element LoadRadio (JsonObject json, object data)
		{
			var caption = GetString (json, "caption");
			var group = GetString (json, "group");

			if (group is not null)
				return new RadioElement (caption, group);
			else
				return new RadioElement (caption);
		}

		static Element LoadCheckbox (JsonObject json, object data)
		{
			var caption = GetString (json, "caption");
			var group = GetString (json, "group");
			var value = GetBoolean (json, "value");

			return new CheckboxElement (caption, value, group);
		}

		static DateTime GetDateWithKind (DateTime dt, DateTimeKind parsedKind)
		{
			// First we check if the given date has a specified Kind, we just return the same date if found.
			if (dt.Kind != DateTimeKind.Unspecified)
				return dt;

			// If not found then we check if we were able to parse a DateTimeKind from the parsedKind param
			else if (parsedKind != DateTimeKind.Unspecified)
				return DateTime.SpecifyKind (dt, parsedKind);

			// If no DateTimeKind from the parsedKind param was found then we check our global property from JsonElement.DateKind
			else if (JsonElement.DateKind != DateTimeKind.Unspecified)
				return DateTime.SpecifyKind (dt, JsonElement.DateKind);

			// If none of the above is found then we just defaut to local
			else
				return DateTime.SpecifyKind (dt, DateTimeKind.Local);
		}

		static Element LoadDateTime (JsonObject json, string type)
		{
			var caption = GetString (json, "caption");
			var date = GetString (json, "value");
			var kind = GetString (json, "kind");
			DateTime datetime;
			DateTimeKind dateKind;

			if (!DateTime.TryParse (date, out datetime))
				return null;

			if (kind is not null) {
				switch (kind.ToLowerInvariant ()) {
				case "local":
					dateKind = DateTimeKind.Local;
					break;
				case "utc":
					dateKind = DateTimeKind.Utc;
					break;
				default:
					dateKind = DateTimeKind.Unspecified;
					break;
				}
			} else
				dateKind = DateTimeKind.Unspecified;

			datetime = GetDateWithKind (datetime, dateKind);

			switch (type) {
			case "date":
				return new DateElement (caption, datetime);
			case "time":
				return new TimeElement (caption, datetime);
			case "datetime":
				return new DateTimeElement (caption, datetime);
			default:
				return null;
			}
		}

		static Element LoadHtmlElement (JsonObject json)
		{
			var caption = GetString (json, "caption");
			var url = GetString (json, "url");

			return new HtmlElement (caption, url);
		}

		void LoadSectionElements (Section section, JsonArray array, object data)
		{
			if (array is null)
				return;

			for (int i = 0; i < array.Count; i++) {
				Element element = null;

				try {
					var json = array [i] as JsonObject;
					if (json is null)
						continue;

					var type = GetString (json, "type");
					switch (type) {
					case "bool":
					case "boolean":
						element = LoadBoolean (json);
						break;

					case "entry":
					case "password":
						element = LoadEntry (json, type == "password");
						break;

					case "string":
						element = LoadString (json, data);
						break;

					case "root":
						element = FromJson (this, json, data);
						break;

					case "radio":
						element = LoadRadio (json, data);
						break;

					case "checkbox":
						element = LoadCheckbox (json, data);
						break;

					case "datetime":
					case "date":
					case "time":
						element = LoadDateTime (json, type);
						break;

					case "html":
						element = LoadHtmlElement (json);
						break;

					default:
						Error ("json element at {0} contain an unknown type `{1}', json {2}", i, type, json);
						break;
					}

					if (element is not null) {
						var id = GetString (json, "id");
						if (id is not null)
							AddMapping (id, element);
					}
				} catch (Exception e) {
					Console.WriteLine ("Error processing Json {0}, exception {1}", array, e);
				}
				if (element is not null)
					section.Add (element);
			}
		}
	}
}
#endif // !NET
