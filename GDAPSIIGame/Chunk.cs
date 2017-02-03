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
		private List<GameObject> obj;
		Rectangle area;

		public List<GameObject> Objects
		{
			get { return obj; }
		}

		public Chunk(Rectangle area)
		{
			this.area = area;
			obj = new List<GameObject>();
		}

		public bool Contains(Vector2 pos)
		{
			return area.Contains(pos);
		}
		
	}
}
