using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GDAPSIIGame
{
    class EntityManager
    {
        //Fields-----------------
        List<Entities.Entity> enemies;

        //Properties-------------

        /// <summary>
        /// The list of all enemies
        /// </summary>
        public List<Entities.Entity> Enemies { get { return enemies; } set { enemies = value; } }

        /// <summary>
        /// The player object
        /// </summary>
        public Player Player { get; set; }


        //Methods----------------

        public EntityManager()
        {
            enemies = new List<Entities.Entity>();
        }

        /// <summary>
        /// Load in sprites
        /// </summary>
        internal void LoadContent(ContentManager Content)
        {
            Texture2D playerTexture = Content.Load<Texture2D>("player");
            Player = new Player(null, 100, 1, playerTexture, new Vector2(playerTexture.Width, playerTexture.Height), new Rectangle(playerTexture.Width, playerTexture.Height, 50, 50));
        }

        /// <summary>
        /// Update entities
        /// </summary>
        internal void Update(GameTime gameTime, KeyboardState previousKbState, KeyboardState kbState)
        {
            Player.Update(previousKbState, kbState);
        }

        /// <summary>
        /// Draw entities
        /// </summary>
        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Player.Draw(spriteBatch);
        }

    }
}
