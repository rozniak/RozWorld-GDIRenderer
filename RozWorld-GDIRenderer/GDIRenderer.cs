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
using Oddmatics.RozWorld.API.Generic;
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
        /// Gets the render context of a window.
        /// </summary>
        /// <param name="window">The index of the window.</param>
        /// <returns>The IRendererContext used by the window if it was found, null otherwise.</returns>
        public override IRendererContext GetContext(byte window)
        {
            if (!Initialised)
                throw new InvalidOperationException("GdiRenderer.GetContext: The renderer has not been initialised.");

            if (window >= 0 && window < Windows.Count)
                return Windows[window].RenderContext;

            throw new ArgumentOutOfRangeException("GdiRenderer.GetContext: Parameter 'window' out of range. There are " +
                Windows.Count.ToString() + " windows open, and a window of index " + window.ToString() + " was specified.");
        }

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
        /// Loads a texture from the specified relative filepath and maps it to the given identifier.
        /// </summary>
        /// <param name="filepath">The relative filepath of the texture.</param>
        /// <param name="identifier">The texture identifier.</param>
        /// <returns>Success if the texture was loaded and mapped to the identifier.</returns>
        public override RwResult LoadTexture(string filepath, string identifier)
        {
            return GdiRendererContext.LoadSharedTexture(filepath, identifier);
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
            if (!Initialised)
                throw new InvalidOperationException("GdiRenderer.SetWindowSize: The renderer has not been initialised.");

            if (window >= 0 && window < Windows.Count)
                Windows[window].Size = new Size(width, height);
            else
                throw new ArgumentOutOfRangeException("GdiRenderer.SetWindowSize: Parameter 'window' out of range. There are " +
                    Windows.Count.ToString() + " windows open, and a window of index " + window.ToString() + " was specified.");
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
            if (!FormsThread.IsAlive)
                throw new InvalidOperationException("GdiRenderer.Stop: GDI+ renderer thread is not running.");

            if (Windows[0].InvokeRequired)
                Windows[0].Invoke(new MethodInvoker(Stop));
            else
            {
                Windows[0].NoClosePrompt = true;
                Windows[0].Close();
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
