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
        ProjectileType projType;

        public Weapon(ProjectileType pT, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(texture, position, boundingBox)
        {
            this.projType = pT;
        }

        /// <summary>
        /// The bullet the weapon fires
        /// </summary>
        public ProjectileType ProjType
        {
            get { return projType; }
            set { projType = value; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        public void Fire(Vector2 position, Vector2 direction)
        {
            ProjectileManager.Instance.Clone(projType, position, direction);
        }
    }
}
