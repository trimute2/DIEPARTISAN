using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GDAPSIIGame.Interface;
using GDAPSIIGame.Weapons;
using Microsoft.Xna.Framework.Input;

namespace GDAPSIIGame.Entities
{
    class TurretEnemy : Enemy
    {
		TurretGun gun;

		public TurretEnemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(health, moveSpeed, texture, position, boundingBox)
		{
			gun = WeaponManager.Instance.TurretGun;
		}

        public TurretEnemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox, int scoreValue) : base(health, moveSpeed, texture, position, boundingBox, scoreValue)
		{
			gun = WeaponManager.Instance.TurretGun;
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
				gun.Angle = -((float)Math.Atan2(Player.Instance.X - this.X, Player.Instance.Y - this.Y));
				Shoot(Player.Instance);
            }
			//Update weapon position
			gun.X = this.X + (BoundingBox.Width / 2);
			gun.Y = this.Y + (BoundingBox.Height / 2);
			gun.Update(gameTime);
            base.Update(gameTime);
        }

		public override void Draw(SpriteBatch spriteBatch)
		{
			gun.Draw(spriteBatch);
			base.Draw(spriteBatch);
		}

		public override void Damage(int dmg)
        {
            Awake = true;
            Hit = true;
            Player.Instance.updateMultiplier(this);
            base.Damage(dmg);
        }

		public void Shoot(GameObject thingToShootAt)
		{
			if (!gun.Fired)
			{
				Vector2 diff = new Vector2 ( thingToShootAt.Position.X - Position.X, thingToShootAt.Position.Y - Position.Y);
				diff.Normalize();
				MouseState unnecessary = Mouse.GetState();
				gun.Fire(diff, unnecessary, unnecessary);
			}
        }
    }
}
