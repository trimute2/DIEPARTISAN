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
        Dictionary<int, GraphNode> nodes;
        GraphNode[,] adjacencyMatrix;

        public Graph(int numNodes)
        {
            nodes = new Dictionary<int, GraphNode>(numNodes);
            adjacencyMatrix = new GraphNode[numNodes, numNodes];
        }

        /// <summary>
        /// Adds a node to the graph
        /// </summary>
        /// <param name="newNode">Node to add</param>
        void Add(GraphNode newNode)
        {
            nodes.Add(newNode.UniqueID, newNode);
        }

        /// <summary>
        /// Should be called once all nodes are made. Fills Adj. Matrix.
        /// </summary>
        void fillAdjacencyMatrix()
        {

        }

        /// <summary>
        /// Finds a __GOOD__ path to the gameobject given
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
       // List<GraphNode> findPath(GameObject go)
       // {
       //     GraphNode closest = findClosestNode(go.Position);
       // 
       // }

        /// <summary>
        /// Finds the first node within a minimum distance of the given position
        /// </summary>
        /// <param name="minDist"></param>
        /// <returns></returns>
        GraphNode findClosestNode(Vector2 position)
        {
            int X = (int)position.X;
            int Y = (int)position.Y;
            int uniqueNum = (1 / 2) * (X + Y) * (X + Y + 1) + Y;
            return nodes[uniqueNum];
        }

    }
}
