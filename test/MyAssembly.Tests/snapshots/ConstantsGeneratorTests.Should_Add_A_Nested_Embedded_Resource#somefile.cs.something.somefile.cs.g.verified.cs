//HintName: Rocket.Surgery.MyAssembly/Rocket.Surgery.MyAssembly.ResourcesGenerator/somefile.cs.something.somefile.cs.g.cs
internal static partial class MyAssembly
{
    ///<summary>Provides access to embedded resources</summary>
    internal static partial class Resources
    {
        ///<summary>Provides access to embedded resources under something</summary>
        internal static partial class something
        {
            ///<summary>comment</summary>
            internal static System.IO.Stream somefilecs() => typeof(MyAssembly).Assembly.GetManifestResourceStream("something.somefile.cs");
        }
    }
}