using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GDAPSIIGame.ICollidabe
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
	interface ICollidable
	{
		Rectangle BoundingBox { get; }

		void CollisionReaction(Rectangle bb, CollisionType ct);
	}
}
