using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDAPSIIGame.Entities
{
    abstract class Enemy : Entity
    {
        //Fields
        private bool awake;
        private bool hit;
        private int scoreValue;

        /// <summary>
        /// If the enemy is activated by the player
        /// </summary>
        public bool Awake
        {
            get { return awake; }
            set { awake = value; }
        }

        public int Score
        {
            get
            {
                if (hit)
                {
                    hit = false;
                    return scoreValue;
                }
                return 0;
            }
        }

		internal bool Hit
		{
			get { return hit; }
			set { hit = value; }
		}

        public Enemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(health, moveSpeed, texture, position, boundingBox)
        {
            awake = false;
            scoreValue = 5;
        }
		public Enemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox, int scoreValue) : base(health, moveSpeed, texture, position, boundingBox)
		{
			awake = false;
			this.scoreValue = scoreValue;
		}
	}
}
