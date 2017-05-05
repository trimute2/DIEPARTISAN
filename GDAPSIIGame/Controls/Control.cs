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

		//Constructors
		public Control()
		{ }

		/// <summary>
		/// Constructor for a keyboard control
		/// </summary>
		public Control(Control_Types name, Keys kbControl, Buttons gpControl)
		{
			this.name = name;
			mouse = false;
			this.kbControl = kbControl;
			this.gpControl = gpControl;
			mControl = MouseButtons.None;
            alternate = null;
		}

		/// <summary>
		/// Constructor for a mouse control
		/// </summary>
		public Control(Control_Types name, MouseButtons mControl, Buttons gpControl)
		{
			this.name = name;
			mouse = true;
			this.mControl = mControl;
			this.gpControl = gpControl;
			kbControl = Keys.None;
            alternate = null;
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
		}

        public void SetAlternate(Control_Types name, Keys kbControl, Buttons gpControl)
        {
            if(!isAlternate)
            {
                isAlternate = false;
                alternate = new Control(name, kbControl, gpControl);
                alternate.isAlternate = true;
            }
        }

        public void SetAlternate(Control_Types name, MouseButtons mControl, Buttons gpControl)
        {
            if (!isAlternate)
            {
                isAlternate = false;
                alternate = new Control(name, mControl, gpControl);
                alternate.isAlternate = true;
            }
        }
    }
}
