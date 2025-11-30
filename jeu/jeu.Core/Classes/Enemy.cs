using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes;

public abstract class Enemy(Vector2 startPosition, float speed)
{
	public Vector2 Position { get; protected set; } = startPosition;
	public Rectangle hitBox => new(
		(int)Position.X - 20,
		(int)Position.Y - 20,
		40,
		40
	);

	public float Speed { get; protected set; } = speed;

	public abstract void Update(GameTime gameTime);
	public virtual void Draw(SpriteBatch spriteBatch, Texture2D pixel)
	{
		spriteBatch.Draw(pixel,
			position: Position,
			sourceRectangle: null,
			rotation: 0f,
			color: Color.Red,
			origin: new Vector2(0.5f, 0.5f),
			scale: new Vector2(40, 40),
			effects: SpriteEffects.None,
			layerDepth: 0f);
	}
}
