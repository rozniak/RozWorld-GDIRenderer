/**
 * Oddmatics.RozWorld.FrontEnd.GdiRenderer -- RozWorld GDI+ Renderer
 *
 * This source-code is part of the client program for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.RozWorld.API.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Oddmatics.RozWorld.FrontEnd.Gdi
{
    /// <summary>
    /// Represents the GDI+ based renderer that will be loaded by the RozWorld client.
    /// </summary>
    public class GdiRenderer : Renderer
    {
        public override bool Initialised { get; protected set; }
        public override byte WindowCount
        {
            get
            {
                if (AppContext != null)
                    return (byte)AppContext.Windows.Count;

                return 0;
            }
        }


        private GdiAppContext AppContext;
        public bool RunRenderer { get; private set; }
        private Thread GdiThread;


        public override bool Initialise()
        {
            if (Initialised)
                throw new InvalidOperationException("GdiRenderer.Initialise: The renderer is already initialised.");

            GdiThread = new Thread(() => {
                GdiAppContext appContext = new GdiAppContext(ref AppContext, this);
                Application.Run(appContext);
            });

            GdiThread.Start();

            Initialised = true;

            return true;
        }

        public override void SetWindowSize(byte window, short width, short height)
        {
            if (window >= 0 && window < WindowCount)
                AppContext.Windows[window].Size = new Size(width, height);
        }

        public override void SetWindows(byte count)
        {
            throw new System.NotImplementedException();
        }

        public override void Start()
        {
            if (RunRenderer || !Initialised)
                throw new InvalidOperationException("GdiRenderer.Start: Invalid state to start this renderer.");

            while (AppContext == null) { } // Wait until the link is created

            RunRenderer = true; // Set this so now the renderer should run
        }

        public override void Stop()
        {
            if (!RunRenderer)
                throw new InvalidOperationException("GdiRenderer.Stop: This renderer has not been started.");

            RunRenderer = false;
            AppContext.Dispose();

            Initialised = false;
        }
    }
}
