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
	enum Weapon_Dir { UpEast, UpWest, UpLeft, Left, DownLeft, DownWest, DownEast, DownRight, Right, UpRight }

    class Weapon : GameObject
    {
        //Fields
        ProjectileType projType;
        private float fireRate;
        private float clipSize;
		private float clip;
        private float reloadSpeed;
        private float fired;
		private float reload;
		private Weapon_Dir dir;
		private float angle;
		private Vector2 origin;
		private Vector2 bulletOffset;
		private Owners owner;
		private SpriteEffects effects;

        public Weapon(ProjectileType pT, Texture2D texture, Vector2 position, Rectangle boundingBox, float fireRate, float clipSize, float reloadSpeed, Vector2 origin, Owners owner) : base(texture, position, boundingBox)
        {
            this.projType = pT; //Type of projectile the weapon fires
            this.fireRate = fireRate; //How fast until the weapon can fire again
            this.clipSize = clipSize; //How large the clip is
			this.clip = clipSize; //The current amount of bullets in the clip
            this.reloadSpeed = reloadSpeed; //How long it takes to reload
			this.reload = 0; //Timer for whether the uesr is reloading
			this.fired = 0; //Whether the weapon has fired
			this.dir = Weapon_Dir.DownWest; //The direction of the weapon for drawing
			this.angle = 0; //The angle of the weapon in radians
			this.origin = origin; //The origin point of the weapon (where the player holds it)
			this.bulletOffset = new Vector2(-boundingBox.Width/2, boundingBox.Height/4);
			this.owner = owner;
			effects = SpriteEffects.None;
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
        /// The angle of the weapon in radians
        /// </summary>
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

		/// <summary>
		/// Whether the weapon is reloading or not
		/// </summary>
		public bool Reload
		{
			get { return reload > 0; }
			private set
			{
				if (value)
				{
					reload = reloadSpeed;
				}
				else reload = 0;
			}
		}

		/// <summary>
		/// Whether the weapon is firing or not
		/// </summary>
		public bool Fired
		{
			get { return fired > 0; }
			private set
			{
				if (value)
				{
					fired = fireRate;
				}
				else fired = 0;
			}
		}

		public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

			switch (dir)
			{
				case Weapon_Dir.UpEast:
					this.X += 20;
					this.bulletOffset = new Vector2(BoundingBox.Width / 4, BoundingBox.Height / 4);
					break;
				case Weapon_Dir.UpWest:
					this.X -= 20;
					this.bulletOffset = new Vector2(BoundingBox.Width / 4, BoundingBox.Height / 4);
					break;
				case Weapon_Dir.UpLeft:
					this.X -= 20;
					this.bulletOffset = new Vector2(-BoundingBox.Width / 4, BoundingBox.Height / 4);
					break;
				case Weapon_Dir.Left:
					this.X -= 20;
					this.bulletOffset = new Vector2(-BoundingBox.Width / 2, BoundingBox.Height / 4);
					break;
				case Weapon_Dir.DownLeft:
					this.X -= 20;
					this.bulletOffset = new Vector2(-BoundingBox.Width , BoundingBox.Height / 4);
					break;
				case Weapon_Dir.DownWest:
					this.X -= 20;
					this.bulletOffset = new Vector2(-BoundingBox.Width / 2, BoundingBox.Height / 4);
					break;
				case Weapon_Dir.DownEast:
					this.X += 20;
					this.bulletOffset = new Vector2(-BoundingBox.Width / 2, BoundingBox.Height / 4);
					break;
				case Weapon_Dir.DownRight:
					this.X += 20;
					this.bulletOffset = new Vector2(-BoundingBox.Width / 4, BoundingBox.Height / 4);
					break;
				case Weapon_Dir.Right:
					this.X += 20;
					this.bulletOffset = new Vector2(BoundingBox.Width / 2, BoundingBox.Height / 4);
					break;
				case Weapon_Dir.UpRight:
					this.X += 20;
					this.bulletOffset = new Vector2(BoundingBox.Width, BoundingBox.Height / 4);
					break;
			}

			//Control when user can fire again after just firing
			if (Fired)
            {
				//Increment fireTimer
				fired -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				//Check if fireTimer meets the threshold
				if (!Fired)
				{
					//Allow the user to fire again and reset timer
					Fired = false;
				}
            }

			if (Reload)
			{
				//Inrement reloadTimer
				reload -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				//Check if reloadTimer meets the threshold
				if (!Reload)
				{
					//Reload the clip
					clip = clipSize;
				}
			}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
			switch (dir)
			{
				case Weapon_Dir.UpEast:
					effects = SpriteEffects.FlipHorizontally;
					break;
				case Weapon_Dir.UpWest:
					effects = SpriteEffects.None;
					break;
				case Weapon_Dir.UpLeft:
					effects = SpriteEffects.None;
					break;
				case Weapon_Dir.Left:
					effects = SpriteEffects.None;
					break;
				case Weapon_Dir.DownLeft:
					effects = SpriteEffects.None;
					break;
				case Weapon_Dir.DownWest:
					effects = SpriteEffects.None;
					break;
				case Weapon_Dir.DownEast:
					effects = SpriteEffects.FlipHorizontally;
					break;
				case Weapon_Dir.DownRight:
					effects = SpriteEffects.FlipHorizontally;
					break;
				case Weapon_Dir.Right:
					effects = SpriteEffects.FlipHorizontally;
					break;
				case Weapon_Dir.UpRight:
					effects = SpriteEffects.FlipHorizontally;
					break;
			}

			spriteBatch.Draw(this.Texture,
                Camera.Instance.GetViewportPosition(this),
                null,
				null,
				origin,
				angle,
				this.Scale,
				Color.White,
				effects);
		}

		/// <summary>
		/// Tell the weapon it is time to reload
		/// </summary>
		public void ReloadWeapon()
		{
			if (!Reload && clip < clipSize)
			{
				Reload = true;
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
            if (!Fired && !Reload && clip > 0)
            {
                Fired = true;
                clip--;
				Matrix rotationMatrix = Matrix.CreateRotationZ(angle);
				Vector2 bulletPosition = Vector2.Transform(bulletOffset, rotationMatrix);

				ProjectileManager.Instance.Clone(projType, Position+bulletPosition, direction, owner);
            }
        }
    }
}
