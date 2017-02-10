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

        public ChunkManager(int chunkWidth, int chunkHeight, List<GameObject> allObjs)
        {
			chunks = new Chunk[chunkNum];
			int ID = 0;
			for(int i = 0; i < numRows; i++)
			{
				for (int j = 0; j< cpr; j++)
				{
					chunks[ID] = new Chunk(
						new Rectangle(chunkWidth * j, chunkHeight * i, chunkWidth, chunkHeight),
						cpr, ID);
					ID++;
				}
			}
			FirstChunk(allObjs);
		}

		/// <summary>
		/// adds game objects to chunks when the game is first initialized
		/// </summary>
		//has yet to be tested
		private void FirstChunk(List<GameObject> allObjs)
		{
			foreach (GameObject obj in allObjs)
			{
				for (int i = 0; i < chunkNum; i++)
				{
					if (chunks[i].Contains(obj.Position))
					{
						chunks[i].Add(obj);
						i = chunkNum;
					}
				}
			}
		}

		/// <summary>
		/// checks what chunks objects are in and moves them between chunks
		/// </summary>
		// has yet to be tested
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

		public void Upadate()
		{
			ChunkIt();
			foreach(Chunk chunk in chunks)
			{
				chunk.CollideObjects();
			}
		}

	}
}
