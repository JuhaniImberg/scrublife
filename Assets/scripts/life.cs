﻿using UnityEngine;
using System.Collections;

public class life : MonoBehaviour
{

	int[,] stuff;
	int[,] theotherstuff;
	bool dirty;
	int height;
	int width;
	float size;
	private float nextRun = 0f;
	public float period = 0.33f;
	private float ver = 0f;
	public bool autorun = true;
	public GameObject block;
	public GameObject poi;
	private bool running;
	public float colortime = 10f;
	public float rotationspeed = 2f;
	public int tick;
	public int aliveamount;
	public int change;
	bool strict = false;
	public bool shouldstrict = false;
	public string[] possible;
	public int selected;
	public Cells[] all;
	public bool autorotate = true;
	public bool autocolor = true;
	public float camerarotation = 0f;
	public float colorrotation = 0f;

	public void InitSystems ()
	{
		this.all = Cells.GetAll ();

		this.selected = 0;
		this.possible = new string[1 + this.all.Length];
		this.possible [0] = "Random";
		for (int i = 0; i < this.all.Length; i++)
		{
			this.possible [i + 1] = this.all [i].name;
		}
	}

	void Start ()
	{
		this.running = true;
		//cubepool.block = this.block;
		//cubepool.AddInitial();
	}

