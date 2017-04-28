using GDAPSIIGame.Pods;
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
        private Vector2 scoreBounding;
        private SpriteFont font;
        //Instantiate Textures and Rectangles for healthbox
        private Texture2D healthbarBackground;
        private Texture2D healthbarForeground;
        private Texture2D healthbarHead;
        private Texture2D ammoIcon;
        private Texture2D pistolIcon;
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
            //Set X and Y values for the game score.
            scoreBounding.X = 10;
            scoreBounding.Y = 10;
            //load font texture
            font = Content.Load<SpriteFont>("font");
            //load textures for the healthbox
            healthbarBackground = Content.Load<Texture2D>("healthbarbg");
            healthbarForeground = Content.Load<Texture2D>("health");
            healthbarHead = Content.Load<Texture2D>("healthhead");
            ammoIcon = Content.Load<Texture2D>("bullets");
            pistolIcon = Content.Load<Texture2D>("pistolicon");
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
        }

        /// <summary>
        /// Draw the UI assets on the screen.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            spriteBatch.Draw(healthbarBackground, new Vector2(40, 20), Color.White);
            spriteBatch.Draw(healthbarForeground, new Vector2(43, 23), Color.White);
            spriteBatch.Draw(healthbarHead, new Vector2(14, 10), Color.White);
            
            if(Player.Instance.CurrWeapon is Weapons.Pistol)
            {
                spriteBatch.Draw(pistolIcon, new Vector2(14, 50), Color.White);
            }

            spriteBatch.Draw(ammoIcon, new Vector2(14, 110), Color.White);

            spriteBatch.DrawString(font, PodManager.Instance.GlobalScore.ToString(), new Vector2(50, 50), Color.Red);
            spriteBatch.DrawString(font, Player.Instance.ScoreMultiplier.ToString(), new Vector2(50, 100), Color.Red);

            if (MapManager.Instance.State == MapState.Enter || MapManager.Instance.State == MapState.Exit)
			{
				spriteBatch.Draw(TextureManager.Instance.GetMenuTexture("Black"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), new Color(Color.White, fade));
			}

		}
    }
}
