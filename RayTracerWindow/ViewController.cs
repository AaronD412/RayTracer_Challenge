using System;
using AppKit;
using Foundation;
using RayTracerLogic;
using System.Threading;
using CoreGraphics;
using System.Runtime.InteropServices;
using RayTracerConsole;

namespace RayTracerWindow
{
    /// <summary>
    /// View controller.
    /// </summary>
    public partial class ViewController : NSViewController
    {
        // Number of bytes per pixel.
        private const int BytesPerPixel = 3;

        // Number of bits per sample
        private const int BitsPerSample = 8;

        private Camera camera;
        private World world;
        private bool stillRendering = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RayTracerWindow.ViewController"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public ViewController(IntPtr handle) : base(handle)
        {
            TermPaperChapter07_10 scene = new TermPaperChapter07_10();

            world = scene.GetWorld();
            camera = scene.GetCamera();
        }

        /// <summary>
        /// Views which did appear.
        /// </summary>
        public override void ViewDidAppear()
        {
            base.ViewDidAppear();

            CGSize size = new CGSize(camera.Width, camera.Height);

            RayTracerView.Window.SetContentSize(size);
            RayTracerImageView.SetFrameSize(size);
            RayTracerImageView.SetFrameOrigin(new CGPoint(0, 0));

            Thread rayTracingThread = new Thread(() =>
            {
                camera.Render(world);
                stillRendering = false;
            });

            rayTracingThread.IsBackground = true;
            rayTracingThread.Priority = ThreadPriority.BelowNormal;
            rayTracingThread.Start();

            Thread renderingThread = new Thread(() =>
            {
                while (stillRendering)
                {
                    InvokeOnMainThread(() =>
                    {
                        Draw();
                    });

                    Thread.Sleep(20);
                }

                InvokeOnMainThread(() =>
                {
                    Draw();
                });
            });

            renderingThread.IsBackground = true;
            renderingThread.Start();
        }

        /// <summary>
        /// Gets or sets the represented object.
        /// </summary>
        /// <value>The represented object.</value>
        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        /// <summary>
        /// Draw this instance.
        /// </summary>
        private void Draw()
        {
            CGSize imageViewSize = RayTracerImageView.Frame.Size;
            NSImage image = new NSImage(new CGSize(imageViewSize.Width, imageViewSize.Height));

            using (NSBitmapImageRep bitmapImageRepresentation = new NSBitmapImageRep(
                IntPtr.Zero,
                camera.Width,
                camera.Height,
                BitsPerSample,
                BytesPerPixel,
                false,
                false,
                NSColorSpace.DeviceRGB,
                BytesPerPixel * camera.Width,
                BitsPerSample * BytesPerPixel))
            {
                IntPtr bitmapData = bitmapImageRepresentation.BitmapData;

                for (int y = 0; y < camera.Height; y++)
                {
                    for (int x = 0; x < camera.Width; x++)
                    {
                        Marshal.WriteByte(bitmapData, BytesPerPixel * (y * camera.Width + x) + 0, AdjustColorComponent(camera.Canvas[x, y].Red));
                        Marshal.WriteByte(bitmapData, BytesPerPixel * (y * camera.Width + x) + 1, AdjustColorComponent(camera.Canvas[x, y].Green));
                        Marshal.WriteByte(bitmapData, BytesPerPixel * (y * camera.Width + x) + 2, AdjustColorComponent(camera.Canvas[x, y].Blue));
                    }
                }

                image.AddRepresentation(bitmapImageRepresentation);
            }

            RayTracerImageView.Image = image;
        }

        // Adjusts the color component to range between 0 and 255 (incl.).
        private byte AdjustColorComponent(double colorComponent)
        {
            double adjustedColorComponent = colorComponent;

            if (colorComponent < 0)
            {
                adjustedColorComponent = 0;
            }
            else if (colorComponent > 1)
            {
                adjustedColorComponent = 1;
            }

            return (byte)System.Math.Round(adjustedColorComponent * 255);
        }
    }
}
