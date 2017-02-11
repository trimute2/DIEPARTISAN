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
    class EntityManager
    {
        //Fields-----------------
        List<Entities.Entity> enemies;
        static private EntityManager instance;

        //Properties-------------

        /// <summary>
        /// The list of all enemies
        /// </summary>
        public List<Entities.Entity> Enemies { get { return enemies; } set { enemies = value; } }

        /// <summary>
        /// The player object
        /// </summary>
        public Player Player {
            get;
            set;
        }


        //Methods----------------

        /// <summary>
        /// Singleton Constructor
        /// </summary>
        private EntityManager()
        {
            enemies = new List<Entities.Entity>();
        }

        /// <summary>
        /// Singleton access
        /// </summary>
        /// <returns></returns>
        static public EntityManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityManager();
                }
                return instance;
            }
        }
        
        /// <summary>
        /// Load in sprites
        /// </summary>
        internal void LoadContent(ContentManager Content)
        {
            Texture2D playerTexture = Content.Load<Texture2D>("player");
            Player = Player.Instantiate(null, 100, 1, playerTexture, new Vector2(playerTexture.Width, playerTexture.Height), new Rectangle(playerTexture.Width, playerTexture.Height, 50, 50));
			ChunkManager.Instance.Add(Player);
        }

        /// <summary>
        /// Update entities
        /// </summary>
        internal void Update(GameTime gameTime, KeyboardState previousKbState, KeyboardState kbState)
        {
            Player.Update(gameTime, previousKbState, kbState);
        }

		internal void Update(GameTime gameTime, GamePadState previousGpState, GamePadState gpState)
		{
			Player.Update(gameTime, previousGpState, gpState);
		}

        /// <summary>
        /// Draw entities
        /// </summary>
        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Player.Draw(spriteBatch);
        }

        internal void Add(Entities.Entity e)
        {
            enemies.Add(e);
        }
    }
}
