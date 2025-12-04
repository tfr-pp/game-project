using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace jeu.Core.Classes.Vue;

public class LevelMenuScreen(SpriteFont font, List<string> levels, Action<string> onSelect, Action onBack) : Screen
{
	private readonly SpriteFont font = font;
	private readonly List<string> levels = levels ?? [];
	private readonly Action<string> onSelect = onSelect;
	private readonly Action onBack = onBack;
	private int selected = 0;

	private Keys prevK;

	public void Draw(SpriteBatch sb)
	{
		var vp = sb.GraphicsDevice.Viewport;
		var centerX = vp.Width / 2f;
		var startY = vp.Height / 6f;
		sb.DrawString(font, "Selectionner un niveau", new Vector2(centerX - font.MeasureString("Selectionner un niveau").X / 2, startY - 40), Color.White);
		for (int i = 0; i < levels.Count; i++)
		{
			var text = levels[i];
			var color = i == selected ? Color.Red : Color.Black;
			var size = font.MeasureString(text);
			sb.DrawString(font, text, new Vector2(centerX - size.X / 2, startY + i * (size.Y + 8)), color);
		}
	}

	public void KeyPressed(Keys key)
	{
		if (key == Keys.Down && prevK != Keys.Down) selected = Math.Min(selected + 1, levels.Count - 1);
		if (key == Keys.Up && prevK != Keys.Up) selected = Math.Max(selected - 1, 0);
		if (key == Keys.Escape && prevK != Keys.Escape) onBack?.Invoke();
		if (key == Keys.Enter && levels.Count > 0 && prevK != Keys.Enter) onSelect(levels[selected]);
		prevK = key;
	}
}