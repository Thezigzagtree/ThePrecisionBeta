using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace JSNodeMap {
	[System.Serializable]
	[ExecuteInEditMode]
	public class Agent : MonoBehaviour {
		public int agentType;

		public float movementSpeed = 1f;    // In Unity units / second

		public Node currentNode;            // Current node the agent is occupying
		public Node lastNode;				// Last node this agent successfully occupied
		public Node targetNode;             // Destination node
		public Queue<Node> nodeRoute;       // FIFO stack of nodes used for traveling
		public Node CurrentMoveTarget {
			get {
				if (nodeRoute.Any()) {
					return nodeRoute.Peek();
				}
				return null;
			}
		}
		public Path onPath;                 // Current path this agent is traveling on. Null if not traveling
		public bool isMoving;

		public bool doMove = true;
		public bool doMoveToTarget;
		public bool doRetreat;

		public int nextMarkerIndex = 0;
		public List<float> markerPosList;

		public Vector3 curWayPoint;
		public float movementFactor;
		public float distTraveled;
		public float percentTraveled;

		private WaitForEndOfFrame wait = new WaitForEndOfFrame();

		public string MoveStats {
			get {
				string stats = "Speed: " + movementSpeed + ", Factor: " + movementFactor;
				stats += ", Dist: " + distTraveled + ", Percent: " + percentTraveled;
				stats += ", DoMove: " + doMove + ", MoveToTarget: " + doMoveToTarget;
				return stats;
			}
		}

		public Map nodeMap;


		public delegate void NodeHandler(Node node);
		public delegate void NodeHandlerBool(Node node, bool target);
		public delegate void AgentHandler(Agent other);
		public delegate void MarkerHandler(int markerIndex);
		public delegate void SelfHandler();

		public event NodeHandler OnMoveStart;
		public event NodeHandler OnMoveEnd;
		public event NodeHandler OnRetreat;
		public event NodeHandler OnCannotReach;

		public event NodeHandlerBool OnNodeArrive;

		public event AgentHandler OnAgentCollide;

		public event MarkerHandler OnMarkerTick;

		public event SelfHandler OnPause;
		public event SelfHandler OnResume;

		// Built-in
		void Awake() {
			nodeRoute = new Queue<Node>();

			// Need to find a starting node
			Collider[] colliders;
			if ((colliders = Physics.OverlapSphere(transform.position, 1f)).Length > 1) {
				foreach (Collider col in colliders) {
					Node nearNode = col.gameObject.GetComponent<Node>();
					if (nearNode) {
						currentNode = nearNode;
						transform.position = currentNode.transform.position;
						return;
					}
				}
			}
		}

		void OnTriggerEnter(Collider other) {
			Agent otherAgent = other.GetComponent<Agent>();
			if (otherAgent) {
				// Raise Agent collide event
				if (OnAgentCollide != null) OnAgentCollide(otherAgent);
			}
		}

		// Private methods

		private IEnumerator MoveToTarget() {
			while (doMoveToTarget && doMove) {
				float moveDist = Time.deltaTime * movementSpeed * movementFactor;
				distTraveled += moveDist;
				percentTraveled = distTraveled / onPath.Distance;

				transform.position = Vector3.MoveTowards(transform.position, curWayPoint, moveDist);

				// Check if passed next marker
				if (nextMarkerIndex < markerPosList.Count && percentTraveled >= markerPosList[nextMarkerIndex]) {
					if (OnMarkerTick != null) OnMarkerTick(nextMarkerIndex);
					nextMarkerIndex++;
				}

				if (percentTraveled >= 0.99f) {
					// Set up for next waypoint
					nextMarkerIndex = 0;
					isMoving = true;
					distTraveled = 0;
					percentTraveled = 0;

					transform.position = curWayPoint;
					currentNode = nodeRoute.Peek();
					lastNode = currentNode;

					nodeRoute.Dequeue();

					// Is this our final destination?
					bool atTargetNode = (targetNode == currentNode);

					// Raise arrive event
					if (OnNodeArrive != null) OnNodeArrive(currentNode, atTargetNode);

					if (nodeRoute.Count == 0) {
						// Arrived at destination

						isMoving = false;
						doMoveToTarget = false;
						targetNode = null;
						distTraveled = 0;
						currentNode.agentsAtNode.Add(this);
						// In case it was going up a hill, set its rotation back to match its end point
						// May not be desirable for maps that don't use elevation changes
						// transform.localRotation = currentNode.transform.localRotation;

						// Raise target arrive event
						if (OnMoveEnd != null) OnMoveEnd(currentNode);

						yield break;
					} else {
						// Set up for next node
						SetupPathAndMarkers();
					}
				}

				yield return wait;
			}
		}

		private IEnumerator Retreat() {
			while (doRetreat && doMove) {
				float moveDist = Time.deltaTime * movementSpeed * movementFactor;
				distTraveled -= moveDist;
				percentTraveled = distTraveled / onPath.Distance;

				transform.position = Vector3.MoveTowards(transform.position, lastNode.transform.position, moveDist);

				// Now we have to go backwards through markers
				if (nextMarkerIndex >= 0 && percentTraveled <= markerPosList[nextMarkerIndex]) {
					if (OnMarkerTick != null) OnMarkerTick(nextMarkerIndex);
					nextMarkerIndex--;
				}

				if (percentTraveled <= 0.01f) {
					// Reached last waypoint. Clean up and end movement.
					nextMarkerIndex = 0;

					transform.position = lastNode.transform.position;

					nodeRoute.Clear();

					// Raise arrive event
					if (OnNodeArrive != null) OnNodeArrive(lastNode, true);

					// Finish moving
					targetNode = null;
					isMoving = false;
					doRetreat = false;
					distTraveled = 0;

					if (OnMoveEnd != null) OnMoveEnd(lastNode);

					yield break;
				}

				yield return wait;
			}
		}

		private void SetupPathAndMarkers() {
			curWayPoint = nodeRoute.Peek().transform.position;

			onPath = Map.FindValidPath(currentNode, nodeRoute.Peek());
			nextMarkerIndex = 0;
			markerPosList = new List<float>(onPath.markerPosList);

			transform.LookAt(nodeRoute.Peek().transform.position);
			movementFactor = (onPath.fromNode == currentNode) ? onPath.fromSpeed : onPath.toSpeed;
		}

		// Public Methods

		public void Initialize(Map nodeMap, Node startingNode) {
			this.nodeMap = nodeMap;
			this.currentNode = startingNode;

			Vector3 lookTarget = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
			FaceDir(lookTarget);
		}

		public void FaceDir(Vector3 dir) {
			transform.LookAt(dir);
		}

		public List<Vector3> GetRoutePointPositions(Node targetNode) {
			List<Vector3> points = new List<Vector3>();

			List<Node> path = Pathfinding.FindRoute(currentNode, targetNode, this);
			if (path.Count > 0) {
				points.Add(transform.position);
				path.RemoveAt(0);
				foreach(Node node in path) {
					points.Add(node.transform.position);
				}
			}

			return points;
		}

		public void MoveToTarget(Node targetNode) {
			// Don't do movement if we're currently moving
			if (isMoving) {
				return;
			}

			this.targetNode = targetNode;

			// Drop out if no current or target node are indicated
			if (currentNode == null || targetNode == null) {
				Debug.LogError("Missing current or goal node");
				return;
			}

			// Drop out if no path to target node can be found, but raise event, too
			List<Node> foundRoute = Pathfinding.FindRoute(currentNode, targetNode, this);
			if (foundRoute == null || foundRoute.Count == 0) {
				
				isMoving = false;
				doMoveToTarget = false;

				// Raise CannotReach event
				if (OnCannotReach != null) OnCannotReach(targetNode);

				targetNode = null;

				return;
			}

			// Raise MoveStart event
			if (OnMoveStart != null) OnMoveStart(targetNode);
			doMoveToTarget = true;

			// Start processing route
			nodeRoute = new Queue<Node>(foundRoute);

			// Dequeue first element if it's our current position
			if (nodeRoute.Peek() == currentNode) {
				nodeRoute.Dequeue();
			}

			if (nodeRoute.Count == 0) {
				// We must already be at the target
				if (OnNodeArrive != null) OnNodeArrive(targetNode, true);
				if (OnMoveEnd != null) OnMoveEnd(targetNode);
				doMoveToTarget = false;
				return;
			}

			isMoving = true;
			distTraveled = 0;
			currentNode.agentsAtNode.Remove(this);

			SetupPathAndMarkers();

			StopCoroutine(MoveToTarget());
			StartCoroutine(MoveToTarget());
		}

		public void MoveToTarget(string targetName) {
			MoveToTarget(nodeMap.GetNodeByName(targetName));
		}

		public void DoRetreat() {
			// Ignore if we're already retreating
			if (doRetreat) {
				return;
			}

			if (Pathfinding.AgentCanMoveAcross(onPath, nodeRoute.Peek(), this)) {
				// Retreat allowed
				transform.LookAt(currentNode.transform.position);
				movementFactor = (onPath.toNode == currentNode) ? onPath.toSpeed : onPath.fromSpeed;

				doMoveToTarget = false;
				doRetreat = true;

				isMoving = true;
				nextMarkerIndex -= 1;

				if (OnRetreat != null) OnRetreat(currentNode);

				StartCoroutine(Retreat());
			}
		}

		public void JumpToNode(Node targetNode) {
			currentNode = targetNode;
			transform.position = targetNode.transform.position;
		}

		public void Pause() {
			if (doMove) {
				doMove = false;
				isMoving = false;

				if (OnPause != null) OnPause();
				// Let coroutines finish their cylce
			}

		}

		public void Play() {
			if (! doMove) {
				doMove = true;

				if (OnResume != null) OnResume();

				// Resume coroutines
				if (doMoveToTarget) {
					StartCoroutine(MoveToTarget());
				} else if (doRetreat) {
					StartCoroutine(Retreat());
				}
			}
		}
	}
}

