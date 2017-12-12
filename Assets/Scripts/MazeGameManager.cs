using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class MazeGameManager : MonoBehaviour {
	// References to necessary gameobjects/components
	private GameObject startCanvas;
	private GameObject updateCanvas;
	private GameObject nameInput;
	private GameObject minimapCamera;
	private ColorPicker colorPicker;
	private MazeManager mazeManager;

	public GameObject playerPrefab;

	// Important game variables
	private string seed;
	public bool gameStarted;
	private float gameTime;

	void Start() {
		// Get all necessary object/component references
		GetObjects();

		// Set initial active/inactive states for objects
		Reset();
	}

	void Update() {
		if (gameStarted)
			gameTime += Time.deltaTime;
	}

	void Reset() {
		gameStarted = false;
		gameTime = 0.0f;

		startCanvas.SetActive(true);
		updateCanvas.SetActive(false);
		minimapCamera.SetActive(false);
	}

	// Called when a player hits a floor object
	public void FloorCollision(string[] coords, string playerName) {
		// Check if we hit an end cell
		if (mazeManager.GetCellType(int.Parse(coords[0]), int.Parse(coords[1])) == CellType.End) {
			StartCoroutine(EndGame(playerName));
		}
	}

	// Called when we need a speed boost
	public IEnumerator SpeedBoost(GameObject player, GameObject powerup) {
		// Check if it was a player that touched the powerup
		if (player.tag == "Player") {
			PlayerController p = player.GetComponent<PlayerController>();

			// Increase the player's speed and deactivate the powerup temporarily
			p.speed += 10f;
			powerup.SetActive(false);

			yield return new WaitForSecondsRealtime(5);

			// Revert the temporary changes
			p.speed -= 10f;
			powerup.SetActive(true);
		}
	}

	// Called when a powerup is collected
	public void PowerupCollision(GameObject player, GameObject powerup) {
		StartCoroutine(SpeedBoost(player, powerup));
	}

	// Starts game end procedures
	public IEnumerator EndGame(string winner) {
		// Stop the game
		gameStarted = false;

		// Create a win message with winner's name and time
		string msg = winner + " won the game! Their time was " + Math.Round(gameTime, 1, MidpointRounding.AwayFromZero) + " seconds.";

		// Set win text to show the win message
		updateCanvas.SetActive(true);
		updateCanvas.GetComponentInChildren<Text>().text = msg;

		yield return new WaitForSecondsRealtime(3);

		// Destroy MAZE and PLAYER
		Destroy(GameObject.Find(winner));
		mazeManager.Destroy();

		// Reset inital state
		Reset();
	}

	// Called during initialization
	private void GetObjects() {
		startCanvas = GameObject.Find("Start Canvas");
		updateCanvas = GameObject.Find("Update Canvas");
		nameInput = GameObject.FindWithTag("playerName");
		minimapCamera = GameObject.Find("Minimap Camera");
		colorPicker = GameObject.Find("Color Picker").GetComponent<ColorPicker>();
		mazeManager = GetComponent<MazeManager>();
	}

	// Called when the user presses the start button
	public void OnStartButtonPressed() {
		string playerName = nameInput.GetComponent<Text>().text;

		// Did the player enter a name?
		if (playerName != "") {
			// Disable the UI and enable the minimap camera
			startCanvas.SetActive(false);
			minimapCamera.SetActive(true);

			// Create a new MAZE and save the seed
			seed = mazeManager.Create();

			// Spawn a new PLAYER and set their name and color
			GameObject newPlayer = Instantiate(playerPrefab) as GameObject;
			newPlayer.GetComponent<PlayerController>().SetInfo(playerName, colorPicker.CurrentColor);

			// Start the game
			gameStarted = true;
		}
		else {
			// TODO: Display an error message
		}
	}
}
