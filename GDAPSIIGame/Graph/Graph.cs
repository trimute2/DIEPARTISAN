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

        public Graph(int numNodes)
        {
            nodes = new Dictionary<int, GraphNode>(numNodes);
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
        /// Finds a __GOOD__ path to the gameobject given
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        //List<GraphNode> findPath(GameObject go)
        //{
        //
        //}

        /// <summary>
        /// Finds the first node within a minimum distance of the given position
        /// </summary>
        /// <param name="minDist">Minimum distance </param>
        /// <param name="position"></param>
        /// <returns></returns>
        //GraphNode findCloseNode(int minDist, Vector2 position)
        //{
        //
        //}

        /// <summary>
        /// Finds the first node within a minimum distance of the given position
        /// </summary>
        /// <param name="minDist"></param>
        /// <returns></returns>
        //GraphNode findClosestNode(Vector2 position)
        //{
        //
        //}

    }
}
