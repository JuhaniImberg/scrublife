using UnityEngine;
using System.Collections;

public class Cells
{

	public int[,] cells;
	public string name;
	public string author;
	public string comment;

	public static string[] GetNames ()
	{
		string[] names = (Resources.Load ("all") as TextAsset).text.Split ("\n" [0]);
		return names;
	}

	public static Cells[] GetAll ()
	{
		string[] names = GetNames ();
		Cells[] all = new Cells[names.Length];
		for (int i = 0; i < names.Length; i++)
		{
			try
			{
				all [i] = Parse (names [i].Trim ());
			}
			catch (System.Exception e)
			{
				Debug.Log ("failed loading: " + names [i]);
			}
		}
		return all;
	}

	public static Cells Parse (string name)
	{
		TextAsset asset = Resources.Load (name) as TextAsset;
		string[] text = asset.text.Split ("\n" [0]);
		Cells data = new Cells ();
		string[] tempcell = new string[text.Length];
		int currentline = 0;
		int longestline = -1;
		for (int i = 0; i < text.Length; i++)
		{
			if (text [i].IndexOf ("!") == 0)   //if comment
			{
				if (text [i].IndexOf ("Name:") != -1)   //if name
				{
					data.name = text [i].Split (new string[] { "Name:" }, System.StringSplitOptions.None) [1].Trim ();
				}
				else if (text [i].IndexOf ("Author:") != -1)     //if author
				{
					data.author = text [i].Split (new string[] { "Author:" }, System.StringSplitOptions.None) [1].Trim ();
				}
				else     // else comment
				{
					data.comment += text [i].Substring (1) + "\n";
				}
			}
			else     //else cell
			{
				tempcell [currentline++] = text [i];
				int tmplength = text [i].Length;
				if (tmplength > longestline)
				{
					longestline = tmplength;
				}
			}
		}

		data.cells = new int[currentline, longestline];
		for (int i = 0; i < currentline; i++)
		{
			for (int j = 0; j < longestline; j++)
			{
				data.cells [i, j] = 0;
			}
			char[] tmpchars = tempcell [i].ToCharArray ();
			for (int j = 0; j < tempcell[i].Length; j++)
			{
				if (tmpchars [j] == 'O')
				{
					data.cells [i, j] = 1;
				}
			}
		}

		Debug.Log (data.name);
		Debug.Log (data.author);
		Debug.Log (data.comment);
		Debug.Log (data.cells);

		return data;
	}

}