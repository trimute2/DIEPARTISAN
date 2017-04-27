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
            IsComplete = false;
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

        public Dictionary<GraphNode, GraphNode> Neighbors
        {
            get { return neighbors; }
        }

        public bool IsComplete
        {
            get; set; 
        }

        /// <summary>
        /// Expands a given node's neighbor list by one expansion 
        /// </summary>
        public void Update()
        {
            //Iterate over all nodes this node can currently get to
            int size = NumNeighbors;
            GraphNode neighbor = null;
            for(int i = 0; i < size; i++)
            {
                neighbor = neighbors.Keys.ElementAt(i);
                //Iterate over all nodes that the current neighbor connects to
                foreach (GraphNode possibleNewNeighbor in neighbor.Neighbors.Keys)
                {
                    //If we can't already connect to that node, add it
                    if (!neighbors.Keys.Contains(possibleNewNeighbor))
                    {
                        neighbors.Add(possibleNewNeighbor, neighbor);
                    }
                }
            }
        }

    }
}
