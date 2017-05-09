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
using GDAPSIIGame.Weapons;

namespace GDAPSIIGame
{
	enum PlayerState { In, GamePlay, Out }

	class Player : Entity
	{
		//Fields
		static private Player instance;
		private Weapon[] weapons;
		private Weapon currWeapon;
		private int weaponId;
		private MouseState mouseState;
		private MouseState prevMouseState;
		private KeyboardState keyState;
		private KeyboardState prevKeyState;
		private GamePadState gpState;
		private GamePadState prevGpState;
		private float hurting;
		private float hurtBlink;
		private float angle;
		private SpriteEffects effect;
		private float timeMult;
		private float firing;
		private ControlManager controlManager;

		private float focusMultiplier;
		private float focusTimer;
		private float varianceMultiplier;
		private float varianceTimer;
		private Enemy lastHit;
		private bool updateVariance;

		//Singleton

		private Player(Weapon weapon, Weapon weapon2, Weapon weapon3, int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(health, moveSpeed, texture, position, boundingBox)
		{
			weapons = new Weapon[] { weapon, weapon2, weapon3 };
			currWeapon = weapons[0];
			weaponId = 0;
			focusMultiplier = 1.0f;
			focusTimer = 0f;
			varianceMultiplier = 1.0f;
			varianceTimer = 0f;
			mouseState = Mouse.GetState();
			prevMouseState = Mouse.GetState();
			keyState = Keyboard.GetState();
			prevKeyState = Keyboard.GetState();
			gpState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
			prevGpState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
			hurting = 0;
			hurtBlink = 0;
			color = Color.White;
			angle = 0;
			effect = new SpriteEffects();
			timeMult = 0;
			firing = 0;
			controlManager = ControlManager.Instance;
		}

		static public Player Instantiate(Weapon weapon, Weapon weapon2, Weapon weapon3, int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox)
		{
			if (instance == null)
			{
				instance = new Player(weapon, weapon2, weapon3, health, moveSpeed, texture, position, boundingBox);
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
		public Weapon CurrWeapon
		{
			get { return currWeapon; }
			set { currWeapon = value; }
		}

		/// <summary>
		/// The first weapon the player knows
		/// </summary>
		public Weapon Weapon
		{
			get { return weapons[0]; }
			set { weapons[0] = value; }
		}

		/// <summary>
		/// The second weapon the player knows
		/// </summary>
		public Weapon Weapon2
		{
			get { return weapons[1]; }
			set { weapons[1] = value; }
		}

		/// <summary>
		/// The array of weapons the player knows
		/// </summary>
		public Weapon[] KnownWeapons
		{
			get { return weapons; }
			set { weapons = value; }
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
					firing = 0.2f;
				}
			}
		}

		public float ScoreMultiplier
		{
			get { return (float)Math.Round((float)(varianceMultiplier * focusMultiplier), 2); }
		}

		//Methods
		public override void Update(GameTime gameTime)
		{
			//Get the newest ControlManager
			controlManager = ControlManager.Instance;

			previousPosition = Position;
			//base.Update(gameTime);
			//Mouse state
			prevMouseState = mouseState;
			mouseState = Mouse.GetState();

			//Current keyboard state
			prevKeyState = keyState;
			keyState = Keyboard.GetState();

			prevGpState = gpState;
			gpState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
			Vector2 camw = Camera.Instance.GetViewportPosition(CurrWeapon.Position);

			//Update player movement
			UpdateInput(gameTime, camw);

			UpdateWeapon(gameTime, camw);

			//Fire weapon only if previous frame didn't have left button being pressed
			if (controlManager.ControlPressed(Control_Types.Fire))
			{
				Vector2 direction;
				if (controlManager.Mode == Control_Mode.GamePad)
				{
					direction = gpState.ThumbSticks.Right;
					direction.Y *= -1;
					direction.Normalize();
					if (gpState.ThumbSticks.Right != Vector2.Zero && this.CurrWeapon.Fire(direction))
					{
						IsFiring = true;
					}
				}
				else
				{
					direction = new Vector2((mouseState.X - camw.X) / 1, (mouseState.Y - camw.Y) / 1);
					direction.Normalize();
					if (this.CurrWeapon.Fire(direction))
					{
						IsFiring = true;
					}
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

			if (focusTimer > 0)
			{
				focusTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
			}
			else
			{
				focusMultiplier = 1.0f;
			}

			if (varianceTimer > 0)
			{
				varianceTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
				updateVariance = false;
			}
			else
			{
				varianceMultiplier = 1.0f;
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

				if (hurtBlink > ((float)1 / 15))
				{
					//Reset blink timer
					hurtBlink -= ((float)1 / 15);
					//Find the correct color
					if (color == Color.Red)
					{
						color = Color.LightGray;
					}
					else color = Color.Red;
				}
			}else if (ScoreMultiplier >= 3 && Health < 100)
			{
				Health += 1;
				color = Color.LightGreen;
			}
			//Change the color back to the default at the end
			else if (color == Color.Red || color == Color.LightGray || color == Color.LightGreen)
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


			base.Update(gameTime);
		}

		public void updateMultiplier(Enemy e)
		{
			if (lastHit == e)
			{
				focusMultiplier += 0.05f;
				focusTimer = 3f;
				updateVariance = false;
			}
			else
			{
				if (updateVariance)
				{
					varianceMultiplier += 0.1f;
					varianceTimer = 3.5f;
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

			currWeapon.Draw(spriteBatch);
		}

		/// <summary>
		/// Parses Input during updates
		/// </summary>
		public void UpdateInput(GameTime gameTime, Vector2 camw)
		{
			timeMult = (float)gameTime.ElapsedGameTime.TotalSeconds / ((float)1 / 60);

			//Basic keyboard movement
			if (controlManager.ControlPressed(Control_Types.Forward))
			{
				if (controlManager.Mode == Control_Mode.GamePad)
				{
					this.Y -= (gpState.ThumbSticks.Left.Y * timeMult) * MoveSpeed;
				}
				else this.Y -= this.MoveSpeed * timeMult;
			}
			else if (controlManager.ControlPressed(Control_Types.Backward))
			{
				if (controlManager.Mode == Control_Mode.GamePad)
				{
					this.Y -= (gpState.ThumbSticks.Left.Y * timeMult) * MoveSpeed;
				}
				else this.Y += this.MoveSpeed * timeMult;
			}

			if (controlManager.ControlPressed(Control_Types.Right))
			{
				if (controlManager.Mode == Control_Mode.GamePad)
				{
					this.X += (gpState.ThumbSticks.Left.X * timeMult) * MoveSpeed;
				}
				else this.X += this.MoveSpeed * timeMult;
			}
			else if (controlManager.ControlPressed(Control_Types.Left))
			{
				if (controlManager.Mode == Control_Mode.GamePad)
				{
					this.X += (gpState.ThumbSticks.Left.X * timeMult) * MoveSpeed;
				}
				else this.X -= this.MoveSpeed * timeMult;
			}

			//Player reloading
			if (controlManager.ControlPressed(Control_Types.Reload))
			{
				this.currWeapon.ReloadWeapon();
			}

			//Calculates the angle between the player and the mouse
			//See below
			//   180
			//-90   90
			//    0
			if (controlManager.Mode == Control_Mode.KBM)
			{
				Vector2 campos = Camera.Instance.GetViewportPosition(this.X + 25, this.Y);
				angle = MathHelper.ToDegrees((float)Math.Atan2(mouseState.X - campos.X, mouseState.Y - campos.Y));
			}
			else
			{
				if (gpState.ThumbSticks.Right != Vector2.Zero)
				{
					angle = MathHelper.ToDegrees((float)Math.Atan2(gpState.ThumbSticks.Right.X, gpState.ThumbSticks.Right.Y * -1));
				}
			}

			if ((controlManager.Mode == Control_Mode.GamePad && gpState.ThumbSticks.Right != Vector2.Zero) || controlManager.Mode == Control_Mode.KBM)
			{
				//Use angle to find player direction
				if ((angle < -157.5) || (angle > 157.5))
				{
					this.Dir = Entity_Dir.Up;
					if (angle < -157.5)
					{
						currWeapon.Dir = Weapon_Dir.UpWest;
					}
					else currWeapon.Dir = Weapon_Dir.UpEast;
				}
				else if ((angle < 157.5) && (angle > 112.5) && this.Dir != Entity_Dir.UpRight)
				{
					this.Dir = Entity_Dir.UpRight;
					currWeapon.Dir = Weapon_Dir.UpRight;
				}
				else if ((angle < 112.5) && (angle > 67.5) && this.Dir != Entity_Dir.Right)
				{
					this.Dir = Entity_Dir.Right;
					currWeapon.Dir = Weapon_Dir.Right;
				}
				else if ((angle < 67.5) && (angle > 22.5) && this.Dir != Entity_Dir.DownRight)
				{
					this.Dir = Entity_Dir.DownRight;
					currWeapon.Dir = Weapon_Dir.DownRight;
				}
				else if ((angle < -22.5) && (angle > -67.5) && this.Dir != Entity_Dir.DownLeft)
				{
					this.Dir = Entity_Dir.DownLeft;
					currWeapon.Dir = Weapon_Dir.DownLeft;
				}
				else if ((angle < -67.5) && (angle > -112.5) && this.Dir != Entity_Dir.Left)
				{
					this.Dir = Entity_Dir.Left;
					currWeapon.Dir = Weapon_Dir.Left;
				}
				else if ((angle < -112.5) && (angle > -157.5) && this.Dir != Entity_Dir.UpLeft)
				{
					this.Dir = Entity_Dir.UpLeft;
					currWeapon.Dir = Weapon_Dir.UpLeft;
				}
				else if ((angle < 22.5) && (angle > -22.5))
				{
					this.Dir = Entity_Dir.Down;
					if (angle < 0)
					{
						currWeapon.Dir = Weapon_Dir.DownWest;
					}
					else currWeapon.Dir = Weapon_Dir.DownEast;
				}
			}

			//Player switching weapons
			if (controlManager.ControlPressedControlPrevReleased(Control_Types.NextWeapon))
			{
				InteruptReload();
				Weapon_Dir oldDir = currWeapon.Dir;
				if (weaponId == weapons.Length - 1)
				{
					weaponId = 0;
				}
				else
				{
					weaponId++;
				}
				currWeapon = weapons[weaponId];
				//if (currWeapon == weapons[0]) { this.currWeapon = weapons[1]; 
				//else if (currWeapon == weapons[1]) { this.currWeapon = weapons[0]; }
				currWeapon.Dir = oldDir;
				UpdateWeapon(gameTime, camw);
			}
			else if (controlManager.ControlPressedControlPrevReleased(Control_Types.PrevWeapon))
			{
				InteruptReload();
				Weapon_Dir oldDir = currWeapon.Dir;
				if (weaponId == 0)
				{
					weaponId = weapons.Length - 1;
				}
				else
				{
					weaponId--;
				}
				//if (currWeapon == weapons[1]) { this.currWeapon = weapons[0]; }
				//else if (currWeapon == weapons[0]) { this.currWeapon = weapons[1]; }
				currWeapon = weapons[weaponId];
				currWeapon.Dir = oldDir;
				UpdateWeapon(gameTime, camw);
			}


		}

		public override void OnCollision(ICollidable obj)
		{
			if (obj is Projectile)
			{
				if (((Projectile)obj).Owner != Owners.Player)
				{
					if (IsHurting == false)
					{
						this.Health -= (int)((Projectile)obj).Damage;
						Console.WriteLine(Health);
						IsHurting = true;
						knockBack = (obj as Projectile).Direction * 216;
						knockBackTime = 0.2f;
					}
				}
			}
			else if (obj is Enemy)
			{
				if ((obj is MeleeEnemy) | (obj is DashEnemy && (obj as DashEnemy).Dashing))
				{
					if (!IsHurting)
					{
						Health -= 10;
						IsHurting = true;
						focusMultiplier *= 0.9f;
						if (focusMultiplier < 1)
						{
							focusMultiplier = 1.0f;
						}
						varianceMultiplier *= 0.9f;
						if (varianceMultiplier < 1)
						{
							varianceMultiplier = 1.0f;
						}
					}
				}
			}
			else
			{
				base.OnCollision(obj);
				Camera.Instance.resetPosition(Position);
			}
		}

		/// <summary>
		/// Completely resets the player for new games
		/// </summary>
		public void ResetPlayer()
		{
			mouseState = Mouse.GetState();
			prevMouseState = Mouse.GetState();
			keyState = Keyboard.GetState();
			prevKeyState = Keyboard.GetState();
			Health = 100;

			this.active = true;
			hurting = 0;
			hurtBlink = 0;
			color = Color.White;
			angle = 0;
			effect = new SpriteEffects();
			timeMult = 0;
			firing = 0;
			varianceMultiplier = 1;
			varianceTimer = 0;
			focusMultiplier = 1;
			focusTimer = 0;

			foreach (Weapon w in weapons)
			{
				w.ResetWeapon();
			}
		}

		/// <summary>
		/// Partially resets the player for switching levels
		/// </summary>
		public void ResetPlayerNewMap()
		{
			mouseState = Mouse.GetState();
			prevMouseState = Mouse.GetState();
			keyState = Keyboard.GetState();
			prevKeyState = Keyboard.GetState();

			this.active = true;
			hurting = 0;
			hurtBlink = 0;
			color = Color.White;
			angle = 0;
			effect = new SpriteEffects();
			timeMult = 0;
			firing = 0;
			varianceMultiplier = 1;
			varianceTimer = 0;
			focusMultiplier = 1;
			focusTimer = 0;

			foreach (Weapon w in weapons)
			{
				w.ResetWeapon();
			}
		}

		//Interupts the current weapon's reload
		private void InteruptReload()
		{
			currWeapon.Reload = false;
		}

		private void UpdateWeapon(GameTime gameTime, Vector2 camw)
		{
			//Update the weapons rotation
			if (controlManager.Mode == Control_Mode.GamePad)
			{
				if (gpState.ThumbSticks.Right != Vector2.Zero)
				{ currWeapon.Angle = -((float)Math.Atan2(gpState.ThumbSticks.Right.X, gpState.ThumbSticks.Right.Y * -1)); }
			}
			else
			{
				currWeapon.Angle = -((float)Math.Atan2(mouseState.X - camw.X, mouseState.Y - camw.Y));
			}
			//Update weapon position
			currWeapon.X = this.X;
			currWeapon.Y = this.Y + 35;
			//Update weapon
			currWeapon.Update(gameTime);
		}
	}
}
