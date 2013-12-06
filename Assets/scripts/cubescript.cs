using UnityEngine;
using System.Collections;

public class cubescript : MonoBehaviour
{

	private float deathtime;
	private float colortime;
	public static float lifespan = 0.2f;
	public static int lifeticks = 5;
	private Color endcolor;
	private Color startcolor;
	private float times = 0f;

	void Start ()
	{
		this.deathtime = Time.time + lifespan * lifeticks;
		this.colortime = Time.time + lifespan;
		startcolor = this.renderer.material.color;
		endcolor = new Color (startcolor.r, startcolor.g, startcolor.b, 0.0f);
	}

	void FixedUpdate ()
	{
		if (Time.time >= this.deathtime)
		{
			Destroy (this.gameObject);
			return;
		}
		if (Time.time >= this.colortime)
		{
			times += Time.deltaTime / lifeticks;
			this.renderer.material.color = Color.Lerp (startcolor, endcolor, times / lifespan);
		}
	}
}
