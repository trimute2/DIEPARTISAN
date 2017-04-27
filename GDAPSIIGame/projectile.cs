using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDAPSIIGame.Interface;
using GDAPSIIGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDAPSIIGame
{
	public enum Owners
	{
		Player, Enemy, None
	}
    class Projectile : GameObject
    {
        //Fields
        private Vector2 direction;
        private int damage;
		private Owners owner;
		private float angle;
		private float distance;

        //Properties
        public Vector2 Direction { get { return direction; } set { direction = value; } }
        public int Damage { get { return damage; } set { damage = value; } }
		public float Angle { get { return angle; } set { angle = value; } }

		public Owners Owner { get { return owner; } }

        //Constructor
        /// <summary>
        /// A movable projectile object
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public Projectile(Texture2D texture, Vector2 position, Rectangle boundingBox, Vector2 direction, int damage, float angle, Owners owner = Owners.None) : base(texture, position, boundingBox)
        {
			this.angle = angle;
            this.direction = direction;
            this.damage = damage;
			this.owner = owner;
			distance = -10;
        }

		public Projectile(Texture2D texture, Vector2 position, Rectangle boundingBox, Vector2 direction, int damage, float angle, float distance, Owners owner = Owners.None) : base(texture, position, boundingBox)
		{
			this.angle = angle;
			this.direction = direction;
			this.damage = damage;
			this.owner = owner;
			this.distance = distance;
		}

		public override void Update(GameTime gameTime)
        {
            //Update the projectile's position based on the vector's values
            //Multiply it by the elapsed game time since last update in milliseconds
            this.X += direction.X * gameTime.ElapsedGameTime.Milliseconds;
            this.Y += direction.Y * gameTime.ElapsedGameTime.Milliseconds;

			if(distance != -10)
			{
				if(distance > 0)
				{
					distance -= this.Position.Length() * gameTime.ElapsedGameTime.Milliseconds;
				}
				else
				{
					active = false;
				}
			}
			base.Update(gameTime);
        }

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.Texture,
				Map.Camera.Instance.GetViewportPosition(this),
				null,
				null,
				null,
				angle,
				this.Scale,
				Color.White,
				SpriteEffects.None);
		}

		public Projectile Clone(Vector2 position, Vector2 direction, Owners owner, float angle) {
            Projectile p = new Projectile(this.Texture, position, this.BoundingBox, direction, this.damage, angle, owner);
            ProjectileManager.Instance.Add(p);
            return p;
        }

		public override void OnCollision(ICollidable obj)
		{
			if (!(obj is Player && owner == Owners.Player) && !(obj is Enemy && owner == Owners.Enemy))
			{
				active = false;
			}
		}

	}
}
