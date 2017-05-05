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

        public List<GraphNode> Path { get; set; }

        public MeleeEnemy(Texture2D texture, Vector2 position, Rectangle boundingBox, int health = 8, int moveSpeed = 2) : base(health, moveSpeed, texture, position, boundingBox)
        {
            color = Color.DarkOrange;
            Path = new List<GraphNode>();
            CurrentTarget = Vector2.Zero;
        }

        public MeleeEnemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox, int scoreValue) : base(health, moveSpeed, texture, position, boundingBox)
        {
            color = Color.DarkOrange;
            Path = new List<GraphNode>();
            CurrentTarget = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            if (Awake)
            {
                if (CurrentTarget == Vector2.Zero || Vector2.Distance(CurrentTarget, Position) < 4)
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
            GraphNode closestToGoal = graph.FindClosestNode(position);
            GraphNode closestToMe = graph.FindClosestNode(this.Position);
            List<GraphNode> path = new List<GraphNode>();
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
            foreach(GraphNode node in closestToMe.Neighbors.Values)
            {
                float thisDistance = Vector2.Distance(node.Position, closestToGoal.Position);
                if (thisDistance < currDistance)
                {
                    currNode = node;
                    currDistance = thisDistance;
                }
            }

            //path.Reverse();
            //path.Insert(0, closestToMe);
            //showPath();
            CurrentTarget = currNode.Position;
        }

        public void showPath()
        {
            foreach (GraphNode node in Path)
            {
                Console.WriteLine(node.Position.X);
            }
        }
    }
}
