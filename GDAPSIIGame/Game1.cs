using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using GDAPSIIGame.Map;
using GDAPSIIGame.Pods;
using System.Threading;
using GDAPSIIGame.Weapons;

namespace GDAPSIIGame
{
    enum GameState { Menu, NewGame, LoadingScreen, GamePlay, GameOver, PauseMenu}
    
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
        UIManager uiManager;
        Camera mainCamera;
		GameState gameState;
		Texture2D mouseTex;
        Vector2 mousePos;
		MouseState mState;
		Vector2 mouseScale;
		SpriteFont font;
		WeaponManager weaponManager;
		TextureManager textureManager;

		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
			this.IsMouseVisible = false;

            //this.graphics.IsFullScreen = true;
			//graphics.ToggleFullScreen();

			//Initialize texture manager
			textureManager = TextureManager.Instance;

            //Initialize entity manager
            entityManager = EntityManager.Instance;

            //Initialize the chunk manager
            chunkManager = ChunkManager.Instance;

			//Initialize projectile manager
			projectileManager = ProjectileManager.Instance;

			//Initialize map manager
			mapManager = MapManager.Instance;

			//Initialize weapon manager
			weaponManager = WeaponManager.Instance;

            //Initialize ui manager
            uiManager = new UIManager();

			//Initialize keyboards
			kbState = new KeyboardState();
			previousKbState = kbState;

