using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDAPSIIGame.Entities
{
    class Entity : GameObject
    {
        private int health;
        private int moveSpeed;
        public Entity(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(texture, position, boundingBox)
        {
            this.health = health;
            this.moveSpeed = moveSpeed;
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public int MoveSpeed
        {
            get { return moveSpeed; }
        }


    }
}
