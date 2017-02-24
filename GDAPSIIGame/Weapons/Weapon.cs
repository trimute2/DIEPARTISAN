using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
		private Vector2 TexPosition;

        public Weapon(ProjectileType pT, Texture2D texture, Vector2 position, Rectangle boundingBox, float fireRate, float clipSize, float reloadSpeed, Vector2 origin) : base(texture, position, boundingBox)
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
			this.angle = 0; //The angle of the weapon
			this.origin = origin; //The origin point of the weapon (where the player holds it)
			this.TexPosition = position;
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

			angle = -MathHelper.ToRadians(Player.Instance.Angle);
			TexPosition = Position;

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
					this.TexPosition.X = this.Position.X - 20;
					break;
				case Weapon_Dir.Left:
					this.TexPosition.X = this.Position.X - 20;
					break;
				case Weapon_Dir.DownLeft:
					this.TexPosition.X = this.Position.X - 20;
					break;
				case Weapon_Dir.Down:
					break;
				case Weapon_Dir.DownRight:
					this.TexPosition.X = this.Position.X + 20;
					break;
				case Weapon_Dir.Right:
					this.TexPosition.X = this.Position.X + 20;
					break;
				case Weapon_Dir.UpRight:
					this.TexPosition.X = this.Position.X + 20;
					break;
			}

			spriteBatch.Draw(this.Texture,
				this.TexPosition,
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
        public void Fire(Vector2 position, Vector2 direction)
        {
			//Check user can fire or if they need to reload
            if (!fired && !reload && clip > 0)
            {
				fired = true;
				clip--;
				ProjectileManager.Instance.Clone(projType, position, direction);
            }
        }
    }
}
