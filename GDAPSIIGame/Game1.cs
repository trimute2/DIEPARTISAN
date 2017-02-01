using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GDAPSIIGame
{
    public class Game1 : Game
    {
		GameObject player;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		Texture2D playerTexture;
		KeyboardState kbState;
		KeyboardState previousKbState;

		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
			player = new GameObject(playerTexture,new Rectangle(0,0,50,50)); 
			kbState = new KeyboardState();
			previousKbState = kbState;
			base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
			playerTexture = Content.Load<Texture2D>("player");
			player.Texture = playerTexture;
        }

        protected override void UnloadContent()
        {

        }

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			ScreenWrap(player);
			previousKbState = kbState;
			kbState = Keyboard.GetState();

			if (kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.Up))
			{
				player.Y -= 5;
			}
			else if (kbState.IsKeyDown(Keys.S) || kbState.IsKeyDown(Keys.Down))
			{
				player.Y += 5;
			}

			if (kbState.IsKeyDown(Keys.D) || kbState.IsKeyDown(Keys.Right))
			{
				player.X += 5;
			}
			else if (kbState.IsKeyDown(Keys.A) || kbState.IsKeyDown(Keys.Left))
			{
				player.X -= 5;
			}

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
			spriteBatch.Begin();
			player.Draw(spriteBatch);
			spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
