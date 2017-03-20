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
    class MapManager
    {
        List<Room> rooms = new List<Room>();
        public MapManager()
        {
            generateMap("testRoom1.txt", this);
        }

        public void Add(Room r)
        {
            rooms.Add(r);
        }

        public void Draw(SpriteBatch spritebatch, Texture2D texture)
        {
            foreach (Room r in rooms)
            {
                r.Draw(spritebatch, texture);
            }
        }

        public static void generateMap(String filename, MapManager m)
        {
            String[] lines = File.ReadAllLines("../../../../Map/" + filename);
            TileType[,] tiles = new TileType[10, 10];

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    tiles[i, j] = (TileType)lines[i][j];
                }
            }

            Room r = new Room(tiles, Vector2.Zero);
            int mapSize = 20;
            int roomSize = 64 * 10;
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    m.Add(new Room(tiles, new Vector2(i*roomSize, j*roomSize)));
                }
            }
        }
    }
}
