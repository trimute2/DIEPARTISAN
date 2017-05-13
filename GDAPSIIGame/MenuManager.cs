﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using GDAPSIIGame.Audio;

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

		private SpriteFont font;

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
		private Vector2 bPosition;
		private Vector2 altBPosition;
		private Vector2 coverPos1;
		private Vector2 coverPos2;
		private Rectangle controlRectangle;
		private bool canMoveUp;
		private bool canMoveDown;

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

			font = textureManager.GetFont("uifont");

			//Main Menu
			title = textureManager.GetMenuTexture("Logo");
			titlePos = new Vector2((GraphicsDevice.Viewport.Width / 2) - title.Width, (10 * ((float)Math.Sin(0 * 1.5f))) + 20);

			Texture2D play = textureManager.GetMenuTexture("Play");
			playButton = new Button(play, new Rectangle((GraphicsDevice.Viewport.Width / 2) - play.Width/2, (GraphicsDevice.Viewport.Height / 2) + 100, 160, 60),
				Color.LightPink);
			mainMenuButtons.Add(playButton);

            Texture2D options = textureManager.GetMenuTexture("Options");
            optionsButton = new Button(options, new Rectangle((GraphicsDevice.Viewport.Width / 2) - options.Width / 2, (GraphicsDevice.Viewport.Height / 2) + 163, 244, 60),
				Color.LightPink, "");
			mainMenuButtons.Add(optionsButton);

            Texture2D exit = textureManager.GetMenuTexture("Exit");
            exitButton = new Button(exit, new Rectangle((GraphicsDevice.Viewport.Width / 2) - exit.Width / 2, (GraphicsDevice.Viewport.Height / 2) + 225, 160, 60),
				Color.LightPink, "");
			mainMenuButtons.Add(exitButton);


			//Options Menu
			optionsBackButton = new Button(play, new Rectangle((GraphicsDevice.Viewport.Width / 2) - play.Width / 2, (GraphicsDevice.Viewport.Height / 2) + 225, 160, 60),
				Color.LightPink, "BACK BUTTON");
			optionsMenuButtons.Add(optionsBackButton);

			bPosition = new Vector2(GraphicsDevice.Viewport.Width / 2 - 100, 50);
			altBPosition = new Vector2(GraphicsDevice.Viewport.Width / 2 + 100, 50);
			coverPos1 = new Vector2((GraphicsDevice.Viewport.Width / 2) - (menuBackground.Width/2), 50 - (menuBackground.Height/2));
			coverPos2 = new Vector2((GraphicsDevice.Viewport.Width / 2) - (menuBackground.Width / 2), 352 + (menuBackground.Height / 2));
			controlRectangle = new Rectangle((GraphicsDevice.Viewport.Width / 2) - (menuBackground.Width / 2), 50 + (menuBackground.Height / 2), 300, 300);
			canMoveUp = false;
			canMoveDown = true;

			Texture2D black = TextureManager.Instance.GetMenuTexture("Black");
			fireButton = new Button(black, new Rectangle(((GraphicsDevice.Viewport.Width / 2) - play.Width / 2) - 100, 155, 160, 60),
				Color.LightPink, "Fire: " + ControlManager.Instance.GetKBMControl(Control_Types.Fire, false));
			optionsMenuButtons.Add(fireButton);

			fireAltButton = new Button(black, new Rectangle(((GraphicsDevice.Viewport.Width / 2) - play.Width / 2) + 100, 155, 160, 60),
				Color.LightPink, "Fire: " + ControlManager.Instance.GetKBMControl(Control_Types.Fire, true));
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
						b.PrevSelected = b.Selected;
						b.Selected = true;

						if (!b.PrevSelected)
						{
							AudioManager.Instance.GetSoundEffect("Blip").Play();
						}
					}
					else
					{
						b.PrevSelected = b.Selected;
						b.Selected = false;
					}
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

		private bool CanMoveUp()
		{
			return (fireButton.Y > 155 && fireAltButton.Y > 155);
		}

		private bool CanMoveDown()
		{
			return (fireButton.Y < 155 && fireAltButton.Y < 155);
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
					//Scrolling the buttons down
					if (mState.ScrollWheelValue < prevMState.ScrollWheelValue)
					{
						foreach (Button b in optionsMenuButtons)
						{
							if (b != optionsBackButton && canMoveDown)
							{
								b.Y += 15;
							}
						}
					}
					//Scrolling the buttons up
					else if (mState.ScrollWheelValue > prevMState.ScrollWheelValue && CanMoveUp())
					{
						foreach (Button b in optionsMenuButtons)
						{
							if (b != optionsBackButton)
							{
								b.Y -= 15;
							}
						}
					}

					//Check if the button is selected
					foreach (Button b in optionsMenuButtons)
					{
						//If the button is the back button
						if (b == optionsBackButton && b.Contains(mState.Position.ToVector2()))
						{
							b.PrevSelected = b.Selected;
							b.Selected = true;

							if (!b.PrevSelected)
							{
								AudioManager.Instance.GetSoundEffect("Blip").Play();
							}
						}
						//If its for any other buttons
						else if (b.Contains(mState.Position.ToVector2()) && controlRectangle.Contains(b.Area))
						{
							b.PrevSelected = b.Selected;
							b.Selected = true;

							if (!b.PrevSelected)
							{
								AudioManager.Instance.GetSoundEffect("Blip").Play();
							}
						}
						else
						{
							b.PrevSelected = b.Selected;
							b.Selected = false;
						}
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
					//FIRE BUTTONS
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

		public void ResetOptionsPositions()
		{

		}

		/// <summary>
		/// The draw for the options menu of the game
		/// </summary>
		public void DrawOptionsMenu(SpriteBatch spriteBatch)
		{
			//Draw background
			spriteBatch.Draw(
				texture: menuBackground,
				position: Vector2.Zero,
				color: Color.White,
				scale: menuBackgroundScale
				);

			//Draw control buttons
			fireButton.Draw(spriteBatch);
			fireAltButton.Draw(spriteBatch);

			//Draw covering backgrounds
			spriteBatch.Draw(
				texture: menuBackground,
				position: coverPos1,
				color: Color.White
				);

			//Draw covering backgrounds
			spriteBatch.Draw(
				texture: menuBackground,
				position: coverPos2,
				color: Color.White
				);

			//Draw options buttons
			optionsBackButton.Draw(spriteBatch);

			//Draw button labels
			spriteBatch.DrawString(
				spriteFont: font,
				text: "Button",
				position: bPosition,
				color: Color.Black
				);

			spriteBatch.DrawString(
				spriteFont: font,
				text: "Alt. Button",
				position: altBPosition,
				color: Color.Black
				);
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
						if (b.Contains(mState.Position.ToVector2()))
						{
							b.PrevSelected = b.Selected;
							b.Selected = true;

							if (!b.PrevSelected)
							{
								AudioManager.Instance.GetSoundEffect("Blip").Play();
							}
						}
						else
						{
							b.PrevSelected = b.Selected;
							b.Selected = false;
						}
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
