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
        List<Vector2> RecentTargets { get; set; }

        /// <summary>
        /// Generates the path to the target
        /// </summary>
        /// <param name="position"></param>
        /// <param name="graph"></param>
        void FindNextTarget(Vector2 position, Graph.Graph graph);
    }
}
