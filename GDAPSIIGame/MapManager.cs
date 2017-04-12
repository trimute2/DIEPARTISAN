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
	class MapManager
	{
		//Fields-----------------
		static private MapManager instance;
		Map.Map currMap;

		//Properties-------------
		public Map.Map CurrMap
		{
			get { return currMap; }
		}


		/// <summary>
		/// Singleton Constructor
		/// </summary>
		private MapManager()
		{
			
		}

		public void LoadContent(ContentManager Content)
		{

		}

		/// <summary>
		/// Singleton access
		/// </summary>
		/// <returns></returns>
		static public MapManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MapManager();
				}
				return instance;
			}
		}

		//Methods
		public void CreateMap(Texture2D wallTexture, Texture2D floorTexture)
		{
			currMap = new Map.Map();
			currMap.initMap(floorTexture, wallTexture);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			currMap.Draw(spriteBatch);
		}
	}
}
