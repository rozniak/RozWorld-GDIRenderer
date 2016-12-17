/**
 * Oddmatics.RozWorld.FrontEnd.GdiViewportForm -- RozWorld GDI+ Viewport Form
 *
 * This source-code is part of the client program for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Oddmatics.RozWorld.FrontEnd.Gdi
{
    /// <summary>
    /// Represents a single Windows Form for the GDI+ viewport.
    /// </summary>
    internal class GdiViewportForm : Form
    {
        private byte ActiveBuffer;
        private Bitmap[] Buffers;
        private Graphics[] Contexts;
        private Timer DrawTimer;
        private Graphics CurrentContext { get { return Contexts[ActiveBuffer]; } }
        private PictureBox _ViewportPictureBox;
        public PictureBox ViewportPictureBox { get { return _ViewportPictureBox; } }


        // TEST CODE ONLY //
        private Random rand = new Random();
        // TEST CODE ONLY //


        public GdiViewportForm(Size size)
        {
            // Set up form
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = size;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "RozWorld GDI+ Prototype"; // TODO: Change this to use a language string

            // Create graphics buffers
            Buffers = new Bitmap[] { new Bitmap(size.Width, size.Height),
                new Bitmap(size.Width, size.Height) };
            Contexts = new Graphics[] { Graphics.FromImage(Buffers[0]),
                Graphics.FromImage(Buffers[1]) };
            ActiveBuffer = 1;

            // Add main viewport image
            _ViewportPictureBox = new PictureBox();
            
            _ViewportPictureBox.Dock = DockStyle.Fill;
            _ViewportPictureBox.Image = Buffers[0];

            this.Controls.Add(_ViewportPictureBox);

            // Create DrawTimer
            DrawTimer = new Timer();
            DrawTimer.Interval = 17; // Roughly 60FPS for now -- TODO: work out a better solution
            DrawTimer.Tick += new EventHandler(DrawTimer_Tick);

            // Add form events
            this.Shown += new EventHandler(GdiViewportForm_Shown);
            // TODO: Add handling closing here!
        }


        public void SwapBuffers()
        {
            // Set image to current buffer, then swap the active buffer to the unused one
            _ViewportPictureBox.Image = Buffers[ActiveBuffer];
            ActiveBuffer = ActiveBuffer == 0 ? (byte)1 : (byte)0;
        }


        /// <summary>
        /// [Event] DrawTimer interval elapsed.
        /// </summary>
        /// <remarks>
        /// This is the main draw function of windows used in this renderer - all drawing logic
        /// should go in this method!
        /// </remarks>
        private void DrawTimer_Tick(object sender, System.EventArgs e)
        {
            CurrentContext.Clear(Color.Black);

            // This bit is for testing purposes to make sure drawing happens correctly.
            CurrentContext.DrawRectangle(Pens.Red, rand.Next(0, 100), rand.Next(0, 100),
                64, 64);

            SwapBuffers();
        }

        /// <summary>
        /// [Event] Form initially shown.
        /// </summary>
        private void GdiViewportForm_Shown(object sender, EventArgs e)
        {
            DrawTimer.Enabled = true;
            DrawTimer.Start();
        }
    }
}
