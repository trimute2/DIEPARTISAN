using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDAPSIIGame.Map
{
    class MapManager
    {
        List<Room> rooms = new List<Room>();
        public MapManager()
        {
            Map.generateMap("testRoom1.txt", this);
        }

        public void Add(Room r)
        {
            rooms.Add(r);
        }

        public void Draw(SpriteBatch spritebatch, Texture2D texture)
        {
            foreach (Room r in rooms)
            {
                //r.Draw(spritebatch, texture);
            }
        }
    }
}
