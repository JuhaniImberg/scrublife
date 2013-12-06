using UnityEngine;
using System.Collections;

public class cubepool : MonoBehaviour {
	
	static int size = 1024;
	static int where = 0;
	static int atonce = 32;
	static GameObject[] objs = new GameObject[size*2];
	public static GameObject block;

	public static void Add (GameObject obj) {
		obj.SetActive(false);
		objs[where++] = obj;
		Debug.Log ("add");
	}

	public static void AddInitial() {
		for(int i = 0; i < size; i++) {
			Add(Instantiate(block) as GameObject);
		}
		Debug.Log ("added initial");
	}

	public static void AddChunk () {
		for(int i = 0; i < atonce; i++) {
			Add(Instantiate(block) as GameObject);
		}
		Debug.Log ("added chunk");
	}

	public static GameObject Get () {
		if(where <= 0) {
			return null;
		}
		GameObject asd = objs[where--];
		asd.SetActive(true);
		Debug.Log ("get");
		return asd;
	}
}
