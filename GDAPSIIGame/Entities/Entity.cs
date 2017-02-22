using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GDAPSIIGame.Interface;

namespace GDAPSIIGame.Entities
{
	/// <summary>
	/// The direction the entity is facing
	/// </summary>
	public enum Entity_Dir { Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight }

    public class Entity : GameObject
    {
        private int health;
        private int moveSpeed;
		private Entity_Dir dir;

        public Entity(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(texture, position, boundingBox)
        {
            this.health = health;
            this.moveSpeed = moveSpeed;
			dir = Entity_Dir.Down;
        }

		/// <summary>
		/// How much health the entity has
		/// </summary>
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

		/// <summary>
		/// How fast the entity is moving
		/// </summary>
        public int MoveSpeed
        {
            get { return moveSpeed; }
        }

		public override void OnCollision(Rectangle bb, CollisionType ct)
		{
			switch (ct)
			{
				case CollisionType.Wall:
					//top left &...
					if (bb.Contains(BoundingBox.Left, BoundingBox.Top))
					{
						// top right
						if (bb.Contains(BoundingBox.Right, BoundingBox.Top))
						{
							this.Y += 5;
						}
						else // bottom left
						if (bb.Contains(BoundingBox.Left, BoundingBox.Bottom))
						{
							this.X += 5;
						}
						else // nothing
						{
							this.X += 5;
							this.Y += 5;
						}
					}
					else //bottom right &...
					if (bb.Contains(BoundingBox.Right, BoundingBox.Bottom))
					{
						// top right
						if (bb.Contains(BoundingBox.Right, BoundingBox.Top))
						{
							this.X -= 5;
						}
						else // bottom left
						if (bb.Contains(BoundingBox.Left, BoundingBox.Bottom))
						{
							this.Y -= 5;
						}
						else // nothing
						{
							this.X -= 5;
							this.Y -= 5;
						}
					}
					else // bottom left
					if (bb.Contains(BoundingBox.Left, BoundingBox.Bottom))
					{
						this.X += 5;
						this.Y -= 5;
					}
					else // top right
					{
						this.X -= 5;
						this.Y += 5;
					}
					break;
			}
		}

		/// <summary>
		/// The direction the entity is facing
		/// </summary>
		public Entity_Dir Dir
		{
			get { return dir; }
			set { dir = value; }
		}
		

	}
}
