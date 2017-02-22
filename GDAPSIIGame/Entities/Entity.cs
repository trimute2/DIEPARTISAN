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
    public class Entity : GameObject
    {
        private int health;
        private int moveSpeed;
        public Entity(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(texture, position, boundingBox)
        {
            this.health = health;
            this.moveSpeed = moveSpeed;
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }
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


	}
}
