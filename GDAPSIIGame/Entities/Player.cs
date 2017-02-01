using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GDAPSIIGame.Entities;

namespace GDAPSIIGame
{
    class Player : Entity
    {
        private Weapon weapon;

        public Player(Weapon weapon, int health, int moveSpeed, Texture2D texture, Rectangle position) : base(health, moveSpeed, texture, position)
        {
            this.weapon = weapon;
        }


        
    }
}
