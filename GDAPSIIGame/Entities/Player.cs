using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GDAPSIIGame.Entities;
using System.Threading;
using System.Runtime.CompilerServices;
using GDAPSIIGame.Map;
using GDAPSIIGame.Interface;

namespace GDAPSIIGame
{
	class Player : Entity
	{
		//Fields
		static private Player instance;
		private Weapons.Weapon weapon;
		private MouseState mouseState;
		private MouseState prevMouseState;
		private KeyboardState keyState;
		private KeyboardState prevKeyState;
		private float hurting;
        private float hurtBlink;
        private Color color;
		private float angle;
		private SpriteEffects effect;
		private float timeMult;
		private float firing;

		private float focusMultiplier;
		private float focusTimer;
		private float varianceMultiplier;
		private float varianceTimer;
		private Enemy lastHit;
		private bool updateVariance;

		//Singleton

		private Player(Weapons.Weapon weapon, int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(health, moveSpeed, texture, position, boundingBox)
		{
			this.weapon = weapon;
			focusMultiplier = 1.0f;
			focusTimer = 0f;
			varianceMultiplier = 1.0f;
			varianceTimer = 0f;
			mouseState = Mouse.GetState();
			prevMouseState = Mouse.GetState();
			keyState = Keyboard.GetState();
			prevKeyState = Keyboard.GetState();
			hurting = 0;
            hurtBlink = 0;
            color = Color.White;
			angle = 0;
			effect = new SpriteEffects();
			timeMult = 0;
			firing = 0;
		}

		static public Player Instantiate(Weapons.Weapon weapon, int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox)
		{
			if (instance == null)
			{
				instance = new Player(weapon, health, moveSpeed, texture, position, boundingBox);
			}
			return instance;
		}

		//Properties

		/// <summary>
		/// The only instance of the player class
		/// </summary>
		static public Player Instance
		{
			get { return instance; }
		}

		/// <summary>
		/// The weapon the player is holding
		/// </summary>
		public Weapons.Weapon Weapon
		{
			get { return weapon; }
			set { weapon = value; }
		}

        /// <summary>
        /// Whether the player is hurting or not
        /// </summary>
        public bool IsHurting
        {
            get { return hurting > 0; }
            set
            {
				if (value)
                {
					hurting = 1.5f;
					hurtBlink = 0f;
                }
            }
        }

		/// <summary>
		/// Whether the player is firing their weapon or not
		/// </summary>
		public bool IsFiring
		{
			get { return firing > 0; }
			set
			{
				if (value)
				{
					firing = 0.05f;
				}
			}
		}

		public float ScoreMultiplier
		{
			get { return (float) Math.Round((float) (varianceMultiplier * focusMultiplier),2); }
		}

		//Methods
		public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
			//Mouse state
			prevMouseState = mouseState;
			mouseState = Mouse.GetState();

			//Current keyboard state
			prevKeyState = keyState;
			keyState = Keyboard.GetState();

			//Update player movement
			UpdateInput(gameTime, keyState, prevKeyState);

			//Update the weapons rotation
			Vector2 camw = Camera.Instance.GetViewportPosition(Weapon.Position);
            weapon.Angle = -((float)Math.Atan2(mouseState.X - camw.X, mouseState.Y - camw.Y));
			//Update weapon position
			weapon.X = this.X + (BoundingBox.Width / 2);
			weapon.Y = this.Y + (BoundingBox.Height / 2);
			//Update weapon
			weapon.Update(gameTime);

			//Fire weapon only if previous frame didn't have left button being pressed
			if (mouseState.LeftButton == ButtonState.Pressed)
			{
				Vector2 direction = new Vector2((mouseState.X - camw.X) / 1, (mouseState.Y - camw.Y) / 1);
				direction.Normalize();
				this.Weapon.Fire(direction, mouseState, prevMouseState);
				if (weapon.Fired)
				{
					IsFiring = true;
				}
			}

			//Check if the player has fired their weapon
			if (IsFiring)
			{
				//Deincrement the timer
				firing -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
				//Shake the camera
				Camera.Instance.Shake(Position, 0.5f);
			}

			//Determine if the player hurting color should be playing
			if (IsHurting)
			{
				//Subtract from the hurting timer if the player is hurting
				hurting -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
				//Increment the blink timer
				hurtBlink += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
				
				//Shake the screen
				if (hurting > 1.3)
				{
					Camera.Instance.Shake(Position, 2);
				}

				if (hurtBlink > ((float)1/15))
				{
					//Reset blink timer
					hurtBlink -= ((float)1/15);
					//Find the correct color
					if (color == Color.Red)
					{
						color = Color.LightGray;
					}
					else color = Color.Red;
				}
			}
			//Change the color back to the default at the end
			else if (color == Color.Red || color == Color.LightGray)
			{
				color = Color.White;
			}

			//Check if the camera is shaking
			if (!IsFiring && hurting < 1.3)
			{
				//Update camera's position
				Camera.Instance.resetPosition(Position);
			}

			//update score multiplier timers
			if(focusTimer > 0)
			{
				focusTimer -= (float) gameTime.ElapsedGameTime.TotalMilliseconds/1000;
			}
			else
			{
				focusMultiplier = 1.0f;
			}

			if(varianceTimer > 0)
			{
				varianceTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds/1000;
			}else
			{
				varianceMultiplier = 1.0f;
			}

			base.Update(gameTime);
        }

