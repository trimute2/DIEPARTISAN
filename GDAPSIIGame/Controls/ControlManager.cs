using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GDAPSIIGame.Controls;
using Newtonsoft.Json;
using System.IO;

namespace GDAPSIIGame
{
	enum Control_Types { Forward, Backward, Left, Right, Reload, Interact, Fire, NextWeapon, PrevWeapon, GPAim }
	enum MouseButtons { LeftButton, MiddleButton, RightButton, XButton1, XButton2, ScrollUp, ScrollDown, None}
	enum Control_Mode { KBM, GamePad}

	class ControlManager
	{
		static private ControlManager instance;
		private List<Control> controls;
        private List<Control> alternateControls;
        private Control_Mode mode;

		//States
		private MouseState ms;
		private MouseState prevMs;
		private MouseState prevPrevMs;
		private KeyboardState kbs;
		private KeyboardState prevKbs;
		private GamePadState gps;
		private GamePadState prevGps;
		private bool setting;

		/// <summary>
		/// Singleton Constructor
		/// </summary>
		private ControlManager()
		{
			controls = new List<Control>();
            alternateControls = new List<Control>();
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

		/// <summary>
		/// Whether the game is setting a new control or not
		/// </summary>
		public bool Setting
		{
			get { return setting; }
			set { setting = value; }
		}

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
			controls[(int)Control_Types.Forward].SetControl(Keys.W, false);
			controls[(int)Control_Types.Backward].SetControl(Keys.S, false);
			controls[(int)Control_Types.Left].SetControl(Keys.A, false);
			controls[(int)Control_Types.Right].SetControl(Keys.D, false);
			//Set gameplay controls for keyboard+mouse
			controls[(int)Control_Types.Reload].SetControl(Keys.R, false);
			controls[(int)Control_Types.Interact].SetControl(Keys.Enter, false);
			controls[(int)Control_Types.Fire].SetControl(MouseButtons.LeftButton, false);
			controls[(int)Control_Types.NextWeapon].SetControl(MouseButtons.ScrollUp, false);
			controls[(int)Control_Types.PrevWeapon].SetControl(MouseButtons.ScrollDown, false);
            alternateControls[(int)Control_Types.NextWeapon].SetControl(Keys.LeftControl, true);

            //Set movement for gamepad
            controls[(int)Control_Types.Forward].SetControl(Buttons.LeftThumbstickUp, false);
			controls[(int)Control_Types.Backward].SetControl(Buttons.LeftThumbstickDown, false);
			controls[(int)Control_Types.Left].SetControl(Buttons.LeftThumbstickLeft, false);
			controls[(int)Control_Types.Right].SetControl(Buttons.LeftThumbstickRight, false);
			//Set gameplay controls for gamepad
			controls[(int)Control_Types.Reload].SetControl(Buttons.RightShoulder, false);
			controls[(int)Control_Types.Interact].SetControl(Buttons.A, false);
			controls[(int)Control_Types.Fire].SetControl(Buttons.RightTrigger, false);
			controls[(int)Control_Types.NextWeapon].SetControl(Buttons.DPadUp, false);
			alternateControls[(int)Control_Types.NextWeapon].SetControl(Buttons.LeftShoulder, true);
			controls[(int)Control_Types.PrevWeapon].SetControl(Buttons.DPadDown, false);

		}

