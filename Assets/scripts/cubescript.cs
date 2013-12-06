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

	private bool AREWEGOING = false;

	void Start ()
	{

	}

	public void GoGoGo ()
	{
		this.deathtime = Time.time + lifespan * lifeticks;
		this.colortime = Time.time + lifespan;
		startcolor = this.renderer.material.color;
		endcolor = new Color (startcolor.r, startcolor.g, startcolor.b, 0.0f);
		this.AREWEGOING = true;
	}

	public void StopStopStop ()
	{
		this.AREWEGOING = false;
		Destroy (this.gameObject);
		//cubepool.Add(this.gameObject);
	}

	void FixedUpdate ()
	{
		if(!this.AREWEGOING) {
			return;
		}
		if (Time.time >= this.deathtime)
		{
			this.StopStopStop();
			//Destroy (this.gameObject);
			//PoolManager.Despawn(this.gameObject);
			//this.gameObject.GetComponent<PoolObject>().Despawn();
			return;
		}
		if (Time.time >= this.colortime)
		{
			times += Time.deltaTime / lifeticks;
			this.renderer.material.color = Color.Lerp (startcolor, endcolor, times / lifespan);
		}
	}
}
