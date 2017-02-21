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
        List<Entities.Entity> entities;
        static private EntityManager instance;
        static private Player player;

        //Properties-------------

        /// <summary>
        /// The list of all entities
        /// </summary>
        public List<Entities.Entity> Entities { get { return entities; } set { entities = value; } }

        /// <summary>
        /// The player object
        /// </summary>
        public Player Player {
            get { return player; }
        }


        //Methods----------------

        /// <summary>
        /// Singleton Constructor
        /// </summary>
        private EntityManager()
        {
            entities = new List<Entities.Entity>();
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
            player = Player.Instantiate(null, 100, 1, playerTexture, new Vector2(playerTexture.Width, playerTexture.Height), new Rectangle(playerTexture.Width, playerTexture.Height, 50, 50));
            Player.Instance.Weapon = new Weapon(ProjectileType.DEFAULT, Player.Instance.Texture, Player.Instance.Position, Player.Instance.BoundingBox, 0.5f, 10f, 2f);
            entities.Add(player);
            ChunkManager.Instance.Add(Player);
        }

        /// <summary>
        /// Update entities
        /// </summary>
        internal void Update(GameTime gameTime)
		{
			foreach (Entities.Entity e in entities)
			{
				e.Update(gameTime);
			}
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
            entities.Add(e);
        }
    }
}
