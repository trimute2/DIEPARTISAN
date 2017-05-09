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
		private Button playButton;
	
		private GameState menuState;
		private Button selected;
		private List<Button> mainMenuButtons;

		//Control setting vars
		Control_Types cont;
		bool alt;

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

		public void LoadContent(ContentManager Content)
		{
			playButton = new Button(TextureManager.Instance.GetBulletTexture("PlayerBullet"), new Rectangle(20, 20, 100, 100), "Set Fire Button");
			mainMenuButtons.Add(playButton);
		}

		public GameState MenuState
		{
			get { return menuState; }
			set {
				menuState = value;
				selected = null;
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

		public void UpdateMainMenu()
		{
			if (ControlManager.Instance.Mode == Control_Mode.KBM)
			{
				//Update states
				prevKbState = kbState;
				kbState = Keyboard.GetState();
				prevMState = mState;
				mState = Mouse.GetState();

				if (ControlManager.Instance.Setting)
				{
					ControlManager.Instance.SetControl(cont, alt);
				}
				else if (playButton.Contains(mState.Position.ToVector2()) && mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Released)
				{
					if (!ControlManager.Instance.Setting)
					{
						cont = Control_Types.Fire;
						alt = false;
						ControlManager.Instance.Setting = true;
					}
				}
			}
		}
	

		public void DrawMainMenu(SpriteBatch sb)
		{
			playButton.Draw(sb);
		}

	}
}
