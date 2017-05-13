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
		private bool prevSelected;
		private string text;
		Color colorSelected;

		public bool Selected
		{
			get { return selected; }
			set { selected = value; }
		}

		public bool PrevSelected
		{
			get { return prevSelected; }
			set { prevSelected = value; }
		}

		public String Text
		{
			get { return text; }
			set { text = value; }
		}

		public Rectangle Area
		{
			get { return area; }
		}

		public int Y
		{
			get { return area.Location.Y; }
			set { area.Location = new Point(area.Location.X, value); }
		}

		public int X
		{
			get { return area.Location.X; }
			set { area.Location = new Point(value, area.Location.Y); }
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

            sb.DrawString(TextureManager.Instance.GetFont("uifont"),
				text,
				new Vector2(area.Center.ToVector2().X - area.Width / 2, area.Center.ToVector2().Y - TextureManager.Instance.GetFont("uifont").LineSpacing/4),
				Color.MediumSeaGreen, 
				0, 
				Vector2.Zero, 
				0.5f, 
				SpriteEffects.None, 0);
        }
    }
}
