using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : SearchAlgorithm
{

	private List<Node> openSet;
	private HashSet<Node> closedSet;
	private Dictionary<Node, AStarData> nodeData;

	public AStar (Grid grid) : base (grid)
	{
		nodeData = new Dictionary<Node, AStarData> ();
		openSet = new List<Node> ();
		closedSet = new HashSet<Node> ();
	}

	private void initAlgorithm() {
		grid.initNeighbours ();
		grid.IsEditable = false;
		Node startNode = grid.StartNode;
		openSet.Add (startNode);
		nodeData.Add (startNode, new AStarData ());
	}

	override public void doStep ()
	{
		if (openSet.Count == 0 && closedSet.Count == 0) {
			initAlgorithm ();
		} else if (Finished || openSet.Count == 0) {
			Finished = true;
			return;
		}
		Node activeNode = openSet [0];
		foreach(Node node in openSet) {
			if (nodeData [node].FCost <= nodeData [activeNode].FCost) {
				activeNode = node;
			}
		}

		openSet.Remove (activeNode);
		closedSet.Add (activeNode);

		activeNode.Type = NodeType.CLOSED;
		if (activeNode == grid.TargetNode) {
			Finished = true;
			paintPath ();
			return;
		}

		foreach (Node neighbour in grid.getNeighbours(activeNode)) {
			if (closedSet.Contains (neighbour))
				continue;

			AStarData neighbourData;
			if (!nodeData.TryGetValue (neighbour, out neighbourData)) {
				neighbourData = new AStarData ();
				nodeData.Add (neighbour, neighbourData);
			}
			int newCostToNeighbour = nodeData [activeNode].GCost + getNodeDistance (activeNode, neighbour);
			if (newCostToNeighbour < neighbourData.GCost || !openSet.Contains (neighbour)) {
				neighbourData.GCost = newCostToNeighbour;
				neighbourData.HCost = getNodeDistance (neighbour, grid.TargetNode);
				neighbourData.Parent = activeNode;

				if (!openSet.Contains (neighbour)) {
					openSet.Add (neighbour);
					neighbour.Type = NodeType.OPEN;
				}
			}
		} 	
	}

	public void paintPath ()
	{
		List<Node> path = new List<Node> ();
		Node currentNode = grid.TargetNode;

		while (nodeData [currentNode].Parent != null) {
			path.Add (currentNode);
			currentNode = nodeData [currentNode].Parent;
		}
		path.Add (currentNode);
		path.ForEach (node => node.Type = NodeType.PATH);
	}

	public int getNodeDistance (Node nodeFrom, Node nodeTo)
	{
		int xDist = (int) Mathf.Abs (nodeFrom.Position.x - nodeTo.Position.x);
		int yDist = (int) Mathf.Abs (nodeFrom.Position.y - nodeTo.Position.y);

		if (xDist > yDist) {
			return 14 * yDist + 10 * (xDist - yDist);
		}
		return 14 * xDist + 10 * (yDist - xDist);
	}
}