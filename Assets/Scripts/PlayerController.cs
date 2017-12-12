using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	private const float DEFAULT_SPEED = 10.0f;

	private Rigidbody rb;
	private MazeGameManager gameManager;
	private float moveHorizontal;
	private float moveVertical;

	public float speed;
	private GameObject cam;
	public Text playerNameText;

	void Start() {
		rb = GetComponent<Rigidbody>();
		playerNameText = GetComponentInChildren<Text>();
		gameManager = GameObject.Find("Game Manager").GetComponent<MazeGameManager>();
		speed = DEFAULT_SPEED;

		transform.position = new Vector3(-1.0f, 0.0f, 0.0f);

		cam = GameObject.Find("Main Camera");
		cam.GetComponent<CameraController>().AttachToPlayer(transform);
	}
	
	void FixedUpdate() {
		if (gameManager.gameStarted) {
			moveHorizontal = Input.GetAxis("Horizontal");
			moveVertical = Input.GetAxis("Vertical");

			float y = cam.transform.rotation.eulerAngles.y;
			
			if (y >= 45.0f && y < 135.0f) {
				rb.AddForce(new Vector3(moveVertical, 0.0f, moveHorizontal) * speed);
			}
			else if (y >= 315.0f || y < 45.0f) {
				rb.AddForce(new Vector3(moveHorizontal, 0.0f, moveVertical) * speed);
			}
			else if (y >= 135.0f && y < 225.0f) {
				rb.AddForce(new Vector3(-moveHorizontal, 0.0f, -moveVertical) * speed);
			}
			else {
				rb.AddForce(new Vector3(-moveVertical, 0.0f, -moveHorizontal) * speed);
			}
		}
	}
	
	// Called when we start touching another object
	void OnCollisionEnter(Collision collision) {
		string objName = collision.gameObject.name;

		// Check if the object is a floor object
		if (objName.Contains("Floor")) {
			gameManager.FloorCollision(objName.Substring(6).Split(','), name);
		}
	}

	// Sets the player's name
	public void SetInfo(string name, Color color) {
		GetComponentInChildren<Text>().text = this.name = name;
		GetComponent<Renderer>().material.color = color;
	}
}
