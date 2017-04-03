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
    class Enemy : Entity
    {
		private bool awake;

		public bool Awake
		{
			get { return awake; }
			set { awake = value; }
		}

		public Enemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(health, moveSpeed, texture, position, boundingBox)
        {
			awake = false;
        }

        public override void Update(GameTime gameTime)
        {
			if (!awake)
			{
				if (Vector2.Distance(
					Player.Instance.BoundingBox.Center.ToVector2(),
					this.BoundingBox.Center.ToVector2()) <= 192)
				{
					this.awake = true;
				}
			}
			else
			{
				Move(Player.Instance);
			}
			base.Update(gameTime);
        }

		public override void Damage(int dmg)
		{
			awake = true;
			base.Damage(dmg);
		}
		public void Move(GameObject thingToMoveTo)
        {
            Vector2 diff = Position - thingToMoveTo.Position;
            if (diff.X > 0)
            {
                X--;
            }
            else  
            {
                X++;
            }

            if (diff.Y > 0)
            {
                Y--;
            }
            else
            {
                Y++;
            }
        }

    }
}
