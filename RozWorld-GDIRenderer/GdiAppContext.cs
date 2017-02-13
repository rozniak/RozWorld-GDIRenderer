/**
 * Oddmatics.RozWorld.FrontEnd.Gdi.GdiAppContext -- RozWorld GDI+ Application Context
 *
 * This source-code is part of the GDI+ renderer for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.RozWorld.API.Generic;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Oddmatics.RozWorld.FrontEnd.Gdi
{
    internal class GdiAppContext : ApplicationContext
    {
        private GdiRenderer Parent;
        private Timer ThreadCheckTimer;
        public List<GdiViewportForm> Windows;


        public GdiAppContext(ref GdiAppContext parentContextLink, GdiRenderer parent)
        {
            Windows = new List<GdiViewportForm>();
            Windows.Add(new GdiViewportForm(new System.Drawing.Size(
                RwCore.Client.DisplayResolutions[0].Width,
                RwCore.Client.DisplayResolutions[0].Height
                )));

            //////////////////////////////////////////////////////////////////////////////////
            // NOTE: This may be crap code and there is possibly a better way of doing this //
            // the thread check timer is for checking the status of whether this renderer   //
            // should continue running (ie. when to close this thread safely) as well as    //
            // manage when windows should open and close as calls to this thread from       //
            // the main thread obviously does not work correctly.                           //
            //////////////////////////////////////////////////////////////////////////////////
            ThreadCheckTimer = new Timer();
            ThreadCheckTimer.Interval = 1000; // TODO: Evaluate a good interval for this
            ThreadCheckTimer.Tick += new EventHandler(ThreadCheckTimer_Tick);

            Parent = parent;

            // Put the reference in place last to avoid race conditions with the creation of the
            // above objects
            parentContextLink = this;

            Run();
        }


        public void Run()
        {
            while (!Parent.RunRenderer) { } // Wait until renderer should start

            Windows[0].Show();

            ThreadCheckTimer.Enabled = true;
            ThreadCheckTimer.Start();
        }


        /// <summary>
        /// [Event] ThreadCheckTimer interval elapsed.
        /// </summary>
        private void ThreadCheckTimer_Tick(object sender, EventArgs e)
        {
            if (!Parent.RunRenderer) // Check if the renderer is to be closed
            {
                ThreadCheckTimer.Stop();
                ThreadCheckTimer.Dispose();

                foreach (var window in Windows)
                {
                    window.Close();
                    window.Dispose();
                }

                this.ExitThread();
                return;
            }

            // TODO: Add check for whether window counts should change
        }
    }
}
