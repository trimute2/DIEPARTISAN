using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDAPSIIGame
{
    class UIManager
    {
        private Texture2D healthbarBackground;
        private Texture2D healthbarForeground;
        private Rectangle healthbarBackgroundBounding;
        private Rectangle healthbarForegroundBounding;
        private int healthbarX;
        private int healthbarY;

        internal void LoadContent(ContentManager Content)
        {
            healthbarBackground = Content.Load<Texture2D>("blackbar");
            healthbarForeground = Content.Load<Texture2D>("redbar");
            healthbarBackgroundBounding = healthbarForegroundBounding = new Rectangle(new Point(10, 10), new Point(32, 4));
            
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
    }
}
