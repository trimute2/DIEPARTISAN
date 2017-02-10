using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
namespace GDAPSIIGame
{
    enum GameState { Main_Menu, Gameplay, Pause_Menu}
    
    public class Game1 : Game
    {
        //Fields
        EntityManager entityManager;
        ProjectileManager projectileManager;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState kbState;
		KeyboardState previousKbState;
		ChunkManager chunkaroo;
		List<GameObject> allObjs;

		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Initialize entity manager
            entityManager = EntityManager.Instance;

			allObjs = new List<GameObject>();

			

            //Initialize projectile manager
            projectileManager = ProjectileManager.Instance;

            //Initialize keyboards
            kbState = new KeyboardState();
			previousKbState = kbState;
            
			base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Load entities
            entityManager.LoadContent(Content);
            //Load projectiles
            projectileManager.LoadContent(Content);

			foreach (GameObject en in entityManager.Enemies)
			{
				allObjs.Add(en);
			}

			allObjs.Add(entityManager.Player);

			chunkaroo = new ChunkManager(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height / 2, allObjs);


			//Rectangle pls = new Rectangle(0, 0, 0, 0);
			//Vector2 aids = Vector2.Zero;
			//Texture2D why = Content.Load<Texture2D>("player");
			//Projectile p = new Projectile(why, Vector2.Zero, new Rectangle(0, 0, 0, 0), Vector2.Zero);
			//Console.WriteLine(why);
			//newthing = (Projectile)p.GetType().GetConstructor(new System.Type[] { why.GetType(), aids.GetType(), pls.GetType(), aids.GetType() }).Invoke(new object[] {why, new Vector2(2,2), new Rectangle(0, 0, 10, 10), new Vector2(.01f, .01f) });

			//Console.WriteLine(newthing.Direction);
		}

        protected override void UnloadContent()
        {

        }

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			//ScreenWrap(player);
			previousKbState = kbState;
			kbState = Keyboard.GetState();

            //Update entities
            entityManager.Update(gameTime, previousKbState, kbState);

            //Update projectiles
            projectileManager.Update(gameTime, previousKbState, kbState);

			chunkaroo.Upadate();
			base.Update(gameTime);
		}

		private void ScreenWrap(GameObject objToWrap)
		{
			if (objToWrap.X < 0)
			{
				objToWrap.X = GraphicsDevice.Viewport.Width - objToWrap.X;
			}
			else if (objToWrap.X > GraphicsDevice.Viewport.Width)
			{
				objToWrap.X = objToWrap.X - GraphicsDevice.Viewport.Width;
			}
			if (objToWrap.Y < 0)
			{
				objToWrap.Y = GraphicsDevice.Viewport.Height - objToWrap.Y;
			}
			else if (objToWrap.Y > GraphicsDevice.Viewport.Height)
			{
				objToWrap.Y = objToWrap.Y - GraphicsDevice.Viewport.Height;
			}
		}

		protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //newthing.Draw();
            //Begin SpriteBatch
			spriteBatch.Begin();

            //Draw entities
			entityManager.Draw(gameTime, spriteBatch);

            //Draw projectiles
            projectileManager.Draw(gameTime, spriteBatch);

            //End SpriteBatch
			spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}
