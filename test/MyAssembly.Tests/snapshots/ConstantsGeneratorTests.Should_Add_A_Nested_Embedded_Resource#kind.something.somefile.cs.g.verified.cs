//HintName: Rocket.Surgery.MyAssembly/Rocket.Surgery.MyAssembly.ResourcesGenerator/kind.something.somefile.cs.g.cs
internal static partial class MyAssembly
{
    ///<summary>Provides access to embedded resources</summary>
    internal static partial class Resources
    {
        ///<summary>Provides access to embedded resources under kind</summary>
        internal static partial class kind
        {
            ///<summary>comment</summary>
            internal static System.IO.Stream somethingsomefilecs() => typeof(MyAssembly).Assembly.GetManifestResourceStream("something/somefile.cs");
        }
    }
}
