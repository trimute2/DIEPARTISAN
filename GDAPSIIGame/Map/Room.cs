using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private TileType[,] tileLayout;
        int connections;
        Vector2 position;

        public Room(TileType[,] tileLayout, Vector2 position)
        {
            this.position = position;
            this.tileLayout = tileLayout;
            this.connections = 0;
        }



        /// <summary>
        /// Matrix of Tiles belonging to this room
        /// </summary>
        public TileType[,] TileLayout
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
                    dirToConnect = (int)Math.Pow(2, r.Next(4));
                }
                connections += dirToConnect;
                numRooms <<= 1;
            } while ((numRooms & 8) == 0);
            
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            Vector2 currPos = Camera.Instance.GetViewportPosition(position);
            int tileSize = 64;
            int roomSize = 10;
            for (int i = 0; i < roomSize; i++)
            {
                for (int j = 0; j < roomSize; j++)
                {
                    spriteBatch.Draw(texture, new Rectangle((int)currPos.X + i*tileSize, (int)currPos.Y + j*tileSize, tileSize, tileSize), Color.White);
                }
            }
        }

        public void initRoom()
        {
            //Should change this later
            int tileSize = 64;
            int roomSize = 10;
            for (int i = 0; i < roomSize; i++)
            {
                for (int j = 0; j < roomSize; j++)
                {
                    switch (tileLayout[i, j])
                    {
                        case TileType.ENEMY:
                            //TODO: Spawn an enemy
                            break;

                        case TileType.PLAYER:
                            //TODo: Decide if we want to do anything to player here
                            break;

                        case TileType.WALL:
                            //TODO: Build a wall
                            break;
                    }
                    //spriteBatch.Draw(texture, new Rectangle((int)currPos.X + i * tileSize, (int)currPos.Y + j * tileSize, tileSize, tileSize), Color.White);
                }
            }
        }
    }
}
