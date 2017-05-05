using GDAPSIIGame.Graph;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDAPSIIGame.Interface
{
    interface IPathFind
    {

        /// <summary>
        /// Current target this moves towards
        /// </summary>
        Vector2 CurrentTarget { get; set; }

        /// <summary>
        /// Gets the current path
        /// </summary>
        List<GraphNode> Path { get; set; }

        /// <summary>
        /// Generates the path to the target
        /// </summary>
        /// <param name="position"></param>
        /// <param name="graph"></param>
        void GeneratePath(Vector2 position, Graph.Graph graph);
    }
}
