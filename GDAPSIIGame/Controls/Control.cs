using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

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
        bool isAlternate;

        //Properties
        public Control_Types Name
        { get { return name; } }

        public Keys KeyboardControl
		{ get { return kbControl; } }

		public Buttons GamePadControl
		{ get { return gpControl; } }

		public MouseButtons MouseControl
		{ get { return mControl; } }

		public bool IsMouseControl
		{ get { return mouse; } }

        public bool IsAlternate
        { get { return isAlternate; } }

        public bool HasGamePadControl
		{ get { return hasGamePadControl; } }

		[JsonConstructor]
		public Control(Control_Types Name, Keys KeyBoardControl, Buttons GamePadControl, MouseButtons MouseControl, bool IsMouseControl, bool IsAlternate, bool HasGamePadControl)
		{
			this.name = Name;
			this.kbControl = KeyBoardControl;
			this.gpControl = GamePadControl;
			this.mControl = MouseControl;
			this.mouse = IsMouseControl;
			this.isAlternate = IsAlternate;
			this.hasGamePadControl = HasGamePadControl;
		}

		//Constructors
		public Control(Control_Types name)
		{
            this.name = name;
            hasGamePadControl = false;
            mouse = false;
            isAlternate = false;
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
            isAlternate = false;
            hasGamePadControl = false;
			gpControl = Buttons.BigButton;
		}

		/// <summary>
		/// Constructor for a keyboard control
		/// </summary>
		public Control(Control_Types name,  Buttons gpControl)
		{
			this.name = name;
			mouse = false;
			this.kbControl = Keys.None;
            isAlternate = false;
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
            isAlternate = false;
            this.mControl = mControl;
			kbControl = Keys.None;
			hasGamePadControl = false;
			gpControl = Buttons.BigButton;
		}

		public void SetControl(Keys kbc, bool alt)
		{
			this.kbControl = kbc;
            isAlternate = alt;
            mouse = false;
		}

		public void SetControl(MouseButtons mc, bool alt)
		{
			this.mControl = mc;
            isAlternate = alt;
            mouse = true;
		}

		public void SetControl(Buttons gpc, bool alt)
		{
			this.gpControl = gpc;
            isAlternate = alt;
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
