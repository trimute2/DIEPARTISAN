using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDAPSIIGame.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GDAPSIIGame.Graph;

namespace GDAPSIIGame.Entities
{
	class DashEnemy : Enemy
	{

		private float dashTime;
		private bool dashing;
		private bool bump;
		private float bumpTime;
		private float dashSpeed;

		public Vector2 CurrentTarget { get; set; }

		public List<Vector2> RecentTargets { get; set; }

		public float DashTime
		{
			get { return dashTime; }
		}

		public bool Dashing
		{
			get { return dashing; }
		}

		public DashEnemy(Texture2D texture, Vector2 position, Rectangle boundingBox, int health = 3, int moveSpeed = 4) : base(health, moveSpeed, texture, position, boundingBox)
		{
			dashTime = 1.5f;
			bumpTime = 0.01f;
			dashing = false;
			dashSpeed = 8f;
			color = Color.Red;
			RecentTargets = new List<Vector2>();
			CurrentTarget = Vector2.Zero;
			bump = false;
		}

		public DashEnemy(int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox, int scoreValue) : base(health, moveSpeed, texture, position, boundingBox)
		{
			dashTime = 1.5f;
			bumpTime = 0.01f;
			dashing = false;
			dashSpeed = 4f;
			color = Color.Red;
			RecentTargets = new List<Vector2>();
			CurrentTarget = Vector2.Zero;
			bump = false;
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
				if (bump)
				{
					if(bumpTime > 0)
					{
						bumpTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
					}else
					{
						bump = false;
					}
				}else if (!(knockBackTime > 0))
				{
					dashTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				}
				if(dashTime <= 0)
				{
					if (dashing)
					{
						dashTime = 3.0f;
						dashing = false;
					}else
					{
						dashTime = 0.25f;
						dashing = true;
					}
				}
				Move(Player.Instance);
			}
			base.Update(gameTime);
		}

		public void Move(GameObject thingToMoveTo)
		{
			if (!(knockBackTime > 0))
			{
				Vector2 diff = Position - thingToMoveTo.Position;
				/*float speed = MoveSpeed;
				if(dashing && !bump)
				{
					speed *= dashSpeed;
				}
				if (speed >= diff.Length())
				{
					Position = thingToMoveTo.Position;
				}
				else
				{
					diff.Normalize();
					this.Position -= diff * speed;
				}*/
				if (dashing && !bump)
				{
					if (MoveSpeed * dashSpeed > Math.Abs(diff.X))
					{
						X = thingToMoveTo.Position.X;
					}
					else
					{
						if (diff.X > 0)
						{
							X -= MoveSpeed * dashSpeed;
						}
						else
						{
							X += MoveSpeed * dashSpeed;
						}
					}
					if (MoveSpeed * dashSpeed > Math.Abs(diff.Y))
					{
						Y = thingToMoveTo.Position.Y;
					}
					else
					{
						if (diff.Y > 0)
						{
							Y -= MoveSpeed * dashSpeed;
						}
						else
						{
							Y += MoveSpeed * dashSpeed;
						}
					}
				}
				else
				{
					if (MoveSpeed > Math.Abs(diff.X))
					{
						X = thingToMoveTo.X;
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
						Y = thingToMoveTo.Y;
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
		}

        public override void OnCollision(ICollidable obj)
        {
            if(obj is Enemy)
			{
				bump = true;
				bumpTime = 0.01f;
			}
			base.OnCollision(obj);
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
			while (RecentTargets.Count > 3)
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
