using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDAPSIIGame.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDAPSIIGame.Entities
{
    abstract class Enemy : Entity
    {
        //Fields
        private bool awake;
        private bool hit;
        private int scoreValue;

        /// <summary>
        /// If the enemy is activated by the player
        /// </summary>
        public bool Awake
        {
            get { return awake; }
            set { awake = value; }
        }

        public int Score
        {
            get
            {
                if (hit)
                {
                    hit = false;
                    return scoreValue;
                }
                return 0;
            }
        }

		internal bool Hit
		{
			get { return hit; }
			set { hit = value; }
		}

		public override void Damage(int dmg)
		{
			Awake = true;
			Hit = true;
			Player.Instance.updateMultiplier(this);
			base.Damage(dmg);
		}

		public Enemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(health, moveSpeed, texture, position, boundingBox)
        {
            awake = false;
            scoreValue = 5;
        }
		public Enemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox, int scoreValue) : base(health, moveSpeed, texture, position, boundingBox)
		{
			awake = false;
			this.scoreValue = scoreValue;
		}

		public override void OnCollision(ICollidable obj)
		{
			if (obj is Enemy)
			{
				Rectangle bb = obj.BoundingBox;

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
					if (bb.Center.Y > BoundingBox.Bottom)
					{
						//point.Y = BoundingBox.Bottom;
						point.Y = this.Y + BoundingBox.Height;
					}
					else if (bb.Center.Y < BoundingBox.Top)
					{
						//point.Y = BoundingBox.Top;
						point.Y = this.Y;
					}
					else
					{
						point.Y = bb.Center.Y;
					}
					if (bb.Center.X > BoundingBox.Right)
					{
						//point.X = BoundingBox.Right;
						point.X = this.X + BoundingBox.Width;
					}
					else if (bb.Center.X < BoundingBox.Left)
					{
						//point.X = BoundingBox.Left;
						point.X = this.X;
					}
					else
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
				if (distLeft < distRight &&
					distLeft < distTop &&
					distLeft < distBottom)
				{
					this.X -= distLeft + 1;
				}
				else if (distRight < distTop &&
				   distRight < distBottom)
				{
					this.X += distRight + 1;
				}
				else if (distTop < distBottom)
				{
					this.Y -= distTop + 1;
				}
				else
				{
					this.Y += distBottom + 1;
				}

				this.ResetBound();
			}
			else {
				base.OnCollision(obj);
			}
        }
    }
}
