using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDAPSIIGame.Graph
{
    class Graph
    {
        /// <summary>
        /// This should be a fibonacci heap but w/e
        /// </summary>
        Dictionary<float, GraphNode> nodes;

        public Graph(int numNodes)
        {
            nodes = new Dictionary<float, GraphNode>(numNodes);
        }

        /// <summary>
        /// Adds a node to the graph
        /// </summary>
        /// <param name="newNode">Node to add</param>
        void Add(GraphNode newNode)
        {
            nodes.Add(newNode.UniqueID, newNode);
            newNode.
        }

        /// <summary>
        /// Finds the GraphNode at the given position
        /// </summary>
        /// <param name="position">position of GraphNode to be returned</param>
        /// <returns>GraphNode at given position</returns>
        GraphNode getNode(Vector2 position)
        {
            float X = position.X;
            float Y = position.Y;
            float uniqueNum = (1 / 2) * (X + Y) * (X + Y + 1) + Y;
            return nodes[uniqueNum];
        }

    }
}
