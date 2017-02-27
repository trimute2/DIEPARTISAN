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
        public static void generateMap(String filename, MapManager m)
        {
            String[] lines = File.ReadAllLines("../../../../Map/"+filename);
            TileType[,] tiles = new TileType[10,10];

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    tiles[i, j] = (TileType)lines[i][j];
                }
            }

            Room r = new Room(tiles);
            m.Add(r);
        }
    }
}
