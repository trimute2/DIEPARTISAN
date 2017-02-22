using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GDAPSIIGame.Interface
{
	public enum CollisionType
	{
		Player, Enemy, Projectile, Wall
	}

	/// <summary>
	/// handles how the object should react to the collision
	/// </summary>
	/// <param name="bb">the bounding box that the entitiy is colliding with</param>
	/// <param name="ct">the collision type</param>
	public interface ICollidable
	{
		Rectangle BoundingBox { get; }

		void OnCollision(Rectangle bb, CollisionType ct);

		bool Collide(ICollidable obj);

		bool Collide(Rectangle box);
	}
}
