using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDAPSIIGame.Entities
{
    class Enemy : Entity
    {
        public Enemy(int health, int moveSpeed, Texture2D texture, Rectangle position) : base(health, moveSpeed, texture, position)
        {
        }

        public void Move(GameObject thingToMoveTo)
        {
            
        } 
    }
}
