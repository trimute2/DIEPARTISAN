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
    class MeleeEnemy : Enemy, IPathfind
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
            CurrentTarget = Player.Instance.Position;
        }

        public MeleeEnemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox, int scoreValue) : base(health, moveSpeed, texture, position, boundingBox)
        {
            color = Color.DarkOrange;
            Path = new List<GraphNode>();
            CurrentTarget = Player.Instance.Position;
        }

        public override void Update(GameTime gameTime)
        {
            if (Awake)
            {
                if (Vector2.Distance(CurrentTarget, Position) < 5)
                {
                    //Console.WriteLine("Close");
                    if (Path.Count > 0)
                    {
                        CurrentTarget = Path[0].Position;
                        Path.RemoveAt(0);
                        Console.WriteLine("Next");
                    }
                    else
                    {
                        CurrentTarget = Player.Instance.Position;
                        //GeneratePath(Player.Instance.Position, Graph.Graph.Instance);
                    }
                }
                else
                {
                    if (Path.Count == 0)
                    {
                        //Console.WriteLine("New paf + {0}", Position.X);
                        GeneratePath(Player.Instance.Position, Graph.Graph.Instance);
                    }
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

        public void GeneratePath(Vector2 position, Graph.Graph graph)
        {
            GraphNode closestToGoal = graph.FindClosestNode(position);
            GraphNode closestToMe = graph.FindClosestNode(this.Position);
            List<GraphNode> path = new List<GraphNode>();

            GraphNode currNode = closestToGoal;
            while (currNode != closestToMe && closestToMe.Neighbors[currNode] != currNode)
            {
                path.Add(currNode);
                currNode = closestToMe.Neighbors[currNode];
            }

            path.Reverse();
            path.Insert(0, closestToMe);
            showPath();
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
