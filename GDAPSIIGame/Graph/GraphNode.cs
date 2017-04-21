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
        private float uniqueID;
        private Vector2 position;
        private List<GraphNode> neighbors;

        public GraphNode(Vector2 position)
        {
            this.position = position;
            float x = position.X;
            float y = position.Y;
            this.uniqueID = (.5f) * (x + y) * (x + y + 1) + y;
        }

        public float UniqueID
        {
            get { return uniqueID; }
        }

        public Vector2 Position
        {
            get { return position; }
        }
    }
}
