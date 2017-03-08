﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDAPSIIGame.Map
{
    class Camera
    {
        private Rectangle size;
        private static Camera instance;

        private Camera()
        {
            
        }

        public void setPosition(Viewport vp)
        {
            float x = Player.Instance.X - vp.Width / 2;
            float y = Player.Instance.Y - vp.Height / 2;
            size = new Rectangle((int)x, (int)y, vp.Width, vp.Height);
        }


        public static Camera Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Camera();
                }
                return instance;
            }
        }

        public int X
        {
            get { return size.X; }
            set { size.X = value; }
        }

        public int Y
        {
            get { return size.Y; }
            set { size.Y = value; }
        }

        public Rectangle Bounds
        {
            get { return size; }
        }

        public Vector2 GetViewportPosition(GameObject go)
        {
            return new Vector2(size.X - go.X, size.Y - go.Y);
        }

        /// <summary>
        /// Tells if a camera vectored coordinate is within the camera's bounds
        /// </summary>
        /// <param name="v">Vector in camera coords</param>
        /// <returns>true/false if vector is within camera bounds</returns>
        public bool InBounds(Vector2 v)
        {
            return v.X < size.Width && v.Y < size.Y;
        }
    }
}
