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

		/// <summary>
		/// Create the a TurretEnemy with the default score value
		/// </summary>
		public TurretEnemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(health, moveSpeed, texture, position, boundingBox)
		{
			gun = WeaponManager.Instance.TurretGun;
			gun.X = this.X + (BoundingBox.Width / 2);
			gun.Y = this.Y + (BoundingBox.Height / 2);
		}

		/// <summary>
		/// Create the a TurretEnemy with a manual score value
		/// </summary>
		public TurretEnemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox, int scoreValue) : base(health, moveSpeed, texture, position, boundingBox, scoreValue)
		{
			gun = WeaponManager.Instance.TurretGun;
			gun.X = this.X + (BoundingBox.Width / 2);
			gun.Y = this.Y + (BoundingBox.Height / 2);
		}

        public override void Update(GameTime gameTime)
        {
			//Check if the player has activated the turret
            if (!Awake)
            {
                if (Vector2.Distance(
                    Player.Instance.BoundingBox.Center.ToVector2(),
                    this.BoundingBox.Center.ToVector2()) <= 192)
                {
                    this.Awake = true;
                }
            }
			//Normal update
            else
            {
				Player p = Player.Instance;

				float num = RotateTowardsPoint(this.X, this.Y, p.X+(p.BoundingBox.Width/2), p.Y+(p.BoundingBox.Height/2), gun.Angle, 0.01f);

				gun.Angle = num;

				Shoot(Player.Instance);
            }

			//Update weapon
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

		//Shoot at the player
		public void Shoot(GameObject thingtoShootAt)
		{
			if (!gun.Fired)
			{
				//Vector2 diff = new Vector2 ( thingToShootAt.Position.X - Position.X, thingToShootAt.Position.Y - Position.Y);
				Matrix m = Matrix.CreateRotationZ(gun.Angle-((float)Math.PI / 4));
				Vector2 diff = Vector2.Transform(new Vector2(1, 1), m);
				diff.Normalize();
				MouseState unnecessary = Mouse.GetState();
				gun.Fire(diff, unnecessary, unnecessary);
			}
        }

		private float RotateTowardsPoint(float srcX, float srcY, float targetX, float targetY, float currRotation, float speed)
		{
			float destinationRotation = (float)(Math.Atan2(srcY - targetY, srcX - targetX) + Math.PI);

			if (Math.Abs((currRotation + 180 - destinationRotation) % 360 - 180) < speed)
				currRotation = destinationRotation;
			else
			{
				if (destinationRotation > currRotation)
				{
					if (currRotation < destinationRotation - Math.PI)
						currRotation -= speed;
					else
						currRotation += speed;
				}
				else if (destinationRotation < currRotation)
				{
					if (currRotation > destinationRotation + Math.PI)
						currRotation += speed;
					else
						currRotation -= speed;
				}
				if (currRotation > Math.PI * 2.0f) currRotation = 0;
				if (currRotation < 0) currRotation = (float)(Math.PI * 2.0f);
			}
			return currRotation;
		}
	}
}
