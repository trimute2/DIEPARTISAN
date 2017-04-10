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
		private int podScore;
		private int damageCaused;

		public Pod()
		{
			Enemies = new List<Enemy>();
			awake = false;
			timeActive = 0f;
		}

		public bool Awake
		{
			get { return awake; }
		}

		public bool Empty
		{
			get { return Enemies.Count == 0; }
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

		public int GetScore()
		{
			int score = 0;
			foreach(Enemy e in Enemies)
			{
				score += e.score;
			}
			score = (int) ((float) score * Player.Instance.ScoreMultiplier);
			podScore += score;
			return score;
		}

		public void RemoveInactive()
		{
			for (int i = Enemies.Count - 1; i >= 0; i--)
			{
				if (!Enemies[i].active)
				{
					Enemies.RemoveAt(i);
				}
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
