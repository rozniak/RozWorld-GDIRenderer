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
        public override byte WindowCount { get { return (byte)AppContext.Windows.Count; } }


        private GdiAppContext AppContext;
        private bool HasStarted;


        public override bool Initialise()
        {
            if (Initialised)
                throw new InvalidOperationException("GdiRenderer.Initialise: The renderer is already initialised.");

            AppContext = new GdiAppContext();
            new Thread(() => Application.Run(AppContext)).Start();

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
            if (HasStarted || !Initialised)
                throw new InvalidOperationException("GdiRenderer.Start: Invalid state to start this renderer.");

            AppContext.Start();
            HasStarted = true;
        }

        public override void Stop()
        {
            if (!HasStarted)
                throw new InvalidOperationException("GdiRenderer.Stop: This renderer has not been started.");

            AppContext.Stop();
            AppContext.Dispose();

            Initialised = false;
            HasStarted = false;
        }
    }
}
