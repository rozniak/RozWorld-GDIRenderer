/**
 * Oddmatics.RozWorld.FrontEnd.GDIRenderer -- RozWorld GDI+ Renderer
 *
 * This source-code is part of the client program for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.RozWorld.API.Client;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Oddmatics.RozWorld.FrontEnd
{
    /// <summary>
    /// Represents the GDI+ based renderer that will be loaded by the RozWorld client.
    /// </summary>
    public class GDIRenderer : Renderer
    {
        public override bool Initialised { get; protected set; }
        public override byte WindowCount
        {
            get { return (byte)Windows.Count; }
        }


        private byte ActiveBuffer;
        private Bitmap[] Buffers;
        private Graphics GfxContext;
        private PictureBox Viewport;
        private List<Form> Windows;


        public override void Draw()
        {
            // TODO: Do drawing here!
            GfxContext.Clear(Color.Black);

            SwapBuffers();
        }

        public override void Initialise()
        {
            // TODO: Replace 800x600 res with resolution info - no magic numbers!!

            // Create form
            var initialForm = new Form();

            initialForm.FormBorderStyle = FormBorderStyle.None;
            initialForm.Size = new Size(800, 600);
            initialForm.FormBorderStyle = FormBorderStyle.FixedSingle; // This is so that the viewport stays 800x600
            initialForm.MaximizeBox = false;
            initialForm.Text = "RozWorld GDI+ Prototype";

            // Create graphics buffers
            Buffers = new Bitmap[] { new Bitmap(800, 600), new Bitmap(800, 600) };
            ActiveBuffer = 1;
            GfxContext = Graphics.FromImage(Buffers[1]);

            // Create PictureBox (temporary - need to move to allow for multiple windows to work)
            Viewport = new PictureBox();

            Viewport.Dock = DockStyle.Fill;
            Viewport.Image = Buffers[0];

            initialForm.Controls.Add(Viewport);
            initialForm.ShowDialog(); // THIS DOES NOT WORK PROPERLY - TODO!
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


        private void SwapBuffers()
        {
            // Set image to current buffer, then swap the active buffer to the unused one
            Viewport.Image = Buffers[ActiveBuffer];
            ActiveBuffer = ActiveBuffer == 0 ? (byte)1 : (byte)0;
            Graphics.FromImage(Buffers[ActiveBuffer]);
        }
    }
}
