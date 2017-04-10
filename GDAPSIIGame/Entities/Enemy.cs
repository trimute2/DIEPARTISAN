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
		private bool hit;
		private int scoreValue;

		public bool Awake
		{
			get { return awake; }
			set { awake = value; }
		}

		public int score
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
			hit = true;
			Player.Instance.updateMultiplier(this);
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
