using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using GDAPSIIGame.Map;
using GDAPSIIGame.Pods;
using System.Threading;
using GDAPSIIGame.Weapons;
using Microsoft.Xna.Framework.Content;

namespace GDAPSIIGame
{
    enum GameState { Menu, NewGame, LoadingScreen, GamePlay, GameOver, PauseMenu}

	public class Game1 : Game
    {
		//Fields
		private static Texture2D pauseRect;
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
        int mapSize;
		Thread l;

		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

			// Resize the screen to 1024 x 768.
			graphics.PreferredBackBufferWidth = 1024;
			graphics.PreferredBackBufferHeight = 768;

			graphics.ApplyChanges();
		}

        protected override void Initialize()
        {
			this.IsMouseVisible = false;

			//Window.IsBorderless = true;
			//graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
			//graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
			//graphics.ApplyChanges();

			mapSize = 2;

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
            uiManager = UIManager.Instance;

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
			Thread ent = new Thread(() => entityManager.LoadContent(Content, GraphicsDevice));
			ent.Name = "Entities";
			threads.Add(ent);
			Thread proj = new Thread(() => projectileManager.LoadContent(Content));
			proj.Name = "Name";
			threads.Add(proj);
			Thread ui = new Thread(() => uiManager.LoadContent(Content));
			ui.Name = "UI";
			threads.Add(ui);
			Thread other = new Thread(() => LoadOther(Content));
			other.Name = "Other";
			threads.Add(other);

			foreach (Thread t in threads)
			{
				t.Start();
				t.Join();
			}
		}

		protected override void UnloadContent()
        {

        }

