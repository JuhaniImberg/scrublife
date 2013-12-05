using UnityEngine;
using System.Collections;

public class menu : MonoBehaviour {

	float guiWidth = 640.0;
	float guiHeight = 400.0;
	Vector3 scale;

	void OnGUI () {
		scale.x = Screen.width/guiWidth;
		scale.y = Screen.height/guiHeight;
		scale.z = 1;
	}
}
