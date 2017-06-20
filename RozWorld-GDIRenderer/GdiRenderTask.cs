/**
 * Oddmatics.RozWorld.FrontEnd.Gdi.GdiRenderTask -- RozWorld GDI+ Renderer Task
 *
 * This source-code is part of the GDI+ renderer for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.RozWorld.API.Client.Graphics;
using Oddmatics.RozWorld.API.Generic;
using System;
using System.Collections.Generic;

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
        /// Constructs or reconstructs this task ready for the renderer to use.
        /// </summary>
        /// <returns>Success is the task was constructed.</returns>
        public RwResult ConstructNow()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Releases all resources used by this GdiRenderTask.
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
