using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using GDAPSIIGame.Entities;

namespace GDAPSIIGame
{
    class EntityManager
    {
        //Fields-----------------
        List<Entity> enemies;
        static private EntityManager instance;
        static private Player player;

		//Properties-------------
		public bool BeatLevel
		{
			get { return enemies.Count == 0; }
		}

        //Methods----------------

        /// <summary>
        /// Singleton Constructor
        /// </summary>
        private EntityManager()
        {
            enemies = new List<Entity>();
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
        /// Create objects
        /// </summary>
        internal void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
			Texture2D playerTexture = TextureManager.Instance.PlayerTextures["PlayerTexture"];
			//Create the player object
			player = Player.Instantiate(Weapons.WeaponManager.Instance.Pistol,
				Weapons.WeaponManager.Instance.Rifle,
				100,
				5,
				playerTexture,
				new Vector2(playerTexture.Width, playerTexture.Height),
				new Rectangle(playerTexture.Width, playerTexture.Height, 40, 60));
			//Create the player's weapon and add it to the player
            ChunkManager.Instance.Add(player);
		}

        /// <summary>
        /// Update entities
        /// </summary>
        internal void Update(GameTime gameTime)
		{
			if(player.IsActive)
			{
				player.Update(gameTime);
			}
			for(int i = enemies.Count-1;i >= 0; i--)
			{
				if (enemies[i].IsActive)
				{
					enemies[i].Update(gameTime);
				}
                else
				{
					enemies.Remove(enemies[i]);
				}
			}
		}

        /// <summary>
        /// Draw entities
        /// </summary>
        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Player.Draw(spriteBatch);
			foreach (Entity en in enemies)
			{
				en.Draw(spriteBatch);
			}
			player.Draw(spriteBatch);
        }

        internal void Add(Entity e)
        {
            enemies.Add(e);
        }

		internal void RemoveEnemies()
		{
			foreach (Entities.Entity e in enemies)
			{
				e.IsActive = false;
			}
			enemies.Clear();
		}
    }
}
