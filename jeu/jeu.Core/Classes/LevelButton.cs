using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace jeu.Core.Classes;

// Pour l'affichage des niveaux dans le menu
// Pas testé
public class LevelButton(int levelId, Rectangle bounds, bool unlocked)
{
	public Rectangle Bounds = bounds;
	public int LevelId = levelId;
	public bool Unlocked = unlocked;
	public string Text = $"Niveau {levelId}";

	public void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D pixel)
	{
		Color color = Unlocked ? Color.Green : Color.Gray;
		spriteBatch.Draw(pixel, Bounds, color);
		spriteBatch.DrawString(font, Text, new Vector2(Bounds.X + 5, Bounds.Y + 5), Color.White);
	}

	public bool IsClicked(MouseState mouse)
	{
		return Unlocked && Bounds.Contains(mouse.Position);
	}
}