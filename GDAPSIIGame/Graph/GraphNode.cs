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
            //Console.WriteLine(position);
        }

        public float UniqueID
        {
            get { return uniqueID; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        /// <summary>
        /// Adds a new node to this nodes list of reachable neighbors.
        /// </summary>
        /// <param name="intermediateNeighbor"></param>
        /// <param name="newNeighbor"></param>
        public void AddNeighbor(GraphNode newNeighbor, GraphNode intermediateNeighbor)
        {
            neighbors.Add(newNeighbor, intermediateNeighbor);
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
        public void Update(int maxSize)
        {
            //Iterate over all nodes this node can currently get to
            int size = NumNeighbors;
            int prevNumNeighbors = NumNeighbors;
            GraphNode neighbor = null;
            for (int i = prevIterNumNeighbors; i < size; i++)
            {
                if (NumNeighbors < maxSize)
                {
                    lock (neighbors)
                    {
                        neighbor = neighbors.Keys.ElementAt(i);
                    }
                    //Iterate over all nodes that the current neighbor connects to

                    List<GraphNode> internalNeighbors = null;
                    lock (neighbor.Neighbors)
                    {
                        internalNeighbors = neighbor.Neighbors.Keys.ToList();
                    }
                    foreach (GraphNode possibleNewNeighbor in internalNeighbors)
                    {
                        //If we can't already connect to that node, add it
                        lock (neighbors)
                        {
                            if (!neighbors.Keys.Contains(possibleNewNeighbor) && possibleNewNeighbor != this)
                            {
                                neighbors.Add(possibleNewNeighbor, neighbor);
                            }
                        }
                    }


                }
            }

            prevIterNumNeighbors = prevNumNeighbors;
        }

        public int prevIterNumNeighbors { get; set; }

    }
}