		protected override void Update(GameTime gameTime)
		{
            base.Update(gameTime);

			//Update mouse texture's position
			mState = Mouse.GetState();
			//ContainMouse(mState);
			mousePos = mState.Position.ToVector2();

			if(kbState.IsKeyDown(Keys.F12) && previousKbState.IsKeyUp(Keys.F12))
			{
				graphics.ToggleFullScreen();
			}

			switch (gameState)
            {
				//The menu of the game
				case GameState.Menu:
					previousKbState = kbState;
					kbState = Keyboard.GetState();
					if (kbState.IsKeyDown(Keys.Enter) && previousKbState.IsKeyUp(Keys.Enter))
					{
						gameState = GameState.NewGame;
					}
					break;

				//When the player starts a new game
				case GameState.NewGame:
					//Reset the player character
					Player.Instance.ResetPlayer();
					PodManager.Instance.FullReset();
					//Reset the map
					mapSize = 2;
					//Go to loading screen to create a new level
					gameState = GameState.LoadingScreen;
					break;

				//Loading screen that generates a new level
				case GameState.LoadingScreen:
					if (l == null || l.ThreadState == ThreadState.Stopped)
					{
						l = new Thread(Loading);
						l.Name = "Loading";
						l.Start();
					}
					break;

				//Player playing a level
				case GameState.GamePlay:
					previousKbState = kbState;
					kbState = Keyboard.GetState();

					if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
					{
						Exit();
					}

					//Check if the player has paused the game
					if (kbState.IsKeyDown(Keys.Enter) && !previousKbState.IsKeyDown(Keys.Enter))
					{
						gameState = GameState.PauseMenu;
					}

					//Initialize Camera
					if (mainCamera == null)
					{
						mainCamera = Camera.Instance;
					}

					//Control for what state the map is in
					switch (mapManager.State)
					{
						//The level is starting
						case MapState.Enter:
							uiManager.Update(gameTime);
							break;
						
						//The level is being played
						case MapState.Play:
							//The player has beat the level
							if (PodManager.Instance.Count == 0)
							{
								if (mapSize < 5)
								{
									mapSize++;
								}
								uiManager.Fade = false;
								mapManager.State = MapState.Exit;
							}

							//Check if the player has died
							if (Player.Instance.Health <= 0 || PodManager.Instance.LevelTime > (mapSize * mapSize * 10))
							{
								mapManager.State = MapState.Died;
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
							break;
						
						//The player is exiting the level after beating it
						case MapState.Exit:
							uiManager.Update(gameTime);
							if (!uiManager.Fade)
							{
								gameState = GameState.LoadingScreen;
							}
							break;
						
						//The player has died on the level
						case MapState.Died:
							gameState = GameState.GameOver;
							break;
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
					previousKbState = kbState;
					kbState = Keyboard.GetState();
					if (kbState.IsKeyDown(Keys.Enter) && !previousKbState.IsKeyDown(Keys.Enter))
                    {
                        gameState = GameState.Menu;
                    }
                    break;
			}
		}

		protected override void Draw(GameTime gameTime)
        {
			GraphicsDevice.Clear(Color.CornflowerBlue);

			//Begin SpriteBatch
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);

			switch (gameState)
			{	
				//Drawing for main menu
				case GameState.Menu:
					//Draw the menu
					spriteBatch.Draw(textureManager.MenuTextures["Logo"], new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

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
					spriteBatch.Draw(TextureManager.Instance.GetMenuTexture("Black"),
						new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
						Color.White);
					break;

				//Drawing for gameplay
				case GameState.GamePlay:
					//Draw Map
					mapManager.Draw(spriteBatch);

					//Draw entities
					entityManager.Draw(gameTime, spriteBatch);

					//Draw projectiles
					projectileManager.Draw(gameTime, spriteBatch);

					mapManager.DrawForeground(spriteBatch);

					//Draw UI
					uiManager.Draw(gameTime, spriteBatch, GraphicsDevice);

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

					//Draw score things
					spriteBatch.DrawString(font, PodManager.Instance.GlobalScore.ToString(), new Vector2(50, 50), Color.Red);
					spriteBatch.DrawString(font, Player.Instance.ScoreMultiplier.ToString(), new Vector2(50, 100), Color.Red);
					spriteBatch.DrawString(font, ((mapSize * mapSize * 10) - PodManager.Instance.LevelTime).ToString(), new Vector2(50, 150), Color.Red);
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
					uiManager.Draw(gameTime, spriteBatch, GraphicsDevice);

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

					//Make the screen gray
					spriteBatch.Draw(pauseRect, Vector2.Zero, Color.White);

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

                    spriteBatch.Draw(textureManager.EnemyTextures["EnemyTexture"], new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
					spriteBatch.DrawString(font, PodManager.Instance.GlobalScore.ToString(), new Vector2(50, 50), Color.Red);
					spriteBatch.DrawString(font, PodManager.Instance.LevelTime.ToString(), new Vector2(50, 150), Color.Red);

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

		/// <summary>
		/// Contains the mouse to the window
		/// </summary>
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

		private void LoadOther(ContentManager content)
		{
			//Make the Camera
			Camera.Instance.setPosition(GraphicsDevice.Viewport);

			pauseRect = new Texture2D(graphics.GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
			Color[] data = new Color[GraphicsDevice.Viewport.Width * GraphicsDevice.Viewport.Height];
			for (int i = 0; i < data.Length; ++i) data[i] = new Color(Color.Black, 0.2f);
			pauseRect.SetData(data);

			font = textureManager.GetFont("uifont");

			//Initiate mouse
			mState = Mouse.GetState();
			mouseTex = textureManager.MouseTextures["MousePointer"];
			mousePos = new Vector2(mState.X, mState.Y);
			mouseScale = new Vector2((float)21 / mouseTex.Width, (float)22 / mouseTex.Height);
		}

		private void Loading()
		{
			//Empty all entities and walls
			entityManager.RemoveEnemies();
			chunkManager.DeleteWalls();
			projectileManager.RemoveProjectiles();

			chunkManager.Resize(mapSize);
			chunkManager.Add(Player.Instance);
			//Create the new map
			MapManager.Instance.CreateMap(textureManager.RoomTextures["IndoorSpriteSheet"], textureManager.RoomTextures["IndoorFloorSpriteSheet"], mapSize);
			PodManager.Instance.Reset();
			//Go to gameplay
			gameState = GameState.GamePlay;
		}
	}
}
