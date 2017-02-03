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
        private Vector2 position;
        private Rectangle boundingBox;

        public GameObject(Texture2D texture, Vector2 position, Rectangle boundingBox) {
            this.texture = texture;
            this.position = position;
            this.boundingBox = boundingBox;
        }

        public float X {
            get { return position.X; }
            set { position.X = value; }
        }

        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public Texture2D Texture
        {
            set { texture = value; }
        }

		public Rectangle BoundingBox
		{
			get { return boundingBox; }
		}

        public virtual void Draw(SpriteBatch sb) {
            sb.Draw(
                texture,
                position,
                Color.White);
        }

    }
}
