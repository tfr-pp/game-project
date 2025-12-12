using System;
using jeu.Core.Classes.Vue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes.Controller;

public class ScreenManager
{
	private readonly GameState currentState = GameState.MainMenu;
	private readonly LevelMenuScreen levelMenuScreen;
	private readonly Screen[] gameScreens;

	public ScreenManager(GameState state, StartScreen startScreen, LevelMenuScreen levelMenuScreen)
	{
		gameScreens = new Screen[Enum.GetNames<GameState>().Length];
		gameScreens[0] = startScreen;
		gameScreens[1] = this.levelMenuScreen = levelMenuScreen;
		currentState = state;
	}

	public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
	{
		graphicsDevice.Clear(Color.Black);
		PresentationParameters pp = graphicsDevice.PresentationParameters;
		Rectangle bgRect = new(0, 0, pp.BackBufferWidth, pp.BackBufferHeight);

		Texture2D bgTexture = gameScreens[currentState == GameState.MainMenu ? 0 : 1].getBgTexture();

		spriteBatch.Draw(bgTexture, bgRect, Color.White);

		if (currentState == GameState.LevelSelect)
		{
			levelMenuScreen.Draw(spriteBatch);
		}
	}
}