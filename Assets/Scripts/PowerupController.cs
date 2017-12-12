using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour {
	private Vector3 rotation;
	private MazeGameManager gameManager;

	void Start() {
		gameManager = GameObject.Find("Game Manager").GetComponent<MazeGameManager>();

		rotation = new Vector3(Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f), Random.Range(-90.0f, 90.0f));
	}

	void Update() {
		transform.Rotate(rotation * Time.deltaTime);
	}

	void OnTriggerEnter(Collider collider) {
		gameManager.PowerupCollision(collider.gameObject, gameObject);
	}
}
