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
        private float xDistBetweenNodes;
        private float yDistBetweenNodes;

        public Graph(int numNodes, float xDistBetween, float yDistBetween)
        {
            nodes = new Dictionary<float, GraphNode>(numNodes);
        }

        /// <summary>
        /// Adds a node to the graph, connecting it to its existing neighbors
        /// </summary>
        /// <param name="newNode">Node to add</param>
        void Add(GraphNode newNode)
        {
            nodes.Add(newNode.UniqueID, newNode);

            //ID of neighbor above this node
            float aboveID = getUniqueID(newNode.Position.X, newNode.Position.Y - yDistBetweenNodes);

            //ID of neighbor to the left of this node
            float leftID = getUniqueID(newNode.Position.X - xDistBetweenNodes, newNode.Position.Y);

            //If the above node exists, add nodes to each other's neighbors lists
            if (nodes.ContainsKey(aboveID))
            {
                newNode.AddNeighbor(nodes[aboveID], nodes[aboveID]);
                nodes[aboveID].AddNeighbor(newNode, newNode);
            }

            //If the left node exists, add nodes to each other's neighbors lists
            if (nodes.ContainsKey(leftID))
            {
                newNode.AddNeighbor(nodes[leftID], nodes[leftID]);
                nodes[leftID].AddNeighbor(newNode, newNode);
            }
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

        float getUniqueID(float X, float Y)
        {
            return (.5f) * (X + Y) * (X + Y + 1) - Y;
        }

    }
}
