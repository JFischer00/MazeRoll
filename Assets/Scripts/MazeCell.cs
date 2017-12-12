using UnityEngine;

public class MazeCell {
	public bool visited = false;
	public GameObject northWall, southWall, eastWall, westWall, floor, powerup;
	public CellType cellType;
}

public enum CellType
{
	Normal,
	Start,
	End,
	Powerup,
	Teleport
}