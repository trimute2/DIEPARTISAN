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
        public Enemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(health, moveSpeed, texture, position, boundingBox)
        {
        }

        public void Move(GameObject thingToMoveTo)
        {
            Vector2 diff = Position - thingToMoveTo.Position;
            if (diff.X > 0)
            {
                X--;
            }
            else
            {
                X++;
            }

            if (diff.Y > 0)
            {
                Y--;
            }
            else
            {
                Y++;
            }
        } 
    }
}