		/// <summary>
		/// Read in controls from a file
		/// </summary>
		public void ReadControlFile()
		{
            //Define StreamReader
            StreamReader input = null;

            //Try catch to get input
            try
            {
				String path = Path.Combine(Environment.GetFolderPath(
					Environment.SpecialFolder.MyDoc‌​uments), "DIE-PARTISAN", "controls.txt");

				if (Directory.Exists(Path.Combine(Environment.GetFolderPath(
					Environment.SpecialFolder.MyDoc‌​uments), "DIE-PARTISAN")))
				{
					input = new StreamReader(path);
					if (input != null)
					{
						for (int i = 0; i < 9; i++)
						{
							controls.Add(new Control((Control_Types)i));
							alternateControls.Add(new Control((Control_Types)i));
						}

						String line;
						while ((line = input.ReadLine()) != null)
						{
							Control c = JsonConvert.DeserializeObject<Control>(line);
							if (!c.IsAlternate)
							{
								controls[(int)c.Name] = c;
							}
							else
							{
								alternateControls[(int)c.Name] = c;
							}
						}
					}
				}
				else
				{
					Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(
					Environment.SpecialFolder.MyDoc‌​uments), "DIE-PARTISAN"));
					for (int i = 0; i < 9; i++)
					{
						controls.Add(new Control((Control_Types)i));
						alternateControls.Add(new Control((Control_Types)i));
					}
					SetDefaults();
					SaveControlFile();
				}
            }
            catch (FileNotFoundException)
            {
				Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(
					Environment.SpecialFolder.MyDoc‌​uments), "DIE-PARTISAN"));
				for (int i = 0; i < 9; i++)
                {
                    controls.Add(new Control((Control_Types)i));
                    alternateControls.Add(new Control((Control_Types)i));
                }
                SetDefaults();
				SaveControlFile();
            }
            //Close the StreamReader
            finally { if (input != null) { input.Close(); } }
        }

        /// <summary>
        /// Save the controls to a external file
        /// </summary>
		public void SaveControlFile()
		{
            //Declare streamwriter
            StreamWriter output = null;

            //Try catch to write controls
            try
            {
				String path = Path.Combine(Environment.GetFolderPath(
					Environment.SpecialFolder.MyDoc‌​uments), "DIE-PARTISAN", "controls.txt");
				//Write controls
				output = new StreamWriter(path);
                foreach (Control c in controls)
                {
                    output.WriteLine(JsonConvert.SerializeObject(c));
                }
                foreach (Control c in alternateControls)
                {
					if (c.IsAlternate)
					{
						output.WriteLine(JsonConvert.SerializeObject(c));
					}
                }
            }
            catch (Exception e)
            {
                //Print error message
                Console.WriteLine("Error with file: " + e.Message);
            }
            //Close the StreamReader
            finally { if (output != null) { output.Close(); } }
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

		public bool SetControl(Control_Types cont, bool alt)
		{
			if (setting)
			{
				//Get if alternate
				Control c;
				if (!alt)
				{
					c = controls[(int)cont];
				}
				else c = alternateControls[(int)cont];

				if (mode == Control_Mode.KBM)
				{
					//Get pressed stuff
					MouseButtons m = GetMouseButton();
					Keys k = GetKey();

					//Check if they're not "null"
					if (m != MouseButtons.None)
					{
						c.SetControl(m, false);
						Console.WriteLine("Set: " + cont + " to " + m);
						setting = false;
						return true;
					}
					else if (k != Keys.None)
					{
						c.SetControl(k, false);
						Console.WriteLine("Set: " + cont + " to " + k);
						setting = false;
						return true;
					}
				}
				else if (mode == Control_Mode.GamePad)
				{
					//Get pressed stuff
					Buttons b = GetGPButton();

					//The BigButton never gets used	
					if (b != Buttons.BigButton)
					{
						c.SetControl(b, false);
						setting = false;
						return true;
					}
				}
				return false;
			}
			else
			{
				setting = true;
				return false;
			}
		}

        /// <summary>
        /// Checks if the current state is pressed and the previous state is released. Includes alternates
        /// </summary>
        public bool ControlPressedControlPrevReleased(Control_Types cont)
        {
            return (ControlPressed(cont, false, false) && ControlReleased(cont, true, false)) || (ControlPressed(cont, false, true) && ControlReleased(cont, true, true));
        }

        /// <summary>
        /// Checks if the current state is pressed
        /// </summary>
        public bool ControlPressed(Control_Types cont)
        {
            return (ControlPressed(cont, false, false)) || (ControlPressed(cont, false, true));
        }

        /// <summary>
        /// Checks if the current state is released
        /// </summary>
        public bool ControlReleased(Control_Types cont)
        {
            return (ControlReleased(cont, false, false)) || (ControlReleased(cont, false, true));
        }

        /// <summary>
        /// Checks if the previous state is pressed
        /// </summary>
        public bool ControPrevPressed(Control_Types cont)
        {
            return (ControlPressed(cont, true, false)) || (ControlPressed(cont, true, true));
        }

        /// <summary>
        /// Checks if the previous state is released
        /// </summary>
        public bool ControlPrevReleased(Control_Types cont)
        {
            return (ControlReleased(cont, true, false)) || (ControlReleased(cont, true, true));
        }

        /// <summary>
        /// Checks if the inputted control is pressed
        /// </summary>
        private bool ControlPressed(Control_Types cont, bool prev, bool alt)
		{
            Control c;
			//If checking for alternate controls, check for it
			if (alt)
            { c = alternateControls[(int)cont]; }
            else c = controls[(int)cont];

            if (c != null)
            {
                //Check if looking for previous states
                if (!prev)
                {
                    //Check for control mode
                    if (mode == Control_Mode.KBM)
                    {
                        if (c.IsMouseControl)
                        {
                            return CheckMousePressed(cont, prev, alt);
                        }
                        else return kbs.IsKeyDown(c.KeyboardControl);
                    }
                    else if (c.HasGamePadControl)
                    {
                        if (c.GamePadControl == Buttons.RightTrigger)
                        {
                            return gps.Triggers.Right > 0;
                        }
                        else if (c.GamePadControl == Buttons.LeftTrigger)
                        {
                            return gps.Triggers.Left > 0;
                        }
                        else return gps.IsButtonDown(c.GamePadControl);
                    }
                }
                else
                {
                    //Check for control mode
                    if (mode == Control_Mode.KBM)
                    {
                        if (c.IsMouseControl)
                        {
                            return CheckMousePressed(cont, prev, alt);
                        }
                        else return prevKbs.IsKeyDown(c.KeyboardControl);
                    }
                    else if (c.HasGamePadControl)
                    {
                        if (c.GamePadControl == Buttons.RightTrigger)
                        {
                            return prevGps.Triggers.Right > 0;
                        }
                        else if (c.GamePadControl == Buttons.LeftTrigger)
                        {
                            return prevGps.Triggers.Left > 0;
                        }
                        else if (c.GamePadControl == Buttons.RightStick)
                        {
                            return true;
                        }
                        else return prevGps.IsButtonDown(c.GamePadControl);
                    }
                }
            }
            return false;
		}

		/// <summary>
		/// Checks if the inputted control is released
		/// </summary>
		private bool ControlReleased(Control_Types cont, bool prev, bool alt)
		{
            Control c;
            //If checking for alternate controls, check for it
            if (alt)
            { c = alternateControls[(int)cont]; }
            else c = controls[(int)cont];

            if (c != null)
            {
                //Check if looking for previous states
                if (!prev)
                {
                    //Check for control mode
                    if (mode == Control_Mode.KBM)
                    {
                        if (c.IsMouseControl)
                        {
                            return CheckMouseReleased(cont, prev, alt);
                        }
                        else return kbs.IsKeyUp(c.KeyboardControl);
                    }
                    else if (c.HasGamePadControl)
                    {
                        if (c.GamePadControl == Buttons.RightTrigger)
                        {
                            return gps.Triggers.Right == 0;
                        }
                        else if (c.GamePadControl == Buttons.LeftTrigger)
                        {
                            return gps.Triggers.Left == 0;
                        }
                        else return gps.IsButtonUp(c.GamePadControl);
                    }
                }
                else
                {
                    //Check for control mode
                    if (mode == Control_Mode.KBM)
                    {
                        if (c.IsMouseControl)
                        {
                            return CheckMouseReleased(cont, prev, alt);
                        }
                        else return prevKbs.IsKeyUp(c.KeyboardControl);
                    }
                    else if (c.HasGamePadControl)
                    {
                        if (c.GamePadControl == Buttons.RightTrigger)
                        {
                            return prevGps.Triggers.Right == 0;
                        }
                        else if (c.GamePadControl == Buttons.LeftTrigger)
                        {
                            return prevGps.Triggers.Left == 0;
                        }
                        else return prevGps.IsButtonUp(c.GamePadControl);
                    }
                }
            }
            return false;
		}

		/// <summary>
		/// Checks if the status of the inputted mouse control is pressed
		/// </summary>
		private bool CheckMousePressed(Control_Types cont, bool prev, bool alt)
		{
            Control c;
            //If checking for alternate controls, check for it
            if (alt)
            { c = alternateControls[(int)cont]; }
            else c = controls[(int)cont];

            if (c != null)
            {
                //Check if looking for previous states
                if (!prev)
                {
                    switch (c.MouseControl)
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
                    switch (c.MouseControl)
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
            }
			return false;
		}

		/// <summary>
		/// Checks if the status of the inputted mouse control is released
		/// </summary>
		private bool CheckMouseReleased(Control_Types cont, bool prev, bool alt)
		{
            Control c;
            //If checking for alternate controls, check for it
            if (alt)
            { c = alternateControls[(int)cont]; }
            else c = controls[(int)cont];

            if (c != null)
            {
                //Check if looking for previous states
                if (!prev)
                {
                    switch (c.MouseControl)
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
                    switch (c.MouseControl)
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
            }
			return false;
		}

		/// <summary>
		/// Returns the currently pressed mouse button
		/// </summary>
		private MouseButtons GetMouseButton()
		{
			if (ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton == ButtonState.Released)
			{
				return MouseButtons.LeftButton;
			}
			else if (ms.MiddleButton == ButtonState.Pressed && prevMs.MiddleButton == ButtonState.Released)
			{
				return MouseButtons.MiddleButton;
			}
			else if (ms.RightButton == ButtonState.Pressed && prevMs.RightButton == ButtonState.Released)
			{
				return MouseButtons.RightButton;
			}
			else if (ms.XButton1 == ButtonState.Pressed && prevMs.XButton1 == ButtonState.Released)
			{
				return MouseButtons.XButton1;
			}
			else if (ms.XButton2 == ButtonState.Pressed && prevMs.XButton2 == ButtonState.Released)
			{
				return MouseButtons.XButton2;
			}
			else if (ms.ScrollWheelValue > prevMs.ScrollWheelValue)
			{
				return MouseButtons.ScrollUp;
			}
			else if (ms.ScrollWheelValue < prevMs.ScrollWheelValue)
			{
				return MouseButtons.ScrollDown;
			}
			else return MouseButtons.None;
		}

		/// <summary>
		/// Returns the currently pressed key
		/// </summary>
		private Keys GetKey()
		{
			foreach (Keys k in kbs.GetPressedKeys())
			{
				if (kbs.IsKeyDown(k) && prevKbs.IsKeyUp(k))
				{
					return k;
				}
			}
			return Keys.None;
		}

		/// <summary>
		/// Returns the currently pressed gamepad button
		/// </summary>
		private Buttons GetGPButton()
		{
			if (gps.Buttons.A == ButtonState.Pressed && prevGps.Buttons.A == ButtonState.Released)
			{
				return Buttons.A;
			}
			else if (gps.Buttons.B == ButtonState.Pressed && prevGps.Buttons.B == ButtonState.Released)
			{
				return Buttons.B;
			}
			else if (gps.Buttons.X == ButtonState.Pressed && prevGps.Buttons.X == ButtonState.Released)
			{
				return Buttons.X;
			}
			else if (gps.Buttons.Y == ButtonState.Pressed && prevGps.Buttons.Y == ButtonState.Released)
			{
				return Buttons.Y;
			}
			else if (gps.Buttons.LeftShoulder == ButtonState.Pressed && prevGps.Buttons.LeftShoulder == ButtonState.Released)
			{
				return Buttons.LeftShoulder;
			}
			else if (gps.Buttons.LeftStick == ButtonState.Pressed && prevGps.Buttons.LeftStick == ButtonState.Released)
			{
				return Buttons.LeftStick;
			}
			else if (gps.Buttons.RightShoulder == ButtonState.Pressed && prevGps.Buttons.RightShoulder == ButtonState.Released)
			{
				return Buttons.RightShoulder;
			}
			else if (gps.Buttons.RightStick == ButtonState.Pressed && prevGps.Buttons.RightStick == ButtonState.Released)
			{
				return Buttons.RightStick;
			}
			else if (gps.Buttons.Start == ButtonState.Pressed && prevGps.Buttons.Start == ButtonState.Released)
			{
				return Buttons.Start;
			}
			else if (gps.Buttons.Back == ButtonState.Pressed && prevGps.Buttons.Back == ButtonState.Released)
			{
				return Buttons.Back;
			}
			else if (gps.DPad.Up == ButtonState.Pressed && prevGps.DPad.Up == ButtonState.Released)
			{
				return Buttons.DPadUp;
			}
			else if (gps.DPad.Down == ButtonState.Pressed && prevGps.DPad.Down == ButtonState.Released)
			{
				return Buttons.DPadDown;
			}
			else if (gps.DPad.Left == ButtonState.Pressed && prevGps.DPad.Left == ButtonState.Released)
			{
				return Buttons.DPadLeft;
			}
			else if (gps.DPad.Right == ButtonState.Pressed && prevGps.DPad.Right == ButtonState.Released)
			{
				return Buttons.DPadRight;
			}
			else if (gps.Triggers.Left > 0 && prevGps.Triggers.Left == 0)
			{
				return Buttons.LeftTrigger;
			}
			else if (gps.Triggers.Right > 0 && prevGps.Triggers.Right == 0)
			{
				return Buttons.RightTrigger;
			}
			else return Buttons.BigButton;
		}
	}
}
