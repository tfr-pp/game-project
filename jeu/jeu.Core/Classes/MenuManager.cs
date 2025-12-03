using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace jeu.Core.Classes
{
	public enum MenuResult { None, Play, OpenLevelSelect, Exit, LevelSelected }

	public class MenuManager
	{
		private SpriteFont font;
		private SpriteBatch spriteBatch;
		public IMenu activeMenu { get; private set; }

		public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
		{
			this.spriteBatch = spriteBatch;
			font = content.Load<SpriteFont>("Default");
		}

		public void ShowMainMenu(Action<MenuResult> onResult)
		{
			activeMenu = new MainMenu(font, onResult);
		}

		public void ShowLevelSelect(IList<string> levelNames, Action<string> onLevelSelected, Action onBack = null)
		{
			activeMenu = new LevelSelectMenu(font, levelNames, onLevelSelected, onBack);
		}

		public void Draw()
		{
			if (activeMenu == null) return;
			activeMenu.Draw(spriteBatch);
		}

		public bool IsActive => activeMenu != null;
		public void Close() => activeMenu = null;
	}

	public interface IMenu
	{
		void Draw(SpriteBatch sb);

		public void KeyPressed(Keys key);
	}
}