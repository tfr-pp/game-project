using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes.Model;

public abstract class Enemy(Vector2 startPosition, float speed)
{
	private readonly Vector2 _halfSize = new(40f, 20f);
	public Vector2 Position { get; protected set; } = startPosition;
	public Rectangle hitBox => new(
		(int)Position.X - 20,
		(int)Position.Y - 20,
		40,
		20
	);

	public float Speed { get; protected set; } = speed;

	public abstract void Update(float dt);
	public virtual void Draw(SpriteBatch spriteBatch, Texture2D enemySprite)
	{
		Vector2 origin = new(enemySprite.Width / 2f, enemySprite.Height / 2f);

		float scaleX = _halfSize.X * 2f / enemySprite.Width;
		float scaleY = _halfSize.Y * 2f / enemySprite.Height;
		Vector2 scale = new(scaleX, scaleY);

		spriteBatch.Draw(enemySprite,
			position: Position,
			sourceRectangle: null,
			rotation: 0f,
			color: Color.White,
			origin: origin,
			scale: scale,
			effects: SpriteEffects.None,
			layerDepth: 0f);
	}
}
