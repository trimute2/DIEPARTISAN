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
        Vector2 direction;

        //Constructor
        /// <summary>
        /// A movable projectile object
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public Projectile(Texture2D texture, Rectangle position, Vector2 direction) : base(texture, position)
        {
            this.direction = direction;
        }

        public void Update(GameTime gameTime)
        {
            //Update the projectile's position based on the vector's values
            //Multiply it by the elapsed game time since last update in milliseconds
            this.X += (int)direction.X * gameTime.ElapsedGameTime.Milliseconds;
            this.Y += (int)direction.Y * gameTime.ElapsedGameTime.Milliseconds;
        }
        
    }
}
