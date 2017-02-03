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
		Chunk[] chunks;
		const int chunkNum = 4;
		/// <summary>
		/// number of rows of chunks
		/// </summary>
		int numRows;
		/// <summary>
		/// number of chunks per row
		/// </summary>
		int cpr;
		List<GameObject> allObjs;

		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
			cpr = 2;
			numRows = chunkNum / cpr;
			chunks = new Chunk[chunkNum];
			int chunkWidth = GraphicsDevice.Viewport.Width / cpr;
			int chunkHieght = GraphicsDevice.Viewport.Height / numRows;
			int ID = 0;
			for(int i = 0; i < numRows; i++)
			{
				for (int j = 0; j< cpr; j++)
				{
					chunks[ID] = new Chunk(
						new Rectangle(chunkWidth * j, chunkHieght * i, chunkWidth, chunkHieght),
						cpr, ID);
					ID++;
				}
			}
			kbState = new KeyboardState();
			previousKbState = kbState;
			allObjs = new List<GameObject>();

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
		//has yet to be tested
		private void FirstChunk()
		{
			foreach(GameObject obj in allObjs)
			{
				for(int i = 0; i < chunkNum; i++)
				{
					if (chunks[i].Contains(obj.Position))
					{
						chunks[i].Add(obj);
						i = chunkNum;
					}
				}
			}
		}

		/// <summary>
		/// checks what chunks objects are in and moves them between chunks
		/// </summary>
		// has yet to be tested
		private void ChunkIt()
		{
			GameObject obj = null;
			int offset = 0;
			for (int i = 0; i < chunkNum; i++)
			{
				//cant use a foreach because sometime objects are removed, 
				for(int j = chunks[i].Objects.Count-1; j >= 0; j--)
				{
					obj = chunks[i].Objects[j];
					if (!chunks[i].Contains(obj.Position))
					{
						offset = chunks[i].CheckAdjacency(obj.Position);
						if (i != offset) // in the case that somehow the objects offset is the chunk that its in do nothing
						{
							if (offset >= chunkNum)
							{
								//incase the offset is greater than the number of chunks
								//subtract the number of chunks from the offset
								//this is essentially how the chunks handle screen wrapping
								offset -= chunkNum;
							}
							chunks[offset].Add(obj);
							chunks[i].Remove(obj);
						}
					}
				}
			}
		}


    }
}
