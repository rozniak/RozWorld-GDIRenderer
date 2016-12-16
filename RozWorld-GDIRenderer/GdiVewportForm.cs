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
        private Graphics GfxContext;
        private PictureBox _ViewportPictureBox;
        public PictureBox ViewportPictureBox { get { return _ViewportPictureBox; } }


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
            ActiveBuffer = 1;
            GfxContext = Graphics.FromImage(Buffers[1]);

            // Add main viewport image
            _ViewportPictureBox = new PictureBox();
            
            _ViewportPictureBox.Dock = DockStyle.Fill;
            _ViewportPictureBox.Image = Buffers[0];

            this.Controls.Add(_ViewportPictureBox);
        }


        public void SwapBuffers()
        {
            // Set image to current buffer, then swap the active buffer to the unused one
            lock (Buffers[ActiveBuffer])
            {
                _ViewportPictureBox.Image = Buffers[ActiveBuffer];
                ActiveBuffer = ActiveBuffer == 0 ? (byte)1 : (byte)0;
                GfxContext = Graphics.FromImage(Buffers[ActiveBuffer]);
            }
        }
    }
}
