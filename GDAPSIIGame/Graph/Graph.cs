using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

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
            this.xDistBetweenNodes = xDistBetween;
            this.yDistBetweenNodes = yDistBetween;
            IsConnected = false;
        }

        /// <summary>
        /// Adds a node to the graph, connecting it to its existing neighbors
        /// </summary>
        /// <param name="newNode">Node to add</param>
        public void Add(GraphNode newNode)
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

            //Console.WriteLine("Node {0} tried {1} and {2} and has {3} neighbors", newNode.UniqueID, aboveID, leftID, newNode.NumNeighbors);
            //Console.WriteLine("Node created with position {0} {1}", newNode.Position.X, newNode.Position.Y);
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
            return (.5f) * (X + Y) * (X + Y + 1) + Y;
        }

        public int Size
        {
            get { return nodes.Count; }
        }

        public Dictionary<float, GraphNode> Nodes
        {
            get { return nodes; }
        }

        public void ConnectGraph()
        {
            bool done = false;
            GraphNode vertex = null;
            int attempts = 0;
            //System.Timers.Timer t = new System.Timers.Timer(1000);
            //t.Start();
            int timeElapsed = 0;
            //t.Elapsed += ((a, b) => { timeElapsed++; });
            while (!done)
            {
                attempts++;
                //Console.WriteLine("Iteration " + attempts);
                done = true;
                for (int i = 0; i < nodes.Count; i++)
                {
                    vertex = nodes.Values.ElementAt(i);
                    if (!vertex.IsComplete)
                    {
                        /*if(attempts == 10 && vertex.NumNeighbors < 10)
                        {
                            Console.WriteLine("{0} instead of {1}", vertex.NumNeighbors, nodes.Count);
                            Console.WriteLine("{0}, {1}", vertex.Position.X, vertex.Position.Y);
                            nodes.Remove(vertex.UniqueID);
                            i--;
                        }*/
                        //Thread update = new Thread(() => {
                            vertex.Update(nodes.Count - 1);
                       // });
                        //update.Start();
                        if (vertex.NumNeighbors == nodes.Count - 1)
                        {
                            vertex.IsComplete = true;
                        }
                        /*else if (vertex.NumNeighbors == 0)
                        {
                            Console.WriteLine("Removing" + vertex.UniqueID);

                            nodes.Remove(vertex.UniqueID);
                            Console.WriteLine("{0}, {1}", vertex.Position.X, vertex.Position.Y);
                            i--;
                        }*/
                        else
                        {
                            done = false;
                        }
                    }
                }
                IsConnected = true;
                //Console.WriteLine("All nodes have {0} connections on average", MeanNeighbors);
                //Console.WriteLine(timeElapsed);
            }


            //t.Stop();
        }

        public bool IsConnected { get; set; }

        public int MeanNeighbors
        {
            get
            {
                int sum = 0;
                foreach (GraphNode node in nodes.Values)
                {
                    sum += node.NumNeighbors;
                }
                return sum / nodes.Count;
            }
        }
    }
}
