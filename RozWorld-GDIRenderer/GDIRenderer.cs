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
        public override byte WindowCount { get { return (byte)Windows.Count; } }


        private List<GdiViewportForm> Windows;


        public override void Draw()
        {
            foreach (var window in Windows)
            {
                window.Draw();
            }
        }


        [STAThread]
        public override void Initialise()
        {
            if (Initialised)
                throw new InvalidOperationException("GdiRenderer.Initialise: The renderer is already initialised.");

            // TODO: Replace 800x600 res with resolution info - no magic numbers!!
            Windows = new List<GdiViewportForm>();
            Windows.Add(new GdiViewportForm(new Size(800, 600)));

            //Windows[0].ShowDialog(); // THIS DOES NOT WORK PROPERLY - TODO!

            new Thread(() => Application.Run(Windows[0])).Start();

            Initialised = true;
        }

        public override void SetWindowSize(byte window, short width, short height)
        {
            if (window >= 0 && window < WindowCount)
            {
                // Set window size 'ere
                Windows[window].Size = new Size(width, height);
            }
        }

        public override void SetWindows(byte count)
        {
            throw new System.NotImplementedException();
        }
    }
}
