using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace GDAPSIIGame
{
    public class GameObject
    {
        private Texture2D texture;
        private Rectangle position;

        public GameObject(Texture2D texture, Rectangle position) {
            this.texture = texture;
            this.position = position;
        }

        public int X {
            get { return position.X; }
            set { position.X = value; }
        }

        public int Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public Rectangle Position
        {
            get { return position; }
        }

        public Texture2D Texture
        {
            set { texture = value; }
        }

        public virtual void Draw(SpriteBatch sb) {
            sb.Draw(
                texture,
                position,
                Color.White);
        }

    }
}
