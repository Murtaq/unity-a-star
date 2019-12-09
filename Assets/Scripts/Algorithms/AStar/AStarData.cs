using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarData
{
	private int fCost, gCost, hCost;

	public int FCost { get { return fCost; } }

	public int GCost {
		get { return gCost; }
		set {
			gCost = value;
			fCost = gCost + hCost;
		}
	}

	public int HCost {
		get { return hCost; }
		set {
			hCost = value; 
			fCost = gCost + hCost;
		}
	}

	public Node Parent { get; set; }

	public AStarData ()
	{
		this.fCost = 0;
		this.gCost = 0;
		this.hCost = 0;
	}
}
