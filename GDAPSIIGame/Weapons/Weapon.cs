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

	enum Range
	{
		Infinite=-10, Short=200, Medium=400, Long=600
	}

	abstract class Weapon : GameObject
	{
		//Fields
		protected ProjectileType projType;
		private float angle;
		private float range;
		private Weapon_Dir dir;

		public Weapon(ProjectileType pT, Texture2D texture, Vector2 position, Rectangle boundingBox, Range range = Range.Infinite) : base(texture, position, boundingBox)
		{
			//switch (range)
			//{
			//	case Range.Short:
			//		this.range = 200;
			//		break;
			//	case Range.Medium:
			//		this.range = 400;
			//		break;
			//	case Range.Long:
			//		this.range = 600;
			//		break;
			//	default:
			//		this.range = Projectile.INFINITE;
			//		break;
			//}
			this.range = (float)range;
			projType = pT;
			this.angle = 0; //The angle of the weapon in radians
			this.dir = Weapon_Dir.DownWest; //The direction of the weapon for drawing
		}

		/// <summary>
		/// The bullet the weapon fires
		/// </summary>
		public ProjectileType ProjType
		{
			get { return projType; }
			//set { projType = value; }
		}

		/// <summary>
		/// The angle of the weapon in radians
		/// </summary>
		public float Angle
		{
			get { return angle; }
			set { angle = value; }
		}

		public float WeapRange
		{
			get { return range; }
		}
		/// <summary>
		/// Whether the weapon is firing or not
		/// </summary>
		public abstract bool Fired
		{
			get; set;
		}

		public abstract bool Reload
		{
			get; set;
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

		public abstract bool Fire(Vector2 direction, MouseState mouseState, MouseState prevMouseState);

		public abstract bool Fire(Vector2 direction, GamePadState gpState, GamePadState prevGpState);

		public abstract void ReloadWeapon();

		public virtual void ResetWeapon()
		{
			//i feel gross for making this like this
		}
	}
}
