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
		TURRET,
		DASHENEMY
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
		private int[,] textureLayout;
		int connections;
		Vector2 position;
		Texture2D wallTextures;
		Texture2D floorTextures;
		private static int tileWidth = 32;
		private static int tileHeight = 32;
		private bool spawnroom;

		public Room(TileType[,] tileLayout, Vector2 position)
		{
			this.position = position;
			this.tileLayout = tileLayout;
			this.textureLayout = new int[tileLayout.GetLength(0), tileLayout.GetLength(1)];
			this.connections = 0;
			spawnroom = false;
		}



		/// <summary>
		/// Matrix of Tiles belonging to this room
		/// </summary>
		public TileType[,] TileLayout
		{
			get { return tileLayout; }
		}

		/// <summary>
		/// Matrix of Tiles belonging to this room
		/// </summary>
		public int[,] TextureLayout
		{
			get { return textureLayout; }
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
			if (!spawnroom)
			{
				for (int i = 0; i < roomSize; i++)
				{
					for (int j = 0; j < roomSize; j++)
					{
						if (tileLayout[i, j] == TileType.WALL)
						{
							spriteBatch.Draw(
								wallTextures,
								new Vector2(
									(int)currPos.X + (i * tileSize),
									(int)currPos.Y + (j * tileSize)),
								GetSourceRectangle(wallTextures, 0),
								Color.White,
								0f,
								Vector2.Zero,
								2,
								SpriteEffects.None,
								0);
						}
						else
						{
							int num = textureLayout[i, j];
							spriteBatch.Draw(
								floorTextures,
								new Vector2(
									(int)currPos.X + i * tileSize,
									(int)currPos.Y + j * tileSize),
								GetSourceRectangle(floorTextures, textureLayout[i, j]),
								Color.White,
								0f,
								Vector2.Zero,
								2,
								SpriteEffects.None,
								0);
						}
					}
				}
			}
			else
			{
				spriteBatch.Draw(
					TextureManager.Instance.GetRoomTexture("playerSpawnBackground"),
					new Vector2(
						(int)currPos.X,
						(int)currPos.Y),
					null,
					Color.White,
					0f,
					Vector2.Zero,
					2,
					SpriteEffects.None,
					0);
			}
		}

		public void DrawForeground(SpriteBatch spriteBatch)
		{
			if (spawnroom)
			{
				Vector2 currPos = Camera.Instance.GetViewportPosition(position);
				spriteBatch.Draw(
					TextureManager.Instance.GetRoomTexture("playerSpawnForeground"),
					new Vector2(
						(int)currPos.X,
						(int)currPos.Y),
					null,
					Color.White,
					0f,
					Vector2.Zero,
					2,
					SpriteEffects.None,
					0);
			}
		}

		/// <summary>
		/// Takes a Room and creates everything that is not a floor tile
		/// </summary>
		/// <param name="enemyTexture">Texture of enemies in this room</param>
		/// <param name="wallTexture">Texture of walls in this room</param>
		public void initRoom(Texture2D wallTextures, Texture2D floorTextures, Graph.Graph graph)
		{
			//Init room's textures
			this.wallTextures = wallTextures;
			this.floorTextures = floorTextures;

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
					Vector2 currPos =
						new Vector2(
									position.X + tileSize * i,
									position.Y + tileSize * j);
					Vector2 midPos =
						new Vector2(
							currPos.X + (tileSize / 4),
							currPos.Y + (tileSize / 4));
					switch (tileLayout[i, j])
					{
						//Create walls
						case TileType.WALL:
							ChunkManager.Instance.Add(
								new Wall(
									floorTextures,
									currPos,
									new Rectangle(
										(int)currPos.X,
										(int)currPos.Y,
										tileSize,
										tileSize)));
							break;

						//Move player
						case TileType.PLAYER:
							Player.Instance.Position = currPos;
							Player.Instance.ResetPlayerNewMap();
							Player.Instance.CurrWeapon.ResetWeapon();

							Weapons.Weapon temp = Player.Instance.CurrWeapon;
							temp.X = currPos.X + Player.Instance.BoundingBox.Width / 2;
							temp.Y = currPos.Y + Player.Instance.BoundingBox.Height / 2;
							Camera.Instance.resetPosition(Player.Instance.Position);

							//Add this position to the graph
							graph.Add(new Graph.GraphNode(midPos));
							spawnroom = true;
							break;

						//Create Melee Enemies
						case TileType.MELEEENEMY:
							
							graph.Add(new Graph.GraphNode(midPos));
							int health = 8;
							int moveSpeed = 2;

							//Create new enemy
							MeleeEnemy newEnemy =
								new MeleeEnemy(
									health,
									moveSpeed,
									TextureManager.Instance.GetEnemyTexture("EnemyTexture"),
									midPos,
									new Rectangle(
										(int)midPos.X,
										(int)midPos.Y,
										enemySpriteWidth,
										enemySpriteHeight));

							//Add Enemy to game
							EntityManager.Instance.Add(newEnemy);
							ChunkManager.Instance.Add(newEnemy);
							pod.Add(newEnemy);
							break;

						//Create Turret Enemies
						case TileType.TURRET:
							int health2 = 10;
							int moveSpeed2 = 0;

							//Create new enemy
							TurretEnemy turret =
								new TurretEnemy(
									health2,
									moveSpeed2,
									TextureManager.Instance.GetEnemyTexture("EnemyTexture"),
									midPos,
									new Rectangle(
										(int)midPos.X,
										(int)midPos.Y,
										enemySpriteWidth,
										enemySpriteHeight));

							//Add Enemy to game
							EntityManager.Instance.Add(turret);
							ChunkManager.Instance.Add(turret);
							pod.Add(turret);
							break;

						case TileType.DASHENEMY:
							graph.Add(new Graph.GraphNode(midPos));
							int health3 = 3;
							int moveSpeed3 = 2;

							//Create new enemy
							DashEnemy dash =
								new DashEnemy(
									health3,
									moveSpeed3,
									TextureManager.Instance.GetEnemyTexture("EnemyTexture"),
									midPos,
									new Rectangle(
										(int)midPos.X,
										(int)midPos.Y,
										enemySpriteWidth,
										enemySpriteHeight));

							//Add Enemy to game
							EntityManager.Instance.Add(dash);
							ChunkManager.Instance.Add(dash);
							pod.Add(dash);
							break;

						default:
							Vector2 currPos5 =
								new Vector2(
									position.X + tileSize * i + (tileSize / 2),
									position.Y + tileSize * j + (tileSize / 2));

							//Add this position to the graph
							graph.Add(new Graph.GraphNode(currPos5));
							break;
					}
				}
			}
			PodManager.Instance.Add(pod);

			InitTextureMap();
		}

		/// <summary>
		/// Decide what textures will show at each place in the map
		/// </summary>
		private void InitTextureMap()
		{
			Dictionary<int, int> convert = new Dictionary<int, int>
			{
				{ 2, 1 }, { 8, 2 }, { 10, 3 }, { 11, 4 }, { 16, 5 }, { 18, 6 }, { 22, 7 }, { 24, 8 }, { 26, 9 }, { 27, 10 },
				{ 30, 11 }, { 31, 12 }, { 64, 13 }, { 66, 14 }, { 72, 15 }, { 74, 16 }, { 75, 17 }, { 80, 18 }, { 82, 19 },
				{ 86, 20 }, { 88, 21 }, { 90, 22 }, { 91, 23 }, { 94, 24 }, { 95, 25 }, { 104, 26 }, { 106, 27 }, { 107, 28 },
				{ 120, 2 }, { 122, 30 }, { 123, 31 }, { 126, 32 }, { 127, 33 }, { 208, 34 }, { 210, 35 }, { 214, 36 }, { 216, 37 },
				{ 218, 38 }, { 219, 39 }, { 222, 40 }, { 223, 41 }, { 248, 42 }, { 250, 43 }, { 251, 44 }, { 254, 45 }, { 255, 46 }, { 0, 47 }
			};

			for (int i = 0; i < tileLayout.Length; i++)
			{
				for (int j = 0; j < tileLayout.Length; j++)
				{
					//If not an edge case
					if(i > 0 && i < tileLayout.GetLength(0)-1 && j > 0 && j < tileLayout.GetLength(1)-1)
					{
						if(tileLayout[i,j] != TileType.WALL)
						{
							int north, northeast, northwest;
							int west;
							int east;
							int south, southeast, southwest;

							//Find north value
							if (tileLayout[i, j - 1] != TileType.WALL)
							{
								north = 1;
							}
							else north = 0;

							//Find west value
							if (tileLayout[i - 1, j] != TileType.WALL)
							{
								west = 1;
							}
							else west = 0;

							//Find east value
							if (tileLayout[i + 1, j] != TileType.WALL)
							{
								east = 1;

							}
							else east = 0;

							//Find south value
							if (tileLayout[i, j + 1] != TileType.WALL)
							{
								south = 1;
							}
							else south = 0;

							//Find northeast value
							if (tileLayout[i + 1, j - 1] != TileType.WALL)
							{
								if (north == 0 || east == 0)
								{
									northeast = 0;
								}
								else northeast = 1;
							}
							else northeast = 0;

							//Find northwest
							if (tileLayout[i - 1, j - 1] != TileType.WALL)
							{
								if (north == 0 || west == 0)
								{
									northwest = 0;
								}
								else northwest = 1;
							}
							else northwest = 0;

							//Find southeast
							if (tileLayout[i + 1, j + 1] != TileType.WALL)
							{
								if (south == 0 || east == 0)
								{
									southeast = 0;
								}
								else southeast = 1;
							}
							else southeast = 0;

							//Find southwest
							if (tileLayout[i - 1, j + 1] != TileType.WALL)
							{
								if (south == 0 || west == 0)
								{
									southwest = 0;
								}
								else southwest = 1;
							}
							else southwest = 0;

							//Assign value to matrix
							int bit = (1*northwest) + (2*north) + (4*northeast) + (8*west) + (16*east) + (32*southwest) + (64*south) + (128*southeast);
							textureLayout[i, j] = convert[bit];
						}
						else
						{
							
						}
					}
				}
			}
		}

		/// <summary>
		/// Get the rectangle for a tileset
		/// </summary>
		private Rectangle GetSourceRectangle(Texture2D tileSetTexture, int tileIndex)
		{
			int col = 0;
			int row = 0;
			for (int i = 0; i<tileIndex; i++)
			{
				col++;
				if (col == tileSetTexture.Width / tileWidth)
				{
					col = 0;
					row++;
				}
			}

			return new Rectangle((col * tileWidth), (row * tileHeight), tileWidth, tileHeight);
		}
	}
}
