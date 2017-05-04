using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GDAPSIIGame.Controls;

namespace GDAPSIIGame
{
	enum Control_Types { Forward, Backward, Left, Right, Reload, Interact, Fire, NextWeapon, PrevWeapon, GPAim }
	enum MouseButtons { LeftButton, MiddleButton, RightButton, XButton1, XButton2, ScrollUp, ScrollDown, None}
	enum Control_Mode { KBM, GamePad}

	class ControlManager
	{
		static private ControlManager instance;
		private List<Control> controls;
		private Control_Mode mode;

		//States
		private MouseState ms;
		private MouseState prevMs;
		private MouseState prevPrevMs;
		private KeyboardState kbs;
		private KeyboardState prevKbs;
		private GamePadState gps;
		private GamePadState prevGps;

		/// <summary>
		/// Singleton Constructor
		/// </summary>
		private ControlManager()
		{
			controls = new List<Control>();
			mode = Control_Mode.KBM;

			//Current states
			gps = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
			kbs = Keyboard.GetState();
			ms = Mouse.GetState();

			//Previous states
			prevGps = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
			prevKbs = Keyboard.GetState();
			prevMs = Mouse.GetState();
			prevPrevMs = Mouse.GetState();

			ReadControlFile();
		}

		/// <summary>
		/// Singleton access
		/// </summary>
		/// <returns></returns>
		static public ControlManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ControlManager();
				}
				return instance;
			}
		}

		public Control_Mode Mode
		{ get { return mode; } }

		public void Update()
		{
			//Update states
			prevPrevMs = prevMs;
			prevGps = gps;
			prevKbs = kbs;
			prevMs = ms;
			gps = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
			kbs = Keyboard.GetState();
			ms = Mouse.GetState();

			//Check for corret input type
			CheckActiveGamePad();
			CheckActiveKBM();
		}

		/// <summary>
		/// If the GamePad is being used, set it as the control device
		/// </summary>
		public void CheckActiveGamePad()
		{
			if (mode != Control_Mode.GamePad && gps.IsConnected)
			{
				//Get the newest gamepad state
				if (gps.PacketNumber != prevGps.PacketNumber)
				{
					//Set the control mode to gamepad
					mode = Control_Mode.GamePad;
				}
			}
		}

		/// <summary>
		/// If the Keyboard is being used, set it as the control device
		/// </summary>
		public void CheckActiveKBM()
		{
			if (mode != Control_Mode.KBM)
			{
				//Check if the keyboard is being used
				if (kbs.GetPressedKeys().Length > 0)
				{
					//Set the control mode to keyboard+mouse
					mode = Control_Mode.KBM;
				}
				else if(ms.Position != prevMs.Position)
				{
					mode = Control_Mode.KBM;
				}
			}
		}

		/// <summary>
		/// Set all controls to their default values
		/// </summary>
		public void SetDefaults()
		{
			//Set movement for keyboard+mouse
			controls[(int)Control_Types.Forward].SetControl(Keys.W);
			controls[(int)Control_Types.Backward].SetControl(Keys.S);
			controls[(int)Control_Types.Left].SetControl(Keys.A);
			controls[(int)Control_Types.Right].SetControl(Keys.D);
			//Set gameplay controls for keyboard+mouse
			controls[(int)Control_Types.Reload].SetControl(Keys.R);
			controls[(int)Control_Types.Interact].SetControl(Keys.Enter);
			controls[(int)Control_Types.Fire].SetControl(MouseButtons.LeftButton);
			controls[(int)Control_Types.NextWeapon].SetControl(MouseButtons.ScrollUp);
			controls[(int)Control_Types.PrevWeapon].SetControl(MouseButtons.ScrollDown);

			//Set movement for gamepad
			controls[(int)Control_Types.Forward].SetControl(Buttons.LeftThumbstickUp);
			controls[(int)Control_Types.Backward].SetControl(Buttons.LeftThumbstickDown);
			controls[(int)Control_Types.Left].SetControl(Buttons.LeftThumbstickLeft);
			controls[(int)Control_Types.Right].SetControl(Buttons.LeftThumbstickRight);
			//Set gameplay controls for gamepad
			controls[(int)Control_Types.Reload].SetControl(Buttons.RightShoulder);
			controls[(int)Control_Types.Interact].SetControl(Buttons.A);
			controls[(int)Control_Types.Fire].SetControl(Buttons.RightTrigger);
			controls[(int)Control_Types.NextWeapon].SetControl(Buttons.DPadUp);
			controls[(int)Control_Types.PrevWeapon].SetControl(Buttons.DPadDown);

		}

		public void ReadControlFile()
		{
			for(int i = 0; i < 9; i++)
			{
				controls.Add(new Control());
			}
			SetDefaults();
		}

		public void SaveControlFile()
		{

		}

		//Return respective controls
		public String GetControl(Control_Types cont)
		{
			return controls[(int)cont].ToString();
		}

		public Keys GetKeyControl(Control_Types cont)
		{
			return controls[(int)cont].KeyboardControl;
		}

		public MouseButtons GetMouseControl(Control_Types cont)
		{
			return controls[(int)cont].MouseControl;
		}

		public Buttons GetGamePadControl(Control_Types cont)
		{
			return controls[(int)cont].GamePadControl;
		}

		/// <summary>
		/// Checks if the inputted control is pressed
		/// </summary>
		public bool ControlPressed(Control_Types cont, bool prev)
		{
			//Check if looking for previous states
			if (!prev)
			{
				//Check for control mode
				if (mode == Control_Mode.KBM)
				{
					if (controls[(int)cont].IsMouseControl)
					{
						return CheckMousePressed(cont, prev);
					}
					else return kbs.IsKeyDown(controls[(int)cont].KeyboardControl);
				}
				else
				{
					if (controls[(int)cont].GamePadControl == Buttons.RightTrigger)
					{
						return gps.Triggers.Right > 0;
					}
					else if (controls[(int)cont].GamePadControl == Buttons.LeftTrigger)
					{
						return gps.Triggers.Left > 0;
					}
					else return gps.IsButtonDown((controls[(int)cont].GamePadControl));
				}
			}
			else
			{
				//Check for control mode
				if (mode == Control_Mode.KBM)
				{
					if (controls[(int)cont].IsMouseControl)
					{
						return CheckMousePressed(cont, prev);
					}
					else return prevKbs.IsKeyDown(controls[(int)cont].KeyboardControl);
				}
				else
				{
					if (controls[(int)cont].GamePadControl == Buttons.RightTrigger)
					{
						return prevGps.Triggers.Right > 0;
					}
					else if (controls[(int)cont].GamePadControl == Buttons.LeftTrigger)
					{
						return prevGps.Triggers.Left > 0;
					}
					else if (controls[(int)cont].GamePadControl == Buttons.RightStick)
					{
						return true;
					}
					else return prevGps.IsButtonDown((controls[(int)cont].GamePadControl));
				}
			}
		}

		/// <summary>
		/// Checks if the inputted control is released
		/// </summary>
		public bool ControlReleased(Control_Types cont, bool prev)
		{
			//Check if looking for previous states
			if (!prev)
			{
				//Check for control mode
				if (mode == Control_Mode.KBM)
				{
					if (controls[(int)cont].IsMouseControl)
					{
						return CheckMouseReleased(cont, prev);
					}
					else return kbs.IsKeyUp(controls[(int)cont].KeyboardControl);
				}
				else
				{
					if (controls[(int)cont].GamePadControl == Buttons.RightTrigger)
					{
						return gps.Triggers.Right == 0;
					}
					else if (controls[(int)cont].GamePadControl == Buttons.LeftTrigger)
					{
						return gps.Triggers.Left == 0;
					}
					else return gps.IsButtonUp((controls[(int)cont].GamePadControl));
				}
			}
			else
			{
				//Check for control mode
				if (mode == Control_Mode.KBM)
				{
					if (controls[(int)cont].IsMouseControl)
					{
						return CheckMouseReleased(cont, prev);
					}
					else return prevKbs.IsKeyUp(controls[(int)cont].KeyboardControl);
				}
				else
				{
					if (controls[(int)cont].GamePadControl == Buttons.RightTrigger)
					{
						return prevGps.Triggers.Right == 0;
					}
					else if (controls[(int)cont].GamePadControl == Buttons.LeftTrigger)
					{
						return prevGps.Triggers.Left == 0;
					}
					else return prevGps.IsButtonUp((controls[(int)cont].GamePadControl));
				}
			}
		}

		/// <summary>
		/// Checks if the status of the inputted mouse control is pressed
		/// </summary>
		public bool CheckMousePressed(Control_Types cont, bool prev)
		{
			//Check if looking for previous states
			if (!prev)
			{
				switch (controls[(int)cont].MouseControl)
				{
					case MouseButtons.LeftButton:
						return ms.LeftButton == ButtonState.Pressed;
					case MouseButtons.MiddleButton:
						return ms.MiddleButton == ButtonState.Pressed;
					case MouseButtons.RightButton:
						return ms.RightButton == ButtonState.Pressed;
					case MouseButtons.XButton1:
						return ms.XButton1 == ButtonState.Pressed;
					case MouseButtons.XButton2:
						return ms.XButton2 == ButtonState.Pressed;
					case MouseButtons.ScrollUp:
						return ms.ScrollWheelValue > prevMs.ScrollWheelValue;
					case MouseButtons.ScrollDown:
						return ms.ScrollWheelValue < prevMs.ScrollWheelValue;
					case MouseButtons.None:
						return false;
				}
			}
			else
			{
				switch (controls[(int)cont].MouseControl)
				{
					case MouseButtons.LeftButton:
						return prevMs.LeftButton == ButtonState.Pressed;
					case MouseButtons.MiddleButton:
						return prevMs.MiddleButton == ButtonState.Pressed;
					case MouseButtons.RightButton:
						return prevMs.RightButton == ButtonState.Pressed;
					case MouseButtons.XButton1:
						return prevMs.XButton1 == ButtonState.Pressed;
					case MouseButtons.XButton2:
						return prevMs.XButton2 == ButtonState.Pressed;
					case MouseButtons.ScrollUp:
						return prevMs.ScrollWheelValue > prevPrevMs.ScrollWheelValue;
					case MouseButtons.ScrollDown:
						return prevMs.ScrollWheelValue < prevPrevMs.ScrollWheelValue;
					case MouseButtons.None:
						return false;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks if the status of the inputted mouse control is released
		/// </summary>
		public bool CheckMouseReleased(Control_Types cont, bool prev)
		{
			//Check if looking for previous states
			if (!prev)
			{
				switch (controls[(int)cont].MouseControl)
				{
					case MouseButtons.LeftButton:
						return ms.LeftButton == ButtonState.Released;
					case MouseButtons.MiddleButton:
						return ms.MiddleButton == ButtonState.Released;
					case MouseButtons.RightButton:
						return ms.RightButton == ButtonState.Released;
					case MouseButtons.XButton1:
						return ms.XButton1 == ButtonState.Released;
					case MouseButtons.XButton2:
						return ms.XButton2 == ButtonState.Released;
					case MouseButtons.ScrollUp:
						return ms.ScrollWheelValue == prevMs.ScrollWheelValue;
					case MouseButtons.ScrollDown:
						return ms.ScrollWheelValue == prevMs.ScrollWheelValue;
					case MouseButtons.None:
						return false;
				}
			}
			else
			{
				switch (controls[(int)cont].MouseControl)
				{
					case MouseButtons.LeftButton:
						return prevMs.LeftButton == ButtonState.Released;
					case MouseButtons.MiddleButton:
						return prevMs.MiddleButton == ButtonState.Released;
					case MouseButtons.RightButton:
						return prevMs.RightButton == ButtonState.Released;
					case MouseButtons.XButton1:
						return prevMs.XButton1 == ButtonState.Released;
					case MouseButtons.XButton2:
						return prevMs.XButton2 == ButtonState.Released;
					case MouseButtons.ScrollUp:
						return prevMs.ScrollWheelValue == prevPrevMs.ScrollWheelValue;
					case MouseButtons.ScrollDown:
						return prevMs.ScrollWheelValue == prevPrevMs.ScrollWheelValue;
					case MouseButtons.None:
						return false;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks if the status of the inputted mouse control is pressed
		/// </summary>
		public bool CheckGPPressed(Control_Types cont, bool prev)
		{
			//Check if looking for previous states
			if (!prev)
			{
				//Check for special controls
				if (controls[(int)cont].GamePadControl == Buttons.RightTrigger)
				{
					return gps.Triggers.Right > 0;
				}
				else if (controls[(int)cont].GamePadControl == Buttons.LeftTrigger)
				{
					return gps.Triggers.Left > 0;
				}
				else if (controls[(int)cont].GamePadControl == Buttons.LeftThumbstickUp)
				{
					return gps.ThumbSticks.Left.Y > 0;
				}
				else if (controls[(int)cont].GamePadControl == Buttons.LeftThumbstickDown)
				{
					return gps.ThumbSticks.Left.Y < 0;
				}
				else if (controls[(int)cont].GamePadControl == Buttons.LeftThumbstickRight)
				{
					return gps.ThumbSticks.Left.X > 0;
				}
				else if (controls[(int)cont].GamePadControl == Buttons.LeftThumbstickLeft)
				{
					return gps.ThumbSticks.Left.X > 0;
				}
			}
			else
			{
				switch (controls[(int)cont].MouseControl)
				{
					case MouseButtons.LeftButton:
						return prevMs.LeftButton == ButtonState.Pressed;
					case MouseButtons.MiddleButton:
						return prevMs.MiddleButton == ButtonState.Pressed;
					case MouseButtons.RightButton:
						return prevMs.RightButton == ButtonState.Pressed;
					case MouseButtons.XButton1:
						return prevMs.XButton1 == ButtonState.Pressed;
					case MouseButtons.XButton2:
						return prevMs.XButton2 == ButtonState.Pressed;
					case MouseButtons.ScrollUp:
						return prevMs.ScrollWheelValue > prevPrevMs.ScrollWheelValue;
					case MouseButtons.ScrollDown:
						return prevMs.ScrollWheelValue < prevPrevMs.ScrollWheelValue;
					case MouseButtons.None:
						return false;
				}
			}
			return false;
		}
	}
}
