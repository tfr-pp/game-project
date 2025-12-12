using System.Numerics;
using System.Xml.Serialization;

namespace jeu.Core.Classes.Model;

/** A point of coordinates X, Y, convertible to Vector2D
 * 
 */
public class Point //pk ????
{
	[XmlAttribute]
	public float X { get; set; }

	[XmlAttribute]
	public float Y { get; set; }

	public Vector2 ToVector2()
	{
		return new Vector2(X, Y);
	}
}