using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace jeu.Core.Classes
{
	class MainMenu(SpriteFont font, Action<MenuResult> callback) : IMenu
	{
		private readonly SpriteFont font = font;
		private readonly Action<MenuResult> callback = callback;
		private readonly string[] items = ["Jouer", "Selectionner niveau", "Quitter"];
		private int selected = 0;

		private Keys prevK;

		public void Draw(SpriteBatch sb)
		{
			var vp = sb.GraphicsDevice.Viewport;
			var centerX = vp.Width / 2f;
			var startY = vp.Height / 3f;
			for (int i = 0; i < items.Length; i++)
			{
				var text = items[i];
				var color = i == selected ? Color.Yellow : Color.White;
				var size = font.MeasureString(text);
				sb.DrawString(font, text, new Vector2(centerX - size.X / 2, startY + i * (size.Y + 12)), color);
			}
		}

		public void KeyPressed(Keys key)
		{
			if (key == Keys.Down && prevK != Keys.Down) selected = (selected + 1) % items.Length;
			if (key == Keys.Up && prevK != Keys.Up) selected = (selected - 1 + items.Length) % items.Length;
			if (key == Keys.Enter && prevK != Keys.Enter)
			{
				switch (selected)
				{
					case 0: callback(MenuResult.Play); break;
					case 1: callback(MenuResult.OpenLevelSelect); break;
					case 2: callback(MenuResult.Exit); break;
				}
			}
			prevK = key;
		}
	}
}