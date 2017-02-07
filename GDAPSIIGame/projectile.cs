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

        //Properties
        public Vector2 Direction { get { return direction; } set { direction = value; } }

        //Constructor
        /// <summary>
        /// A movable projectile object
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public Projectile(Texture2D texture, Vector2 position, Rectangle boundingBox, Vector2 direction) : base(texture, position, boundingBox)
        {
            this.direction = direction;
        }

        public void Update(GameTime gameTime, GameObject player)
        {
            //Update the projectile's position based on the vector's values
            //Multiply it by the elapsed game time since last update in milliseconds
            this.X += direction.X * gameTime.ElapsedGameTime.Milliseconds;
            this.Y += direction.Y * gameTime.ElapsedGameTime.Milliseconds;
            
            
            if (BoundingBox.Intersects(player.BoundingBox))
            {
                Console.WriteLine("hit");
            }
            else Console.WriteLine("miss");
        }
        
    }
}
