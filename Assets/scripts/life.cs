using UnityEngine;
using System.Collections;

public class life : MonoBehaviour {

	int[,] stuff;
	int[,] theotherstuff;
	bool dirty;
	int height;
	int width;
	float size;

	private float nextRun = 0f;
	public float period = 0.2f;

	private float ver = 0f;

	public bool autorun = true;

	public GameObject block;
	public GameObject poi;

	private bool running;

	void Start () {
		this.running = false;
	}

	public void Initialize(int height, int width, float size) {
		this.height = height;
		this.width = width;
		this.size = size;
		this.stuff = new int[this.height,this.width];
		this.theotherstuff = new int[this.height,this.width];
		
		for(int i = 0; i < this.height; i++) {
			for(int j = 0; j < this.width; j++) {
				this.stuff[i, j] = (Random.value>0.5f?1:0);
				this.theotherstuff[i, j] = 0;
			}
		}
		
		this.dirty = true;
		this.running = true;
	}

	int Mod (int x, int m) {
		int r = x%m;
		return r<0 ? r+m : r;
	}

	int Neighbours (int[,] world, int x, int y) {
		int count = 0;

		if(world[Mod(x-1, this.width), Mod (y-1, this.height)] > 0) { count++; }
		if(world[Mod(x-1, this.width), Mod (y  , this.height)] > 0) { count++; }
		if(world[Mod(x-1, this.width), Mod (y+1, this.height)] > 0) { count++; }

		if(world[x, Mod (y-1, this.height)] > 0) { count++; }
		if(world[x, Mod (y+1, this.height)] > 0) { count++; }

		if(world[Mod(x+1, this.width), Mod (y-1, this.height)] > 0) { count++; }
		if(world[Mod(x+1, this.width), Mod (y  , this.height)] > 0) { count++; }
		if(world[Mod(x+1, this.width), Mod (y+1, this.height)] > 0) { count++; }

		return count;
	}

	void RunLife () {
		for(int i = 0; i < this.height; i++) {
			for(int j = 0; j < this.width; j++) {
				int n = Neighbours(this.stuff, i, j);
				if(this.stuff[i, j] > 0) {
					if(n < 4 && n > 1) { this.theotherstuff[i, j] = 1; }
				} else {
					if(n == 3) { this.theotherstuff[i, j] = 1; }
				}
			}
		}

		for(int i = 0; i < this.height; i++) {
			for(int j = 0; j < this.width; j++) {
				this.stuff[i, j] = this.theotherstuff[i, j];
				this.theotherstuff[i, j] = 0;
			}
		}
		this.dirty = true;
	}

	void Update() {

		this.transform.LookAt(poi.transform);
		this.transform.Translate(Vector3.right * Time.deltaTime * 2);

	}

	void FixedUpdate() {
		if(!this.running)
			return;

		cubescript.lifespan = this.period;

		if(this.autorun && Time.time > nextRun) {
			Debug.Log(nextRun);
			nextRun += period;
			RunLife();
		}

		if(this.dirty == true) {

			this.transform.Translate(new Vector3(0f, this.size*(1f), 0f), Space.World);
			poi.transform.Translate(0f, this.size*(1f), 0f);

			for(int i = 0; i < this.height; i++) {
				for(int j = 0; j < this.width; j++) {
					if(stuff[i, j] > 0) {
						GameObject clone;
						clone = Instantiate(block, new Vector3((j-0.5f-(width/2))*this.size, this.ver, -(i-0.5f-(height/2))*this.size), Quaternion.identity) as GameObject;
						clone.transform.localScale *= this.size;
					}
				}
			}
			this.ver += this.size*(1f);

			this.dirty = false;
		}

		this.camera.backgroundColor = UnityEditor.EditorGUIUtility.HSVToRGB( ((Time.time/60)%1f) ,1f,1f);

	}
}
