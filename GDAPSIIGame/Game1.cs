using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
		Rectangle upLeft;
		Rectangle upRight;
		Rectangle lowLeft;
		Rectangle lowRight;
		List<GameObject> allObjs;
		List<List<GameObject>> chunks;

		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
			float halfWidth = GraphicsDevice.Viewport.Width / 2;
			float halfHieght = GraphicsDevice.Viewport.Height / 2;
			upLeft = new Rectangle(0, 0, (int)halfWidth, (int)halfHieght);
			upRight = new Rectangle((int)halfWidth, 0, (int)halfWidth, (int)halfHieght);
			lowLeft = new Rectangle(0, (int)halfHieght, (int)halfWidth, (int)halfHieght);
			//lowRight = new Rectangle((int)halfWidth, (int)halfHieght, (int)halfWidth, (int)halfHieght);
			kbState = new KeyboardState();
			previousKbState = kbState;
			allObjs = new List<GameObject>();
			chunks = new List<List<GameObject>>();
			for (int i = 0; i < 4; i++)
			{
				chunks.Add(new List<GameObject>());
			}

			base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
			playerTexture = Content.Load<Texture2D>("player");
			//player.Texture = playerTexture;
            player = new GameObject(playerTexture, new Vector2(playerTexture.Width, playerTexture.Height), new Rectangle(playerTexture.Width, playerTexture.Height, 50, 50));
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

		/// <summary>
		/// adds game objects to chunks when the game is first initialized
		/// </summary>
		//could be handled better will change latter
		private void FirstChunk()
		{
			
			foreach (GameObject obj in allObjs)
			{
				if (upLeft.Contains(obj.Position))
				{
					chunks[0].Add(obj);
				}else if (upRight.Contains(obj.Position))
				{
					chunks[1].Add(obj);
				}else if (lowLeft.Contains(obj.Position))
				{
					chunks[2].Add(obj);
				}else
				{
					chunks[3].Add(obj);
				}
			}
		}

		private void ChunkIt()
		{
			foreach(List<GameObject> objList in chunks)
			{
				
			}
		}
    }
}
