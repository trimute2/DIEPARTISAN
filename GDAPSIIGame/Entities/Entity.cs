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

		/// <summary>
		/// The direction the entity is facing
		/// </summary>
		public Entity_Dir Dir
		{
			get { return dir; }
			set { dir = value; }
		}

		public override void OnCollision(ICollidable obj)
		{
			Rectangle bb = obj.BoundingBox;
            
            if (obj is Wall)
			{
				ResetBound();
				//the clossest point on (or in) the player's bounding box to the wall's center 
                Vector2 point = Vector2.Zero;

				// if the players bounding box contains the wall's center set point to the walls center
				if (BoundingBox.Contains(bb.Center))
				{
					point = bb.Center.ToVector2();
				}
				else
				{
					//find the clossest point on the player's bounding box to the wall's center
					if(bb.Center.Y > BoundingBox.Bottom)
					{
						//point.Y = BoundingBox.Bottom;
						point.Y = this.Y + BoundingBox.Height;
					}else if(bb.Center.Y < BoundingBox.Top)
					{
						//point.Y = BoundingBox.Top;
						point.Y = this.Y;
					}else
					{
						point.Y = bb.Center.Y;
					}
					if(bb.Center.X > BoundingBox.Right)
					{
						//point.X = BoundingBox.Right;
						point.X = this.X + BoundingBox.Width;
					}else if(bb.Center.X < BoundingBox.Left)
					{
						//point.X = BoundingBox.Left;
						point.X = this.X;
					}else
					{
						point.X = bb.Center.X;
					}
				}

                //get the distance between each side of the wall and the point 
                float distLeft = point.X - bb.Left;
                float distRight = bb.Right - point.X;
                float distTop = point.Y - bb.Top;
                float distBottom = bb.Bottom - point.Y;


				//offset the player by the shortest distance
				if(distLeft < distRight &&
					distLeft < distTop &&
					distLeft < distBottom)
				{
					this.X -= distLeft+1;
					if(this is Player && ((Player)this).Weapon != null)
					{
						((Player)this).Weapon.X -= distLeft + 1;
					}
				}else if(distRight < distTop &&
					distRight < distBottom)
				{
					this.X += distRight+1;
					if (this is Player && ((Player)this).Weapon != null)
					{
						((Player)this).Weapon.X += distRight + 1;
					}
				}
				else if(distTop < distBottom)
				{
					this.Y -= distTop;
					if (this is Player && ((Player)this).Weapon != null)
					{
						((Player)this).Weapon.Y -= distTop;
					}
				}
				else
				{
					this.Y += distBottom;
					if (this is Player && ((Player)this).Weapon != null)
					{
						((Player)this).Weapon.Y += distBottom;
					}
				}

				this.ResetBound();

            }else if(obj is Projectile)
			{
				this.health -= (int)((Projectile)obj).Damage;
			}

		}

		public override void Update(GameTime gameTime)
		{
			if(health <= 0)
			{
				active = false;
			}
			base.Update(gameTime);
		}

	}
}
