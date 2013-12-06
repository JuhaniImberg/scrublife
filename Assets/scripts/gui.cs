using UnityEngine;
using System.Collections;

public class gui : MonoBehaviour {

	public life lif;

	int height = 16;
	int width = 16;
	int size = 2;

	float zoom = 1f;

	bool showgui = true;

	float fps;

	public GUISkin custom;

	// Use this for initialization
	void Start () {
		this.StartLife();
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

		this.fps = (1.0f/Time.deltaTime);
	}

	void OnGUI () {
		if(!this.showgui)
			return;
		GUI.skin = custom;

		GUILayout.BeginArea(new Rect(5, 5, 280, 458), GUI.skin.GetStyle("Box"));

		/* GENERATION */

		GUILayout.Label("Generation", GUI.skin.customStyles[0]);

		GUILayout.Label("Area "+this.width+"x"+this.height);
		this.height = (int)GUILayout.HorizontalSlider((float)this.height, 4, 32);
		this.width = this.height;

		GUILayout.Label("Scale "+((float)(this.size*4))/16);
		this.size = (int)GUILayout.HorizontalSlider((float)this.size, 1f, 8f);

		this.lif.shouldstrict = !GUILayout.Toggle(!this.lif.shouldstrict, "Wrapping");

		if(GUILayout.Button("Restart (Space)")) {
			StartLife();
		}

		/* ANIMATION */
		GUILayout.Space(10);
		GUILayout.Label("Animation", GUI.skin.customStyles[0]);
		
		GUILayout.Label("Speed: ~"+(int)(1/(this.lif.period)+0.5)+" ticks a second");
		this.lif.period = GUILayout.HorizontalSlider(this.lif.period, 1f, 0.1f);
		
		GUILayout.Label("Trail: "+cubescript.lifeticks+" ticks long");
		cubescript.lifeticks = (int)GUILayout.HorizontalSlider((float)cubescript.lifeticks, 1f, 50f);

		/* CAMERA */
		GUILayout.Space(10);
		GUILayout.Label("Camera", GUI.skin.customStyles[0]);

		GUILayout.Label("Zoom");
		this.zoom = GUILayout.HorizontalSlider(this.zoom, 3f, 0.1f);

		GUILayout.Label("Rotation speed");
		this.lif.rotationspeed = GUILayout.HorizontalSlider(this.lif.rotationspeed, -16f, 16f);

		GUILayout.Label("Color rotation speed");
		this.lif.colortime = GUILayout.HorizontalSlider(this.lif.colortime, 1f, 60f);

		if(GUILayout.Button("Hide GUI (S)")) {
			this.showgui = !this.showgui;
		}
		
		GUILayout.EndArea();

		GUILayout.BeginArea(new Rect(Screen.width-200, 5, 195, 100), GUI.skin.GetStyle("Box"));

		GUILayout.Label("Stats", GUI.skin.customStyles[0]);

		GUILayout.Label("~"+(int)this.fps+" Frames Per Second");
		GUILayout.Label(this.lif.tick+" ticks of this life");
		GUILayout.Label(this.lif.aliveamount+" cells alive");

		GUILayout.EndArea();

	}
}
