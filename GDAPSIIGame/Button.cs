using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDAPSIIGame
{
    class Button
    {

        private Texture2D texture;
        private Rectangle area;
		private bool selected;
        private string text;

		public bool Selected
		{
			set { selected = value; }
		}


        public Button(Texture2D texture, Rectangle area, string text)
        {
            this.texture = texture;
            this.area = area;
            this.text = text;
			selected = false;
        }
        
        public bool Contains(Vector2 pos)
        {
            return area.Contains(pos);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, area, Color.White);
            sb.DrawString(TextureManager.Instance.GetFont("uifont"),text,area.Center.ToVector2(),Color.MediumSeaGreen);
        }
    }
}
