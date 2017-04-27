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
using GDAPSIIGame.Map;

namespace GDAPSIIGame.Entities
{
    class TurretEnemy : Enemy
    {
		private TurretGun gun;
		private Vector2 origin;
		private Vector2 drawPos;
		private bool fired;

		/// <summary>
		/// Create the a TurretEnemy with the default score value
		/// </summary>
		public TurretEnemy(Texture2D texture, Vector2 position, Rectangle boundingBox, int health = 10, int moveSpeed = 0) : base(health, moveSpeed, texture, position, boundingBox)
		{
			origin = new Vector2(texture.Width/2, texture.Height/2);
			drawPos = new Vector2(this.X + (BoundingBox.Width / 2), this.Y + (BoundingBox.Height / 2));
			gun = WeaponManager.Instance.TurretGun;
			gun.X = this.X + (BoundingBox.Width / 2);
			gun.Y = this.Y + (BoundingBox.Height / 2);
			knockBackable = false;
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

				float newAngle = RotateTowardsPoint(this.X, this.Y, p.X, p.Y, gun.Angle, 0.02f);
				gun.Angle = newAngle;

				float destinationRotation = (float)(Math.Atan2(Y - p.Y, X - p.X ) + Math.PI);
				//Shoot when only in a certain distance of player
				if (destinationRotation < newAngle + (Math.PI / 6) && destinationRotation > newAngle - (Math.PI / 6))
				{
					Shoot(Player.Instance);
				}
            }

			//Update weapon
			gun.Update(gameTime);
            base.Update(gameTime);
        }

		public override void Draw(SpriteBatch spriteBatch)
		{
			//gun.Draw(spriteBatch);
			spriteBatch.Draw(this.Texture,
				Camera.Instance.GetViewportPosition(drawPos),
				null,
				null,
				origin,
				gun.Angle,
				this.Scale,
				Color.White);
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
				diff = diff / 6;
				MouseState unnecessary = Mouse.GetState();
				fired = gun.Fire(diff, unnecessary, unnecessary);
			}
        }

		/// <summary>
		/// Does the math to rotate towards another target at a given speed
		/// </summary>
		/// <returns>The new angle</returns>
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