		public void updateMultiplier(Enemy e)
		{
			if(lastHit == e)
			{
				focusMultiplier += 0.1f;
				focusTimer = 5f;
				updateVariance = false;
			}else
			{
				if (updateVariance)
				{
					varianceMultiplier += 0.1f;
					varianceTimer = 5f;
				}
				updateVariance = true;
				lastHit = e;
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
        {
			//Find the right sprite to draw
			//Determine player direction and get the corresponding sprite
			switch (this.Dir)
			{
				case Entity_Dir.Up:
					effect = SpriteEffects.None;
					break;
				case Entity_Dir.UpLeft:
					effect = SpriteEffects.FlipHorizontally;
					break;
				case Entity_Dir.Left:
					effect = SpriteEffects.FlipHorizontally;
					break;
				case Entity_Dir.DownLeft:
					effect = SpriteEffects.FlipHorizontally;
					break;
				case Entity_Dir.Down:
					effect = SpriteEffects.None;
					break;
				case Entity_Dir.DownRight:
					effect = SpriteEffects.None;
					break;
				case Entity_Dir.Right:
					effect = SpriteEffects.None;
					break;
				case Entity_Dir.UpRight:
					effect = SpriteEffects.None;
					break;
			}

			spriteBatch.Draw(this.Texture,
				Camera.Instance.GetViewportPosition(this),
				null,
				null,
				Vector2.Zero,
				0.0f,
				this.Scale,
				color,
				effect);

			weapon.Draw(spriteBatch);
        }

        /// <summary>
        /// Parses Input during updates
        /// </summary>
        /// <param name="keyState">KeyboardState</param>
        public void UpdateInput(GameTime gameTime, KeyboardState keyState, KeyboardState prevKeyState)
        {
            timeMult = (float)gameTime.ElapsedGameTime.TotalSeconds / ((float)1/60);

			//Basic keyboard movement
			if (keyState.IsKeyDown(Keys.W) || keyState.IsKeyDown(Keys.Up))
			{
				this.Y -= this.MoveSpeed * timeMult;
				//Camera.Instance.Y -= (int)(this.MoveSpeed*timeMult);
			}
			else if (keyState.IsKeyDown(Keys.S) || keyState.IsKeyDown(Keys.Down))
			{
				this.Y += this.MoveSpeed * timeMult;
				//Camera.Instance.Y += (int)(this.MoveSpeed * timeMult);
			}

			if (keyState.IsKeyDown(Keys.D) || keyState.IsKeyDown(Keys.Right))
			{
				this.X += this.MoveSpeed * timeMult;
				//Camera.Instance.X += (int)(this.MoveSpeed * timeMult);
			}
			else if (keyState.IsKeyDown(Keys.A) || keyState.IsKeyDown(Keys.Left))
			{
				this.X -= this.MoveSpeed * timeMult;
				//Camera.Instance.X -= (int)(this.MoveSpeed * timeMult);
			}

            

			//Player reloading
			if (keyState.IsKeyDown(Keys.R) && prevKeyState.IsKeyUp(Keys.R))
			{
				this.weapon.ReloadWeapon();
			}

			//Calculates the angle between the player and the mouse
			//See below
			//   180
			//-90   90
			//    0
			Vector2 campos = Camera.Instance.GetViewportPosition(this.X+25,this.Y);
			angle = MathHelper.ToDegrees((float)Math.Atan2(mouseState.X - campos.X, mouseState.Y - campos.Y));

			//Use angle to find player direction
			if ((angle < -157.5) || (angle > 157.5))
			{
				this.Dir = Entity_Dir.Up;
				if (angle < -157.5)
				{
					weapon.Dir = Weapons.Weapon_Dir.UpWest;
				}
				else weapon.Dir = Weapons.Weapon_Dir.UpEast;
			}
			else if ((angle < 157.5) && (angle > 112.5) && this.Dir != Entity_Dir.UpRight)
			{
				this.Dir = Entity_Dir.UpRight;
				weapon.Dir = Weapons.Weapon_Dir.UpRight;
			}
			else if ((angle < 112.5) && (angle > 67.5) && this.Dir != Entity_Dir.Right)
			{
				this.Dir = Entity_Dir.Right;
				weapon.Dir = Weapons.Weapon_Dir.Right;
			}
			else if ((angle < 67.5) && (angle > 22.5) && this.Dir != Entity_Dir.DownRight)
			{
				this.Dir = Entity_Dir.DownRight;
				weapon.Dir = Weapons.Weapon_Dir.DownRight;
			}
			else if ((angle < -22.5) && (angle > -67.5) && this.Dir != Entity_Dir.DownLeft)
			{
				this.Dir = Entity_Dir.DownLeft;
				weapon.Dir = Weapons.Weapon_Dir.DownLeft;
			}
			else if ((angle < -67.5) && (angle > -112.5) && this.Dir != Entity_Dir.Left)
			{
				this.Dir = Entity_Dir.Left;
				weapon.Dir = Weapons.Weapon_Dir.Left;
			}
			else if ((angle < -112.5) && (angle > -157.5) && this.Dir != Entity_Dir.UpLeft)
			{
				this.Dir = Entity_Dir.UpLeft;
				weapon.Dir = Weapons.Weapon_Dir.UpLeft;
			}
			else if ((angle < 22.5) && (angle > -22.5))
			{
				this.Dir = Entity_Dir.Down;
				if(angle < 0)
				{
					weapon.Dir = Weapons.Weapon_Dir.DownWest;
				}
				else weapon.Dir = Weapons.Weapon_Dir.DownEast;
			}
        }

		public override void OnCollision(ICollidable obj)
		{
			if (obj is Projectile)
			{
				if (((Projectile)obj).Owner != Owners.Player)
				{
					this.Health -= (int)((Projectile)obj).Damage;
				}
			}
            else if(obj is Enemy)
            {
                if (!IsHurting)
                {
                    Health -= 20;
					IsHurting = true;
					focusMultiplier *= 0.9f;
					if(focusMultiplier < 1)
					{
						focusMultiplier = 1.0f;
					}
					varianceMultiplier *= 0.9f;
					if(varianceMultiplier < 1)
					{
						varianceMultiplier = 1.0f;
					}
                }
            }
			else
			{
				base.OnCollision(obj);
				Camera.Instance.resetPosition(Position);
			}
		}

		public void ResetPlayer()
		{
			mouseState = Mouse.GetState();
			prevMouseState = Mouse.GetState();
			keyState = Keyboard.GetState();
			prevKeyState = Keyboard.GetState();
			Health = 100;

			hurting = 0;
			hurtBlink = 0;
			color = Color.White;
			angle = 0;
			effect = new SpriteEffects();
			timeMult = 0;
			firing = 0;
		}

	}
}
