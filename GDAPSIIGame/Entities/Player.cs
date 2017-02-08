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
        static private Player instance;
        private Weapon weapon;
        private Player(Weapon weapon, int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(health, moveSpeed, texture, position, boundingBox)
        {
            this.weapon = weapon;
        }

        static public Player Instantiate(Weapon weapon, int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox)
        {
            if (instance == null) {
                instance = new Player(weapon, health, moveSpeed, texture, position, boundingBox);
            }
            return instance;
        }

        static public Player Instance
        {
            get { return instance; }
        }

        public void Update(KeyboardState previousKbState, KeyboardState kbState)
        {
            if (kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.Up))
            {
                this.Y -= 5;
            }
            else if (kbState.IsKeyDown(Keys.S) || kbState.IsKeyDown(Keys.Down))
            {
                this.Y += 5;
            }

            if (kbState.IsKeyDown(Keys.D) || kbState.IsKeyDown(Keys.Right))
            {
                this.X += 5;
            }
            else if (kbState.IsKeyDown(Keys.A) || kbState.IsKeyDown(Keys.Left))
            {
                this.X -= 5;
            }
        }
    }
}
