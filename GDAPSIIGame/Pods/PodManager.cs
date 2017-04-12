using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GDAPSIIGame.Pods
{
    class PodManager
    {

		static private PodManager instance;
        private List<Pod> pods;
		private int globalScore;

		private PodManager()
		{
			pods = new List<Pod>();
			globalScore = 0;
		}

		public int GlobalScore
		{
			get { return globalScore; }
		}

		static public PodManager Instance
		{
			get
			{
				if(instance == null)
				{
					instance = new PodManager();
				}
				return instance;
			}
		}

		public int Count
		{
			get
			{
				return pods.Count;
			}
		}

		public void Add(Pod p)
        {
            pods.Add(p);
        }

		public void Update(GameTime gameTime)
		{
			if (pods.Count > 0)
			{
				for (int i = pods.Count - 1; i >= 0; i--)
				{
					pods[i].Update(gameTime);
					globalScore += pods[i].GetScore();
					pods[i].RemoveInactive();
					if (pods[i].Empty)
					{
						pods.RemoveAt(i);
					}
				}
			}
		}
	}
}
