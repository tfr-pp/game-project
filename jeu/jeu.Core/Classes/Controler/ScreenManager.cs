using System;
using System.Drawing;
using Microsoft.Xna.Framework.Graphics;
using static Microsoft.Xna.Framework.Color;
using static Microsoft.Xna.Framework.Rectangle;

namespace jeu.Core.Classes;

public class ScreenManager
{
	private GameState currentState;
	private StartScreen startScreen;
	private  LevelMenuScreen levelMenuScreen;

	private Screen[] gameScreens;
	private int idCurScreen = 0;
/*
	private HighScoresScreen highScoresScreen;
	private LevelMenuScreen levelMenuScreen;
	private OptionsMenuScreen optionsMenuScreen;
*/	
	public ScreenManager(GameState state, StartScreen startScreen, LevelMenuScreen levelMenuScreen)
	{
		gameScreens = new Screen[Enum.GetNames(typeof(GameState)).Length];
		gameScreens[0] = this.startScreen = startScreen;
		gameScreens[1] = this.levelMenuScreen = levelMenuScreen;
		currentState = state;
		switch (currentState)
		{
			case GameState.MainMenu:
				idCurScreen = 0;
				break;
			case GameState.LevelSelect:
				idCurScreen = 1;
				break;
			case GameState.Options:
				idCurScreen = 2;
				break;
			case GameState.HighScores:
				idCurScreen = 3;
				break;
			default:
				throw new Exception("Unknown state: " + currentState);
		}
	}

	public void Update(float dt)
	{
	}

	public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
	{
		spriteBatch.Begin();
		Microsoft.Xna.Framework.Rectangle bgRect = new Microsoft.Xna.Framework.Rectangle(0, 0, graphicsDevice.PresentationParameters.BackBufferWidth,
			graphicsDevice.PresentationParameters.BackBufferHeight);
		Texture2D bgTexture = gameScreens[idCurScreen].getBgTexture();
		spriteBatch.Draw(bgTexture, bgRect, Microsoft.Xna.Framework.Color.White);
		spriteBatch.End();
	}
	
	public void setState(GameState state)
	{
		currentState = state;
	}

}