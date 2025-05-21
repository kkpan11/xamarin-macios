//
// Authors:
//   Miguel de Icaza (miguel@gnome.org)
//   Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2012-2014 Xamarin Inc
//
// The class can be either constructed from a string (from user code)
// or from a handle (from iphone-sharp.dll internal calls).  This
// delays the creation of the actual managed string until actually
// required
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#nullable enable

using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
#if IOS && !COREBUILD
using Contacts;
using Intents;
#endif

namespace CoreLocation {

	// CLLocationDegrees -> double -> CLLocation.h

	// CLLocation.h
	/// <summary>Geographical coordinates.</summary>
	///     <remarks>The geographical coordinates use the WGS 84 reference frame.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CLLocationCoordinate2D {
		/// <summary>Latitude in degrees. Positive values are north of the equator, negative values are south of the equator.</summary>
		///         <remarks>To be added.</remarks>
		public /* CLLocationDegrees */ double Latitude;
		/// <summary>Longitude in degrees.</summary>
		///         <remarks>The value is relative to the zero meridian. Positive values point east, negative values point west.</remarks>
		public /* CLLocationDegrees */ double Longitude;

		/// <param name="latitude">The latitude in degrees, where positive values are north of the equator.</param>
		///         <param name="longitude">The longitude in degrees relative to the zero meridian, where positive values are east of the meridian.</param>
		///         <summary>Constructor that allows the latitude and longitude to be specified.</summary>
		///         <remarks>To be added.</remarks>
		public CLLocationCoordinate2D (double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}

		[DllImport (Constants.CoreLocationLibrary)]
		static extern /* BOOL */ byte CLLocationCoordinate2DIsValid (CLLocationCoordinate2D cord);

		/// <summary>Whether the coordinate is valid.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>This method will return false if the latitude is greater than 90 or less than -90. It will also return false if longitude is greater than 180 or less than -180.</para>
		///         </remarks>
		public bool IsValid ()
		{
			return CLLocationCoordinate2DIsValid (this) != 0;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return $"(Latitude={Latitude}, Longitude={Longitude}";
		}
	}

#if IOS && !COREBUILD // This code comes from Intents.CLPlacemark_INIntentsAdditions Category
	/// <summary>Associates data such as street address with a coordinate.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreLocation/Reference/CLPlacemark_class/index.html">Apple documentation for <c>CLPlacemark</c></related>
	public partial class CLPlacemark {
		/// <param name="location">To be added.</param>
		///         <param name="name">To be added.</param>
		///         <param name="postalAddress">To be added.</param>
		///         <summary>Creates a new placemark from the given name, location, and postal address.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		static public CLPlacemark GetPlacemark (CLLocation location, string name, CNPostalAddress postalAddress)
		{
			return (null as CLPlacemark)!._GetPlacemark (location, name, postalAddress);
		}
	}
#endif
}
