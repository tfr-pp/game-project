using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes.Controller;

public class ScreenManager
{
	private GameState currentState;
	private StartScreen startScreen;
	private LevelMenuScreen levelMenuScreen;

	private Screen[] gameScreens;
	private int idCurScreen = 0;
	/*
		private HighScoresScreen highScoresScreen;
		private LevelMenuScreen levelMenuScreen;
		private OptionsMenuScreen optionsMenuScreen;
	*/
	public ScreenManager(GameState state, StartScreen startScreen, LevelMenuScreen levelMenuScreen)
	{
		gameScreens = new Screen[Enum.GetNames<GameState>().Length];
		gameScreens[0] = this.startScreen = startScreen;
		gameScreens[1] = this.levelMenuScreen = levelMenuScreen;
		currentState = state;
		idCurScreen = currentState switch
		{
			GameState.MainMenu => 0,
			GameState.LevelSelect => 1,
			GameState.Options => 2,
			GameState.HighScores => 3,
			_ => throw new Exception("Unknown state: " + currentState),
		};
	}

	public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
	{
		var pp = graphicsDevice.PresentationParameters;
		Rectangle bgRect = new(0, 0, pp.BackBufferWidth, pp.BackBufferHeight);

		Texture2D bgTexture = gameScreens[idCurScreen].getBgTexture();

		spriteBatch.Draw(bgTexture, bgRect, Color.White);
	}

}