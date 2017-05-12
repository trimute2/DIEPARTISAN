using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GDAPSIIGame.Interface;
using GDAPSIIGame.Audio;

namespace GDAPSIIGame.Entities
{
	/// <summary>
	/// The direction the entity is facing
	/// </summary>
	public enum Entity_Dir { UpEast, UpWest, UpLeft, Left, DownLeft, DownWest, DownEast, DownRight, Right, UpRight }
	public enum Entity_State { Idling, Moving}

    public class Entity : GameObject
    {
        private int health;
        private int moveSpeed;
		private Entity_Dir dir;
		private Vector2 currentMove;
		protected Vector2 previousPosition;
		protected Vector2 knockBack;
		protected float knockBackTime;
		protected bool knockBackable;

        public Entity(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(texture, position, boundingBox)
        {
            this.health = health;
            this.moveSpeed = moveSpeed;
			dir = Entity_Dir.DownEast;
			knockBackTime = 0.0f;
			knockBack = Vector2.Zero;
			knockBackable = true;
			previousPosition = position;
			currentMove = Vector2.Zero;
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

		public virtual void Damage(int dmg)
		{
			this.health -= dmg;
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

				if (Math.Abs(currentMove.X) > bb.Width / 2)
				{
					if(currentMove.X > 0)
					{
						distRight = float.MaxValue;
					}else
					{
						distLeft = float.MaxValue;
					}
				}
				if (Math.Abs(currentMove.Y) > bb.Height / 2)
				{
					if (currentMove.Y > 0)
					{
						distBottom = float.MaxValue;
					}
					else
					{
						distTop = float.MaxValue;
					}
				}
				//offset the player by the shortest distance
				if (distLeft < distRight &&
					distLeft < distTop &&
					(distLeft < distBottom ||
					(distLeft == distBottom && ((Wall)obj).Bellow)))
				{
					this.X -= distLeft+1;
					if(this is Player && ((Player)this).CurrWeapon != null)
					{
						((Player)this).CurrWeapon.X -= distLeft + 1;
					}
				}else if(distRight < distTop &&
					(distRight < distBottom ||
					(distRight == distBottom && ((Wall)obj).Bellow)))
				{
					this.X += distRight+1;
					if (this is Player && ((Player)this).CurrWeapon != null)
					{
						((Player)this).CurrWeapon.X += distRight + 1;
					}
				}
				else if(distTop < distBottom)
				{
					this.Y -= distTop+1;
					if (this is Player && ((Player)this).CurrWeapon != null)
					{
						((Player)this).CurrWeapon.Y -= distTop+1;
					}
				}
				else
				{
					this.Y += distBottom+1;
					if (this is Player && ((Player)this).CurrWeapon != null)
					{
						((Player)this).CurrWeapon.Y += distBottom+1;
					}
				}

				currentMove = Position - previousPosition;

				this.ResetBound();

            }
			else if(obj is Projectile)
			{
				if (!(this is Player) && ((obj as Projectile).Owner == Owners.Player) && (obj as GameObject).IsActive)
				{
					this.Damage(((Projectile)obj).Damage);
					AudioManager.Instance.GetSoundEffect("Hurt").Play();
					if ((obj as Projectile).Knockback != 0)
					{
						knockBack = (obj as Projectile).Direction * (obj as Projectile).Knockback;
						knockBackTime = 0.2f;
					}
				}
			}

		}

		public override void Update(GameTime gameTime)
		{
			if(health <= 0)
			{
				active = false;
			}
			if(knockBackTime > 0 && knockBackable)
			{
				this.Position += knockBack * knockBackTime;
				knockBackTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			currentMove = Position - previousPosition;
			base.Update(gameTime);
		}

	}
}
