using GDAPSIIGame.Entities;
using GDAPSIIGame.Pods;
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
        WALL,
        ENEMY,
        PLAYER
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
		Texture2D wallTexture;
		Texture2D floorTexture;

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

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 currPos = Camera.Instance.GetViewportPosition(position);
            int tileSize = 64;
            int roomSize = 10;
            for (int i = 0; i < roomSize; i++)
            {
                for (int j = 0; j < roomSize; j++)
                {
                    spriteBatch.Draw(
                        tileLayout[i, j] == TileType.WALL ? wallTexture : floorTexture,
                        new Rectangle(
                            (int)currPos.X + i*tileSize, 
                            (int)currPos.Y + j*tileSize, 
                            tileSize, 
                            tileSize), 
                        Color.White);
                }
            }
        }

		/// <summary>
		/// Takes a Room and creates everything that is not a floor tile
		/// </summary>
		/// <param name="enemyTexture">Texture of enemies in this room</param>
		/// <param name="wallTexture">Texture of walls in this room</param>
		public void initRoom(Texture2D enemyTexture, Texture2D floorTexture, Texture2D wallTexture)
        {
			//Init room's textures
			this.wallTexture = wallTexture;
			this.floorTexture = floorTexture;

            //Should change this later
            int tileSize = 64;
            int roomSize = 10;
            int enemySpriteWidth = 32;
            int enemySpriteHeight = 32;
            int wallSpriteWidth = 32;
            int wallSpriteHeight = 32;
			Pod pod = new Pod();
            for (int i = 0; i < roomSize; i++)
            {
                for (int j = 0; j < roomSize; j++)
                {
                    switch (tileLayout[i, j])
                    {
                        case TileType.ENEMY:
                            Vector2 currPos = 
                                new Vector2(
                                    position.X + tileSize * i, 
                                    position.Y + tileSize * j);
                            int health = 3;
                            int moveSpeed = 1;

                            //Create new enemy
                            MeleeEnemy newEnemy = 
                                new MeleeEnemy(
                                    health, 
                                    moveSpeed, 
                                    enemyTexture, 
                                    currPos, 
                                    new Rectangle(
                                        (int)currPos.X, 
                                        (int)currPos.Y, 
                                        enemySpriteWidth,
                                        enemySpriteHeight));

                            //Add Enemy to game
                            EntityManager.Instance.Add(newEnemy);
                            ChunkManager.Instance.Add(newEnemy);
							pod.Add(newEnemy);
                            break;

                        case TileType.PLAYER:
                            //TODO: Decide if we want to do anything to player here
                            break;

                        case TileType.WALL:
                            //Console.WriteLine("WALL!");
                            Vector2 currPos2 =
                                new Vector2(
                                    position.X + tileSize * i, 
                                    position.Y + tileSize * j);
                            ChunkManager.Instance.Add(
                                new Wall(
                                    wallTexture, 
                                    currPos2, 
                                    new Rectangle(
                                        (int)currPos2.X, 
                                        (int)currPos2.Y, 
                                        tileSize, 
                                        tileSize)));
                            break;

                    }
                    //spriteBatch.Draw(texture, new Rectangle((int)currPos.X + i * tileSize, (int)currPos.Y + j * tileSize, tileSize, tileSize), Color.White);
                }
            }
			PodManager.Instance.Add(pod);
        }
    }
}
