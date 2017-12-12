using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour {
	public int mazeRows, mazeColumns, powerupChance;
	public GameObject wall, powerup;
	public float size = 2f;

	private MazeCell[,] mazeCells;

	public string Create() {
		// Create a full maze grid
		InitializeMaze();

		// Create a maze by removing walls and save the seed
		MazeAlgorithm ma = new HuntAndKillMazeAlgorithm(mazeCells);
		string seed = ma.CreateMaze();
		
		// Set certain cells to contain powerups or be the start/end cell
		SetCellTypes(false);

		return seed;
	}

	public void Load(string seed) {
		// Create a full maze grid
		InitializeMaze();

		// Create a maze by removing cells based on the seed given
		MazeAlgorithm ma = new HuntAndKillMazeAlgorithm(mazeCells);
		ma.LoadMaze(seed);

		// Set certain cells to contain powerups or be the start/end cell
		SetCellTypes(true);
	}

	void SetCellTypes(bool loading) {
		/*if (loading) {
			foreach (string item in powerupCoords) {
				int r = int.Parse(item.Split('|')[1]), c = int.Parse(item.Split('|')[2]);
				mazeCells[r, c].cellType = CellType.Powerup;
				mazeCells[r, c].powerup = Instantiate(powerup, new Vector3(r*size, -(size/2f) + 1f, c*size), Quaternion.identity) as GameObject;
				mazeCells[r, c].powerup.name = "Powerup " + r + "," + c;
			}
		}
		else {*/
			for (int r = 0; r < mazeRows; r++) {
				for (int c = 0; c < mazeColumns; c++) {
					if (r == 0 && c == 0)
						mazeCells[r, c].cellType = CellType.Start;
					else if (r == (mazeRows - 1) && c == (mazeColumns - 1))
						mazeCells[r, c].cellType = CellType.End;
					else if (UnityEngine.Random.Range(0, powerupChance) == 0) {
						mazeCells[r, c].cellType = CellType.Powerup;
						mazeCells[r, c].powerup = Instantiate(powerup, new Vector3(r*size, -(size/2f) + 1f, c*size), Quaternion.identity) as GameObject;
						mazeCells[r, c].powerup.name = "Powerup " + r + "," + c;
					}
				}
			}
		//}
	}

	public CellType GetCellType(int x, int y) {
		return mazeCells[x, y].cellType;
	}

	public void Destroy() {
		for (int r = 0; r < mazeRows; r++) {
			for (int c = 0; c < mazeColumns; c++) {
				if (mazeCells[r, c].floor)
					GameObject.Destroy(mazeCells[r, c].floor);
				if (mazeCells[r, c].northWall)
					GameObject.Destroy(mazeCells[r, c].northWall);
				if (mazeCells[r, c].southWall)
					GameObject.Destroy(mazeCells[r, c].southWall);
				if (mazeCells[r, c].eastWall)
					GameObject.Destroy(mazeCells[r, c].eastWall);
				if (mazeCells[r, c].westWall)
					GameObject.Destroy(mazeCells[r, c].westWall);
				if (mazeCells[r, c].powerup)
					GameObject.Destroy(mazeCells[r, c].powerup);

				mazeCells[r, c] = null;
			}
		}
	}

	private void InitializeMaze() {

		mazeCells = new MazeCell[mazeRows,mazeColumns];

		for (int r = 0; r < mazeRows; r++) {
			for (int c = 0; c < mazeColumns; c++) {
				mazeCells[r, c] = new MazeCell ();

				mazeCells[r, c].floor = Instantiate (wall, new Vector3(r*size, -(size/2f), c*size), Quaternion.identity) as GameObject;
				mazeCells[r, c].floor.name = "Floor " + r + "," + c;
				mazeCells[r, c].floor.transform.Rotate(Vector3.right, 90f);

				if (c == 0) {
					mazeCells[r,c].westWall = Instantiate(wall, new Vector3(r*size, 0, (c*size) - (size/2f)), Quaternion.identity) as GameObject;
					mazeCells[r, c].westWall.name = "West Wall " + r + "," + c;
				}

				mazeCells[r, c].eastWall = Instantiate(wall, new Vector3(r*size, 0, (c*size) + (size/2f)), Quaternion.identity) as GameObject;
				mazeCells[r, c].eastWall.name = "East Wall " + r + "," + c;

				if (r == 0) {
					mazeCells[r, c].northWall = Instantiate(wall, new Vector3((r*size) - (size/2f), 0, c*size), Quaternion.identity) as GameObject;
					mazeCells[r, c].northWall.name = "North Wall " + r + "," + c;
					mazeCells[r, c].northWall.transform.Rotate(Vector3.up * 90f);
				}

				mazeCells[r,c].southWall = Instantiate (wall, new Vector3((r*size) + (size/2f), 0, c*size), Quaternion.identity) as GameObject;
				mazeCells[r, c].southWall.name = "South Wall " + r + "," + c;
				mazeCells[r, c].southWall.transform.Rotate(Vector3.up * 90f);

				mazeCells[r, c].cellType = CellType.Normal;
			}
		}
	}
}
