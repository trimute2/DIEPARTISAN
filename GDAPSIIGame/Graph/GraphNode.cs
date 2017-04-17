using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDAPSIIGame.Graph
{
    class GraphNode
    {
        private int uniqueID;
        private Vector2 position;

        public int UniqueID
        {
            get { return uniqueID; }
        }

        public Vector2 Position
        {
            get { return position; }
        }
    }
}
