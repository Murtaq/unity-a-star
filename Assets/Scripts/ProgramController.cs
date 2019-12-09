using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgramController : MonoBehaviour
{
	private const float SPEED_SLOW = 0.75f;
	private const float SPEED_MEDIUM = 0.1f;
	private const float SPEED_FAST = 0.005f;
	private const float NODE_SCALE = 0.5f;
	private const float NODE_OUTLINE = 0.1f;
	private const int GRID_SIZE_X = 20;
	private const int GRID_SIZE_Y = 20;

	[SerializeField]
	private GameObject playButton;
	[SerializeField]
	private GameObject stopButton;
	[SerializeField]
	private GameObject resetButton;
	[SerializeField]
	private GameObject plusButton;
	[SerializeField]
	private GameObject minusButton;

	private SearchAlgorithm algorithm;
	private GameSpeed speed;
	private Grid grid;
	private float timeSinceLastUpdate;

	void Start ()
	{
		initCamera ();
		grid = new Grid (GRID_SIZE_X, GRID_SIZE_Y, NODE_SCALE, NODE_OUTLINE);
		algorithm = new AStar (grid);
		speed = GameSpeed.PAUSED;
		timeSinceLastUpdate = 0.0f;
	}

	private void initCamera ()
	{
		Vector3 translation = new Vector3 ((GRID_SIZE_X - 1) * NODE_SCALE / 2, (GRID_SIZE_Y - 1) * NODE_SCALE / 2);
		Vector2 botLeft = new Vector2 (-GRID_SIZE_X / 4 * NODE_SCALE, -GRID_SIZE_Y / 4 * NODE_SCALE);
		Vector2 topRight = new Vector2 (
			                   GRID_SIZE_X * NODE_SCALE + GRID_SIZE_X / 4 * NODE_SCALE,
			                   GRID_SIZE_Y * NODE_SCALE + GRID_SIZE_Y / 4 * NODE_SCALE);
		Camera.main.GetComponent<Transform> ().Translate (translation);
		Camera.main.GetComponent<CameraMovement> ().setMovementLimits (botLeft, topRight);
	}

	void Update ()
	{
		if (updateAllowed ()) {
			algorithm.doStep ();
		}
	}

	private bool updateAllowed ()
	{
		timeSinceLastUpdate = timeSinceLastUpdate + Time.deltaTime;
		if (speed == GameSpeed.PAUSED) {
			return false;
		}
		if ((speed == GameSpeed.FAST && timeSinceLastUpdate > SPEED_FAST) ||
		    (speed == GameSpeed.MEDIUM && timeSinceLastUpdate > SPEED_MEDIUM) ||
		    (speed == GameSpeed.SLOW && timeSinceLastUpdate > SPEED_SLOW)) {
			timeSinceLastUpdate = 0.0f;
			return true;
		}
		return false;
	}

	public void increaseGameSpeed ()
	{
		if (speed == GameSpeed.SLOW) {
			speed = GameSpeed.MEDIUM;
			minusButton.GetComponent<Button> ().interactable = true;
		} else if (speed == GameSpeed.MEDIUM) {
			speed = GameSpeed.FAST;
			plusButton.GetComponent<Button> ().interactable = false;
		}
	}

	public void decreaseGameSpeed ()
	{
		if (speed == GameSpeed.FAST) {
			speed = GameSpeed.MEDIUM;
			plusButton.GetComponent<Button> ().interactable = true;
		} else if (speed == GameSpeed.MEDIUM) {
			speed = GameSpeed.SLOW;
			minusButton.GetComponent<Button> ().interactable = false;
		} 
	}

	public void startAlgorithm ()
	{
		speed = GameSpeed.MEDIUM;
	}

	public void pauseAlgorithm ()
	{
		speed = GameSpeed.PAUSED;
	}

	public void doAlgorithmStepWithPause ()
	{
		speed = GameSpeed.PAUSED;
		algorithm.doStep ();
	}

	public void finalizeAlgorithm ()
	{
		while (!algorithm.Finished) {
			algorithm.doStep ();
		}
	}

	public void resetAlgorithm ()
	{
		speed = GameSpeed.PAUSED;
		grid.reset ();
		algorithm = new AStar (grid);
	}

	public void setNodeTypeDefault ()
	{
		grid.ActiveNodeType = NodeType.DEFAULT;
	}

	public void setNodeTypeWall ()
	{
		grid.ActiveNodeType = NodeType.UNWALKABLE;
	}

	public void setNodeTypeStart ()
	{
		grid.ActiveNodeType = NodeType.START;
	}

	public void setNodeTypeTarget ()
	{
		grid.ActiveNodeType = NodeType.TARGET;
	}

}
