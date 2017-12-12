using UnityEngine;
using System.Collections;

public class ProceduralNumberGenerator {
	private static int currentPosition = 0;
	private static string key = "123424123342421432233144441212334432121223344";

	public static int GetNextNumber() {
		string currentNum = key.Substring(currentPosition++ % key.Length, 1);
		return int.Parse (currentNum);
	}

	public static void SetKey(string seed) {
		key = seed;
	}
}
