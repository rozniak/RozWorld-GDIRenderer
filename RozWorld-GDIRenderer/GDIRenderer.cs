/**
 * Oddmatics.RozWorld.FrontEnd.Gdi.GdiRenderer -- RozWorld GDI+ Renderer
 *
 * This source-code is part of the GDI+ renderer for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-GDIRenderer>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.RozWorld.API.Client.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Oddmatics.RozWorld.FrontEnd.Gdi
{
    /// <summary>
    /// Represents the GDI+ based renderer that will be loaded by the RozWorld client.
    /// </summary>
    public class GdiRenderer : Renderer
    {
        /// <summary>
        /// Gets the IRendererContext object instance used to interact with this renderer.
        /// </summary>
        public override IRendererContext Context { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets the value that indicates whether this renderer has been initialised.
        /// </summary>
        public override bool Initialised { get; protected set; }

        /// <summary>
        /// Gets the amount of windows active in this renderer.
        /// </summary>
        public override byte WindowCount
        {
            get { return (byte)Windows.Count; }
        }


        /// <summary>
        /// Occurs when the user closes this renderer's last window.
        /// </summary>
        public override event EventHandler Closed;


        /// <summary>
        /// The thread that hosts the Windows Forms used to display window graphics.
        /// </summary>
        private Thread FormsThread { get; set; }

        /// <summary>
        /// The currently available windows.
        /// </summary>
        private List<GdiViewportForm> Windows { get; set; }


        /// <summary>
        /// Initialises this renderer.
        /// </summary>
        /// <returns>True if the renderer was successfully initialised.</returns>
        public override bool Initialise()
        {
            if (Initialised)
                throw new InvalidOperationException("GdiRenderer.Initialise: The renderer is already initialised.");

            Windows = new List<GdiViewportForm>();
            Windows.Add(new GdiViewportForm(new Size(1366, 768))); // Resolution hard coded for testing purposes

            Windows[0].FormClosed += GdiRenderer_FormClosed;

            FormsThread = new Thread(() => Application.Run(Windows[0]));

            Initialised = true;

            return true;
        }

        /// <summary>
        /// Sets the amount of windows in this renderer.
        /// </summary>
        /// <param name="count">The amount of windows.</param>
        public override void SetWindows(byte count)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Sets the size of a window.
        /// </summary>
        /// <param name="window">The index of the window.</param>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        public override void SetWindowSize(byte window, short width, short height)
        {
            //if (window >= 0 && window < WindowCount)
            //    AppContext.Windows[window].Size = new Size(width, height);
        }

        /// <summary>
        /// Starts this renderer.
        /// </summary>
        [STAThread]
        public override void Start()
        {
            if (!Initialised)
                throw new InvalidOperationException("GdiRenderer.Start: This renderer has not been initialised yet.");

            FormsThread.Start();
        }

        /// <summary>
        /// Stops this renderer.
        /// </summary>
        public override void Stop()
        {
            if (FormsThread.IsAlive)
            {
                if (Windows[0].InvokeRequired)
                    Windows[0].Invoke(new MethodInvoker(Stop));
                else
                {
                    Windows[0].NoClosePrompt = true;
                    Windows[0].Close();
                }
            }

            Initialised = false;
        }


        /// <summary>
        /// [Event] Last form was closed.
        /// </summary>
        private void GdiRenderer_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}
