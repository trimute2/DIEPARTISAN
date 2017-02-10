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
    enum Player_Dir { Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight}

    class Player : Entity
    {
        //Fields
        static private Player instance;
        private Weapon weapon;
        private Player_Dir dir;


        //Singleton

        private Player(Weapon weapon, int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(health, moveSpeed, texture, position, boundingBox)
        {
            this.weapon = weapon;
            dir = Player_Dir.Down;
        }

        static public Player Instantiate(Weapon weapon, int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox)
        {
            if (instance == null) {
                instance = new Player(weapon, health, moveSpeed, texture, position, boundingBox);
            }
            return instance;
        }


        //Properties

        static public Player Instance
        {
            get { return instance; }
        }

        public Weapon Weapon
        {
            get { return Weapon; }
            set { Weapon = value; }
        }

        public Player_Dir Dir
        {
            get { return dir; }
            private set { dir = value; }
        }


        //Methods
        public void Update(GameTime gameTime, KeyboardState previousKbState, KeyboardState kbState)
        {
            base.Update(gameTime);

            //Basic keyboard movement
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

            //Get the mouse's state
            MouseState mouseState = Mouse.GetState();

            //Calculates the angle between the player and the mouse
            //See below
            //   180
            //-90   90
            //    0
            float angle = MathHelper.ToDegrees((float)Math.Atan2(mouseState.X - Position.X, mouseState.Y - Position.Y));

            //Use angle to find player direction
            if ((angle < -157.5) || (angle > 157.5))
            {
                dir = Player_Dir.Up;
            }
            else if ((angle < 157.5) && (angle > 112.5))
            {
                dir = Player_Dir.UpRight;
            }
            else if ((angle < 112.5) && (angle > 67.5))
            {
                dir = Player_Dir.Right;
            }
            else if ((angle < 67.5) && (angle > 22.5))
            {
                dir = Player_Dir.DownRight;
            }
            else if ((angle < -22.5) && (angle > -67.5))
            {
                dir = Player_Dir.DownLeft;
            }
            else if ((angle < -67.5) && (angle > -112.5))
            {
                dir = Player_Dir.Left;
            }
            else if ((angle < -112.5) && (angle > -157.5))
            {
                dir = Player_Dir.UpLeft;
            }
            else if ((angle < 22.5) || (angle > -22.5))
            {
                dir = Player_Dir.Down;
            }
            Console.WriteLine(dir);            
        }

        /// <summary>
        /// Fires the player's weapon
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public void Fire(Vector2 position, Vector2 direction)
        {
            weapon.Fire(position, direction);
        }
    }
}
