/**
 * Oddmatics.RozWorld.FrontEnd.Gdi.GdiRendererContext -- RozWorld GDI+ Renderer Context
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
using System.IO;

namespace Oddmatics.RozWorld.FrontEnd.Gdi
{
    /// <summary>
    /// Represents the renderer context implementation for GDI+ windows.
    /// </summary>
    internal class GdiRendererContext : IRendererContext
    {
        /// <summary>
        /// Gets the shared textures bank.
        /// </summary>
        public static Dictionary<int, Bitmap> SharedTextures { get; private set; }


        /// <summary>
        /// The list of IDs that were used but have now been made available.
        /// </summary>
        private static List<int> FreeIds { get; set; }

        /// <summary>
        /// The mappings between texture identifiers and their loaded IDs.
        /// </summary>
        private static Dictionary<string, int> TextureIdentifierMappings { get; set; }


        /// <summary>
        /// Gets the size of the client area of the parent window.
        /// </summary>
        public RwSize ClientSize
        {
            get { return new RwSize(Parent.ClientSize.Width, Parent.ClientSize.Height); }
        }

        
        /// <summary>
        /// The parent window.
        /// </summary>
        public GdiViewportForm Parent
        {
            get { return _Parent; }
            set
            {
                if (_Parent != null)
                    throw new MethodAccessException("GdiRendererContext.Parent.set: Illegal set attempt - " +
                        "not allowed to set parent outside of initial construction.");

                _Parent = value;
            }
        }
        private GdiViewportForm _Parent;


        /// <summary>
        /// Creates a font texture.
        /// </summary>
        /// <param name="fontFilename">The filename of the font file to use.</param>
        /// <param name="text">The text string.</param>
        /// <param name="pt">The font size in points (pt).</param>
        /// <param name="colour">The colour of the text.</param>
        /// <returns>The texture ID of the created font texture, -1 if the creation failed.</returns>
        public int CreateFontTexture(string fontFilename, string text, byte pt, Colour colour)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a task for the renderer.
        /// </summary>
        /// <returns>The IRenderTask object this method creates.</returns>
        public IRenderTask CreateTask()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a texture.
        /// </summary>
        /// <param name="id">The ID of the texture to delete.</param>
        /// <returns>Success if the texture was deleted.</returns>
        public RwResult DeleteTexture(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads a texture from the specified relative filepath and maps it to the given identifier.
        /// </summary>
        /// <param name="filepath">The relative filepath of the texture.</param>
        /// <param name="identifier">The texture identifier.</param>
        /// <returns>Success if the texture was loaded and mapped to the identifier.</returns>
        public static RwResult LoadSharedTexture(string filepath, string identifier)
        {
            if (SharedTextures == null)
                SharedTextures = new Dictionary<int, Bitmap>();

            string lowIdentifier = identifier.ToString();

            if (TextureIdentifierMappings.ContainsKey(lowIdentifier))
                return RwResult.DuplicateAssetIdentifier;
            
            if (!File.Exists(filepath))
                return RwResult.FileNotFound;

            Bitmap loadedTexture = null;

            try
            {
                loadedTexture = (Bitmap)Bitmap.FromFile(filepath);
            }
            catch (OutOfMemoryException memEx)
            {
                return RwResult.InvalidFileFormat;
            }

            int id = 0;

            if (FreeIds.Count > 0)
            {
                id = FreeIds[0];
                FreeIds.RemoveAt(0);
            }
            else
                id = SharedTextures.Count;

            TextureIdentifierMappings.Add(lowIdentifier, id);
            SharedTextures.Add(id, loadedTexture);

            return RwResult.Success;
        }
    }
}
