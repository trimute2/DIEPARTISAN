using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GDAPSIIGame.Weapons
{
	class WeaponManager
	{
		//Fields-----------------
		static private WeaponManager instance;
		Texture2D playerTexture;

		//Weapons
		public Pistol Pistol
		{
			get
			{
				return new Pistol(ProjectileType.PISTOL,
				TextureManager.Instance.WeaponTextures["PistolTexture"],
				Vector2.Zero,
				new Rectangle(0, 0, 20, 30),
				0.15f, 6, 0.9f,
				new Vector2(playerTexture.Bounds.X + playerTexture.Bounds.Width, playerTexture.Bounds.Top + playerTexture.Bounds.Height / 2),
				Owners.Player,
				Range.Short);
			}
		}

		public Rifle Rifle
		{
			get
			{
				return new Rifle(ProjectileType.RIFLE,
				TextureManager.Instance.WeaponTextures["PistolTexture"],
				Vector2.Zero,
				new Rectangle(0, 0, 20, 50),
				0.11f, 20, 1.7f,
				new Vector2(playerTexture.Bounds.X + playerTexture.Bounds.Width, playerTexture.Bounds.Top + playerTexture.Bounds.Height / 2),
				Owners.Player,
				Range.Long);
			}
		}

		public TurretGun TurretGun
		{
			get
			{
				return new TurretGun(ProjectileType.TURRET,
				TextureManager.Instance.WeaponTextures["PistolTexture"],
				Vector2.Zero,
				new Rectangle(0, 0, 10, 20),
				1f,
				Vector2.Zero,
				Owners.Enemy);
			}
		}

		public Shotgun ShotGun
		{
			get
			{
				return new Shotgun(ProjectileType.SHOTGUN,
				TextureManager.Instance.WeaponTextures["PistolTexture"],
				Vector2.Zero,
				new Rectangle(0, 0, 20, 50),
				//fire rate, clip size, reload speed
				0.75f, 10, 1.1f,
				new Vector2(playerTexture.Bounds.X + playerTexture.Bounds.Width, playerTexture.Bounds.Top + playerTexture.Bounds.Height / 2),
				Owners.Player,
				Range.Medium);
			}
		}
		//Methods----------------

		/// <summary>
		/// Singleton Constructor
		/// </summary>
		private WeaponManager()
		{ }

		public void LoadContent(ContentManager Content)
		{
			playerTexture = TextureManager.Instance.PlayerTextures["PlayerTexture"];
		}

		/// <summary>
		/// Singleton access
		/// </summary>
		/// <returns></returns>
		static public WeaponManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new WeaponManager();
				}
				return instance;
			}
		}
	}
}
