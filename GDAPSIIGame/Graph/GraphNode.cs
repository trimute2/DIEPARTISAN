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
        private Dictionary<GraphNode, GraphNode> neighbors;

        public GraphNode(Vector2 position)
        {
            this.position = position;
            float x = position.X;
            float y = position.Y;
            this.uniqueID = (.5f) * (x + y) * (x + y + 1) + y;
            neighbors = new Dictionary<GraphNode, GraphNode>();
        }

        public float UniqueID
        {
            get { return uniqueID; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public void AddNeighbor(GraphNode intermediateNeighbor, GraphNode newNeighbor)
        {
            neighbors.Add(intermediateNeighbor, newNeighbor);
        }

        public int NumNeighbors
        {
            get { return neighbors.Count; }
        }

        public void Update()
        {
            foreach(KeyValuePair<GraphNode, GraphNode> neighbor in neighbors)
            {

            }
        }

    }
}
