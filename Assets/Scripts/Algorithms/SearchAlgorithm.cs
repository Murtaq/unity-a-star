using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SearchAlgorithm
{

	public bool Finished { get; set; }

	protected Grid grid;

	protected SearchAlgorithm (Grid grid)
	{
		this.grid = grid;
		this.Finished = false;
	}

	public abstract void doStep ();

}