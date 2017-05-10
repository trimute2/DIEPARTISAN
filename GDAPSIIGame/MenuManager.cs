using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace GDAPSIIGame
{
	class MenuManager
	{
		private KeyboardState kbState;
		private KeyboardState prevKbState;
		private MouseState mState;
		private MouseState prevMState;
		private GamePadState gpState;
		private GamePadState prevGpState;

		private static MenuManager instance;

		private Texture2D menuBackground;
		private Vector2 menuBackgroundScale;

		//Main Menu
		private Texture2D title;
		private Vector2 titlePos;
		private float titleTimer;
		private Button playButton;
		private Button exitButton;
		private Button optionsButton;
		private List<Button> mainMenuButtons;
		private bool mainMenuChange;
		private bool mainMenuOptions;
		private bool exit;

		//Options Menu
		private List<Button> optionsMenuButtons;
		private Button optionsBackButton;

		//Controls Menu
		private Button settingButton;
		private Button fireButton;
		private Button fireAltButton;

		//Pause Menu
		private List<Button> pauseMenuButtons;
		private Button pauseMainMenuButton;

		private GameState menuState;

		//Control setting vars
		Control_Types cont;
		bool alt;

		//Main menu properties
		public bool MainMenuChange
		{
			get { return mainMenuChange; }
			set { mainMenuChange = value; }
		}

		//Main menu properties
		public bool MainMenuOptions
		{
			get { return mainMenuOptions; }
			set { mainMenuOptions = value; }
		}

		public bool Exit
		{
			get { return exit; }
			set { exit = value; }
		}

		public static MenuManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MenuManager();
				}
				return instance;
			}
		}

		public void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
		{
			TextureManager textureManager = TextureManager.Instance;

			menuBackground = textureManager.GetMenuTexture("MenuBackground");
			menuBackgroundScale = new Vector2((float)GraphicsDevice.Viewport.Width / menuBackground.Width, (float)GraphicsDevice.Viewport.Height / menuBackground.Height);


			//Main Menu
			title = textureManager.GetMenuTexture("Logo");
			titlePos = new Vector2((GraphicsDevice.Viewport.Width / 2) - title.Width, (10 * ((float)Math.Sin(0 * 1.5f))) + 20);

			Texture2D play = textureManager.GetMenuTexture("Play");
			playButton = new Button(play, new Rectangle((GraphicsDevice.Viewport.Width / 2) - play.Width/2, (GraphicsDevice.Viewport.Height / 2) + 100, 160, 60),
				Color.LightPink);
			mainMenuButtons.Add(playButton);

			optionsButton = new Button(play, new Rectangle((GraphicsDevice.Viewport.Width / 2) - play.Width / 2, (GraphicsDevice.Viewport.Height / 2) + 163, 160, 60),
				Color.LightPink, "OPTIONS OPTIONS OPTIONS");
			mainMenuButtons.Add(optionsButton);

			exitButton = new Button(play, new Rectangle((GraphicsDevice.Viewport.Width / 2) - play.Width / 2, (GraphicsDevice.Viewport.Height / 2) + 225, 160, 60),
				Color.LightPink, "EXIT EXIT EXIT");
			mainMenuButtons.Add(exitButton);


			//Options Menu
			optionsBackButton = new Button(play, new Rectangle((GraphicsDevice.Viewport.Width / 2) - play.Width / 2, (GraphicsDevice.Viewport.Height / 2) + 225, 160, 60),
				Color.LightPink, "BACK BUTTON");
			optionsMenuButtons.Add(optionsBackButton);

			Texture2D black = TextureManager.Instance.GetMenuTexture("Black");
			fireButton = new Button(black, new Rectangle(((GraphicsDevice.Viewport.Width / 2) - play.Width / 2) - 100, 225, 160, 60),
				Color.LightPink, "Fire: " + ControlManager.Instance.GetKBMControl(Control_Types.Fire, false));
			optionsMenuButtons.Add(fireButton);

			fireAltButton = new Button(black, new Rectangle(((GraphicsDevice.Viewport.Width / 2) - play.Width / 2) + 100, 225, 160, 60),
				Color.LightPink, "Fire (Alternate): " + ControlManager.Instance.GetKBMControl(Control_Types.Fire, true));
			optionsMenuButtons.Add(fireAltButton);


			//Pause Menu
			pauseMainMenuButton = new Button(black, new Rectangle(200, 75, 160, 60),
				Color.LightPink, "Main Menu");
			pauseMenuButtons.Add(pauseMainMenuButton);

		}

		public GameState MenuState
		{
			get { return menuState; }
			set
			{
				menuState = value;
			}
		}

		/// <summary>
		/// Instantiate the menu manager
		/// </summary>
		private MenuManager()
		{
			kbState = Keyboard.GetState();
			prevKbState = kbState;
			mState = Mouse.GetState();
			prevMState = mState;
			gpState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
			prevGpState = gpState;

			//Instantiate values
			alt = false;
			mainMenuChange = false;
			exit = false;
			mainMenuOptions = false;
			titleTimer = 0;

			mainMenuButtons = new List<Button>();
			optionsMenuButtons = new List<Button>();
			pauseMenuButtons = new List<Button>();
		}

		/// <summary>
		/// The update for the main menu of the game
		/// </summary>
		public void UpdateMainMenu(GameTime gameTime)
		{
			titlePos.Y = (10*((float)Math.Sin(titleTimer*1.5f)))+20;
			titleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (ControlManager.Instance.Mode == Control_Mode.KBM)
			{
				//Update states
				prevKbState = kbState;
				kbState = Keyboard.GetState();
				prevMState = mState;
				mState = Mouse.GetState();

				//Check if the button is selected
				foreach (Button b in mainMenuButtons)
				{
					if (b.Contains(mState.Position.ToVector2()))
					{
						b.Selected = true;
					}
					else b.Selected = false;
				}
				
				//Check if button is clicked
				if (playButton.Selected && 
					((mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Released) 
					|| (kbState.IsKeyDown(Keys.Enter) && prevKbState.IsKeyUp(Keys.Enter))))
				{
					if (!ControlManager.Instance.Setting)
					{
						mainMenuChange = true;
					}
				}
				else if (optionsButton.Selected &&
					((mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Released)
					|| (kbState.IsKeyDown(Keys.Enter) && prevKbState.IsKeyUp(Keys.Enter))))
				{
					if (!ControlManager.Instance.Setting)
					{
						mainMenuOptions = true;
					}
				}
				else if (exitButton.Selected &&
					((mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Released)
					|| (kbState.IsKeyDown(Keys.Enter) && prevKbState.IsKeyUp(Keys.Enter))))
				{
					if (!ControlManager.Instance.Setting)
					{
						exit = true;
					}
				}
			}
		}


		/// <summary>
		/// The draw for the main menu of the game
		/// </summary>
		public void DrawMainMenu(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(
				texture: menuBackground,
				position: Vector2.Zero,
				color: Color.White,
				scale: menuBackgroundScale
				);

			spriteBatch.Draw(
				texture: title,
				position: titlePos,
				color: Color.White,
				scale: new Vector2(2, 2)
				);

			playButton.Draw(spriteBatch);
			optionsButton.Draw(spriteBatch);
			exitButton.Draw(spriteBatch);
		}

		/// <summary>
		/// The update for the options menu of the game
		/// </summary>
		public void UpdateOptionsMenu(GameTime gameTime)
		{
			if (ControlManager.Instance.Mode == Control_Mode.KBM)
			{
				//Update states
				prevKbState = kbState;
				kbState = Keyboard.GetState();
				prevMState = mState;
				mState = Mouse.GetState();

				String result;
				//Check if the player is currently setting a control
				if (ControlManager.Instance.Setting)
				{
					if (ControlManager.Instance.SetControl(cont, alt, out result))
					{
						ControlManager.Instance.SaveControlFile();
					}
					settingButton.Text = result;
				}
				else
				{
					//Check if the button is selected
					foreach (Button b in optionsMenuButtons)
					{
						if (b.Contains(mState.Position.ToVector2()))
						{
							b.Selected = true;
						}
						else b.Selected = false;
					}

					//Check if button is clicked
					if (optionsBackButton.Selected &&
						((mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Released)
						|| (kbState.IsKeyDown(Keys.Enter) && prevKbState.IsKeyUp(Keys.Enter))))
					{
						if (!ControlManager.Instance.Setting)
						{
							mainMenuChange = true;
						}
					}
					else if(fireButton.Selected &&
						((mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Released)
						|| (kbState.IsKeyDown(Keys.Enter) && prevKbState.IsKeyUp(Keys.Enter))))
					{
						settingButton = fireButton;
						cont = Control_Types.Fire;
						alt = false;
						ControlManager.Instance.Setting = true;
					}
					else if (fireAltButton.Selected &&
						((mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Released)
						|| (kbState.IsKeyDown(Keys.Enter) && prevKbState.IsKeyUp(Keys.Enter))))
					{
						settingButton = fireAltButton;
						cont = Control_Types.Fire;
						alt = true;
						ControlManager.Instance.Setting = true;
					}
				}
			}
		}

		/// <summary>
		/// The draw for the options menu of the game
		/// </summary>
		public void DrawOptionsMenu(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(
				texture: menuBackground,
				position: Vector2.Zero,
				color: Color.White,
				scale: menuBackgroundScale
				);

			optionsBackButton.Draw(spriteBatch);
			fireButton.Draw(spriteBatch);
			fireAltButton.Draw(spriteBatch);
		}

		/// <summary>
		/// The update for the pause menu of the game
		/// </summary>
		public void UpdatePauseMenu(GameTime gameTime)
		{
			if (ControlManager.Instance.Mode == Control_Mode.KBM)
			{
				//Update states
				prevKbState = kbState;
				kbState = Keyboard.GetState();
				prevMState = mState;
				mState = Mouse.GetState();

				//Check if the button is selected
				foreach (Button b in pauseMenuButtons)
				{
					if (b.Contains(mState.Position.ToVector2()))
					{
						b.Selected = true;
					}
					else b.Selected = false;
				}
				
				//Check if button is clicked
				if (pauseMainMenuButton.Selected &&
					((mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Released)
					|| (kbState.IsKeyDown(Keys.Enter) && prevKbState.IsKeyUp(Keys.Enter))))
				{
					if (!ControlManager.Instance.Setting)
					{
						mainMenuChange = true;
					}
				}
			}
		}

		/// <summary>
		/// The draw for the pause menu of the game
		/// </summary>
		public void DrawPauseMenu(SpriteBatch spriteBatch)
		{
			pauseMainMenuButton.Draw(spriteBatch);
		}
	}
}
