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
		Color colorSelected;

		public bool Selected
		{
			get { return selected; }
			set { selected = value; }
		}


        public Button(Texture2D texture, Rectangle area, Color colorSelected, string text)
        {
            this.texture = texture;
            this.area = area;
            this.text = text;
			selected = false;
			this.colorSelected = colorSelected;
        }

		public Button(Texture2D texture, Rectangle area, Color colorSelected)
		{
			this.texture = texture;
			this.area = area;
			this.text = "";
			selected = false;
			this.colorSelected = colorSelected;
		}

		public bool Contains(Vector2 pos)
        {
            return area.Contains(pos);
        }

        public void Draw(SpriteBatch sb)
        {
			if(selected)
			{
				sb.Draw(texture, area, colorSelected);
			}
            else sb.Draw(texture, area, Color.White);

            sb.DrawString(TextureManager.Instance.GetFont("uifont"),text,area.Center.ToVector2(),Color.MediumSeaGreen);
        }
    }
}
