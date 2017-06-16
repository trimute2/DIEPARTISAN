using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDAPSIIGame
{
	enum BaseResolution { r1920, r960, r480 }
	enum Resolutions { r1920 = 1920, r1152 = 1152, r960 = 960, r480 = 480 }

	class ResolutionManager
	{
		//Fields
		private static ResolutionManager instance;
		private BaseResolution baseResolution;
		private Resolutions resolution;
		private int resMult;

		//Properties
		/// <summary>
		/// The current resolution's multiplier for texture sizes
		/// </summary>
		public int ResMult { get { return resMult; } }

		public static ResolutionManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ResolutionManager();
				}
				return instance;
			}
		}

		//Constructor
		private ResolutionManager()
		{
			resMult = 0;
			baseResolution = BaseResolution.r480;
			resolution = Resolutions.r480;
			GetResolution();
		}

		//Methods
		/// <summary>
		/// Set the resolution
		/// </summary>
		public void SetResolution(Resolutions res)
		{
			resolution = res;

			if((int)res > 960)
			{
				baseResolution = BaseResolution.r1920;
				resMult = 1;
			}
			else if ((int)res > 480)
			{
				baseResolution = BaseResolution.r960;
			}
			else
			{
				baseResolution = BaseResolution.r480;
			}
		}

		/// <summary>
		/// Get the resolution from a file or create the default
		/// </summary>
		public void GetResolution()
		{
			//Check if settingsfile exists
			if (false)
			{

			}
			//File doesn't exist
			else
			{
				DetectResolution();
			}
		}

		/// <summary>
		/// Detect the current monitor's resolution
		/// </summary>
		public void DetectResolution()
		{
			List<DisplayMode> SupportedModes = new List<DisplayMode>();
			foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
			{
				SupportedModes.Add(mode);
			}

			Console.WriteLine("test");
		}
	}
}
