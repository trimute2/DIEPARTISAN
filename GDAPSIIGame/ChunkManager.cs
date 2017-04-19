using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDAPSIIGame
{
    class ChunkManager
    {
		static private ChunkManager instance;
		private List<Chunk> chunks;
        private int chunkNum;
        /// <summary>
		/// number of rows of chunks
		/// </summary>
		private int numRows;
        /// <summary>
        /// number of chunks per row
        /// </summary>
        private int cpr;
		private Wall top;
		private Wall right;
		private Wall bottom;
		private Wall left;

        private ChunkManager()
        {
			chunks = new List<Chunk>();
			//orignal code before ability to resize
			//int id = 0;
			//for (int i = 0; i < numrows; i++)
			//{
			//	for (int j = 0; j < cpr; j++)
			//	{
			//		chunks[id] = new chunk(
			//			new rectangle(640 * j, 640 * i, 640, 640),
			//			cpr, id);
			//		id++;
			//	}
			//}
			Resize(20);
		}

		public void Resize(int size)
		{
			chunks.Clear();
			chunkNum = size * size;
			numRows = size;
			cpr = size;
			int length =  640 * size;
			Texture2D tx = null;
			if (TextureManager.Instance.RoomTextures.ContainsKey("WallTexture"))
			{
				tx = TextureManager.Instance.RoomTextures["WallTexture"];
			}
			top = new Wall(tx, new Vector2(0, -32),
				new Rectangle(0, -32, length, 32));
			right = new Wall(tx, new Vector2(length, 0),
				new Rectangle(length, 0, 32, length));
			bottom = new Wall(tx, new Vector2(0, length),
				new Rectangle(0, length, length, 32));
			left = new Wall(tx, new Vector2(-32, 0),
				new Rectangle(-32, 0, 32, length));

			int ID = 0;
			for (int i = 0; i < numRows; i++)
			{
				for (int j = 0; j < cpr; j++)
				{
					chunks.Add(new Chunk(
						new Rectangle(640 * j, 640 * i, 640, 640),
						cpr, ID));
					ID++;
				}
			}
		}
		
		static public ChunkManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ChunkManager();
				}
				return instance;
			}
		}


		/// <summary>
		/// adds an object to chunks
		/// </summary>
		/// <param name="obj">the object to be added</param>
		public void Add(GameObject obj)
		{
			for(int i = 0; i < chunkNum; i++)
			{
				if (chunks[i].Contains(obj.Position))
				{
					chunks[i].Add(obj);
					i = chunkNum;
				}
			}
		}

		//old method
		/// <summary>
		/// adds game objects to chunks when the game is first initialized
		/// </summary>
		//private void FirstChunk(List<GameObject> allObjs)
		//{
		//	foreach (GameObject obj in allObjs)
		//	{
		//		for (int i = 0; i < chunkNum; i++)
		//		{
		//			if (chunks[i].Contains(obj.Position))
		//			{
		//				chunks[i].Add(obj);
		//				i = chunkNum;
		//			}
		//		}
		//	}
		//}


		//feel free to rename
		/// <summary>
		/// checks what chunks objects are in and moves them between chunks
		/// </summary>
		private void ChunkIt()
		{
			GameObject obj = null;
			int offset = 0;
			for (int i = 0; i < chunkNum; i++)
			{
				//cant use a foreach because sometime objects are removed, 
				for (int j = chunks[i].Objects.Count - 1; j >= 0; j--)
				{
					obj = chunks[i].Objects[j];
					if (!chunks[i].Contains(obj.Position))
					{
						offset = chunks[i].CheckAdjacency(obj.Position);
						if (i != offset) // in the case that somehow the objects offset is the chunk that its in do nothing
						{
							if (offset >= chunkNum)
							{
								//incase the offset is greater than the number of chunks
								//subtract the number of chunks from the offset
								//this is essentially how the chunks handle screen wrapping
								offset -= chunkNum;
							}else if(offset < 0)
							{
								offset += chunkNum;
							}
							chunks[offset].Add(obj);
							chunks[i].Remove(obj);
						}
					}
				}
			}
		}

		public void Bound()
		{
			foreach(Chunk c in chunks)
			{
				foreach(GameObject obj in c.Objects)
				{
					if(!(obj is Wall))
					{
						if (obj.Collide(top))
						{
							obj.OnCollision(top);
						}else if (obj.Collide(bottom))
						{
							obj.OnCollision(bottom);
						}
						if (obj.Collide(left))
						{
							obj.OnCollision(left);
						}else if (obj.Collide(right))
						{
							obj.OnCollision(right);
						}
					}
				}
			}

		}

		/// <summary>
		/// checks objects that overlap one or more chunks
		/// </summary>
		public void ChunkOverlap()
		{
			int offset = 0;
			List<GameObject> gol = null;
			for(int i = 0; i < chunkNum; i++)
			{
				offset = Offset(i + 1);
				gol = chunks[i].GetOverlap(chunks[offset]);
				if (gol.Count > 0)
				{
					chunks[offset].CollideAgainst(gol);
				}
				offset = Offset(i + cpr);
				gol = chunks[i].GetOverlap(chunks[offset]);
				if (gol.Count > 0)
				{
					chunks[offset].CollideAgainst(gol);
				}
				offset = Offset(i + cpr + 1);
				gol = chunks[i].GetOverlap(chunks[offset]);
				if(gol.Count > 0)
				{
					chunks[offset].CollideAgainst(gol);
				}

			}
		}

		/// <summary>
		/// makes sure a number is within bounds of the chunks array
		/// </summary>
		/// <param name="off">the number to check against</param>
		/// <returns></returns>
		private int Offset(int off)
		{
			if(off >= chunkNum)
			{
				return off - chunkNum;
			}else if(off < 0)
			{
				return off + chunkNum;
			}
			return off;
		}

		public void Update()
		{
			Bound();
			ChunkIt();
			foreach(Chunk chunk in chunks)
			{
				chunk.RemoveInactive();
				chunk.CollideObjects();
			}
			if (chunkNum > 1)
			{
				ChunkOverlap();
			}
		}

		public void DeleteWalls()
		{
			foreach (Chunk chunk in chunks)
			{
				foreach (GameObject obj in chunk.Objects)
				{
					if (obj is Wall)
					{
						obj.IsActive = false;
					}
				}
			}
		}

	}
}
