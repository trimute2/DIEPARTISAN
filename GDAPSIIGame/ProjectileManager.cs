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
    enum ProjectileType
    {
        DEFAULT = 1
    }
    class ProjectileManager
    {
        //Fields-----------------
        //Singleton Instance of ProjectileManager
        static ProjectileManager instance;
        //List of Projectiles in game
        volatile private static List<Projectile> projectiles;
        //List of one of every projectile type for cloning
        private static List<Projectile> hiddenProjectiles;

        //Properties-------------

        /// <summary>
        /// A List of all projectiles
        /// </summary>
        public List<Projectile> Projectiles { get { return projectiles; } set { projectiles = value; } }


        //Methods----------------

        private ProjectileManager()
        {
            projectiles = new List<Projectile>();
            hiddenProjectiles = new List<Projectile>();
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
            Projectiles.Add(new Projectile(texture, new Vector2(texture.Width, texture.Height), new Rectangle(texture.Width, texture.Height, 25, 25), new Vector2(-0.05f, -0.05f), 1));
            hiddenProjectiles.Add(new Projectile(texture, new Vector2(-100, -100), new Rectangle(texture.Width, texture.Height, 25, 25), new Vector2(0f, 0f), 1));
        }

        /// <summary>
        /// Update projectiles
        /// </summary>
        internal void Update(GameTime gameTime, KeyboardState previousKbState, KeyboardState kbState)
        {
            lock (Projectiles)
            {
                foreach (Projectile p in Projectiles)
                {
                    p.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Draw projectiles
        /// </summary>
        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            lock (Projectiles)
            {
                foreach (Projectile p in Projectiles)
                {
                    p.Draw(spriteBatch);
                }
            }
        }

        internal void Add(Projectile p)
        {
            projectiles.Add(p);
            ChunkManager.Instance.Add(p);
        }

        internal Projectile Clone(ProjectileType pT, Vector2 currPosition, Vector2 currDirection)
        {
            switch (pT)
            {
                case ProjectileType.DEFAULT:
                    return hiddenProjectiles[(int)pT - 1].Clone(currPosition, currDirection);
            }
            return null;
        }
    }
}
