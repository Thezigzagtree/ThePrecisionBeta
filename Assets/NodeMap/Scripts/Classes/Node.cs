using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using JSNodeMap;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JSNodeMap {
	[System.Serializable]
	[ExecuteInEditMode]
	public class Node : MonoBehaviour {
		public SpriteRenderer spriteRenderer;
		public MeshRenderer meshRenderer;
		public MeshFilter meshFilter;

		public BoxCollider col;

		public string nodeName = "Node ";
		public int nodeType;

		public Map nodeMap;
		public List<Agent> agentsAtNode;

		[SerializeField]
		public List<Path> paths;
		public bool drawNode = true;

		// Built-in

		void Awake() {
			if (paths == null) {
				paths = new List<Path>();
			}
			if (agentsAtNode == null) {
				agentsAtNode = new List<Agent>();
			}
		}
#if UNITY_EDITOR
		void OnDrawGizmos() {
			if (paths == null) return;

			Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
			if (Selection.Contains(gameObject)) {
				Gizmos.color = new Color(0.8f, 0.8f, 0f, 0.5f);
			}

			Gizmos.DrawSphere(transform.position, 1f);
			for (int i = 0; i < paths.Count; i++) {
				Path currentPath = paths[i] as Path;
				if (currentPath == null) {
					paths.RemoveAt(i);
					continue;
				}
				if (!currentPath.IsValid()) {
					continue;
				}

				Gizmos.color = new Color(0f, 0f, 0.8f, 0.5f);
				if (Selection.Contains(paths[i].toNode.gameObject) && Selection.Contains(paths[i].fromNode.gameObject)) {
					Gizmos.color = Color.blue;
				}

				Gizmos.DrawLine(currentPath.fromNode.transform.position, currentPath.toNode.transform.position);
			}
		}
#endif
		// Private Methods

		// Public Methods

		public bool pathEndPoint ()
		{
			bool someYes = false;
			bool someNo = false;
			for (int i = 0; i < this.GetAllPaths().Length; i++)
			{
				if (this.GetAllPaths()[i].GetMovementType() == MovementType.TwoWay)
				{
					someYes = true;
				}
				else
					someNo = true;
			}

			
			if (someYes && someNo)
				return true;
				
			else
				return false;

				
		}

		public void Initialize(Map nodeMap) {
			this.nodeMap = nodeMap;
			this.nodeName += nodeMap.mapNodes.Count;

			drawNode = nodeMap.drawNodes;

			SetDrawMode(nodeMap.drawMode, nodeMap.nodeData);
		}

		//Creates a path from this node to other
		public Path CreatePath(Node toNode, bool drawPath) {
			GameObject newPathObj = new GameObject("Path");
			newPathObj.transform.SetParent(transform);
			newPathObj.transform.position = transform.position;
			// The line renderer always wants to point towards the X axis in local view, so rotate it to make it
			// face the same direction as our map's "up"
			Quaternion pathRot = (nodeMap.drawMode == DrawMode.Object) ? Quaternion.Euler(90f, 0f, 0f) : Quaternion.Euler(0f, 0f, 0f);
			newPathObj.transform.localRotation = pathRot;
			newPathObj.isStatic = true;

			Path newPath = newPathObj.AddComponent<Path>();
			newPath.Initialize(nodeMap, this, toNode);
			newPath.drawMarkers = drawPath;

			paths.Add(newPath);

			return newPath;
		}

		public Path[] GetPaths() {
			if (paths == null || paths.Count == 0) {
				paths = GetComponentsInChildren<Path>().ToList();
			}
			return paths.ToArray();
		}

		public Path[] GetAllPaths() {
			return nodeMap.GetPathsToNode(this).Concat(GetPaths()).ToArray();
		}

		public List<Agent> GetAgentsAtNode() {
			return agentsAtNode;
		}

		public Node GetClosestNode(Agent agent = null, bool disallowOccupied = false, string type = null) {
			Node closestNode = null;
			float closestDist = Mathf.Infinity;

			List<Node> neighborNodes = Pathfinding.GetNeighbors(this, agent);
			foreach(Node node in neighborNodes) {
				// If this node has agents and we're not allowing that, skip to the next one
				if (node.agentsAtNode.Count > 0 && disallowOccupied) {
					continue;
				}

				// If a type is defined, the node must be of that type
				if (! System.String.IsNullOrEmpty(type) && ! nodeMap.IsNodeType(this, type)) {
					continue;
				}

				float dist = Vector3.Distance(node.transform.position, this.transform.position);
				if (dist < closestDist) {
					closestDist = dist;
					closestNode = node;
				}
			}
			return closestNode;
		}

		public void Redraw() {
			if (nodeMap.drawMode == DrawMode.Sprite) {
				spriteRenderer.sprite = nodeMap.nodeData.nodeTypes[nodeType].sprite;
			} else {
				meshFilter.mesh = nodeMap.nodeData.nodeTypes[nodeType].mesh;
			}
		}

		public void SetDrawMode(DrawMode drawMode, NodeTypeData data) {
			if (drawMode == DrawMode.Sprite) {
				// Clear out 3D renderers
				DestroyImmediate(meshFilter);
				DestroyImmediate(meshRenderer);
				DestroyImmediate(col);
				meshFilter = null;
				meshRenderer = null;
				col = null;

				// Set 2D renderer
				if (spriteRenderer == null) {
					spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
				}
				spriteRenderer.sprite = data.nodeTypes[nodeType].sprite;

				// Because we want the sprites laying down on their side (relative to the map), we'll flip the Y value
				spriteRenderer.flipY = true;
				transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
				foreach (Path path in paths) {
					path.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
				}

				spriteRenderer.sortingLayerName = "NodeMap";
				spriteRenderer.sortingOrder = 2;

				spriteRenderer.enabled = drawNode;

			} else {
				// Clear out 2D renderers
				DestroyImmediate(spriteRenderer);
				DestroyImmediate(col);
				spriteRenderer = null;
				col = null;

				// Set 3D renderer
				if (meshFilter == null) {
					meshFilter = gameObject.AddComponent<MeshFilter>();
				}
				meshFilter.mesh = data.nodeTypes[nodeType].mesh;

				if (meshRenderer == null) {
					meshRenderer = gameObject.AddComponent<MeshRenderer>();
				}
				meshRenderer.sharedMaterial = data.meshMaterial;

				transform.localRotation = Quaternion.identity;
				foreach(Path path in paths) {
					path.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
				}

				meshRenderer.enabled = drawNode;
			}

			// Set collider
			if (col == null) {
				col = gameObject.AddComponent<BoxCollider>();
			}
		}

		public void SetVisible(bool isVisible) {
			if (nodeMap.drawMode == DrawMode.Sprite) {
				spriteRenderer.enabled = isVisible;
			} else {
				meshRenderer.enabled = isVisible;
			}
			drawNode = isVisible;
		}
	}
}