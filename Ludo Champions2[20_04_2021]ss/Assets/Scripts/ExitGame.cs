using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{

    public GameObject Exitpanel;
    void Awake()
	{

		DontDestroyOnLoad(transform.gameObject);
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			// sound.Play();
			Exitpanel.SetActive(true);

		}
	}
	public void YesExit()
	{

		Application.Quit();
	}
}
