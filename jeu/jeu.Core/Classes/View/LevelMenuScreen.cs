using System;
using System.Collections.Generic;
using jeu.Core.Classes.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace jeu.Core.Classes.View;

/** A LevelMenuScreen class for selecting levels view
 *
 */
public class LevelMenuScreen(SpriteFont font, List<string> levelsNames, List<string> levelsIds, Action<string> onSelect, Action onBack) : Screen
{
	private readonly SpriteFont font = font;
	private readonly List<string> levelsNames = levelsNames ?? [];
	private readonly List<string> levelsIds = levelsIds ?? [];
	private readonly Action<string> onSelect = onSelect;
	private readonly Action onBack = onBack;
	private int selected = 0;
	private Keys prevK;

	public void Draw(SpriteBatch sb)
	{
		Viewport vp = sb.GraphicsDevice.Viewport;
		float centerX = vp.Width * 0.5f;
		float startY = vp.Height * 0.30f;

		sb.Draw(
			TextureCache.Pixel,
			new Rectangle((int)(vp.Width * 0.25f), (int)(vp.Height * 0.15f),
						  (int)(vp.Width * 0.5f), (int)(vp.Height * 0.7f)),
			Color.Black * 0.4f
		);

		string title = "Selectionner un niveau";
		sb.DrawString(font, title,
			new Vector2(centerX - font.MeasureString(title).X / 2, startY - 80),
			Color.White);

		for (int i = 0; i < levelsNames.Count; i++)
		{
			string text = levelsNames[i];
			bool isSelected = i == selected;
			float scale = isSelected ? 1.2f : 1f;
			Color color = isSelected ? Color.Yellow : Color.White;

			Vector2 size = font.MeasureString(text) * scale;

			sb.DrawString(font, text,
				new Vector2(centerX - size.X / 2 + 2, startY + i * 40 + 2),
				Color.Black * 0.6f, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

			sb.DrawString(font, text,
				new Vector2(centerX - size.X / 2, startY + i * 40),
				color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

			if (isSelected)
			{
				sb.DrawString(font, ">",
					new Vector2(centerX - size.X / 2 - 40, startY + i * 40),
					Color.Yellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
	}

	/** Changes menu selection up or down (doesn't wrap)
	 * \param key the key pressed
	 */
	public void KeyPressed(Keys key)
	{
		if (key == Keys.Down && prevK != Keys.Down)
			selected = (selected + 1) % levelsNames.Count;

		if (key == Keys.Up && prevK != Keys.Up)
			selected = (selected - 1 + levelsNames.Count) % levelsNames.Count;

		if (key == Keys.Escape && prevK != Keys.Escape)
			onBack?.Invoke();

		if (key == Keys.Enter && prevK != Keys.Enter && levelsNames.Count > 0)
			onSelect?.Invoke(levelsIds[selected]);

		prevK = key;
	}
}
