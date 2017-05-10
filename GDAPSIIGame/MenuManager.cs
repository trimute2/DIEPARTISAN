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

		//Main Menu
		private Texture2D menuBackground;
		private Vector2 menuBackgroundScale;
		private Texture2D title;
		private Vector2 titlePos;
		private Button playButton;
		private List<Button> mainMenuButtons;
		private bool mainMenuChange;


		private GameState menuState;

		//Control setting vars
		Control_Types cont;
		bool alt;

		public bool MainMenuChange
		{
			get { return mainMenuChange; }
			set { mainMenuChange = false; }
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

			//Main Menu
			menuBackground = textureManager.GetMenuTexture("MenuBackground");
			menuBackgroundScale = new Vector2((float)GraphicsDevice.Viewport.Width / menuBackground.Width, (float)GraphicsDevice.Viewport.Height / menuBackground.Height);

			title = textureManager.GetMenuTexture("Logo");
			titlePos = new Vector2((GraphicsDevice.Viewport.Width / 2) - title.Width, (10 * ((float)Math.Sin(0 * 1.5f))) + 20);

			Texture2D play = textureManager.GetMenuTexture("Play");
			playButton = new Button(play, new Rectangle((GraphicsDevice.Viewport.Width / 2) - play.Width/2, (GraphicsDevice.Viewport.Height / 2) + 100, 160, 60),
				Color.LightPink);
			mainMenuButtons.Add(playButton);
		}

		public GameState MenuState
		{
			get { return menuState; }
			set
			{
				menuState = value;
				mainMenuChange = false;
			}
		}

		private MenuManager()
		{
			kbState = Keyboard.GetState();
			prevKbState = kbState;
			mState = Mouse.GetState();
			prevMState = mState;
			gpState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
			prevGpState = gpState;
			alt = false;

			mainMenuButtons = new List<Button>();
		}

		public void UpdateMainMenu(GameTime gameTime)
		{
			titlePos.Y = (10*((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds*1.5f)))+20;

			if (ControlManager.Instance.Mode == Control_Mode.KBM)
			{
				//Update states
				prevKbState = kbState;
				kbState = Keyboard.GetState();
				prevMState = mState;
				mState = Mouse.GetState();

				//cont = Control_Types.Fire;
				//alt = false;
				//ControlManager.Instance.Setting = true;

				//Check if the player is currently setting a control
				if (ControlManager.Instance.Setting)
				{
					ControlManager.Instance.SetControl(cont, alt);
				}
				else
				{
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
				}
			}
		}
	

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
		}

	}
}
