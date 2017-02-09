using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDAPSIIGame
{
    class Weapon : GameObject
    {
        Projectile bullet;

        public Weapon(Projectile projectile, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(texture, position, boundingBox)
        {
            this.bullet = projectile;
        }

        /// <summary>
        /// The bullet the weapon fires
        /// </summary>
        public Projectile Bullet
        {
            get { return bullet; }
            set { bullet = value; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
