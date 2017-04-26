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
    class MeleeEnemy : Enemy
    {

		public MeleeEnemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(health, moveSpeed, texture, position, boundingBox)
		{ }

		public MeleeEnemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox, int scoreValue) : base(health, moveSpeed, texture, position, boundingBox)
		{ }

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
				Move(Player.Instance);
			}
			base.Update(gameTime);
        }

		public void Move(GameObject thingToMoveTo)
        {
			if (!(knockBackTime > 0))
			{
				Vector2 diff = Position - thingToMoveTo.Position;
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
