/**
 * Oddmatics.RozWorld.FrontEnd.Gdi.GdiRenderTask -- RozWorld GDI+ Renderer Task
 *
 * This source-code is part of the GDI+ renderer for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-GDIRenderer>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.RozWorld.API.Client.Graphics;
using Oddmatics.RozWorld.API.Generic;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Oddmatics.RozWorld.FrontEnd.Gdi
{
    /// <summary>
    /// Represents a task that is operated by the GDI+ renderer.
    /// </summary>
    internal class GdiRenderTask : IRenderTask
    {
        /// <summary>
        /// Gets or sets the list of render parts in this task.
        /// </summary>
        public List<RenderPart> Parts { get; set; }

        /// <summary>
        /// Gets or sets the rotation amount (in radians) that will be applied to all render parts in this task.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the rotation origin that will be used when rotating all render parts in this task.
        /// </summary>
        public RwPoint RotationOrigin { get; set; }

        /// <summary>
        /// Gets or sets the tint effect that will be applied to all render parts in this task.
        /// </summary>
        public byte[] Tint { get; set; }


        /// <summary>
        /// Gets the resultant bitmap that was constructed by this task.
        /// </summary>
        public Bitmap ConstructedBitmap { get; private set; }

        /// <summary>
        /// Gets the location to draw the constructed bitmap.
        /// </summary>
        public Point Location { get; private set; }


        /// <summary>
        /// Constructs or reconstructs this task ready for the renderer to use.
        /// </summary>
        /// <returns>Success is the task was constructed.</returns>
        public RwResult ConstructNow()
        {
            Rectangle taskRect = GetTaskRectangle();
            var taskBmp = new Bitmap(taskRect.Width, taskRect.Height);

            using (Graphics gfx = Graphics.FromImage(taskBmp))
            {
                // TODO: Code this
            }

            Location = taskRect.Location;
            
            // Swap the bitmaps
            Bitmap oldBmp = ConstructedBitmap;

            ConstructedBitmap = taskBmp;
            oldBmp.Dispose();

            return RwResult.Success;
        }

        /// <summary>
        /// Releases all resources used by this GdiRenderTask.
        /// </summary>
        public void Dispose()
        {
            ConstructedBitmap.Dispose();
        }


        /// <summary>
        /// Measures the expected size of the bitmap this task will create when constructed.
        /// </summary>
        /// <returns>The size of the bitmap that this task will create when constructed.</returns>
        private Rectangle GetTaskRectangle()
        {
            int bottomMost = int.MinValue;
            int leftMost = int.MaxValue;
            int rightMost = int.MinValue;
            int topMost = int.MaxValue;

            foreach (RenderPart part in Parts)
            {
                foreach (RwPoint vertex in part.DrawVertices)
                {
                    if (vertex.Y > bottomMost) bottomMost = vertex.Y;
                    if (vertex.X < leftMost) leftMost = vertex.X;
                    if (vertex.X > rightMost) rightMost = vertex.X;
                    if (vertex.Y < topMost) topMost = vertex.Y;
                }
            }

            return new Rectangle(
                new Point(leftMost, topMost),
                new Size(rightMost - leftMost, bottomMost - topMost)
                );
        }
    }
}
