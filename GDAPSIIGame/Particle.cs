using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDAPSIIGame
{
	class Particle : GameObject
	{
		private double particaleTime;
		private Vector2 direction;

		public Particle(Texture2D texture, Vector2 position, Rectangle boundingBox, Vector2 direction, double time) : base(texture, position, boundingBox)
		{
			this.particaleTime = time;
			this.direction = direction;
		}

		public override void Update(GameTime gameTime)
		{
			if(particaleTime <= 0)
			{
				this.active = false;
			}else
			{
				particaleTime -= gameTime.ElapsedGameTime.TotalSeconds;
			}
			this.Position += direction;
			base.Update(gameTime);
		}

	}
}
