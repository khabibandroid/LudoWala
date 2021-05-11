using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HiddenSplash : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("FirstLoad", 0);
        SceneManager.LoadScene(1);
    }
}
