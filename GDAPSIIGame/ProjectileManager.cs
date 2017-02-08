using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GDAPSIIGame
{
    class ProjectileManager
    {
        //Fields-----------------
        static ProjectileManager instance;
        private static List<Projectile> projectiles;

        //Properties-------------

        /// <summary>
        /// A List of all projectiles
        /// </summary>
        public List<Projectile> Projectiles { get { return projectiles; } set { projectiles = value; } }


        //Methods----------------

        private ProjectileManager()
        {
            projectiles = new List<Projectile>();
        }

        public static ProjectileManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProjectileManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// Load in sprites
        /// </summary>
        internal void LoadContent(ContentManager Content)
        {
            Texture2D texture = Content.Load<Texture2D>("player");
            Projectiles.Add(new Projectile(texture, new Vector2(texture.Width, texture.Height), new Rectangle(texture.Width, texture.Height, 25, 25), new Vector2(-0.05f, -0.05f)));
        }

        /// <summary>
        /// Update projectiles
        /// </summary>
        internal void Update(GameTime gameTime, KeyboardState previousKbState, KeyboardState kbState)
        {
            foreach (Projectile p in Projectiles)
            {
                p.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw projectiles
        /// </summary>
        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Projectile p in Projectiles)
            {
                p.Draw(spriteBatch);
            }
        }
    }
}
