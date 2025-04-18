// 
// UIDocumentBrowserViewController.cs
//
// Copyright 2018 Microsoft 
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

#if IOS

using System;
using Foundation;
using ObjCRuntime;

namespace UIKit {

	public partial class UIDocumentBrowserViewController {

		static bool CheckSystemVersion ()
		{
#if IOS
			return SystemVersion.CheckiOS (12, 0);
#else
#error Unknown platform
#endif
		}

		/// <param name="documentUrl">The document URL for which to get a transition controller.</param>
		///         <summary>Creates and returns a transition controller for the document at the specified URL.</summary>
		///         <returns>Developers should only specify values for <paramref name="documentUrl" /> that were obtained from the document browser.</returns>
		///         <remarks>To be added.</remarks>
		public virtual UIDocumentBrowserTransitionController GetTransitionController (NSUrl documentUrl)
		{
			if (CheckSystemVersion ())
				return _NewGetTransitionController (documentUrl);
			return _DeprecatedGetTransitionController (documentUrl);
		}
	}
}

#endif
