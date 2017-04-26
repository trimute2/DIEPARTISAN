using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDAPSIIGame.Entities
{
	class DashEnemy : Enemy
	{

		private float dashTime;
		private bool dashing;
		private float dashSpeed;

		public float DashTime
		{
			get { return dashTime; }
		}

		public bool Dashing
		{
			get { return dashing; }
		}

		public DashEnemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(health, moveSpeed, texture, position, boundingBox)
		{
			dashTime = 1.5f;
			dashing = false;
			dashSpeed = 4f;
		}

		public DashEnemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox, int scoreValue) : base(health, moveSpeed, texture, position, boundingBox)
		{
			dashTime = 1.5f;
			dashing = false;
			dashSpeed = 4f;
		}

		public override void Update(GameTime gameTime)
		{
			if (!Awake)
			{
				if (Vector2.Distance(
					Player.Instance.BoundingBox.Center.ToVector2(),
					this.BoundingBox.Center.ToVector2()) <= 192)
				{
					this.Awake = true;
				}
			}
			else
			{
				if (!(knockBackTime > 0))
				{
					dashTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				}
				if(dashTime <= 0)
				{
					if (dashing)
					{
						dashTime = 3.0f;
						dashing = false;
					}else
					{
						dashTime = 0.25f;
						dashing = true;
					}
				}
				Move(Player.Instance);
			}
			base.Update(gameTime);
		}

		public void Move(GameObject thingToMoveTo)
		{
			if (!(knockBackTime > 0))
			{
				Vector2 diff = Position - thingToMoveTo.Position;
				if (dashing)
				{
					if (MoveSpeed * dashSpeed > Math.Abs(diff.X))
					{
						X = thingToMoveTo.Position.X;
					}
					else
					{
						if (diff.X > 0)
						{
							X -= MoveSpeed * dashSpeed;
						}
						else
						{
							X += MoveSpeed * dashSpeed;
						}
					}
					if (MoveSpeed * dashSpeed > Math.Abs(diff.Y))
					{
						Y = thingToMoveTo.Position.Y;
					}
					else
					{
						if (diff.Y > 0)
						{
							Y -= MoveSpeed * dashSpeed;
						}
						else
						{
							Y += MoveSpeed * dashSpeed;
						}
					}
				}
				else
				{
					if (MoveSpeed > Math.Abs(diff.X))
					{
						X = thingToMoveTo.X;
					}
					else
					{
						if (diff.X > 0)
						{
							X -= MoveSpeed;
						}
						else
						{
							X += MoveSpeed;
						}
					}
					if (MoveSpeed > Math.Abs(diff.Y))
					{
						Y = thingToMoveTo.Y;
					}
					else
					{
						if (diff.Y > 0)
						{
							Y -= MoveSpeed;
						}
						else
						{
							Y += MoveSpeed;
						}
					}
				}
			}
		}

	}
}
