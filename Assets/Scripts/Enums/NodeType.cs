using UnityEngine;


public class NodeType{

	public static readonly NodeType DEFAULT = new NodeType(Color.white); // Regular node
	public static readonly NodeType UNWALKABLE = new NodeType(Color.black); // Algorithm can't traverse this
	public static readonly NodeType START = new NodeType(new Color(0.071f, 0.639f, 0, 1)); // Algorithm starts here
	public static readonly NodeType TARGET = new NodeType(new Color(0, 0.216f, 1, 1)); // Algorithm wants to get here
	public static readonly NodeType OPEN = new NodeType(Color.yellow); // Algorithm may consider this node
	public static readonly NodeType CLOSED = new NodeType(new Color (1, 0.2f, 0, 1)); // Algorithm has processed this node
	public static readonly NodeType PATH = new NodeType(new Color(0.2f, 0.8f, 0.9f, 1)); // Node is part of the final path

	public Material NodeMaterial { get; private set; }

	private NodeType(Color nodeColor) {
		Shader shade = Shader.Find ("Standard");
		NodeMaterial = new Material (shade);
		NodeMaterial.color = nodeColor;
	}
}