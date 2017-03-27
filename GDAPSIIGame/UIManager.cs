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
        //Instantiate Textures and Rectangles for healthbox
        private Texture2D healthbarBackground;
        private Texture2D healthbarForeground;
        private Rectangle healthbarBackgroundBounding;
        private Rectangle healthbarForegroundBounding;
        //X and Y screen values for drawing the healthbox
        private int healthbarX;
        private int healthbarY;
        //Height and Width values for the default full healthbox
        private int healthBarWidth;
        private int healthBarHeight;
        //Creating the max health for the player for draw reference **WILL BE UPDATED WITH PLAYER PROPERTY**
        private int playerMaxHealth = Player.Instance.Health;

        internal void LoadContent(ContentManager Content)
        {
            //load textures for the healthbox
            healthbarBackground = Content.Load<Texture2D>("blackbar");
            healthbarForeground = Content.Load<Texture2D>("redbar");
            //set both the background and foreground of the healthbox to the same bounding area
            healthbarBackgroundBounding = healthbarForegroundBounding = new Rectangle(new Point(10, 10), new Point(32, 4));
            
        }

        public void Update(GameTime gameTime)
        {
            //If the health isin't at max:
            if (Player.Instance.Health != playerMaxHealth)
            {
                
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
    }
}
