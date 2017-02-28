using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using GDAPSIIGame.Map;

namespace GDAPSIIGame
{
    enum GameState { MainMenu, GamePlay, PauseMenu}
    
    public class Game1 : Game
    {
        //Fields
        EntityManager entityManager;
        ProjectileManager projectileManager;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState kbState;
		KeyboardState previousKbState;
		ChunkManager chunkManager;
        MapManager mapManager;
        Texture2D theTexture;
        Camera mainCamera;
		GameState gameState;

		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
			this.IsMouseVisible = true;
			graphics.PreferredBackBufferWidth = 2000;
			graphics.ApplyChanges();
			

            //Initialize entity manager
            entityManager = EntityManager.Instance;

			//Initialize the chunk manager
			chunkManager = ChunkManager.Instance;

            //Initialize projectile manager
            projectileManager = ProjectileManager.Instance;

            //Initialize map manager
            mapManager = new MapManager();


            //Initialize keyboards
            kbState = new KeyboardState();
			previousKbState = kbState;

			gameState = GameState.MainMenu;
			base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Load entities
            entityManager.LoadContent(Content);
            //Load projectiles
            projectileManager.LoadContent(Content);
            //Load the one and only texture
            theTexture = Content.Load<Texture2D>("playernew");

            //Rectangle pls = new Rectangle(0, 0, 0, 0);
            //Vector2 aids = Vector2.Zero;
            //Texture2D why = Content.Load<Texture2D>("player");
            //Projectile p = new Projectile(why, Vector2.Zero, new Rectangle(0, 0, 0, 0), Vector2.Zero);
            //Console.WriteLine(why);
            //newthing = (Projectile)p.GetType().GetConstructor(new System.Type[] { why.GetType(), aids.GetType(), pls.GetType(), aids.GetType() }).Invoke(new object[] {why, new Vector2(2,2), new Rectangle(0, 0, 10, 10), new Vector2(.01f, .01f) });
            //Console.WriteLine(newthing.Direction);
        }

        protected override void UnloadContent()
        {

        }

		protected override void Update(GameTime gameTime)
		{
			switch (gameState) {
				case GameState.GamePlay:
					if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
					{
						Exit();
					}
					//ScreenWrap(player);
					previousKbState = kbState;
					kbState = Keyboard.GetState();

					if (kbState.IsKeyDown(Keys.Enter)&& !previousKbState.IsKeyDown(Keys.Enter))
					{
						gameState = GameState.PauseMenu;
					}

					//Update entities
					entityManager.Update(gameTime);

					//Update projectiles
					projectileManager.Update(gameTime, previousKbState, kbState);

			//Update chunks
			chunkManager.Update();


            //initialize Camera
            if (mainCamera == null)
            {
                mainCamera = new Camera(GraphicsDevice.Viewport);
            }

            base.Update(gameTime);
					//Update chunks
					chunkManager.Update();
					break;
				case GameState.MainMenu:
					kbState = Keyboard.GetState();
					if (kbState.IsKeyDown(Keys.Enter))
					{
						gameState = GameState.GamePlay;
					}
					break;
				case GameState.PauseMenu:
					previousKbState = kbState;
					kbState = Keyboard.GetState();
					if (kbState.IsKeyDown(Keys.Enter) && !previousKbState.IsKeyDown(Keys.Enter))
					{
						gameState = GameState.GamePlay;
					}
					break;
			}
			base.Update(gameTime);
		}

		private void ScreenWrap(GameObject objToWrap)
		{
			if (objToWrap.X < 0)
			{
				objToWrap.X = GraphicsDevice.Viewport.Width - objToWrap.X;
			}
			else if (objToWrap.X > GraphicsDevice.Viewport.Width)
			{
				objToWrap.X = objToWrap.X - GraphicsDevice.Viewport.Width;
			}
			if (objToWrap.Y < 0)
			{
				objToWrap.Y = GraphicsDevice.Viewport.Height - objToWrap.Y;
			}
			else if (objToWrap.Y > GraphicsDevice.Viewport.Height)
			{
				objToWrap.Y = objToWrap.Y - GraphicsDevice.Viewport.Height;
			}
		}

		protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //newthing.Draw();
            //Begin SpriteBatch
			spriteBatch.Begin();
			switch (gameState)
			{
				case GameState.GamePlay:
					//Draw Map
					mapManager.Draw(spriteBatch, theTexture);

					//Draw entities
					entityManager.Draw(gameTime, spriteBatch);

					//Draw projectiles
					projectileManager.Draw(gameTime, spriteBatch);
					break;
			}

            //End SpriteBatch
			spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}
