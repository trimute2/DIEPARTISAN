using GDAPSIIGame.Graph;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDAPSIIGame.Entities
{
    interface IPathfind
    {
        /// <summary>
        /// Current target to move to
        /// </summary>
        Vector2 CurrentTarget { get; set; }

        /// <summary>
        /// Path currently being moved on. Null if nothing.
        /// </summary>
        List<GraphNode> Path { get; set; }

        /// <summary>
        /// Generates a path
        /// </summary>
        /// <param name="position"></param>
        /// <param name="graph"></param>
        void GeneratePath(Vector2 position, Graph.Graph graph);

    }
}
