﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GDAPSIIGame.Map
{
    class Map
    {
        private List<Room> rooms = new List<Room>();

        public Map(int mapSize)
        {
			//TODO: randomize choice of rooms
			String[] files = Directory.GetFiles("../../../../Levels/", "*.cmap");
            generateMap(files, this, mapSize);
        }

        public void Add(Room r)
        {
            rooms.Add(r);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            foreach (Room r in rooms)
            {
                r.Draw(spritebatch);
            }
        }

        public static void generateMap(String[] filenames, Map m, int mapSize)
        {
            Random randy = new Random();
            string filename;
			//Room coords for spawn room
			int r = randy.Next(0, mapSize);
			int c = randy.Next(0, mapSize);

			for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
					//When the rows and columns match the player spawn coords
					if (i == r && j == c)
					{
						filename = "playerSpawn.cmap";

						String[] lines = File.ReadAllLines("../../../../Levels/" + filename);
						TileType[,] tiles = new TileType[10, 10];

						for (int k = 0; k < lines.Length; k++)
						{
							for (int l = 0; l < lines[0].Length; l++)
							{

								//The 48 comes from the fact that 0's ascii code is 48
								tiles[l, k] = (TileType)(lines[l][k] - 48);
								//Console.WriteLine(tiles[l, k] + "----------");
							}
						}

						int roomSize = 64 * 10;
						Room ro = new Room(tiles, new Vector2(i * roomSize, j * roomSize));
						m.Add(ro);
					}
					else
					{
						//Loop until the room is not the player's spawn room
						while (true)
						{
							filename = filenames[randy.Next(filenames.Length)];
							if (!filename.Contains("playerSpawn"))
							{
								break;
							}
						}
						String[] lines = File.ReadAllLines("../Levels/" + filename);
						TileType[,] tiles = new TileType[10, 10];

						for (int k = 0; k < lines.Length; k++)
						{
							for (int l = 0; l < lines[0].Length; l++)
							{

								//The 48 comes from the fact that 0's ascii code is 48
								tiles[l, k] = (TileType)(lines[l][k] - 48);
								//Console.WriteLine(tiles[l, k] + "----------");
							}
						}


						int roomSize = 64 * 10;
						Room ro = new Room(tiles, new Vector2(i * roomSize, j * roomSize));
						m.Add(ro);
					}
                }
            }
        }


        /// <summary>
        /// Instantiates Gameobjects in Rooms, then sets spaces to floor.
        /// </summary>
        public void initMap(Texture2D floorTexture, Texture2D wallTexture)
        {
            foreach (Room r in rooms)
            {
                r.initRoom(floorTexture, floorTexture, wallTexture);
            }
        }
    }
}