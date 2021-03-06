﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GDAPSIIGame.Map;
using Microsoft.Xna.Framework.Input;

namespace GDAPSIIGame.Weapons
{
	class TurretGun : Weapon
	{
		//Fields
		private float fireRate;
		private float fired;
		private Vector2 origin;
		private Vector2 bulletOffset;
		private Owners owner;
		private SpriteEffects effects;

		public TurretGun(ProjectileType pT, Texture2D texture, Vector2 position, Rectangle boundingBox, float fireRate, Vector2 origin, Owners owner)
			: base(pT, texture, position, boundingBox, Range.Long)
        {
			this.fireRate = fireRate; //How fast until the weapon can fire again
			this.fired = 0; //Whether the weapon has fired
			this.origin = origin; //The origin point of the weapon (where the player holds it)
			this.bulletOffset = new Vector2(-boundingBox.Width / 2, boundingBox.Height / 4);
			this.owner = owner;
			effects = SpriteEffects.None;
		}

		/// <summary>
		/// Whether the weapon is firing or not
		/// </summary>
		public override bool Fired
		{
			get { return fired > 0; }
			set
			{
				if (value)
				{
					fired = fireRate;
				}
				else fired = 0;
			}
		}

		/// <summary>
		/// This gun does not reload do not use this property!
		/// </summary>
		public override bool Reload
		{
			get	{ return false; }
			set	{ }
		}

		/// <summary>
		/// This gun does not use an ammo system!
		/// </summary>
		public override int MaxAmmo
		{ get { return 0; } }

		/// <summary>
		/// This gun does not use an ammo system!
		/// </summary>
		public override int CurrAmmo
		{ get { return 0; } }

		public override void Update(GameTime gameTime)
		{
			switch (Dir)
			{
				case Weapons.Weapon_Dir.UpEast:
					//this.bulletOffset = new Vector2(BoundingBox.Width / 4, BoundingBox.Height / 4);
					bulletOffset.X = BoundingBox.Width / 4;
					break;
				case Weapons.Weapon_Dir.UpWest:
					//this.bulletOffset = new Vector2(BoundingBox.Width / 4, BoundingBox.Height / 4);
					bulletOffset.X = BoundingBox.Width / 4;
					break;
				case Weapons.Weapon_Dir.UpLeft:
					//this.bulletOffset = new Vector2(-BoundingBox.Width / 4, BoundingBox.Height / 4);
					bulletOffset.X = -BoundingBox.Width / 4;
					break;
				case Weapons.Weapon_Dir.Left:
					//this.bulletOffset = new Vector2(-BoundingBox.Width / 2, BoundingBox.Height / 4);
					bulletOffset.X = -BoundingBox.Width / 2;
					break;
				case Weapons.Weapon_Dir.DownLeft:
					//this.bulletOffset = new Vector2(-BoundingBox.Width, BoundingBox.Height / 4);
					bulletOffset.X = -BoundingBox.Width;
					break;
				case Weapons.Weapon_Dir.DownWest:
					//this.bulletOffset = new Vector2(-BoundingBox.Width / 2, BoundingBox.Height / 4);
					bulletOffset.X = -BoundingBox.Width / 2;
					break;
				case Weapons.Weapon_Dir.DownEast:
					//this.bulletOffset = new Vector2(-BoundingBox.Width / 2, BoundingBox.Height / 4);
					bulletOffset.X = -BoundingBox.Width / 2;
					break;
				case Weapons.Weapon_Dir.DownRight:
					//this.bulletOffset = new Vector2(-BoundingBox.Width / 4, BoundingBox.Height / 4);
					bulletOffset.X = -BoundingBox.Width / 4;
					break;
				case Weapons.Weapon_Dir.Right:
					//this.bulletOffset = new Vector2(BoundingBox.Width / 2, BoundingBox.Height / 4);
					bulletOffset.X = BoundingBox.Width / 2;
					break;
				case Weapons.Weapon_Dir.UpRight:
					//this.bulletOffset = new Vector2(BoundingBox.Width, BoundingBox.Height / 4);
					bulletOffset.X = BoundingBox.Width;
					break;
			}

			//Control when user can fire again after just firing
			if (Fired)
			{
				//Increment fireTimer
				fired -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				//Check if fireTimer meets the threshold
			}

			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			switch (Dir)
			{
				case Weapons.Weapon_Dir.UpEast:
					effects = SpriteEffects.FlipHorizontally;
					break;
				case Weapons.Weapon_Dir.UpWest:
					effects = SpriteEffects.None;
					break;
				case Weapons.Weapon_Dir.UpLeft:
					effects = SpriteEffects.None;
					break;
				case Weapons.Weapon_Dir.Left:
					effects = SpriteEffects.None;
					break;
				case Weapons.Weapon_Dir.DownLeft:
					effects = SpriteEffects.None;
					break;
				case Weapons.Weapon_Dir.DownWest:
					effects = SpriteEffects.None;
					break;
				case Weapons.Weapon_Dir.DownEast:
					effects = SpriteEffects.FlipHorizontally;
					break;
				case Weapons.Weapon_Dir.DownRight:
					effects = SpriteEffects.FlipHorizontally;
					break;
				case Weapons.Weapon_Dir.Right:
					effects = SpriteEffects.FlipHorizontally;
					break;
				case Weapons.Weapon_Dir.UpRight:
					effects = SpriteEffects.FlipHorizontally;
					break;
			}

			spriteBatch.Draw(this.Texture,
				Camera.Instance.GetViewportPosition(this),
				null,
				null,
				origin,
				Angle-((float)Math.PI/2),
				this.Scale,
				Color.White,
				effects);
		}

		/// <summary>
		/// Fire a bullet from the weapon
		/// </summary>
		/// <param name="direction">The speed that the bullet is moving</param>
		public override bool Fire(Vector2 direction)
		{
			//Check user can fire or if they need to reload
			if (!Fired)
			{
				Fired = true;
				Matrix rotationMatrix = Matrix.CreateRotationZ(Angle);
				Vector2 bulletPosition = Vector2.Transform(bulletOffset, rotationMatrix);
				ProjectileManager.Instance.Clone(ProjType, Position, direction, Angle, owner, WeapRange);
				return true;
			}
			return false;
		}

		public override void ReloadWeapon()
		{ }
	}
}
