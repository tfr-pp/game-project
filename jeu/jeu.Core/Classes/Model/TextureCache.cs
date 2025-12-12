using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes.Model;

public static class TextureCache
{
	public static Texture2D Pixel;

	public static void Initialize(GraphicsDevice gd)
	{
		Pixel = new Texture2D(gd, 1, 1);
		Pixel.SetData([Color.White]);
	}
}
