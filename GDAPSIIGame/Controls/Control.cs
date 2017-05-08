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

		public bool HasGamePadControl
		{ get { return hasGamePadControl; } }

		//Constructors
		public Control(Control_Types name)
		{
            this.name = name;
            hasGamePadControl = false;
            mouse = false;
            this.kbControl = Keys.None;
            mControl = MouseButtons.None;
        }

		/// <summary>
		/// Constructor for a keyboard control
		/// </summary>
		public Control(Control_Types name, Keys kbControl)
		{
			this.name = name;
			mouse = false;
			this.kbControl = kbControl;
			mControl = MouseButtons.None;
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
	}
}
