using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using GDAPSIIGame.Map;
using GDAPSIIGame.Pods;
using System.Threading;
using GDAPSIIGame.Weapons;
using GDAPSIIGame.Audio;
using Microsoft.Xna.Framework.Content;

namespace GDAPSIIGame
{
    enum GameState { InitialLoad, Menu, OptionsMenu, NewGame, LoadingScreen, GamePlay, GameOver, PauseMenu }

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
		GamePadState gpState;
		GamePadState previousGpState;
		ChunkManager chunkManager;
		MapManager mapManager;
		AudioManager audioManager;
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
		ControlManager controlManager;
		MenuManager menuManager;
        int mapSize;
		Thread l;
		bool startLoad;

		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.Title = "DIE-PARTISAN";
			// Resize the screen to 1152 x 648.
			graphics.PreferredBackBufferWidth = 1152;
			graphics.PreferredBackBufferHeight = 648;

			graphics.ApplyChanges();
		}

        protected override void Initialize()
        {
			this.IsMouseVisible = false;

			//Window.IsBorderless = true;
			//graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
			//graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
			//graphics.ApplyChanges();

            //this.graphics.IsFullScreen = true;
			//graphics.ToggleFullScreen();

			//Initialize texture manager
			textureManager = TextureManager.Instance;

			//Initialize audio manager
			audioManager = AudioManager.Instance;

            //Initialize entity manager
            entityManager = EntityManager.Instance;

			//Initialize control manager
			controlManager = ControlManager.Instance;

            //Initialize the chunk manager
            chunkManager = ChunkManager.Instance;

			//Initialize projectile manager
			projectileManager = ProjectileManager.Instance;

			//Initialize menu manager
			menuManager = MenuManager.Instance;

			//Initialize map manager
			mapManager = MapManager.Instance;

			//Initialize weapon manager
			weaponManager = WeaponManager.Instance;

            //Initialize ui manager
            uiManager = UIManager.Instance;

			gameState = GameState.InitialLoad;
			base.Initialize();
        }

        protected override void LoadContent()
        {
			spriteBatch = new SpriteBatch(GraphicsDevice);
			startLoad = true;
			textureManager.InitialLoadContent(Content);
		}

		protected override void UnloadContent()
        { }

		protected override void Update(GameTime gameTime)
		{
            base.Update(gameTime);

			//Update mouse texture's position
			mState = Mouse.GetState();
			//ContainMouse(mState);
			if (controlManager.Mode == Control_Mode.KBM)
			{
				mousePos = mState.Position.ToVector2();
			}
			else
			{
				//Get the correct position for the thumbsticks and make it look good
				Vector2 thumb = gpState.ThumbSticks.Right;
				thumb.Y *= -1;
				mousePos = Camera.Instance.GetViewportPosition(Player.Instance.Position +
					new Vector2(Player.Instance.BoundingBox.Width / 2, Player.Instance.BoundingBox.Height / 2)
					+ (thumb * 200));
			}

			switch (gameState)
            {
				//The initial loading of the game
				case GameState.InitialLoad:
					previousKbState = kbState;
					kbState = Keyboard.GetState();
					if (startLoad)
					{
						InitialLoad();
						startLoad = false;
					}
					uiManager.UpdateSplashScreens(gameTime);
					if(uiManager.SplashScreen > 4)
					{
						gameState = GameState.Menu;
					}
					if(kbState.GetPressedKeys().Length > 0 && previousKbState.GetPressedKeys().Length == 0)
					{
						if (uiManager.SplashScreen < 2) { uiManager.SplashScreen = 2; }
						else if (uiManager.SplashScreen < 5) { uiManager.SplashScreen = 5; }
					}
					break;

				//The menu of the game
				case GameState.Menu:
					//Update controls and menu
					controlManager.Update();
					menuManager.UpdateMainMenu(gameTime);

                    //Get input
                    if (menuManager.MainMenuChange)
					{
						menuManager.MainMenuChange = false;
						gameState = GameState.NewGame;
					}
					//Get input
					else if (menuManager.MainMenuOptions)
					{
						menuManager.MainMenuOptions = false;
						gameState = GameState.OptionsMenu;
					}
					else if (menuManager.Exit)
					{
						menuManager.Exit = false;
						Exit();
					}
					break;

				//The options menu of the game
				case GameState.OptionsMenu:
					//Update controls and menu
					controlManager.Update();
					menuManager.UpdateOptionsMenu(gameTime);

					//Get input
					if (menuManager.MainMenuChange)
					{
						menuManager.MainMenuChange = false;
						gameState = GameState.Menu;
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
					controlManager.Update();

					previousGpState = gpState;
					gpState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);

					if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
					{
						Exit();
					}

					//Check if the player has paused the game
					if (controlManager.ControlPressedControlPrevReleased(Control_Types.Interact))
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
							if (Player.Instance.Health <= 0 || PodManager.Instance.LevelTime > (mapSize * mapSize * 15))
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
							//Update projectiles
							projectileManager.Update(gameTime, previousKbState, kbState);

							//Update chunks
							chunkManager.Update();

							//Update UI
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
					//Update controls and menu
					controlManager.Update();
					menuManager.UpdatePauseMenu(gameTime);

					//Get input
					if (controlManager.ControlPressedControlPrevReleased(Control_Types.Interact))
					{
						gameState = GameState.GamePlay;
					}
					else if (menuManager.MainMenuChange)
					{
						menuManager.MainMenuChange = false;
						gameState = GameState.Menu;
					}
					break;

				//When the Player dies
				case GameState.GameOver:
					controlManager.Update();

					//Get input
					if (controlManager.ControlPressedControlPrevReleased(Control_Types.Interact))
                    {
						gameState = GameState.Menu;
					}
					break;
			}
		}

		protected override void Draw(GameTime gameTime)
        {
			GraphicsDevice.Clear(Color.Black);

			//Begin SpriteBatch
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);

			switch (gameState)
			{
				case GameState.InitialLoad:
					uiManager.DrawSplashScreens(spriteBatch, GraphicsDevice);
					break;

				//Drawing for main menu
				case GameState.Menu:
					//Draw the menu
					menuManager.DrawMainMenu(spriteBatch);

					//Draw the mouse texture
					spriteBatch.Draw(mouseTex,
							mousePos - new Vector2(10, 13),
							null,
							null,
							Vector2.Zero,
							0.0f,
							mouseScale,
							null,
							0);
					break;

				//Drawing for options menu
				case GameState.OptionsMenu:
					//Draw the menu
					menuManager.DrawOptionsMenu(spriteBatch);

					//Draw the mouse texture
					spriteBatch.Draw(mouseTex,
							mousePos - new Vector2(10, 13),
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
							mousePos - new Vector2(10, 13),
							null,
							null,
							Vector2.Zero,
							0.0f,
							mouseScale,
							null,
							0);
					break;

				//Drawing for pause menu
				case GameState.PauseMenu:
					//Draw Map
					mapManager.Draw(spriteBatch);

					//Draw entities
					entityManager.Draw(gameTime, spriteBatch);

					//Draw projectiles
					projectileManager.Draw(gameTime, spriteBatch);

					mapManager.DrawForeground(spriteBatch);

					//Draw UI
					uiManager.Draw(gameTime, spriteBatch, GraphicsDevice);

					//Make the screen gray
					spriteBatch.Draw(pauseRect, Vector2.Zero, Color.White);

					//Draw the menu
					menuManager.DrawPauseMenu(spriteBatch);

					//Draw the mouse texture
					spriteBatch.Draw(mouseTex,
							mousePos - new Vector2(10, 13),
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
							mousePos - new Vector2(10, 13),
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

		private void InitialLoad()
		{
			List<Thread> threads = new List<Thread>();

			//Make all the threads
			Thread tex = new Thread(() => textureManager.LoadContent(Content));
			tex.Name = "Textures";
			threads.Add(tex);
			Thread audio = new Thread(() => audioManager.LoadContent(Content));
			audio.Name = "Audio";
			threads.Add(audio);
			Thread weap = new Thread(() => weaponManager.LoadContent(Content));
			weap.Name = "Weapons";
			threads.Add(weap);
			Thread ent = new Thread(() => entityManager.LoadContent(Content, GraphicsDevice));
			ent.Name = "Entities";
			threads.Add(ent);
			Thread proj = new Thread(() => projectileManager.LoadContent(Content));
			proj.Name = "Name";
			threads.Add(proj);
			Thread menu = new Thread(() => menuManager.LoadContent(Content, GraphicsDevice));
			menu.Name = "Menu";
			threads.Add(menu);
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

		private void LoadOther(ContentManager content)
		{
			mapSize = 2;

			//Make the Camera
			Camera.Instance.setPosition(GraphicsDevice.Viewport);

			pauseRect = new Texture2D(graphics.GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
			Color[] data = new Color[GraphicsDevice.Viewport.Width * GraphicsDevice.Viewport.Height];
			for (int i = 0; i < data.Length; ++i) data[i] = new Color(Color.Black, 0.2f);
			pauseRect.SetData(data);

			font = textureManager.GetFont("uifont");

			//Initialize keyboards
			kbState = new KeyboardState();
			previousKbState = kbState;

			//Initiate mouse
			mState = Mouse.GetState();
			mouseTex = textureManager.MouseTextures["MousePointer"];
			mousePos = new Vector2(mState.X, mState.Y);
			mouseScale = new Vector2((float)21 / mouseTex.Width, (float)22 / mouseTex.Height);
			//gameState = GameState.Menu;
		}

		private void Loading()
		{
			//Empty all entities and walls
			entityManager.RemoveEnemies();
			chunkManager.DeleteWalls();
			projectileManager.RemoveProjectiles();

			chunkManager.Resize(mapSize);
			//Create the new map
			MapManager.Instance.CreateMap(textureManager.RoomTextures["IndoorSpriteSheet"], textureManager.RoomTextures["IndoorFloorSpriteSheet"], mapSize);
			PodManager.Instance.Reset();
			//Add the player to chunk
			chunkManager.Add(Player.Instance);
			uiManager.SetMapSize((int)(Math.Pow((double)mapSize, 2.0) * 15));
			//Go to gameplay
			gameState = GameState.GamePlay;
		}
	}
}
