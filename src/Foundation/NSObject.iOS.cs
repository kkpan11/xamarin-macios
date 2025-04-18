
#if !MONOMAC

using System;
using System.Reflection;

namespace Foundation {
	/// <include file="../../docs/api/Foundation/NSObject.xml" path="/Documentation/Docs[@DocId='T:Foundation.NSObject']/*" />
	public partial class NSObject {
#if !COREBUILD

#if !NET
		[Obsolete ("Use 'PlatformAssembly' for easier code sharing across platforms.")]
		public readonly static Assembly MonoTouchAssembly = typeof (NSObject).Assembly;
#endif
#endif // !COREBUILD
	}
}

#endif // !MONOMAC
