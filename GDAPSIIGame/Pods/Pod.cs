using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GDAPSIIGame.Entities;

namespace GDAPSIIGame.Pods
{
	class Pod
	{

		private List<Enemy> Enemies;
		private bool awake;
		private float timeActive;

		public Pod()
		{
			Enemies = new List<Enemy>();
			awake = false;
			timeActive = 0f;
		}

		public void Add(Enemy en)
		{
			Enemies.Add(en);
		}

		public void Update(GameTime gameTime)
		{
			if (!awake) {
				for (int i = 0; i < Enemies.Count - 1; i++)
				{
					if (Enemies[i].Awake)
					{
						this.awake = true;
						WakeAll();
						i = Enemies.Count;
					}
				}
			}else
			{
				timeActive += (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
		}

		private void WakeAll()
		{
			foreach (Enemy en in Enemies)
			{
				en.Awake = true;
			}
		}

	}
}
