using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes;

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
	public virtual void Draw(SpriteBatch spriteBatch, Texture2D ennemySprite)
	{
		var origin = new Vector2(ennemySprite.Width / 2f, ennemySprite.Height / 2f);

		var scaleX = _halfSize.X * 2f / ennemySprite.Width;
		var scaleY = _halfSize.Y * 2f / ennemySprite.Height;
		var scale = new Vector2(scaleX, scaleY);
		
		spriteBatch.Draw(ennemySprite,
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
