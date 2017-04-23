using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDAPSIIGame
{
	class Wall : GameObject
	{

		private bool bellow;

		public bool Bellow
		{
			get { return bellow; }
			set { bellow = value; }
		}

		public Wall(Texture2D texture, Vector2 position, Rectangle boundingBox) : base(texture, position, boundingBox)
		{
			bellow = false;
		}
	}
}
