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
        private byte ActiveBuffer;
        private Bitmap[] Buffers;
        private Graphics[] Contexts;
        private Timer DrawTimer;
        private Graphics CurrentContext { get { return Contexts[ActiveBuffer]; } }
        private PictureBox _ViewportPictureBox;
        public PictureBox ViewportPictureBox { get { return _ViewportPictureBox; } }

        
        //TEST CODE!//
        private float[] VertexData;
        private System.Drawing.Size InitialSize;
        // // // // //


        public GdiViewportForm(System.Drawing.Size size)
        {
            // Set up form
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = size;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = RwCore.Client.ClientWindowTitle; // TODO: Change this to use a language string

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


            // Test code // // // //
            const int tileWidth = 64;
            const int tileHeight = 64;
            const int tilemapWidth = 32;
            const int tilemapHeight = 32;

            InitialSize = size;
            VertexData = new float[tilemapWidth * tilemapHeight * 6 * 2];

            for (int y = 0; y < tilemapHeight; y++)
            {
                for (int x = 0; x < tilemapWidth; x++)
                {
                    float xCoordLeft = (tileWidth * x) / (float)InitialSize.Width;
                    float xCoordRight = (tileWidth * (x + 1)) / (float)InitialSize.Width;
                    float yCoordTop = (tileHeight * y) / (float)InitialSize.Height;
                    float yCoordBottom = (tileHeight * (y + 1)) / (float)InitialSize.Height;

                    int baseIndex = (y * tilemapWidth) + (x * 12);

                    float[] quad = new float[] {
                        xCoordLeft, yCoordTop,
                        xCoordLeft, yCoordBottom,
                        xCoordRight, yCoordBottom,

                        xCoordRight, yCoordBottom,
                        xCoordLeft, yCoordTop,
                        xCoordRight, yCoordTop
                    };

                    // Copy data between arrays
                    Array.Copy(quad, 0, VertexData, baseIndex, 12);
                }
            }
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
