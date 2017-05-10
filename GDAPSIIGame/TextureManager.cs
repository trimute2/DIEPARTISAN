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
	class TextureManager
	{
		//Fields-----------------
		static private TextureManager instance;
		Dictionary<String, Texture2D> playerTextures;
		Dictionary<String, Texture2D> enemyTextures;
		Dictionary<String, Texture2D> bulletTextures;
		Dictionary<String, Texture2D> weaponTextures;
		Dictionary<String, Texture2D> roomTextures;
		Dictionary<String, Texture2D> mouseTextures;
		Dictionary<String, Texture2D> menuTextures;
		Dictionary<String, SpriteFont> fonts;

		//Methods----------------

		/// <summary>
		/// Singleton Constructor
		/// </summary>
		private TextureManager()
		{
			playerTextures = new Dictionary <String, Texture2D>();
			enemyTextures = new Dictionary<String, Texture2D>();
			bulletTextures = new Dictionary<String, Texture2D>();
			weaponTextures = new Dictionary<String, Texture2D>();
			roomTextures = new Dictionary<String, Texture2D>();
			mouseTextures = new Dictionary<String, Texture2D>();
			menuTextures = new Dictionary<string, Texture2D>();
			fonts = new Dictionary<string, SpriteFont>();
		}

		/// <summary>
		/// Singleton access
		/// </summary>
		/// <returns></returns>
		static public TextureManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new TextureManager();
				}
				return instance;
			}
		}

		//Properties
		public Dictionary<String, Texture2D> PlayerTextures
		{ get { return playerTextures; } }

		public Dictionary<String, Texture2D> EnemyTextures
		{ get { return enemyTextures; } }

		public Dictionary<String, Texture2D> BulletTextures
		{ get { return bulletTextures; } }

		public Dictionary<String, Texture2D> WeaponTextures
		{ get { return weaponTextures; } }

		public Dictionary<String, Texture2D> RoomTextures
		{ get { return roomTextures; } }

		public Dictionary<String, Texture2D> MouseTextures
		{ get { return mouseTextures; } }

		public Dictionary<String, Texture2D> MenuTextures
		{ get { return menuTextures; } }

		public Dictionary<String, SpriteFont> Fonts
		{ get { return fonts; } }

		internal void InitialLoadContent(ContentManager Content)
		{
			menuTextures.Add("Black", Content.Load<Texture2D>("black"));
		}

		/// <summary>
		/// Load in sprites
		/// </summary>
		internal void LoadContent(ContentManager Content)
		{
			//Load player textures
			playerTextures.Add("PlayerTexture", Content.Load<Texture2D>("spr_player"));

			//Load enemy textures
			enemyTextures.Add("EnemyTexture", Content.Load<Texture2D>("enemy1"));

			//Load bullet textures
			bulletTextures.Add("PlayerBullet", Content.Load<Texture2D>("bullet"));

			//Load pistol texture
			weaponTextures.Add("PistolTexture", Content.Load<Texture2D>("pistolweapon"));

            //Load shotgun texture
            weaponTextures.Add("ShotgunTexture", Content.Load<Texture2D>("shotgunweapon"));

            //Load rifle texture
            weaponTextures.Add("RifleTexture", Content.Load<Texture2D>("rifleweapon"));

            //Load mouse textures
            mouseTextures.Add("MousePointer", Content.Load<Texture2D>("cursor"));

			//Load room textures
			roomTextures.Add("WallTexture", Content.Load<Texture2D>("playerBullet"));
			roomTextures.Add("FloorTexture", Content.Load<Texture2D>("playerNew"));
			roomTextures.Add("IndoorSpriteSheet", Content.Load<Texture2D>("indoorTileset"));
			roomTextures.Add("IndoorFloorSpriteSheet", Content.Load<Texture2D>("indoorFloorTileset"));
			roomTextures.Add("playerSpawnBackground", Content.Load<Texture2D>("spawnRoomBackground"));
			roomTextures.Add("playerSpawnForeground", Content.Load<Texture2D>("spawnRoomForeground"));

			//Load menu textures
			menuTextures.Add("Logo", Content.Load<Texture2D>("tempLogo"));
			menuTextures.Add("MenuBackground", Content.Load<Texture2D>("menuBackground"));
			menuTextures.Add("Play", Content.Load<Texture2D>("PLAY"));

			//Add fonts
			fonts.Add("uifont", Content.Load<SpriteFont>("font"));
		}


		/// <summary>
		/// Gets any texture in the enemyTextures dictionary
		/// </summary>
		public Texture2D GetEnemyTexture(String name)
		{
			if (enemyTextures.ContainsKey(name))
			{
				return enemyTextures[name];
			}
			else return null;
		}

		/// <summary>
		/// Gets any texture in the enemyTextures dictionary
		/// </summary>
		public Texture2D GetBulletTexture(String name)
		{
			if (bulletTextures.ContainsKey(name))
			{
				return bulletTextures[name];
			}
			else return null;
		}

		/// <summary>
		/// Gets any texture in the roomTextures dictionary
		/// </summary>
		public Texture2D GetRoomTexture(String name)
		{
			if (roomTextures.ContainsKey(name))
			{
				return roomTextures[name];
			}
			else return null;
		}

		/// <summary>
		/// Gets any texture in the menuTextures dictionary
		/// </summary>
		public Texture2D GetMenuTexture(String name)
		{
			if (menuTextures.ContainsKey(name))
			{
				return menuTextures[name];
			}
			else return null;
		}

		/// <summary>
		/// Gets any texture in the enemyTextures dictionary
		/// </summary>
		public SpriteFont GetFont(String name)
		{
			if (fonts.ContainsKey(name))
			{
				return fonts[name];
			}
			else return null;
		}
	}
}
