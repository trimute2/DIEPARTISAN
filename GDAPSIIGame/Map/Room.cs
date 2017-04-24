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
        PLAYER,
		MELEEENEMY,
		TURRET
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
		Texture2D textures;
		private static int tileWidth = 32;
		private static int tileHeight = 32;
		private static int offset = 1;

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
					if (tileLayout[i, j] == TileType.WALL)
					{
						spriteBatch.Draw(
							textures,
							new Vector2(
								(int)currPos.X + i * tileSize,
								(int)currPos.Y + j * tileSize),
							GetSourceRectangle(textures, 0),
							Color.White);
					}
					else if (tileLayout[i, j] == TileType.FLOOR)
					{
						spriteBatch.Draw(
							textures,
							new Vector2(
								(int)currPos.X + i * tileSize,
								(int)currPos.Y + j * tileSize),
							GetSourceRectangle(textures, 11),
							Color.White);
					}
				}
            }
        }

		/// <summary>
		/// Takes a Room and creates everything that is not a floor tile
		/// </summary>
		/// <param name="enemyTexture">Texture of enemies in this room</param>
		/// <param name="wallTexture">Texture of walls in this room</param>
		public void initRoom(Texture2D roomTextures, Graph.Graph g)
        {
			//Init room's textures
			textures = roomTextures;

            //Should change this later
            int tileSize = 64;
            int roomSize = 10;
            int enemySpriteWidth = 32;
            int enemySpriteHeight = 32;
			Pod pod = new Pod();
            for (int i = 0; i < roomSize; i++)
            {
                for (int j = 0; j < roomSize; j++)
                {
                    switch (tileLayout[i, j])
                    {
						//Create walls
						case TileType.WALL:
							Vector2 currPos2 =
								new Vector2(
									position.X + tileSize * i,
									position.Y + tileSize * j);
							ChunkManager.Instance.Add(
								new Wall(
									roomTextures,
									currPos2,
									new Rectangle(
										(int)currPos2.X,
										(int)currPos2.Y,
										tileSize,
										tileSize)));
							break;
						
						//Move player
						case TileType.PLAYER:
							Vector2 currPos4 =
								new Vector2(
									position.X + tileSize * i,
									position.Y + tileSize * j);
							Player.Instance.Position = currPos4;
							Camera.Instance.resetPosition(Player.Instance.Position);
							break;
						
						//Create Melee Enemies
						case TileType.MELEEENEMY:
                            Vector2 currPos = 
                                new Vector2(
                                    position.X + tileSize * i + (tileSize / 4), 
                                    position.Y + tileSize * j + (tileSize / 4));
                            int health = 3;
                            int moveSpeed = 2;

                            //Create new enemy
                            MeleeEnemy newEnemy = 
                                new MeleeEnemy(
                                    health, 
                                    moveSpeed, 
                                    TextureManager.Instance.GetEnemyTexture("EnemyTexture"), 
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

						//Create Turret Enemies
						case TileType.TURRET:
							Vector2 currPos3 =
							   new Vector2(
								   position.X + tileSize * i + (tileSize / 4),
								   position.Y + tileSize * j + (tileSize/4));
							int health2 = 3;
							int moveSpeed2 = 0;

							//Create new enemy
							TurretEnemy turret =
								new TurretEnemy(
									health2,
									moveSpeed2,
									TextureManager.Instance.GetEnemyTexture("EnemyTexture"),
									currPos3,
									new Rectangle(
										(int)currPos3.X,
										(int)currPos3.Y,
										enemySpriteWidth,
										enemySpriteHeight));

							//Add Enemy to game
							EntityManager.Instance.Add(turret);
							ChunkManager.Instance.Add(turret);
							pod.Add(turret);
							break;
                    }
                }
            }
			PodManager.Instance.Add(pod);
        }

		static private Rectangle GetSourceRectangle(Texture2D tileSetTexture, int tileIndex)
		{
			int tileY = tileIndex / (tileSetTexture.Height / tileHeight-offset);
			int tileX = tileIndex % (tileSetTexture.Width / tileWidth);

			return new Rectangle((tileX * tileWidth), (tileY * tileHeight), tileWidth, tileHeight);
		}
	}
}
