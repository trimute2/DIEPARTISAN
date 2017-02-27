using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDAPSIIGame.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDAPSIIGame
{
	
    public class GameObject : ICollidable
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle boundingBox;
        private Vector2 scale;

		public GameObject(Texture2D texture, Vector2 position, Rectangle boundingBox) {
            this.texture = texture;
            this.position = position;
            this.boundingBox = boundingBox;
            //Console.WriteLine(texture.Width);
            //Console.WriteLine(boundingBox.Width);
            scale = new Vector2((float)boundingBox.Width/texture.Width,  (float)boundingBox.Height/texture.Height);
            //Console.WriteLine(scale);
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
            get { return texture; }
            set { texture = value; }
        }

		public Rectangle BoundingBox
		{
			get { return boundingBox; }
		}

        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture,
                position,
                null,
                null,
                Vector2.Zero,
                0.0f,
                scale,
                null,
                0);
        }

        public virtual void Update(GameTime gameTime)
        {
            boundingBox.X = (int)position.X;
            boundingBox.Y = (int)position.Y;
        }

        /// <summary>
        /// Check if the boundingboxes of two rectangles collide
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>True if the rectangles collide, false otherwise</returns>
        public bool Collide(ICollidable obj)
        {
            return BoundingBox.Intersects(obj.BoundingBox);
        }

		/// <summary>
		/// Check if the boundingboxes of two rectangles collide
		/// </summary>
		/// <param name="box">The rectangle to check</param>
		/// <returns>if the two rectangles collide</returns>
		public bool Collide(Rectangle box)
		{
			return BoundingBox.Intersects(box);
		}

		public virtual void OnCollision(ICollidable obj)
		{
			//throw new NotImplementedException();
		}
	}
}
