using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
	public bool Walkable { get; private set; }

	public Vector2 Position { get; private set; }

	private Grid grid;
	private Renderer rend;

	private NodeType type;
	public NodeType Type {
		get{ return type; }
		set{
			type = value;
			rend.sharedMaterial = type.NodeMaterial;
		}
	}

	public void Setup (Grid grid, NodeType type, Vector2 position, float nodeScale, float outlinePercent)
	{
		this.grid = grid;
		Type = type;
		Position = position;
		Walkable = true;
		transform.localPosition = new Vector3 (position.x * nodeScale, position.y * nodeScale, 0);
		transform.localScale = Vector3.one * nodeScale * (1 - outlinePercent);
	}

	public void Awake ()
	{
		rend = GetComponent<Renderer> ();
		rend.enabled = true;
	}

	void OnMouseDown () {
		if (EventSystem.current.IsPointerOverGameObject ())
			return;
		grid.notifyNodeUpdate (this);
	}

	void OnMouseEnter() {
		if (Input.GetMouseButton (0) && !EventSystem.current.IsPointerOverGameObject ())
			grid.notifyNodeUpdate (this);
	}
}