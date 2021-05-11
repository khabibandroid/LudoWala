using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSound : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		if (PlayerPrefs.GetInt(StaticStrings.SoundsKey, 0) == 0)
		{
			AudioListener.volume = 1;
		}
		else
		{
			AudioListener.volume = 0;
		}
	}
	public void destroy()
	{
		if (this.gameObject != null)
			DestroyImmediate(this.gameObject);
	}
	// Update is called once per frame
	void Update()
	{

	}
}

