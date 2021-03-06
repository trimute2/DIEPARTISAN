﻿using System;
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
		TURRET = 3,
		SHOTGUN = 4
    }

    class ProjectileManager
    {
        //Fields-----------------
        //Singleton Instance of ProjectileManager
        static ProjectileManager instance;
        //List of Projectiles in game
        volatile private static List<Projectile> projectiles;
		private List<Particle> particles;
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
			particles = new List<Particle>();
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
			//Pistol
			hiddenProjectiles.Add(new Projectile(texture, new Vector2(-100, -100), new Rectangle(texture.Width, texture.Height, 12, 6), new Vector2(0f, 0f), 3, 0, Owners.None, 24, Destroy_Type.None));
			//Rifle
			hiddenProjectiles.Add(new Projectile(texture, new Vector2(-100, -100), new Rectangle(texture.Width, texture.Height, 12, 6), new Vector2(0f, 0f), 2, 0, Owners.None, 0, Destroy_Type.Projectiles));
			//Turret
			hiddenProjectiles.Add(new Projectile(texture, new Vector2(-100, -100), new Rectangle(texture.Width, texture.Height, 12, 6), new Vector2(0f, 0f), 15, 0));
			//Shotgun
			hiddenProjectiles.Add(new Projectile(texture, new Vector2(-100, -100), new Rectangle(texture.Width, texture.Height, 12, 6), Vector2.Zero, 1, 0, Owners.None, 60, Destroy_Type.Projectiles));
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
			for(int i = particles.Count -1; i >= 0; i--)
			{
				if (particles[i].IsActive)
				{
					particles[i].Update(gameTime);
				}
				else
				{
					particles.Remove(particles[i]);
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
				if (p.Drawable())
				{
					p.Draw(spriteBatch);
				}
			}
			foreach(Particle p in particles)
			{
				if (p.Drawable())
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

		internal void AddParticle(Particle p)
		{
			particles.Add(p);
		}
		/// <summary>
		/// removes a projectile
		/// </summary>
		/// <param name="p">the projectile to remove</param>
		internal void Remove(Projectile p)
		{
			projectiles.Remove(p);
		}

        internal Projectile Clone(ProjectileType pT, Vector2 currPosition, Vector2 currDirection, float angle, Owners owner, float range)
        {
            switch (pT)
            {
                case ProjectileType.PISTOL:
                    return hiddenProjectiles[(int)pT - 1].Clone(currPosition, currDirection, owner, angle, range);
				case ProjectileType.RIFLE:
					return hiddenProjectiles[(int)pT - 1].Clone(currPosition, currDirection, owner, angle, range);
				case ProjectileType.TURRET:
					return hiddenProjectiles[(int)pT - 1].Clone(currPosition, currDirection, owner, angle, range);
				case ProjectileType.SHOTGUN:
					return hiddenProjectiles[(int)pT - 1].Clone(currPosition, currDirection, owner, angle, range);
			}
            return null;
        }
		
		internal void RemoveProjectiles()
		{
			foreach (Projectile p in projectiles)
			{
				p.active = false;
			}
			projectiles.Clear();
		}

    }
}
