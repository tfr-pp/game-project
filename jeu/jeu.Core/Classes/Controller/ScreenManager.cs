using System;
using jeu.Core.Classes.Vue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes.Controller;

public class ScreenManager
{
	private GameState currentState;
	private LevelMenuScreen levelMenuScreen;

	private Screen[] gameScreens;
	private int idCurScreen = 0;
	public ScreenManager(GameState state, StartScreen startScreen, LevelMenuScreen levelMenuScreen)
	{
		gameScreens = new Screen[Enum.GetNames<GameState>().Length];
		gameScreens[0] = startScreen;
		gameScreens[1] = this.levelMenuScreen = levelMenuScreen;
		currentState = state;
		idCurScreen = currentState switch
		{
			GameState.MainMenu => 0,
			GameState.LevelSelect => 1,
			_ => throw new Exception("Unknown state: " + currentState),
		};
	}

	public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
	{
		graphicsDevice.Clear(Color.Black);
		var pp = graphicsDevice.PresentationParameters;
		Rectangle bgRect = new(0, 0, pp.BackBufferWidth, pp.BackBufferHeight);

		Texture2D bgTexture = gameScreens[idCurScreen].getBgTexture();

		spriteBatch.Draw(bgTexture, bgRect, Color.White);

		if (currentState == GameState.LevelSelect)
		{
			levelMenuScreen.Draw(spriteBatch);
		}
	}

}