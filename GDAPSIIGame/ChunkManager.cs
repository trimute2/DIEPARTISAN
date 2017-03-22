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
        private Chunk[] chunks;
        private const int chunkNum = 8;
        /// <summary>
		/// number of rows of chunks
		/// </summary>
		private const int numRows = 2;
        /// <summary>
        /// number of chunks per row
        /// </summary>
        private int cpr = 4;

        private ChunkManager()
        {
			chunks = new Chunk[chunkNum];
			int ID = 0;
			for(int i = 0; i < numRows; i++)
			{
				for (int j = 0; j< cpr; j++)
				{
					chunks[ID] = new Chunk(
						new Rectangle(200 * j, 240 * i, 200, 240),
						cpr, ID);
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
			ChunkIt();
			foreach(Chunk chunk in chunks)
			{
				chunk.RemoveInactive();
				chunk.CollideObjects();
			}
			ChunkOverlap();
		}

	}
}
