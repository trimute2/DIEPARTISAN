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
	enum MapState { Enter, Play, Exit, Died}

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
		public MapState State
		{
			get { return currMap.State; }
			set { currMap.State = value; }
		}


		/// <summary>
		/// Singleton Constructor
		/// </summary>
		private MapManager()
		{ }

		public void LoadContent(ContentManager Content)
		{ }

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
		public void CreateMap(Texture2D wallTextures, Texture2D floorTextures, int mapSize)
		{
			currMap = new Map.Map(mapSize);
			currMap.initMap(wallTextures, floorTextures);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			currMap.Draw(spriteBatch);
		}

		public void DrawForeground(SpriteBatch spriteBatch)
		{
			currMap.DrawForeground(spriteBatch);
		}
	}
}
