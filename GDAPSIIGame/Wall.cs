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
		public Wall(Texture2D texture, Vector2 position, Rectangle boundingBox) : base(texture, position, boundingBox)
		{
		}
	}
}
