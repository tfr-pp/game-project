using System;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes.Vue;

public abstract class Screen
{
	public Texture2D bgTexture;

	public Texture2D getBgTexture()
	{
		if (bgTexture == null)
		{
			throw new NullReferenceException("Background Texture is null");
		}
		else
		{
			return bgTexture;
		}
	}
}