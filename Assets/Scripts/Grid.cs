using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{

	public NodeType ActiveNodeType { get; set; }

	private Node startNode, targetNode;

	public Node StartNode {
		get { return startNode; }
		set { 
			startNode.Type = NodeType.DEFAULT;
			startNode = value;
			startNode.Type = NodeType.START;
		}
	}

	public Node TargetNode {
		get { return targetNode; }
		set { 
			targetNode.Type = NodeType.DEFAULT;
			targetNode = value;
			targetNode.Type = NodeType.TARGET;
		}
	}

	public bool IsEditable { get; set; }

	private int gridSizeX, gridSizeY;
	private Node[,] nodeGrid;
	private Dictionary<Node, List<Node>> neighbours;

	public Grid (int gridSizeX, int gridSizeY, float nodeScale, float nodeOutline)
	{
		this.gridSizeX = gridSizeX;
		this.gridSizeY = gridSizeY;
		ActiveNodeType = NodeType.DEFAULT;
		IsEditable = true;
		initGrid (nodeScale, nodeOutline);
	}


	private void initGrid (float nodeScale, float nodeOutline)
	{
		nodeGrid = new Node[gridSizeX, gridSizeY];
		for (int x = 0; x < gridSizeX; x++) {
			for (int y = 0; y < gridSizeY; y++) {
				GameObject nodeObject = MonoBehaviour.Instantiate (Resources.Load ("Prefabs/Node")) as GameObject;
				Node nextNode = nodeObject.GetComponent<Node> ();
				nextNode.Setup (this, NodeType.DEFAULT, new Vector2 (x, y), nodeScale, nodeOutline);
				nodeGrid [x, y] = nextNode;
			}
		}
		startNode = nodeGrid [0, 0];
		startNode.Type = NodeType.START;
		targetNode = nodeGrid [gridSizeX - 1, gridSizeY - 1];
		targetNode.Type = NodeType.TARGET;
	}


	public void initNeighbours ()
	{
		neighbours = new Dictionary<Node, List<Node>> ();
		for (int x = 0; x < gridSizeX; x++) {
			for (int y = 0; y < gridSizeY; y++) {
				List<Node> neighbourList = new List<Node> ();
				if (isValidNeighbour (x - 1, y))
					neighbourList.Add (nodeGrid [x - 1, y]);
				if (isValidNeighbour (x - 1, y - 1))
					neighbourList.Add (nodeGrid [x - 1, y - 1]);
				if (isValidNeighbour (x - 1, y + 1))
					neighbourList.Add (nodeGrid [x - 1, y + 1]);
				if (isValidNeighbour (x + 1, y))
					neighbourList.Add (nodeGrid [x + 1, y]);
				if (isValidNeighbour (x + 1, y - 1))
					neighbourList.Add (nodeGrid [x + 1, y - 1]);
				if (isValidNeighbour (x + 1, y + 1))
					neighbourList.Add (nodeGrid [x + 1, y + 1]);
				if (isValidNeighbour (x, y - 1))
					neighbourList.Add (nodeGrid [x, y - 1]);
				if (isValidNeighbour (x, y + 1))
					neighbourList.Add (nodeGrid [x, y + 1]);
				neighbours.Add (nodeGrid [x, y], neighbourList);
			}
		}
	}

	private bool isValidNeighbour (int x, int y)
	{
		return x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY && nodeGrid [x, y].Type != NodeType.UNWALKABLE;
	}

	public void reset ()
	{
		IsEditable = true;
		ActiveNodeType = NodeType.DEFAULT;
		for (int x = 0; x < gridSizeX; x++) {
			for (int y = 0; y < gridSizeY; y++) {
				nodeGrid [x, y].Type = NodeType.DEFAULT;
			}
		}
		StartNode = nodeGrid [0, 0];
		TargetNode = nodeGrid [gridSizeX - 1, gridSizeY - 1];
	}

	public void notifyNodeUpdate (Node clickedNode)
	{
		if (!IsEditable)
			return;
		if (ActiveNodeType == NodeType.START && clickedNode != TargetNode) {
			StartNode = clickedNode;
		} else if (ActiveNodeType == NodeType.TARGET && clickedNode != StartNode) {
			TargetNode = clickedNode;
		} else if (clickedNode.Type != NodeType.START && clickedNode.Type != NodeType.TARGET) {
			clickedNode.Type = ActiveNodeType;
		}
	}

	public List<Node> getNeighbours (Node node)
	{
		return neighbours [node];
	}

}
