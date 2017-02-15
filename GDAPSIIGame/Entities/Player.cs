using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GDAPSIIGame.Entities;
using System.Threading;

namespace GDAPSIIGame
{
    enum Player_Dir { Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight }

    class Player : Entity
    {
        //Fields
        static private Player instance;
        private Weapon weapon;
        private Player_Dir dir;
        private Thread inputThread;

        //Singleton

        private Player(Weapon weapon, int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(health, moveSpeed, texture, position, boundingBox)
        {
            this.weapon = weapon;
            dir = Player_Dir.Down;
            inputThread = null;
        }

        static public Player Instantiate(Weapon weapon, int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox)
        {
            if (instance == null)
            {
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
            get { return weapon; }
            set { weapon = value; }
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
            if (inputThread == null)
            {
                inputThread = new Thread(() => parseInput(gameTime));
                inputThread.Start();
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
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

        /// <summary>
        /// Parses Input during updates
        /// </summary>
        /// <param name="kbState">KeyboardState</param>
        public void parseInput(GameTime gameTime)
        {

            KeyboardState kbState;
            float currTime = 0;
            float prevTime = 0;
            float deltaTime = 0;
            while (true)
            {
                //currTime = (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
                //deltaTime = currTime - prevTime;
                kbState = Keyboard.GetState();
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

                MouseState mouseState = Mouse.GetState();

                if (kbState.IsKeyDown(Keys.Space))
                {
                    lock (ProjectileManager.Instance.Projectiles)
                    {
                        ProjectileManager.Instance.Clone(weapon.ProjType, Position, new Vector2((mouseState.X - instance.X) / 1000, (mouseState.Y - instance.Y) / 1000));
                    }
                    Console.WriteLine("SPACEEE");
                }

                //Get the mouse's state


                //Calculates the angle between the player and the mouse
                //See below
                //   180
                //-90   90
                //    0
                float angle = MathHelper.ToDegrees((float)Math.Atan2(mouseState.X - Position.X, mouseState.Y - Position.Y));

                //Use angle to find player direction
                if ((angle < -157.5) || (angle > 157.5) && dir != Player_Dir.Up)
                {
                    dir = Player_Dir.Up;
                }
                else if ((angle < 157.5) && (angle > 112.5) && dir != Player_Dir.UpRight)
                {
                    dir = Player_Dir.UpRight;
                }
                else if ((angle < 112.5) && (angle > 67.5) && dir != Player_Dir.Right)
                {
                    dir = Player_Dir.Right;
                }
                else if ((angle < 67.5) && (angle > 22.5) && dir != Player_Dir.DownRight)
                {
                    dir = Player_Dir.DownRight;
                }
                else if ((angle < -22.5) && (angle > -67.5) && dir != Player_Dir.DownLeft)
                {
                    dir = Player_Dir.DownLeft;
                }
                else if ((angle < -67.5) && (angle > -112.5) && dir != Player_Dir.Left)
                {
                    dir = Player_Dir.Left;
                }
                else if ((angle < -112.5) && (angle > -157.5) && dir != Player_Dir.UpLeft)
                {
                    dir = Player_Dir.UpLeft;
                }
                else if ((angle < 22.5) && (angle > -22.5) && dir != Player_Dir.Down)
                {
                    dir = Player_Dir.Down;
                }
                Thread.Sleep(16);
                Console.WriteLine(angle);
                Console.WriteLine(dir);
                prevTime = currTime;
            }
        }
    }
}
