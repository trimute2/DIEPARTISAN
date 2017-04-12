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
		Pistol pistol;
		Rifle rifle;

		//Weapons
		public Pistol Pistol
		{ get { return pistol; } }

		public Rifle Rifle
		{ get { return rifle; } }

		//Methods----------------

		/// <summary>
		/// Singleton Constructor
		/// </summary>
		private WeaponManager()
		{
			pistol = null;
			rifle = null;
		}

		public void LoadContent(ContentManager Content)
		{
			Texture2D playerTexture = TextureManager.Instance.PlayerTextures["PlayerTexture"];
			pistol = new Pistol(ProjectileType.DEFAULT,
				TextureManager.Instance.WeaponTextures["PistolTexture"],
				Vector2.Zero,
				new Rectangle(0, 0, 20, 30),
				0.2f, 100f, 0.5f,
				new Vector2(playerTexture.Bounds.X + playerTexture.Bounds.Width, playerTexture.Bounds.Top + playerTexture.Bounds.Height / 2),
				Owners.Player);
			rifle = new Rifle(ProjectileType.DEFAULT,
				TextureManager.Instance.WeaponTextures["PistolTexture"],
				Vector2.Zero,
				new Rectangle(0, 0, 20, 50),
				0.2f, 100f, 0.5f,
				new Vector2(playerTexture.Bounds.X + playerTexture.Bounds.Width, playerTexture.Bounds.Top + playerTexture.Bounds.Height / 2),
				Owners.Player);
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
