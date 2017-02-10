using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDAPSIIGame
{
    class Projectile : GameObject
    {
        //Fields
        private Vector2 direction;
        private float damage;

        //Properties
        public Vector2 Direction { get { return direction; } set { direction = value; } }
        public float Damage { get { return damage; } set { damage = value; } }

        //Constructor
        /// <summary>
        /// A movable projectile object
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public Projectile(Texture2D texture, Vector2 position, Rectangle boundingBox, Vector2 direction, float damage) : base(texture, position, boundingBox)
        {
            this.direction = direction;
            this.damage = damage;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Update the projectile's position based on the vector's values
            //Multiply it by the elapsed game time since last update in milliseconds
            this.X += direction.X * gameTime.ElapsedGameTime.Milliseconds;
            this.Y += direction.Y * gameTime.ElapsedGameTime.Milliseconds;
        }

        public Projectile Clone(Vector2 position, Vector2 direction) {
            return new Projectile(this.Texture, position, this.BoundingBox, direction, this.damage);
        }
        
    }
}
