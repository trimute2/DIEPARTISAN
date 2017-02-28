using Microsoft.Xna.Framework;
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
            set { size.X = value; }
        }

        public int Y
        {
            set { size.Y = value; }
        }

        public Rectangle Bounds
        {
            get { return size; }
        }
    }
}
