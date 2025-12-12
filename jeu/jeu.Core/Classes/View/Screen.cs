using System;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes.View;

/** A Screen abstract class for every game view
 *
 */
public abstract class Screen
{
	protected Texture2D bgTexture;

	/** Gets the texture for view background
	 * \return Texture2D the background texture
	 * \throws NullReferenceException if bgTexture is null
	 */
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

	public void setBgTexture(Texture2D texture)
	{
		bgTexture = texture;
	}
}