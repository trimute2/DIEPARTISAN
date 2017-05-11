using GDAPSIIGame.Pods;
using GDAPSIIGame.Map;
using GDAPSIIGame.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GDAPSIIGame
{
    class UIManager
    {
        private SpriteFont font;
        //Instantiate Textures and Rectangles for healthbox
        private Texture2D healthbarBackground;
        private Texture2D healthbarForeground;
        private Texture2D healthbarHead;
        private Texture2D ammoIcon;
        private Texture2D pistolIcon;
        private Texture2D shotgunIcon;
        private Texture2D rifleIcon;
        private Texture2D reloadIcon;
        private Texture2D clockIcon;
        private Texture2D enemiesLeftIcon;

        //Creating the max health for the player for draw reference
        private int playerMaxHealth;
        private int mapTime;
        private float healthBarWidth;

		//Fading
		private int splashFade;
		private float splashTimer;
        private float fadeTimer;
        private float fade;
        private float scalar;

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

        /// <summary>
        /// empty private instantiation
        /// </summary>
        private UIManager()
        {
			splashFade = 0;
			splashTimer = 1.5f;
		}

        /// <summary>
        /// Sets the map size to calculate the map time
        /// </summary>
        /// <param name="mapSize"></param>
        public void SetMapSize(int mapSize)
        {
            mapTime = mapSize;
        }

		public int SplashScreen
		{
			get { return splashFade; }
			set
			{
				splashFade = value;
				switch (value)
				{
					case 0:
						splashTimer = 1.5f;
						break;
					case 1:
						Fade = true;
						break;
					case 2:
						splashTimer = 1.5f;
						break;
					case 3:
						Fade = false;
						break;
					case 4:
						splashTimer = 0.5f;
						break;
				}
			}
		}

        /// <summary>
        /// Screen fade for transitions
        /// </summary>
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
            //load font texture
            font = Content.Load<SpriteFont>("font");
            //load textures for the healthbox
            healthbarBackground = Content.Load<Texture2D>("healthbarbg");
            healthbarForeground = Content.Load<Texture2D>("health");
            healthbarHead = Content.Load<Texture2D>("healthhead");
            ammoIcon = Content.Load<Texture2D>("bullets");
            pistolIcon = Content.Load<Texture2D>("pistolicon");
            shotgunIcon = Content.Load<Texture2D>("shotgunicon");
            rifleIcon = Content.Load<Texture2D>("rifleicon");
            reloadIcon = Content.Load<Texture2D>("reloadIcon");
            font = Content.Load<SpriteFont>("UIText");
            clockIcon = Content.Load<Texture2D>("clockicon");
            enemiesLeftIcon = Content.Load<Texture2D>("enemieslefticon");
        }

		/// <summary>
		/// Update the splash screens at the beggining of the game
		/// </summary>
		public void UpdateSplashScreens(GameTime gameTime)
		{
			if (splashFade == 0 && splashTimer > 0)
			{
				splashTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
			}
			else if (splashFade == 0 && splashTimer <= 0)
			{
				splashFade = 1;
				Fade = true;
			}
			else if (splashFade == 1 && Fade)
			{
				fadeTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
				fade -= ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000) / 2;
			}
			else if (splashFade == 1 && !Fade)
			{
				splashFade = 2;
				splashTimer = 1.5f;
			}
			else if (splashFade == 2 && splashTimer > 0)
			{
				splashTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
			}
			else if (splashFade == 2 && splashTimer <= 0)
			{
				splashFade = 3;
				Fade = false;
			}
			else if (splashFade == 3 && Fade)
			{
				fadeTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
				fade += ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000) / 2;
			}
			else if (splashFade == 3 && !Fade)
			{
				splashFade = 4;
				splashTimer = 0.5f;
			}
			else if (splashFade == 4 && splashTimer > 0)
			{
				splashTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
			}
			else if (splashFade == 4 && splashTimer <= 0)
			{
				splashFade = 5;
			}
		}

        /// <summary>
        /// Update the UI Manager once per frame. Managed through Game1.cs.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            scalar += .3f;

            //Fadein
            if (MapManager.Instance.State == MapState.Enter && Fade)
            {
                fadeTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                fade -= ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000) / 2;
            }
            else if (MapManager.Instance.State == MapState.Exit && Fade)
            {
                fadeTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                fade += ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000) / 2;
            }
            else if (MapManager.Instance.State == MapState.Enter)
            {
                MapManager.Instance.State = MapState.Play;
            }

            if (Player.Instance.Health != playerMaxHealth)
            {
                double percent = (double)Player.Instance.Health / (double)playerMaxHealth;
                healthBarWidth = (float)(percent * healthbarForeground.Width);
                //Debug.WriteLine("HEALTHBAR WIDTH: " + percent + " * " + healthbarForeground.Width + " = " + healthBarWidth);
            }
            else
            {
                healthBarWidth = healthbarForeground.Width;
            }
        }

		/// <summary>
		/// Draw the splash screens at the beggining of the game
		/// </summary>
		public void DrawSplashScreens(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
		{
			if (splashFade == 0)
			{
				spriteBatch.Draw(
					TextureManager.Instance.GetMenuTexture("Black2"),
					new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
					Color.White
					);
			}
			else if (splashFade == 1)
			{
				spriteBatch.Draw(
					TextureManager.Instance.GetMenuTexture("Brickchop"),
					new Rectangle((GraphicsDevice.Viewport.Width / 2) - TextureManager.Instance.GetMenuTexture("Brickchop").Width / 6,
						(GraphicsDevice.Viewport.Height / 2) - TextureManager.Instance.GetMenuTexture("Brickchop").Height / 6,
						TextureManager.Instance.GetMenuTexture("Brickchop").Width / 3, TextureManager.Instance.GetMenuTexture("Brickchop").Height / 3),
					Color.White
					);

				spriteBatch.Draw(
					TextureManager.Instance.GetMenuTexture("Black2"),
					new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
					new Color(Color.White, fade)
					);
			}
			else if (splashFade == 2)
			{
				spriteBatch.Draw(
					TextureManager.Instance.GetMenuTexture("Black2"),
					new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
					Color.White
					);

				spriteBatch.Draw(
					TextureManager.Instance.GetMenuTexture("Brickchop"),
					new Rectangle((GraphicsDevice.Viewport.Width / 2) - TextureManager.Instance.GetMenuTexture("Brickchop").Width / 6,
						(GraphicsDevice.Viewport.Height / 2) - TextureManager.Instance.GetMenuTexture("Brickchop").Height / 6,
						TextureManager.Instance.GetMenuTexture("Brickchop").Width / 3, TextureManager.Instance.GetMenuTexture("Brickchop").Height / 3),
					Color.White
					);
			}
			else if (splashFade == 3)
			{
				spriteBatch.Draw(
					TextureManager.Instance.GetMenuTexture("Brickchop"),
					new Rectangle((GraphicsDevice.Viewport.Width / 2) - TextureManager.Instance.GetMenuTexture("Brickchop").Width / 6,
						(GraphicsDevice.Viewport.Height / 2) - TextureManager.Instance.GetMenuTexture("Brickchop").Height / 6,
						TextureManager.Instance.GetMenuTexture("Brickchop").Width / 3, TextureManager.Instance.GetMenuTexture("Brickchop").Height / 3),
					Color.White
					);

				spriteBatch.Draw(
					TextureManager.Instance.GetMenuTexture("Black2"),
					new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
					new Color(Color.White, fade)
					);
			}
			else if (splashFade == 4)
			{
				spriteBatch.Draw(
					TextureManager.Instance.GetMenuTexture("Black2"),
					new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
					new Color(Color.White, fade)
					);
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

            spriteBatch.Draw(
                texture: healthbarForeground,
                position: new Vector2(44, 24),
                sourceRectangle: new Rectangle(new Point(0, 0), new Point((int)healthBarWidth, 15)),
                color: Color.White
                );

            spriteBatch.Draw(healthbarHead, new Vector2(14, 9), Color.White);

            if (Player.Instance.CurrWeapon is Weapons.Pistol)
            {
                spriteBatch.Draw(
                    pistolIcon,
                    new Vector2(14, 49),
                    Color.White
                    );
            }
            else
            if (Player.Instance.CurrWeapon is Weapons.Shotgun)
            {
                spriteBatch.Draw(
                    shotgunIcon,
                    new Vector2(14, 49),
                    Color.White
                    );
            }
            else
            if (Player.Instance.CurrWeapon is Weapons.Rifle)
            {
                spriteBatch.Draw(
                    rifleIcon,
                    new Vector2(14, 49),
                    Color.White
                    );
            }

            spriteBatch.Draw(ammoIcon, new Vector2(14, 109), Color.White);

            spriteBatch.Draw(
                texture: clockIcon,
                position: new Vector2(1100, 10)
                );

            if(mapTime != 0)
            {
                spriteBatch.DrawString(
                    spriteFont: font,
                    text: (int)(mapTime - PodManager.Instance.LevelTime) + "",
                    position: new Vector2(1040, 14),
                    color: Color.White
                    );
            }

            spriteBatch.Draw(
                texture: enemiesLeftIcon,
                position: new Vector2(14, 590)
                );

            spriteBatch.DrawString(
                spriteFont: font,
                text: EntityManager.Instance.NumEnemies + "",
                position: new Vector2(60, 594),
                color: Color.White
                );

            spriteBatch.DrawString(
                spriteFont: font,
                text: Player.Instance.CurrWeapon.CurrAmmo + "",
                position: new Vector2(57, 113),
                color: Color.Black
                );

            spriteBatch.DrawString(
                spriteFont: font,
                text: Player.Instance.CurrWeapon.CurrAmmo + "",
                position: new Vector2(60, 110),
                color: Color.White
                );

            if (Player.Instance.CurrWeapon.Reload)
            {
                spriteBatch.Draw(
                    texture: reloadIcon,
                    origin: new Vector2(reloadIcon.Width / 2, reloadIcon.Height / 2),
                    position: Camera.Instance.GetViewportPosition(Player.Instance),
                    rotation: scalar
                    );
            }

            if (MapManager.Instance.State == MapState.Enter || MapManager.Instance.State == MapState.Exit)
            {
                spriteBatch.Draw(
                    TextureManager.Instance.GetMenuTexture("Black"),
                    new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
                    new Color(Color.White, fade)
                    );
            }
        }
    }
}
