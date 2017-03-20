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
            //Make the Camera
            Camera.Instance.setPosition(GraphicsDevice.Viewport);
            //Load projectiles
            projectileManager.LoadContent(Content);
			//Load the one and only texture
			theTexture = Content.Load<Texture2D>("playernew");
		}

		protected override void UnloadContent()
        {

        }

		protected override void Update(GameTime gameTime)
		{
            base.Update(gameTime);

            switch (gameState)
            {
				case GameState.GamePlay:
					if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
					{
						Exit();
					}
					
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
						mainCamera = Camera.Instance;
					}

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
		}

		protected override void Draw(GameTime gameTime)
        {
			GraphicsDevice.Clear(Color.CornflowerBlue);

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
				case GameState.MainMenu:
					spriteBatch.Draw(theTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
					break;
			}

			//End SpriteBatch
			spriteBatch.End();

			base.Draw(gameTime);
        }


    }
}
