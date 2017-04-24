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

        //Properties
        public Vector2 Direction { get { return direction; } set { direction = value; } }
        public int Damage { get { return damage; } set { damage = value; } }

		public Owners Owner { get { return owner; } }

        //Constructor
        /// <summary>
        /// A movable projectile object
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public Projectile(Texture2D texture, Vector2 position, Rectangle boundingBox, Vector2 direction, int damage, Owners owner = Owners.None) : base(texture, position, boundingBox)
        {
            this.direction = direction;
            this.damage = damage;
			this.owner = owner;
        }

        public override void Update(GameTime gameTime)
        {
            //Update the projectile's position based on the vector's values
            //Multiply it by the elapsed game time since last update in milliseconds
            this.X += direction.X * gameTime.ElapsedGameTime.Milliseconds;
            this.Y += direction.Y * gameTime.ElapsedGameTime.Milliseconds;
			base.Update(gameTime);
        }

        public Projectile Clone(Vector2 position, Vector2 direction, Owners owner) {
            Projectile p = new Projectile(this.Texture, position, this.BoundingBox, direction, this.damage, owner);
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
