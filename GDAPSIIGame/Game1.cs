using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
            //Initialize entity manager
            entityManager = EntityManager.Instance;

            //Initialize projectile manager
            projectileManager = ProjectileManager.Instance;

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

            allObjs = new List<GameObject>();

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
