using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GDAPSIIGame
{
	class MenuManager
	{
		private static MenuManager instance;
		private Button playButton;
	
		private GameState menuState;
		private Button selected;

		public MenuManager Instance
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

		}

		public void Update()
		{
			if (ControlManager.Instance.Mode == Control_Mode.KBM)
			{
				switch (menuState)
				{
					case GameState.Menu:
						
						break;
				}
			}else
			{

			}
		}

		public void Draw(SpriteBatch sb)
		{
			switch (menuState)
			{
				case GameState.Menu:
					
					break;
			}
		}

	}
}
