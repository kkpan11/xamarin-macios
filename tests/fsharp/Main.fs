namespace fsharp

open UIKit

#if !TODAY_EXTENSION

module Main = 
    [<EntryPoint>]
    let main args = 
        UIApplication.Main(args, null, typedefof<AppDelegate>)
        0

#endif
