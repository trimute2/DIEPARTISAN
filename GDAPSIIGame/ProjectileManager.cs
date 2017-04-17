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
        PISTOL = 1,
		RIFLE = 2,
		TURRET = 3
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
			Texture2D texture = TextureManager.Instance.BulletTextures["PlayerBullet"];
			//Projectiles.Add(new Projectile(texture, new Vector2(texture.Width, texture.Height), new Rectangle(texture.Width, texture.Height, 25, 25), new Vector2(-0.05f, -0.05f), 1));
			hiddenProjectiles.Add(new Projectile(texture, new Vector2(-100, -100), new Rectangle(texture.Width, texture.Height, 25, 25), new Vector2(0f, 0f), 2));
			hiddenProjectiles.Add(new Projectile(texture, new Vector2(-100, -100), new Rectangle(texture.Width, texture.Height, 25, 25), new Vector2(0f, 0f), 1));
			hiddenProjectiles.Add(new Projectile(texture, new Vector2(-100, -100), new Rectangle(texture.Width, texture.Height, 25, 25), new Vector2(0f, 0f), 3));
		}

        /// <summary>
        /// Update projectiles
        /// </summary>
        internal void Update(GameTime gameTime, KeyboardState previousKbState, KeyboardState kbState)
        {
			for (int i = projectiles.Count - 1; i >= 0; i--)
			{
				if (projectiles[i].IsActive)
				{
					projectiles[i].Update(gameTime);
				}
				else
				{
					projectiles.Remove(projectiles[i]);
				}
			}

			/*foreach (Projectile p in Projectiles)
			{
				p.Update(gameTime);
			}*/
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

        internal void Add(Projectile p)
        {
            projectiles.Add(p);
            ChunkManager.Instance.Add(p);
        }

		/// <summary>
		/// removes a projectile
		/// </summary>
		/// <param name="p">the projectile to remove</param>
		internal void Remove(Projectile p)
		{
			projectiles.Remove(p);
		}

        internal Projectile Clone(ProjectileType pT, Vector2 currPosition, Vector2 currDirection, Owners owner)
        {
            switch (pT)
            {
                case ProjectileType.PISTOL:
                    return hiddenProjectiles[(int)pT - 1].Clone(currPosition, currDirection, owner);
				case ProjectileType.RIFLE:
					return hiddenProjectiles[(int)pT - 1].Clone(currPosition, currDirection, owner);
				case ProjectileType.TURRET:
					return hiddenProjectiles[(int)pT - 1].Clone(currPosition, currDirection, owner);
			}
            return null;
        }
    }
}
