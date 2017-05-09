using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GDAPSIIGame.Interface;
using GDAPSIIGame.Graph;

namespace GDAPSIIGame.Entities
{
    class MeleeEnemy : Enemy, IPathFind
    {
        /// <summary>
        /// Current target to move towards
        /// </summary>
        public Vector2 CurrentTarget { get; set; }

        public List<Vector2> RecentTargets { get; set; }

        public MeleeEnemy(Texture2D texture, Vector2 position, Rectangle boundingBox, int health = 8, int moveSpeed = 5) : base(health, moveSpeed, texture, position, boundingBox)
        {
            color = Color.DarkOrange;
            RecentTargets = new List<Vector2>();
            CurrentTarget = Vector2.Zero;
        }

        public MeleeEnemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox, int scoreValue) : base(health, moveSpeed, texture, position, boundingBox)
        {
            color = Color.DarkOrange;
            RecentTargets = new List<Vector2>();
            CurrentTarget = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
			previousPosition = Position;
            if (Awake)
            {
                if (CurrentTarget == Vector2.Zero || Vector2.Distance(CurrentTarget, Position) < 5)
                {
                    //Console.WriteLine("Close");
                    /*
                    if (Path.Count > 0)
                    {
                        CurrentTarget = Path[0].Position;
                        Path.RemoveAt(0);
                        Console.WriteLine("Next");
                    }*/
                    //else
                    //{
                    //CurrentTarget = Player.Instance.Position;
                    FindNextTarget(Player.Instance.Position, Graph.Graph.Instance);
                    //}
                }
                else
                {
                    //Console.WriteLine("New paf + {0}", Position.X);
                    //FindNextTarget(Player.Instance.Position, Graph.Graph.Instance);
                }
            }
            if (!Awake)
            {
                if (Vector2.Distance(
                    Player.Instance.BoundingBox.Center.ToVector2(),
                    this.BoundingBox.Center.ToVector2()) <= 192)
                {
                    this.Awake = true;
                }
            }
            else
            {
                Move(gameTime);
            }
            base.Update(gameTime);
        }

        public void Move(GameTime gt)
        {
            if (!(knockBackTime > 0))
            {
                //float timeMult = (float)gt.ElapsedGameTime.TotalSeconds / ((float)1 / 60);
                Vector2 diff = Position - CurrentTarget;
                //if(MoveSpeed >= diff.Length())
                //{
                //	Position = thingToMoveTo.Position;
                //}else
                //{
                //	diff.Normalize();
                //	this.Position -= diff * MoveSpeed;
                //}

                if (MoveSpeed > Math.Abs(diff.X))
                {
                    X = CurrentTarget.X;
                }
                else
                {
                    if (diff.X > 0)
                    {
                        X -= MoveSpeed;
                    }
                    else
                    {
                        X += MoveSpeed;
                    }
                }
                if (MoveSpeed > Math.Abs(diff.Y))
                {
                    Y = CurrentTarget.Y;
                }
                else
                {
                    if (diff.Y > 0)
                    {
                        Y -= MoveSpeed;
                    }
                    else
                    {
                        Y += MoveSpeed;
                    }
                }
            }
        }

        public void FindNextTarget(Vector2 position, Graph.Graph graph)
        {
            //Find closest Node to self and goal
            GraphNode closestToGoal = graph.FindClosestNode(position);
            GraphNode closestToMe = graph.FindClosestNode(this.Position);
            float currDistance = float.MaxValue;

            GraphNode currNode = closestToGoal;
            /*
            GraphNode prevNode = closestToMe;
            while (currNode != closestToMe && closestToMe.Neighbors[currNode] != currNode)
            {
                prevNode = currNode;
                currNode = closestToMe.Neighbors[currNode];
            }
            */

            //Find closest Node to player from neighbors
            foreach (GraphNode node in closestToMe.Neighbors.Values)
            {
                if (!RecentTargets.Contains(node.Position))
                {
                    float thisDistance = Vector2.Distance(node.Position, closestToGoal.Position);
                    if (thisDistance < currDistance)
                    {
                        currNode = node;
                        currDistance = thisDistance;
                    }
                }
            }

            //Keep track of recent nodes so we don't loop
            while(RecentTargets.Count > 3)
            {
                RecentTargets.RemoveAt(0);
            }
            //path.Reverse();
            //path.Insert(0, closestToMe);
            //showPath();

            //Set target and add to recenttargets so we don't move backwards
            CurrentTarget = currNode.Position;
            RecentTargets.Add(CurrentTarget);
        }
    }
}