	public void Initialize (int height, int width, float size)
	{
		this.height = height;
		this.width = width;
		this.size = size;
		this.stuff = new int[this.height, this.width];
		this.theotherstuff = new int[this.height, this.width];

		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				this.stuff [i, j] = 0; //(Random.value>0.5f?1:0);
				this.theotherstuff [i, j] = 0;
			}
		}

		if (this.possible [this.selected].Equals ("Random"))
		{
			for (int i = 0; i < this.height; i++)
			{
				for (int j = 0; j < this.width; j++)
				{
					this.stuff [i, j] = (Random.value > 0.5f ? 1 : 0);
				}
			}
		}
		else
		{
			this.DoArray (this.all [this.selected - 1].cells);
		}

		this.tick = 0;
		this.dirty = true;
		this.running = true;
		this.strict = this.shouldstrict;
	}

	void DoArray (int[,] arr)
	{
		int offsetx = (this.width - arr.GetLength (1)) / 2;
		int offsety = (this.height - arr.GetLength (0)) / 2;
		for (int i = 0; i < arr.GetLength(0); i++)
		{
			for (int j = 0; j < arr.GetLength(1); j++)
			{
				try
				{
					this.stuff [i + offsety, j + offsetx] = arr [i, j];
				}
				catch (System.Exception e)
				{

				}
			}
		}
	}

	int Mod (int x, int m)
	{
		int r = x % m;
		return r < 0 ? r + m : r;
	}

	int StrictNeighbours (int[,] world, int x, int y)
	{
		int count = 0;

		if (x - 1 >= 0)
		{
			if (y - 1 >= 0)
			{
				if (world [x - 1, y - 1] > 0)
				{
					count++;
				}
			}
			if (world [x - 1, y] > 0)
			{
				count++;
			}
			if (y + 1 < this.height)
			{
				if (world [x - 1, y + 1] > 0)
				{
					count++;
				}
			}
		}

		if (y - 1 >= 0)
		{
			if (world [x, y - 1] > 0)
			{
				count++;
			}
		}
		if (y + 1 < this.height)
		{
			if (world [x, y + 1] > 0)
			{
				count++;
			}
		}

		if (x + 1 < this.width)
		{
			if (y - 1 >= 0)
			{
				if (world [x + 1, y - 1] > 0)
				{
					count++;
				}
			}
			if (world [x + 1, y] > 0)
			{
				count++;
			}
			if (y + 1 < this.height)
			{
				if (world [x + 1, y + 1] > 0)
				{
					count++;
				}
			}
		}

		return count;
	}

	int FluffyNeighbours (int[,] world, int x, int y)
	{
		int count = 0;

		if (world [Mod (x - 1, this.width), Mod (y - 1, this.height)] > 0)
		{
			count++;
		}
		if (world [Mod (x - 1, this.width), Mod (y, this.height)] > 0)
		{
			count++;
		}
		if (world [Mod (x - 1, this.width), Mod (y + 1, this.height)] > 0)
		{
			count++;
		}

		if (world [x, Mod (y - 1, this.height)] > 0)
		{
			count++;
		}
		if (world [x, Mod (y + 1, this.height)] > 0)
		{
			count++;
		}

		if (world [Mod (x + 1, this.width), Mod (y - 1, this.height)] > 0)
		{
			count++;
		}
		if (world [Mod (x + 1, this.width), Mod (y, this.height)] > 0)
		{
			count++;
		}
		if (world [Mod (x + 1, this.width), Mod (y + 1, this.height)] > 0)
		{
			count++;
		}

		return count;
	}

	int Neighbours (int[,] world, int x, int y)
	{
		return (this.strict ? StrictNeighbours (world, x, y) : FluffyNeighbours (world, x, y));
	}

	int Alive ()
	{
		int amount = 0;
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				amount += this.stuff [i, j];
			}
		}
		return amount;
	}

	void RunLife ()
	{
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				int n = Neighbours (this.stuff, i, j);
				if (this.stuff [i, j] > 0)
				{
					if (n < 4 && n > 1)
					{
						this.theotherstuff [i, j] = 1;
					}
				}
				else
				{
					if (n == 3)
					{
						this.theotherstuff [i, j] = 1;
					}
				}
			}
		}

		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				this.stuff [i, j] = this.theotherstuff [i, j];
				this.theotherstuff [i, j] = 0;
			}
		}
		this.dirty = true;
		this.tick++;
		int tmp = this.Alive ();
		this.change = tmp - this.aliveamount;
		this.aliveamount = tmp;
	}

	void Update ()
	{

		this.transform.LookAt (poi.transform);
		if (this.autorotate)
		{
			this.transform.Translate (Vector3.right * Time.deltaTime * this.rotationspeed);
		}
		else
		{
			this.transform.Translate (Vector3.right * this.camerarotation);
		}

	}

	void FixedUpdate ()
	{
		if (!this.running)
			return;

		cubescript.lifespan = this.period;

		if (this.autorun && Time.time > nextRun)
		{
			//Debug.Log(nextRun);
			nextRun += period;
			RunLife ();
		}

		if (this.dirty == true)
		{

			this.transform.Translate (new Vector3 (0f, this.size * (1f), 0f), Space.World);
			poi.transform.Translate (0f, this.size * (1f), 0f);

			for (int i = 0; i < this.height; i++)
			{
				for (int j = 0; j < this.width; j++)
				{
					if (stuff [i, j] > 0)
					{
						GameObject clone;
						//clone = Instantiate (block, new Vector3 ((j - 0.5f - (width / 2)) * this.size, this.ver, -(i - 0.5f - (height / 2)) * this.size), Quaternion.identity) as GameObject;
						//clone = PoolManager.Spawn("Cube");
						clone = Instantiate(block) as GameObject;
						//clone = cubepool.Get();
						clone.transform.Translate(new Vector3 ((j - 0.5f - (width / 2)) * this.size, this.ver, -(i - 0.5f - (height / 2)) * this.size));
						clone.transform.localScale *= this.size;
						clone.GetComponent<cubescript>().GoGoGo();
					}
				}
			}
			this.ver += this.size * (1f);

			this.dirty = false;
		}

		if (this.autocolor)
		{
			this.camera.backgroundColor = ColorFromHSV ((Time.time * this.colortime) % 360, 1f, 1f);
		}
		else
		{
			this.camera.backgroundColor = ColorFromHSV (this.colorrotation, 1f, 1f);
		}

	}

	public static Color ColorFromHSV (float h, float s, float v, float a = 1)
	{
		// no saturation, we can return the value across the board (grayscale)
		if (s == 0)
			return new Color (v, v, v, a);

		// which chunk of the rainbow are we in?
		float sector = h / 60;

		// split across the decimal (ie 3.87 into 3 and 0.87)
		int i = (int)sector;
		float f = sector - i;

		float p = v * (1 - s);
		float q = v * (1 - s * f);
		float t = v * (1 - s * (1 - f));

		// build our rgb color
		Color color = new Color (0, 0, 0, a);

		switch (i)
		{
		case 0:
			color.r = v;
			color.g = t;
			color.b = p;
			break;

		case 1:
			color.r = q;
			color.g = v;
			color.b = p;
			break;

		case 2:
			color.r = p;
			color.g = v;
			color.b = t;
			break;

		case 3:
			color.r = p;
			color.g = q;
			color.b = v;
			break;

		case 4:
			color.r = t;
			color.g = p;
			color.b = v;
			break;

		default:
			color.r = v;
			color.g = p;
			color.b = q;
			break;
		}

		return color;
	}
}
