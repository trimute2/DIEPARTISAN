using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GDAPSIIGame.Interface;
using GDAPSIIGame.Weapons;

namespace GDAPSIIGame.Entities
{
    class TurretEnemy : Enemy
    {
		TurretGun gun;

		public TurretEnemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(health, moveSpeed, texture, position, boundingBox)
		{ }

        public TurretEnemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox, int scoreValue) : base(health, moveSpeed, texture, position, boundingBox, scoreValue)
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
                Shoot(Player.Instance);
            }
            base.Update(gameTime);
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
            Vector2 diff = Position - thingToShootAt.Position;
            
        }
    }
}
