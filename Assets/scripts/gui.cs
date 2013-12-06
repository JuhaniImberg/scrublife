using UnityEngine;
using System.Collections;

public class gui : MonoBehaviour
{

	public life lif;
	int height = 64;
	int width = 64;
	int size = 2;
	float zoom = 1f;
	bool showgui = true;
	float fps;
	public GUISkin custom;
	bool selectingmode = false;
	Vector2 scrollpos = new Vector2 (0, 0);

	// Use this for initialization
	void Start ()
	{
		this.lif.InitSystems ();
		this.lif.selected = 7;
		this.StartLife ();
	}

	void StartLife ()
	{
		this.lif.Initialize (this.width, this.height, (float)(this.size) / 8);
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetAxis ("Mouse ScrollWheel") > 0)
		{
			if (this.zoom > 0.2f)
				this.zoom -= 0.1f;
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0)
		{
			if (this.zoom < 2.8f)
				this.zoom += 0.1f;
		}
		if (Input.GetKeyDown (KeyCode.Space))
		{
			StartLife ();
		}
		if (Input.GetKeyDown (KeyCode.S))
		{
			this.showgui = !this.showgui;
		}

		this.camera.fieldOfView = 60f * this.zoom;

		this.fps = (1.0f / Time.deltaTime);
	}

	void OnGUI ()
	{
		if (!this.showgui)
			return;
		GUI.skin = custom;

		/* STATUS */
		GUILayout.BeginArea (new Rect (Screen.width - 200, 5, 195, 100), GUI.skin.GetStyle ("Box"));
		GUILayout.Label ("Stats", GUI.skin.customStyles [0]);
		GUILayout.Label ("~" + (int)this.fps + " Frames Per Second");
		GUILayout.Label (this.lif.tick + " ticks of this life");
		GUILayout.Label (this.lif.aliveamount + " (" + (this.lif.change >= 0 ? ("+" + this.lif.change.ToString ()) : (this.lif.change.ToString ())) + ") cells alive");
		GUILayout.EndArea ();
		/* END STATUS */

		if (this.selectingmode)
		{
			GUILayout.BeginArea (new Rect (5, 5, 280, 483), GUI.skin.GetStyle ("Box"));

			if (GUILayout.Button ("Back"))
			{
				this.selectingmode = false;
			}

			this.scrollpos = GUILayout.BeginScrollView (this.scrollpos, GUILayout.Height (250));

			for (int i = 0; i < this.lif.possible.Length; i++)
			{
				if (GUILayout.Button (this.lif.possible [i], (this.lif.selected == i ?GUI.skin.customStyles [1]:GUI.skin.GetStyle("button"))))
				{
					this.lif.selected = i;
				}
			}

			GUILayout.EndScrollView ();

			if (this.lif.selected != 0)
			{

				GUILayout.Label ("Name: " + this.lif.all [this.lif.selected - 1].name);
				GUILayout.Label ("Author: " + this.lif.all [this.lif.selected - 1].author);
				GUILayout.Label ("Size: " + this.lif.all [this.lif.selected - 1].cells.GetLength(0) + "x" + this.lif.all [this.lif.selected - 1].cells.GetLength(1)+" containing "+this.lif.all[this.lif.selected-1].numofcells+" cells");
				GUILayout.Label (this.lif.all [this.lif.selected - 1].comment);

			} else {
				GUILayout.Label ("Name: Random");
				GUILayout.Label ("Author: Your computer");
				GUILayout.Label ("Size: " +this.width+"x"+this.height+" containing ~"+(this.width*this.height/2)+" cells");
				GUILayout.Label ("Warning: Might cause lag");
			}

			GUILayout.FlexibleSpace();

			if (GUILayout.Button ("Restart (Space)"))
			{
				StartLife ();
			}

			GUILayout.EndArea ();
			return;
		}

		GUILayout.BeginArea (new Rect (5, 5, 280, 483), GUI.skin.GetStyle ("Box"));

		/* GENERATION */

		GUILayout.Label ("Generation", GUI.skin.customStyles [0]);

		GUILayout.Label ("Area " + this.width + "x" + this.height);
		this.height = (int)GUILayout.HorizontalSlider ((float)this.height, 8, 128);
		this.width = this.height;

		GUILayout.Label ("Scale " + ((float)(this.size * 4)) / 32);
		this.size = (int)GUILayout.HorizontalSlider ((float)this.size, 1f, 8f);

		this.lif.shouldstrict = !GUILayout.Toggle (!this.lif.shouldstrict, "Wrapping");

		if (GUILayout.Button ("Select mode (Current: " + this.lif.possible [this.lif.selected] + ")"))
		{
			this.selectingmode = true;
		}

		if (GUILayout.Button ("Restart (Space)"))
		{
			StartLife ();
		}

		/* ANIMATION */
		GUILayout.Space (10);
		GUILayout.Label ("Animation", GUI.skin.customStyles [0]);

		GUILayout.Label ("Speed: ~" + (int)(1 / (this.lif.period) + 0.5) + " ticks a second");
		this.lif.period = GUILayout.HorizontalSlider (this.lif.period, 1f, 0.1f);

		GUILayout.Label ("Trail: " + cubescript.lifeticks + " ticks long");
		cubescript.lifeticks = (int)GUILayout.HorizontalSlider ((float)cubescript.lifeticks, 1f, 50f);

		/* CAMERA */
		GUILayout.Space (10);
		GUILayout.Label ("Camera", GUI.skin.customStyles [0]);

		GUILayout.Label ("Zoom");
		this.zoom = GUILayout.HorizontalSlider (this.zoom, 3f, 0.1f);

		if (this.lif.autorotate)
		{
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Rotation speed", GUILayout.Width (150));
			this.lif.autorotate = GUILayout.Toggle (this.lif.autorotate, "on");
			GUILayout.EndHorizontal ();
			this.lif.rotationspeed = GUILayout.HorizontalSlider (this.lif.rotationspeed, -16f, 16f);
		}
		else
		{
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Rotation", GUILayout.Width (150));
			this.lif.autorotate = GUILayout.Toggle (this.lif.autorotate, "on");
			GUILayout.EndHorizontal ();
			GUILayout.Space (16);
		}

		if (this.lif.autocolor)
		{
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Color rotation speed", GUILayout.Width (150));
			bool asd = GUILayout.Toggle (this.lif.autocolor, "automatic");
			if (asd != this.lif.autocolor)
			{
				this.lif.colorrotation = ((Time.time * this.lif.colortime) % 360);
			}
			this.lif.autocolor = asd;
			GUILayout.EndHorizontal ();
			this.lif.colortime = GUILayout.HorizontalSlider (this.lif.colortime, 1f, 60f);
		}
		else
		{
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Color rotation", GUILayout.Width (150));
			this.lif.autocolor = GUILayout.Toggle (this.lif.autocolor, "automatic");
			GUILayout.EndHorizontal ();
			this.lif.colorrotation = GUILayout.HorizontalSlider (this.lif.colorrotation, 0f, 359f);
		}

		if (GUILayout.Button ("Hide GUI (S)"))
		{
			this.showgui = !this.showgui;
		}

		GUILayout.EndArea ();

	}
}
