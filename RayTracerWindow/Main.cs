using AppKit;

namespace RayTracerWindow
{
    /// <summary>
    /// Main class for the ray tracer window.
    /// </summary>
    static class MainClass
    {
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        static void Main(string[] args)
        {
            NSApplication.Init();
            NSApplication.Main(args);
        }
    }
}
