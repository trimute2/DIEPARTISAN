﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDAPSIIGame.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GDAPSIIGame.Map;

namespace GDAPSIIGame
{
	
    public class GameObject : ICollidable
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle boundingBox;
        private Vector2 scale;
		protected Color color;
		public bool active;

		public GameObject(Texture2D texture, Vector2 position, Rectangle boundingBox) {
			this.color = Color.White;
            this.texture = texture;
            this.position = position;
            this.boundingBox = boundingBox;
			//Console.WriteLine(texture.Width);
			//Console.WriteLine(boundingBox.Width);
			
			if (texture != null)
			{
				scale = new Vector2((float)boundingBox.Width / texture.Width, (float)boundingBox.Height / texture.Height);
			}
            //Console.WriteLine(scale);
			active = true;
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
			set { position = value; }
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

		public bool IsActive
		{
			get { return active; }
			set { active = value; }
		}

		public bool Drawable()
		{
			Vector2 camPos = Camera.Instance.GetViewportPosition(this);
			return Camera.Instance.InBounds(camPos);
		}

		public virtual void Draw(SpriteBatch spriteBatch) {
            Vector2 camPos = Camera.Instance.GetViewportPosition(this);
            if (Camera.Instance.InBounds(camPos))
            {
                spriteBatch.Draw(texture,
                    camPos,
                    null,
                    null,
                    Vector2.Zero,
                    0.0f,
                    scale,
                    color,
                    0);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            boundingBox.X = (int)position.X;
            boundingBox.Y = (int)position.Y;
        }

		public void ResetBound()
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
