using System;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes.Vue;

public class StartScreen : Screen
{
	public void LoadContent(Texture2D bgTexture2D)
	{
		bgTexture = bgTexture2D;
	}

	public void selectOpt(JeuGame game)
	{
		game.setState(GameState.LevelSelect);
	}
}