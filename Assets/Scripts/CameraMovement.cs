using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

	[SerializeField]
	private float cameraSpeed = 0;

	private float mouseWheelPosition = 0;
	private Vector2 limitBotLeft, limitTopRight;
	private Vector3 initialPosition;

	void Awake ()
	{
		initialPosition = GetComponent<Transform> ().position;
	}

	void Update ()
	{
		moveCamera ();
	}


	private void moveCamera ()
	{
		if (Input.GetKey (KeyCode.W)) {
			GetComponent<Transform> ().Translate (Vector3.up * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.A)) {
			GetComponent<Transform> ().Translate (Vector3.left * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.S)) {
			GetComponent<Transform> ().Translate (Vector3.down * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.D)) {
			GetComponent<Transform> ().Translate (Vector3.right * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.Space)) {
			transform.position = initialPosition;
		}
		if (Input.mouseScrollDelta.y > mouseWheelPosition) {
			GetComponent<Camera> ().orthographicSize = GetComponent<Camera> ().orthographicSize * 0.8f;
		} else if (Input.mouseScrollDelta.y < mouseWheelPosition) {
			GetComponent<Camera> ().orthographicSize = GetComponent<Camera> ().orthographicSize * 1.2f;
		}

		transform.position = new Vector3 (
			Mathf.Clamp (transform.position.x, this.limitBotLeft.x, limitTopRight.x), 
			Mathf.Clamp (transform.position.y, this.limitBotLeft.y, limitTopRight.y),
			Mathf.Clamp (transform.position.z, -10, -10)
		);
	}

	public void setMovementLimits (Vector2 limitBotLeft, Vector2 limitTopRight)
	{
		this.limitBotLeft = limitBotLeft;
		this.limitTopRight = limitTopRight;
	}
}
