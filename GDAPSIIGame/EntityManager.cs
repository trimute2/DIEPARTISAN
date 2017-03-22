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
            //Load in the player texture
            Texture2D playerTexture = Content.Load<Texture2D>("spr_player");
			Texture2D playerBullet = Content.Load<Texture2D>("playerBullet");
			//Create the player object
			player = Player.Instantiate(null,
                100,
                5,
                playerTexture,
                new Vector2(playerTexture.Width, playerTexture.Height),
                new Rectangle(playerTexture.Width, playerTexture.Height, 40, 60));
            //Create the player's weapon and add it to the player
            Player.Instance.Weapon = new Weapon(ProjectileType.DEFAULT,
				playerBullet,
				Player.Instance.Position,
				new Rectangle((int)player.X, (int)player.Y, 20, 60),
				0.2f, 100f, 0.5f,
				new Vector2(playerTexture.Bounds.X+playerTexture.Bounds.Width/2, playerTexture.Bounds.Top+playerTexture.Bounds.Height/4),
				Owners.Player);
            entities.Add(player);
            ChunkManager.Instance.Add(Player);
        }

        /// <summary>
        /// Update entities
        /// </summary>
        internal void Update(GameTime gameTime)
		{
			for(int i = entities.Count-1;i >= 0; i--)
			{
				if (entities[i].IsActive)
				{
					entities[i].Update(gameTime);
				}else
				{
					entities.Remove(entities[i]);
				}
			}
			/*foreach (Entities.Entity e in entities)
			{
				e.Update(gameTime);
			}*/
		}

        /// <summary>
        /// Draw entities
        /// </summary>
        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Player.Draw(spriteBatch);
			foreach (Entities.Entity en in entities)
			{
				en.Draw(spriteBatch);
			}
        }

        internal void Add(Entities.Entity e)
        {
            entities.Add(e);
        }
    }
}
