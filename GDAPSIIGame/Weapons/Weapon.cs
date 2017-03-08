using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GDAPSIIGame.Map;

namespace GDAPSIIGame
{
	/// <summary>
	/// The direction the weapon sprite is facing
	/// </summary>
	enum Weapon_Dir { Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight }

    class Weapon : GameObject
    {
        //Fields
        ProjectileType projType;
        private float fireRate;
        private float clipSize;
		private float clip;
        private float reloadSpeed;
        private float fireTimer;
        private bool fired;
		private bool reload;
		private float reloadTimer;
		private Weapon_Dir dir;
		private float angle;
		private Vector2 origin;
		//private Vector2 muzzlePos;
		private Vector2 bulletOffset;
		private Owners owner;

        public Weapon(ProjectileType pT, Texture2D texture, Vector2 position, Rectangle boundingBox, float fireRate, float clipSize, float reloadSpeed, Vector2 origin, Owners owner) : base(texture, position, boundingBox)
        {
            this.projType = pT; //Type of projectile the weapon fires
            this.fireRate = fireRate; //How fast until the weapon can fire again
            this.clipSize = clipSize; //How large the clip is
			this.clip = clipSize; //The current amount of bullets in the clip
            this.reloadSpeed = reloadSpeed; //How long it takes to reload
			this.reload = false; //Whether the uesr is reloading
			this.reloadTimer = 0; //The timer used to increment a reload
            this.fireTimer = 0; //The timer used to control weapon fire rates
			this.fired = false; //Whether the weapon has fired
			this.dir = Weapon_Dir.Down; //The direction of the weapon for drawing
			this.angle = 0; //The angle of the weapon in radians
			this.origin = origin; //The origin point of the weapon (where the player holds it)
			//this.muzzlePos = new Vector2();
			this.bulletOffset = new Vector2(-boundingBox.Width/2, boundingBox.Height/4);
			this.owner = owner;
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
		/// The orientation of the weapon
		/// </summary>
		public Weapon_Dir Dir
		{
			get { return dir; }
			set { dir = value; }
		}

        /// <summary>
        /// The angle of the player's weapon in radians
        /// </summary>
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

		/// <summary>
		/// The position of the muzzle on the weapon
		/// </summary>
		//public Vector2 MuzzlePos
		//{
		//	get { return MuzzlePos; }
		//	set { muzzlePos = value; }
		//}

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

			switch (dir)
			{
				case Weapon_Dir.Up:
					this.bulletOffset = new Vector2(BoundingBox.Width / 4, BoundingBox.Height / 4);
					break;
				case Weapon_Dir.UpLeft:
					this.X = this.X - 20;
					this.bulletOffset = new Vector2(-BoundingBox.Width / 4, BoundingBox.Height / 4);
					break;
				case Weapon_Dir.Left:
					this.X = this.X - 20;
					this.bulletOffset = new Vector2(-BoundingBox.Width / 2, BoundingBox.Height / 4);
					break;
				case Weapon_Dir.DownLeft:
					this.X -= 20;
					this.bulletOffset = new Vector2(-BoundingBox.Width , BoundingBox.Height / 4);
					break;
				case Weapon_Dir.Down:
					this.bulletOffset = new Vector2(-BoundingBox.Width / 2, BoundingBox.Height / 4);
					break;
				case Weapon_Dir.DownRight:
					this.X = this.X + 20;
					this.bulletOffset = new Vector2(-BoundingBox.Width / 4, BoundingBox.Height / 4);
					break;
				case Weapon_Dir.Right:
					this.X = this.X + 20;
					this.bulletOffset = new Vector2(BoundingBox.Width / 2, BoundingBox.Height / 4);
					break;
				case Weapon_Dir.UpRight:
					this.X = this.X + 20;
					this.bulletOffset = new Vector2(BoundingBox.Width, BoundingBox.Height / 4);
					break;
			}

			//Update the muzzle's position
			//muzzlePos = Position + new Vector2(BoundingBox.Width/2, BoundingBox.Width);
			//float deltaOne = Position.X - (Position.X-BoundingBox.Width);
			//float deltaTwo = Position.Y - (Position.Y+BoundingBox.Height);
			//originPosition = new Vector2((Position.X-BoundingBox.Width) + origin.X * deltaOne, (Position.Y + BoundingBox.Height) + origin.Y * deltaTwo);
			//muzzlePos = RotateVector2(muzzlePos, angle, Position);

			//Control when user can fire again after just firing
			if (fired)
            {
				//Increment fireTimer
				fireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
				//Check if fireTimer meets the threshold
				if (fireTimer >= fireRate)
				{
					//Allow the user to fire again and reset timer
					fired = false;
					fireTimer -= fireRate;
				}
            }

			if (reload)
			{
				//Inrement reloadTimer
				reloadTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
				//Check if reloadTimer meets the threshold
				if (reloadTimer >= reloadSpeed)
				{
					//Reload the clip
					reload = false;
					clip = clipSize;
					reloadTimer -= reloadSpeed;
				}
			}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
			switch (dir)
			{
				case Weapon_Dir.Up: 
					break;
				case Weapon_Dir.UpLeft:
					break;
				case Weapon_Dir.Left:
					break;
				case Weapon_Dir.DownLeft:
					break;
				case Weapon_Dir.Down:
					break;
				case Weapon_Dir.DownRight:
					break;
				case Weapon_Dir.Right:
					break;
				case Weapon_Dir.UpRight:
					break;
			}

			spriteBatch.Draw(this.Texture,
                Camera.Instance.GetViewportPosition(this),
                null,
				null,
				origin,
				angle,
				this.Scale,
				Color.White);
		}

		/// <summary>
		/// Tell the weapon it is time to reload
		/// </summary>
		public void Reload()
		{
			if (!reload && clip < clipSize)
			{
				reload = true;
			}
		}

		/// <summary>
		/// Fire a bullet from the weapon
		/// </summary>
		/// <param name="position">The position the bullet is spawned at</param>
		/// <param name="direction">The speed that the bullet is moving</param>
        public void Fire(Vector2 direction)
        {
            //Check user can fire or if they need to reload
            if (!fired && !reload && clip > 0)
            {
                fired = true;
                clip--;
				Matrix rotationMatrix = Matrix.CreateRotationZ(angle);
				Vector2 bulletPosition = Vector2.Transform(bulletOffset, rotationMatrix);

				ProjectileManager.Instance.Clone(projType, Camera.Instance.GetViewportPosition(Position+bulletPosition), direction, owner);
            }
        }

		//private Vector2 RotateVector2(Vector2 point, float radians, Vector2 pivot)
		//{
		//	//Get the cos and sin of the angle
		//	float cosRadians = (float)Math.Cos(radians);
		//	float sinRadians = (float)Math.Sin(radians);

		//	//Get the vector between the two points
		//	Vector2 translatedPoint = new Vector2();
		//	translatedPoint.X = point.X - pivot.X;
		//	translatedPoint.Y = point.Y - pivot.Y;

		//	//Rotate the point
		//	Vector2 rotatedPoint = new Vector2();
		//	rotatedPoint.X = translatedPoint.X * cosRadians - translatedPoint.Y * sinRadians + pivot.X;
		//	rotatedPoint.Y = translatedPoint.X * sinRadians + translatedPoint.Y * cosRadians + pivot.Y;

		//	return rotatedPoint;
		//}
    }
}
