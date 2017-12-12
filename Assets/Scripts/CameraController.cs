using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	Transform player;

	public void AttachToPlayer(Transform p) {
		player = p;
	}

	void LateUpdate() {
		if (player)
			transform.position = player.position + new Vector3(0.0f, 10.0f, -6.0f);
	}
}
