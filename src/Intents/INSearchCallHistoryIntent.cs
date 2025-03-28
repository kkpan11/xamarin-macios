//
// INSearchCallHistoryIntent.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#if !__MACOS__
#if !TVOS
using System;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace Intents {
	public partial class INSearchCallHistoryIntent {

		/// <summary>Gets a Boolean value that indicates whether to search for unseen calls.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? Unseen {
			get { return WeakUnseen?.BoolValue; }
		}
	}
}
#endif
#endif // __MACOS__
