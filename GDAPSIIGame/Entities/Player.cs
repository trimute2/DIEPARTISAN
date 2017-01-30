using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GDAPSIIGame
{
    class Player
    {
        private string name;
        private int x, y;
        private float health;
        private float speed;
        private Weapon weapon;
        private Texture2D texture;

        public Player(string name, int x, int y, float health, float speed, Weapon weapon, Texture2D texture)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.health = health;
            this.speed = speed;
            this.weapon = weapon;
            this.texture = texture;
        }
    }
}
