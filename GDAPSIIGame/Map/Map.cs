using Microsoft.Xna.Framework;
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
        List<Room> rooms = new List<Room>();
        public Map()
        {
            //TODO: randomize choice of rooms
            String[] files = new String[] { "testRoom1.txt", "testRoom2.txt" };
            generateMap(files, this);
        }

        public void Add(Room r)
        {
            rooms.Add(r);
        }

        public void Draw(SpriteBatch spritebatch, Texture2D floorTexture, Texture2D wallTexture)
        {
            foreach (Room r in rooms)
            {
                r.Draw(spritebatch, floorTexture, wallTexture);
            }
        }

        public static void generateMap(String[] filenames, Map m)
        {
            int mapSize = 2;
            Random randy = new Random();
            string filename;
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    filename = filenames[randy.Next(filenames.Length)];
                    String[] lines = File.ReadAllLines("../../../../Map/" + filename);
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

                    Room r = new Room(tiles, Vector2.Zero);
                    int roomSize = 64 * 10;

                    m.Add(new Room(tiles, new Vector2(i * roomSize, j * roomSize)));
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
                r.initRoom(floorTexture, wallTexture);
            }
        }
    }
}
