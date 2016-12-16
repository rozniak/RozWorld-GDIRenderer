/**
 * Oddmatics.RozWorld.FrontEnd.GdiAppContext -- RozWorld GDI+ Application Context
 *
 * This source-code is part of the client program for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.RozWorld.API.Generic;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Oddmatics.RozWorld.FrontEnd.Gdi
{
    internal class GdiAppContext : ApplicationContext
    {
        public List<GdiViewportForm> Windows;
        private Timer DrawTimer;


        public GdiAppContext()
        {
            Windows = new List<GdiViewportForm>();
            Windows.Add(new GdiViewportForm(new System.Drawing.Size(
                RwCore.Client.DisplayResolutions[0].Width,
                RwCore.Client.DisplayResolutions[0].Height
                )));
            
            DrawTimer = new Timer();
            DrawTimer.Interval = 17; // Roughly 60FPS for now -- TODO: work out a better solution
        }


        public void Start()
        {
            DrawTimer.Enabled = true;
            DrawTimer.Start();

            Windows[0].Show();
        }

        public void Stop()
        {
            DrawTimer.Stop();
            DrawTimer.Dispose();

            foreach (var window in Windows)
            {
                window.Dispose(); // Ensure all forms are disposed
            }

            this.ExitThread();
        }
    }
}
