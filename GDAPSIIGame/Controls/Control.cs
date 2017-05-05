using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDAPSIIGame.Controls
{
	class Control
	{
		//Fields
		Control_Types name;
		bool mouse;
		Keys kbControl;
		Buttons gpControl;
		MouseButtons mControl;
        Control alternate;
        bool isAlternate;
		bool hasGamePadControl;

		//Properties
		public Keys KeyboardControl
		{ get { return kbControl; } }

		public Buttons GamePadControl
		{ get { return gpControl; } }

		public MouseButtons MouseControl
		{ get { return mControl; } }

		public bool IsMouseControl
		{ get { return mouse; } }

        public Control Alternate
        { get { return alternate; } }

		public bool HasGamePadControl
		{ get { return hasGamePadControl; } }

		//Constructors
		public Control()
		{ }

		/// <summary>
		/// Constructor for a keyboard control
		/// </summary>
		public Control(Control_Types name, Keys kbControl)
		{
			this.name = name;
			mouse = false;
			this.kbControl = kbControl;
			mControl = MouseButtons.None;
            alternate = null;
			hasGamePadControl = false;
		}

		/// <summary>
		/// Constructor for a keyboard control
		/// </summary>
		public Control(Control_Types name,  Buttons gpControl)
		{
			this.name = name;
			mouse = false;
			this.kbControl = Keys.None;
			this.gpControl = gpControl;
			mControl = MouseButtons.None;
			alternate = null;
			hasGamePadControl = true;
		}

		/// <summary>
		/// Constructor for a mouse control
		/// </summary>
		public Control(Control_Types name, MouseButtons mControl)
		{
			this.name = name;
			mouse = true;
			this.mControl = mControl;
			kbControl = Keys.None;
            alternate = null;
			hasGamePadControl = false;
		}

		public void SetControl(Keys kbc)
		{
			this.kbControl = kbc;
			mouse = false;
		}

		public void SetControl(MouseButtons mc)
		{
			this.mControl = mc;
			mouse = true;
		}

		public void SetControl(Buttons gpc)
		{
			this.gpControl = gpc;
			hasGamePadControl = true;
		}

		public void RemoveGamePadControl()
		{
			hasGamePadControl = false;
		}

		public bool HasControls()
		{
			return ((kbControl != Keys.None) || (mControl != MouseButtons.None) || (hasGamePadControl));
		}

        public void SetAlternate(Control_Types name, Keys kbControl)
        {
            if(!isAlternate)
            {
				if (alternate == null)
				{
					alternate = new Control(name, kbControl);
					alternate.isAlternate = true;
				}
				else
				{
					alternate.SetControl(kbControl);
				}
            }
        }

        public void SetAlternate(Control_Types name, MouseButtons mControl)
        {
			if (!isAlternate)
			{
				if (alternate == null)
				{
					alternate = new Control(name, mControl);
					alternate.isAlternate = true;
				}
				else
				{
					alternate.SetControl(mControl);
				}
			}
		}

		public void SetAlternate(Control_Types name, Buttons gpControl)
		{
			if (!isAlternate)
			{
				if (alternate == null)
				{
					alternate = new Control(name, gpControl);
					alternate.isAlternate = true;
				}
				else
				{
					alternate.SetControl(gpControl);
				}
			}
		}
	}
}
