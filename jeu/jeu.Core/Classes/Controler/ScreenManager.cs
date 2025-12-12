using System;
using jeu.Core.Classes.Vue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes.Controller;

/** A manager for every skylink view/screen
 * 
 */
public class ScreenManager
{
	private readonly GameState currentState = GameState.MainMenu;
	private readonly LevelMenuScreen levelMenuScreen;
	private readonly Screen[] gameScreens;

	/** Constructor
	 * \param state a member of GameState enum, the current state of the game
	 * \param startScreen a StartScreen, the displayed screen when launching the game
	 * \param levelMenuScreen a LevelMenuScreen, the displayed screen when selecting a level
	 */
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