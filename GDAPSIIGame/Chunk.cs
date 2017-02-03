using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GDAPSIIGame
{
	class Chunk
	{
		/// <summary>
		/// list of all GameObjects in the chunk
		/// </summary>
		private List<GameObject> objects;
		/// <summary>
		/// the area of the chunk
		/// </summary>
		private Rectangle area;
		/// <summary>
		/// the number of chunks per row
		/// </summary>
		private int cpr;
		/// <summary>
		/// the ID of the chunk
		/// </summary>
		private int chunkID;

		/// <summary>
		/// the list of all GameObjects in the chunk
		/// </summary>
		public List<GameObject> Objects
		{
			get { return objects; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="area">the area of the chunk</param>
		/// <param name="cpr">the number of chunks per row</param>
		/// <param name="ID">the ID of the chunk</param>
		public Chunk(Rectangle area,int cpr, int ID)
		{
			this.area = area;
			this.cpr = cpr;
			chunkID = ID;
			objects = new List<GameObject>();
		}

		/// <summary>
		/// Checks whether or no a point is within the area of the chunk
		/// </summary>
		/// <param name="pos">the point to be checked</param>
		/// <returns>if the point is in the chunk</returns>
		public bool Contains(Vector2 pos)
		{
			return area.Contains(pos);
		}

		/// <summary>
		/// takes in a point and determins the ID of the chunk that contains it
		/// </summary>
		/// <param name="pos">the position to be checked</param>
		/// <returns>the ID of the chunk that contains it</returns>
		public int CheckAdjacency(Vector2 pos)
		{
			if (Contains(pos))
			{
				return 0;
			}
			int adjChunk = 0;
			if(pos.X > area.Right)
			{
				adjChunk += 1;
			}else if(pos.X < area.Left)
			{
				adjChunk -= 1;
			}
			if(pos.Y < area.Top)
			{
				adjChunk -= cpr;
			}else if(pos.Y > area.Bottom)
			{
				adjChunk += cpr;
			}
			return adjChunk + chunkID;
		}
		
		// i put this here so that to add an object you didnt have to type out Chunk.Objects.Add(obj); 
		/// <summary>
		/// adds a game object to the chunks list of objects
		/// </summary>
		/// <param name="obj">the object to be added</param>
		public void Add(GameObject obj)
		{
			objects.Add(obj);
		}

		/// <summary>
		/// removes an object from the chunks list of objects
		/// </summary>
		/// <param name="obj"></param>
		public void Remove(GameObject obj)
		{
			objects.Remove(obj);
		}
	}
}
