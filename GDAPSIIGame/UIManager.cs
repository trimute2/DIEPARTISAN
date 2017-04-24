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

		//Fading
		private float fadeTimer;
		private float fade;

		private static UIManager instance;

		/// <summary>
		/// Singleton access
		/// </summary>
		/// <returns></returns>
		static public UIManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new UIManager();
				}
				return instance;
			}
		}

		private UIManager()
		{ }

		public bool Fade
		{
			get { return fadeTimer > 0; }
			set
			{
				if (value)
				{
					fadeTimer = 2f;
					fade = 1f;
				}
				else
				{
					fadeTimer = 2f;
					fade = 0;
				}
			}
		}

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
			//Fadein
			if (MapManager.Instance.State == MapState.Enter && Fade)
			{
				fadeTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
				fade -= ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000)/2;
			}
			else if (MapManager.Instance.State == MapState.Exit && Fade)
			{
				fadeTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
				fade += ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000) / 2;
			}
			else if(MapManager.Instance.State == MapState.Enter)
			{
				MapManager.Instance.State = MapState.Play;
			}

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
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            spriteBatch.Draw(healthbarBackground, healthbarBackgroundBounding, Color.White);
            spriteBatch.Draw(healthbarForeground, healthbarForegroundBounding, Color.White);

			if (MapManager.Instance.State == MapState.Enter || MapManager.Instance.State == MapState.Exit)
			{
				spriteBatch.Draw(TextureManager.Instance.GetMenuTexture("Black"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), new Color(Color.White, fade));
			}

		}
    }
}
