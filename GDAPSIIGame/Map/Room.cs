using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDAPSIIGame.Map
{
    /// <summary>
    /// Different Tile Types that go into a Room
    /// </summary>
    enum TileType
    {
        FLOOR,
        PLAYER,
        ENEMY,
        WALL
    }

    /// <summary>
    /// Possible room connections by direction
    /// </summary>
    enum Connection
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }
    class Room
    {
        //The literal tilemap of the room
        private TileType[][] tileLayout;
        int connections;

        public Room(TileType[][] tileLayout)
        {
            this.tileLayout = tileLayout;
            connections = 0;
        }

        /// <summary>
        /// Matrix of Tiles belonging to this room
        /// </summary>
        public TileType[][] TileLayout
        {
            get { return tileLayout; }
        }

        /// <summary>
        /// Naive implementation of rolling die to get up to 4 random rooms
        /// Uses some fun bit shifting
        /// </summary>
        public void GenerateConnections(Random r)
        {
            int numRooms = r.Next(15) + 1;
            int dirToConnect = 0;
            do
            {
                dirToConnect = r.Next(4) + 1;
                while ((connections & dirToConnect) == dirToConnect)
                {
                    dirToConnect = r.Next(4) + 1;
                }
                connections = dirToConnect;
                numRooms <<= 1;
            } while ((numRooms & 8) == 0);
        }


    }
}
