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
        private Texture2D reloadIcon;
        private Texture2D clockIcon;
        //Creating the max health for the player for draw reference
        private int playerMaxHealth;
        private int mapTime;
        private float healthBarWidth;

        //Fading
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
        { }

        /// <summary>
        /// Sets the map size to calculate the map time
        /// </summary>
        /// <param name="mapSize"></param>
        public void SetMapSize(int mapSize)
        {
            mapTime = mapSize;
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
            reloadIcon = Content.Load<Texture2D>("reloadIcon");
            font = Content.Load<SpriteFont>("UIText");
            clockIcon = Content.Load<Texture2D>("clockicon");
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

            spriteBatch.Draw(ammoIcon, new Vector2(14, 109), Color.White);

            if (MapManager.Instance.State == MapState.Enter || MapManager.Instance.State == MapState.Exit)
            {
                spriteBatch.Draw(
                    TextureManager.Instance.GetMenuTexture("Black"),
                    new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
                    new Color(Color.White, fade)
                    );
            }

            spriteBatch.Draw(
                texture: clockIcon,
                position: new Vector2(1100, 10)
                );

            if(mapTime != 0)
            {
                spriteBatch.DrawString(
                    spriteFont: font,
                    text: (int)(mapTime - PodManager.Instance.LevelTime) + "",
                    position: new Vector2(1050, 14),
                    color: Color.White
                    );
            }

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
        }
    }
}
