using UnityEngine;
using System.Collections;

public class gui : MonoBehaviour {

	
	float guiWidth = 1280.0f;
	float guiHeight = 720.0f;
	Vector3 scale;

	public life lif;

	int height = 8;
	int width = 8;
	int size = 4;

	float zoom = 1f;

	bool showgui = true;

	// Use this for initialization
	void Start () {
		StartLife();
	}

	void StartLife() {
		this.lif.Initialize(this.width, this.height, (float)(this.size)/4);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("Mouse ScrollWheel") > 0) {
			if(this.zoom > 0.2f)
				this.zoom -= 0.1f;
		}
		if(Input.GetAxis("Mouse ScrollWheel") < 0) {
			if(this.zoom < 2.8f)
				this.zoom += 0.1f;
		}
		if(Input.GetKeyDown(KeyCode.Space)) {
			StartLife();
		}
		if(Input.GetKeyDown(KeyCode.S)) {
			this.showgui = !this.showgui;
		}

		this.camera.fieldOfView = 60f*this.zoom;
	}

	void OnGUI () {
		if(!this.showgui)
			return;

		scale.x = Screen.width/guiWidth;
		scale.y = Screen.height/guiHeight;
		scale.z = 1;
		Matrix4x4 oldmatrix = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
		
		GUILayout.BeginArea(new Rect(0, 0, 280, 720), GUI.skin.GetStyle("Box"));

		GUILayout.Label("Area "+this.width+"x"+this.height);
		this.height = (int)GUILayout.HorizontalSlider((float)this.height, 4, 32);
		this.width = this.height;

		GUILayout.Label("Scale "+((float)(this.size*4))/16);
		this.size = (int)GUILayout.HorizontalSlider((float)this.size, 1f, 8f);

		if(GUILayout.Button("Restart (Space)")) {
			StartLife();
		}

		GUILayout.Label("Zoom");
		this.zoom = GUILayout.HorizontalSlider(this.zoom, 3f, 0.1f);

		GUILayout.Label("Speed: ~"+(int)(1/(this.lif.period)+0.5)+" ticks a second");
		this.lif.period = GUILayout.HorizontalSlider(this.lif.period, 1f, 0.1f);

		GUILayout.Label("Trail: "+cubescript.lifeticks+" ticks long");
		cubescript.lifeticks = (int)GUILayout.HorizontalSlider((float)cubescript.lifeticks, 1f, 50f);

		if(GUILayout.Button("Hide GUI (S)")) {
			this.showgui = !this.showgui;
		}
		
		GUILayout.EndArea();
		
		GUI.matrix = oldmatrix;
	}
}
