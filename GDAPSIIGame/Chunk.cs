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
		private int recLevl;

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
			recLevl = 0;
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
		/// checks wheather a bounding box is contained within the area of the chunk
		/// </summary>
		/// <param name="rec">the bounding box to be checked</param>
		/// <returns>if the bounding box overlaps the chunk</returns>
		public bool Intersects(Rectangle rec)
		{
			return area.Intersects(rec);
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

		public void CollideObjects()
		{
			for(int i = 0; i < objects.Count-1; i++)
			{
				for(int j = i+1; j < objects.Count; j++)
				{
					if (objects[i].Collide(objects[j]))
					{
						objects[i].OnCollision(objects[j]);
						objects[j].OnCollision(objects[i]);
					}
				}
			}
			for(int i = 0; i < Objects.Count; i++)
			{
				if(objects[i] is Wall)
				{
					recLevl = 0;
					ContainCollision(objects[i]);
				}
			}
		}

		public void RemoveInactive()
		{
			for(int i = objects.Count-1; i >= 0; i--)
			{
				if (!((objects[i]).IsActive))
				{
					objects.Remove(objects[i]);
				}
			}
		}

		/// <summary>
		/// checks an object not included in the chunk against objects in the chunk
		/// this is for cases where objects overlap two chunks
		/// </summary>
		/// <param name="obj">an object to check against</param>
		public void CollideAgainst(GameObject obj)
		{
			foreach(GameObject io in objects)
			{
				if (io.Collide(obj))
				{
					io.OnCollision(obj);
					obj.OnCollision(io);
				}
			}
		}

		/// <summary>
		/// checks a list of objects not included in the chunk against objects in the chunk
		/// this is for cases where objects overlap two chunks
		/// </summary>
		/// <param name="obj">the list of objects to check against</param>
		public void CollideAgainst(List<GameObject> objs)
		{
			foreach(GameObject obj in objs)
			{
				CollideAgainst(obj);
			}
		}

		/// <summary>
		/// gets objects in this chunk that overlap with another chunk
		/// </summary>
		/// <param name="ch">a chunk to check against</param>
		/// <returns>a list of objects that collide with both chunks</returns>
		public List<GameObject> GetOverlap(Chunk ch)
		{
			List<GameObject> overlap = new List<GameObject>();
			foreach(GameObject obj in Objects)
			{
				if (ch.Intersects(obj.BoundingBox))
				{
					overlap.Add(obj);
				}
			}
			return overlap;
		}

		public void ContainCollision(GameObject obj)
		{
			foreach(GameObject otherObj in objects)
			{
				if(obj.Collide(otherObj) && !(otherObj is Projectile) && otherObj != obj)
				{
					otherObj.OnCollision(obj);
					recLevl++;
					ContainCollision(otherObj, obj);
				}
			}
		}

		private void ContainCollision(GameObject obj, GameObject caller)
		{
			foreach (GameObject otherObj in objects)
			{
				if (obj.Collide(otherObj) && !(otherObj is Projectile) && otherObj != caller && otherObj != obj)
				{
					otherObj.OnCollision(obj);
					recLevl++;
					ContainCollision(otherObj , obj);
				}
			}
		}

	}
}