			gameState = GameState.Menu;
			base.Initialize();
        }

        protected override void LoadContent()
        {
			spriteBatch = new SpriteBatch(GraphicsDevice);

			List<Thread> threads = new List<Thread>();

			//Make all the threads
			Thread tex = new Thread(() => textureManager.LoadContent(Content) );
			tex.Name = "Textures";
			threads.Add(tex);
			Thread weap = new Thread(() => weaponManager.LoadContent(Content));
			weap.Name = "Weapons";
			threads.Add(weap);
			Thread ent = new Thread(() => entityManager.LoadContent(Content));
			ent.Name = "Entities";
			threads.Add(ent);
			Thread proj = new Thread(() => projectileManager.LoadContent(Content));
			proj.Name = "Name";
			threads.Add(proj);
			Thread ui = new Thread(() => uiManager.LoadContent(Content));
			ui.Name = "UI";
			threads.Add(ui);

			foreach (Thread t in threads)
			{
				t.Start();
				t.Join();
			}

			//Make the Camera
			Camera.Instance.setPosition(GraphicsDevice.Viewport);

			font = Content.Load<SpriteFont>("Font");

			//Initiate mouse
			mState = Mouse.GetState();
			mouseTex = textureManager.MouseTextures["MousePointer"];
			mousePos = new Vector2(mState.X, mState.Y);
			mouseScale = new Vector2((float)16 / mouseTex.Width, (float)16 / mouseTex.Height);
        }

		protected override void UnloadContent()
        {

        }

		protected override void Update(GameTime gameTime)
		{
            base.Update(gameTime);

			//Update mouse texture's position
			mState = Mouse.GetState();
			ContainMouse(mState);
			mousePos = mState.Position.ToVector2();

			switch (gameState)
            {
				//The menu of the game
				case GameState.Menu:
					kbState = Keyboard.GetState();
					if (kbState.IsKeyDown(Keys.Enter))
					{
						gameState = GameState.NewGame;
					}
					break;

				//When the player starts a new game
				case GameState.NewGame:
					//Reset the player character
					Player.Instance.ResetPlayer();

					//Go to loading screen to create a new level
					gameState = GameState.LoadingScreen;
					break;

				//Loading screen that generates a new level
				case GameState.LoadingScreen:
					//Empty all entities and walls
					entityManager.RemoveEnemies();
					chunkManager.DeleteWalls();

					//Create the new map
					MapManager.Instance.CreateMap(textureManager.RoomTextures["WallTexture"], textureManager.RoomTextures["FloorTexture"]);

					//Go to gameplay
					gameState = GameState.GamePlay;
					break;

				//Player playing a level
				case GameState.GamePlay:
					if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
					{
						Exit();
					}

					previousKbState = kbState;
					kbState = Keyboard.GetState();

					//Check if the player has paused the game
					if (kbState.IsKeyDown(Keys.Enter) && !previousKbState.IsKeyDown(Keys.Enter))
					{
						gameState = GameState.PauseMenu;
					}

					//Update entities
					entityManager.Update(gameTime);

					//Update projectiles
					projectileManager.Update(gameTime, previousKbState, kbState);

					//Update chunks
					chunkManager.Update();

					//Update UI
					uiManager.Update(gameTime);

					PodManager.Instance.Update(gameTime);

					//Initialize Camera
					if (mainCamera == null)
					{
						mainCamera = Camera.Instance;
					}

					//Check if the player has beat the levels
					if (entityManager.BeatLevel)
					{
						gameState = GameState.LoadingScreen;
					}

					//Check if the player has died
					if (Player.Instance.Health <= 0)
					{
						gameState = GameState.GameOver;
					}

					break;

				//When the game is paused
				case GameState.PauseMenu:
					previousKbState = kbState;
					kbState = Keyboard.GetState();
					if (kbState.IsKeyDown(Keys.Enter) && !previousKbState.IsKeyDown(Keys.Enter))
					{
						gameState = GameState.GamePlay;
					}
					break;

				//When the Player dies
				case GameState.GameOver:
					gameState = GameState.Menu;
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
				//Drawing for main menu
				case GameState.Menu:
					//Draw the menu
					spriteBatch.Draw(textureManager.EnemyTextures["EnemyTexture"], new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

					//Draw the mouse texture
					spriteBatch.Draw(mouseTex,
							mousePos,
							null,
							null,
							Vector2.Zero,
							0.0f,
							mouseScale,
							null,
							0);
					break;
				
				//Drawing for loading screen
				case GameState.LoadingScreen:
					break;

				//Drawing for gameplay
				case GameState.GamePlay:
					//Draw Map
					mapManager.Draw(spriteBatch);

					//Draw entities
					entityManager.Draw(gameTime, spriteBatch);

					//Draw projectiles
					projectileManager.Draw(gameTime, spriteBatch);

					//Draw UI
					uiManager.Draw(gameTime, spriteBatch);

					//Draw the mouse texture
					spriteBatch.Draw(mouseTex,
							mousePos,
							null,
							null,
							Vector2.Zero,
							0.0f,
							mouseScale,
							null,
							0);
					spriteBatch.DrawString(font, PodManager.Instance.GlobalScore.ToString(), new Vector2(50, 50), Color.Red);
					spriteBatch.DrawString(font, Player.Instance.ScoreMultiplier.ToString(), new Vector2(50, 100), Color.Red);
					break;

				//Drawing for pause menu
				case GameState.PauseMenu:
					//Draw Map
					mapManager.Draw(spriteBatch);

					//Draw entities
					entityManager.Draw(gameTime, spriteBatch);

					//Draw projectiles
					projectileManager.Draw(gameTime, spriteBatch);

					//Draw UI
					uiManager.Draw(gameTime, spriteBatch);

					//Draw the mouse texture
					spriteBatch.Draw(mouseTex,
							mousePos,
							null,
							null,
							Vector2.Zero,
							0.0f,
							mouseScale,
							null,
							0);
					break;

				//Drawing for game over
				case GameState.GameOver:
					//Draw the mouse texture
					spriteBatch.Draw(mouseTex,
							mousePos,
							null,
							null,
							Vector2.Zero,
							0.0f,
							mouseScale,
							null,
							0);
					break;

			}

			//End SpriteBatch
			spriteBatch.End();

			base.Draw(gameTime);
        }

		private void ContainMouse(MouseState mState)
		{
			if(mState.X < 0)
			{
				Mouse.SetPosition(0, mState.Y);
			}
			else if(mState.X > GraphicsDevice.Viewport.Width)
			{
				Mouse.SetPosition(GraphicsDevice.Viewport.Width, mState.Y);
			}

			if(mState.Y < 0)
			{
				Mouse.SetPosition(mState.X, 0);
			}
			else if (mState.Y > GraphicsDevice.Viewport.Height)
			{
				Mouse.SetPosition(mState.X, GraphicsDevice.Viewport.Height);
			}
		}
    }
}
