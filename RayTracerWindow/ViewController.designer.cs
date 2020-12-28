// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace RayTracerWindow
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        AppKit.NSImageView RayTracerImageView { get; set; }

        [Outlet]
        AppKit.NSView RayTracerView { get; set; }
        
        void ReleaseDesignerOutlets ()
        {
            if (RayTracerView != null) {
                RayTracerView.Dispose ();
                RayTracerView = null;
            }

            if (RayTracerImageView != null) {
                RayTracerImageView.Dispose ();
                RayTracerImageView = null;
            }
        }
    }
}
