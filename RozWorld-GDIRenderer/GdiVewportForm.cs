/**
 * Oddmatics.RozWorld.FrontEnd.Gdi.GdiViewportForm -- RozWorld GDI+ Viewport Form
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
using System.Drawing;
using System.Windows.Forms;

namespace Oddmatics.RozWorld.FrontEnd.Gdi
{
    /// <summary>
    /// Represents a single Windows Form for the GDI+ viewport.
    /// </summary>
    internal class GdiViewportForm : Form
    {
        /// <summary>
        /// Gets or sets whether to prompt the user when closing this window.
        /// </summary>
        public bool NoClosePrompt { get; set; }

        /// <summary>
        /// Gets the render context of this window.
        /// </summary>
        public GdiRendererContext RenderContext { get; private set; }


        /// <summary>
        /// The index of the active backbuffer bitmap.
        /// </summary>
        private byte ActiveBuffer;

        /// <summary>
        /// The buffer bitmaps.
        /// </summary>
        private Bitmap[] Buffers;

        /// <summary>
        /// The graphics contexts for the buffers.
        /// </summary>
        private Graphics[] Contexts;

        /// <summary>
        /// The draw timer for updating the graphics.
        /// </summary>
        private Timer DrawTimer;

        /// <summary>
        /// The current graphics context.
        /// </summary>
        private Graphics CurrentContext { get { return Contexts[ActiveBuffer]; } }

        /// <summary>
        /// The PictureBox control that represents the viewport.
        /// </summary>
        public PictureBox ViewportPictureBox
        {
            get { return _ViewportPictureBox; }
        }
        private PictureBox _ViewportPictureBox;


        //TEST CODE!//
        private float[] VertexData;
        private Size InitialSize;
        // // // // //


        /// <summary>
        /// Initializes a new instance of the GdiViewportForm class.
        /// </summary>
        /// <param name="size">The size of the viewport.</param>
        public GdiViewportForm(Size size)
        {
            // Set up form
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = size;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = RwCore.Client.ClientWindowTitle;

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

        /// <summary>
        /// Swaps the graphics buffers.
        /// </summary>
        private void SwapBuffers()
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
            for (int tri = 0; tri < VertexData.Length; tri += 6)
            {
                float[] floatBuffer = new float[3 * 2]; // 3 points of 2 dimensions
                Point[] realPoints = new Point[3];

                Array.Copy(VertexData, tri, floatBuffer, 0, 6);

                for (int point = 0; point < 3; point++)
                {
                    realPoints[point] = new Point(
                        (int)(floatBuffer[point * 2] * InitialSize.Width),
                        (int)(floatBuffer[point * 2 + 1] * InitialSize.Height)
                        );
                }

                CurrentContext.DrawPolygon(Pens.Blue, realPoints);
            }

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
