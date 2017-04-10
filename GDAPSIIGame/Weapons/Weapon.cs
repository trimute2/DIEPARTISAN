using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDAPSIIGame.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GDAPSIIGame.Map;
using Microsoft.Xna.Framework.Input;

namespace GDAPSIIGame.Weapons
{

	/// <summary>
	/// The direction the weapon sprite is facing
	/// </summary>
	enum Weapon_Dir { UpEast, UpWest, UpLeft, Left, DownLeft, DownWest, DownEast, DownRight, Right, UpRight }

	abstract class Weapon : GameObject
	{
		//Fields
		ProjectileType projType;
		private float angle;
		private Weapons.Weapon_Dir dir;

		public Weapon(ProjectileType pT, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(texture, position, boundingBox)
		{
			projType = pT;
			this.angle = 0; //The angle of the weapon in radians
			this.dir = Weapons.Weapon_Dir.DownWest; //The direction of the weapon for drawing
		}

		/// <summary>
		/// The bullet the weapon fires
		/// </summary>
		public ProjectileType ProjType
		{
			get { return projType; }
			set { projType = value; }
		}

		/// <summary>
		/// The angle of the weapon in radians
		/// </summary>
		public float Angle
		{
			get { return angle; }
			set { angle = value; }
		}

		/// <summary>
		/// Whether the weapon is firing or not
		/// </summary>
		public abstract bool Fired
		{
			get;
			set;
		}

		/// <summary>
		/// The orientation of the weapon
		/// </summary>
		public Weapons.Weapon_Dir Dir
		{
			get { return dir; }
			set { dir = value; }
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
		}

		public abstract void Fire(Vector2 direction, MouseState mouseState, MouseState prevMouseState);

		public abstract void ReloadWeapon();
	}
}
