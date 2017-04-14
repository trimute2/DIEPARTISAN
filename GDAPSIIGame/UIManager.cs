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
        private int healthbarWidth;
        private int healthbarHeight;
		//Creating the max health for the player for draw reference **WILL BE UPDATED WITH PLAYER PROPERTY**
		private int playerMaxHealth;

        /// <summary>
        /// Load content from the UI Manager's assets.
        /// </summary>
        /// <param name="Content"></param>
        internal void LoadContent(ContentManager Content)
        {
			playerMaxHealth = Player.Instance.Health;
            //Set X and Y values for the healthbar, as well as values for the bounding box.
            healthbarX = 10;
            healthbarY = 10;
            healthbarWidth = 80;
            healthbarHeight = 20;
            //load textures for the healthbox
            healthbarBackground = Content.Load<Texture2D>("blackbar");
            healthbarForeground = Content.Load<Texture2D>("redbar");
            //set both the background and foreground of the healthbox to the same bounding area
            healthbarBackgroundBounding = healthbarForegroundBounding = new Rectangle(new Point(healthbarX, healthbarY), new Point(healthbarWidth, healthbarHeight));
		}

        /// <summary>
        /// Update the UI Manager once per frame. Managed through Game1.cs.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //If the health isin't at max:
            if (Player.Instance.Health != playerMaxHealth)
            {
                double percentHealth = (int)(Player.Instance.Health / playerMaxHealth);
                healthbarForegroundBounding.Width -= (int)(percentHealth * healthbarWidth);
            }
        }

        /// <summary>
        /// Draw the UI assets on the screen.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(healthbarBackground, healthbarBackgroundBounding, Color.White);
            spriteBatch.Draw(healthbarForeground, healthbarForegroundBounding, Color.White);
        }
    }
}
