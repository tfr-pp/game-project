using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes.Vue;

/** A StartScreen class for title view
 * 
 */
public class StartScreen : Screen
{
	public void LoadContent(Texture2D bgTexture2D)
	{
		bgTexture = bgTexture2D;
	}

	/** Goes from title to the select levels menu
	 * \param game a JeuGame the SkyLink Main
	 */
	public void selectOpt(JeuGame game)
	{
		game.setState(GameState.LevelSelect);
	}
}