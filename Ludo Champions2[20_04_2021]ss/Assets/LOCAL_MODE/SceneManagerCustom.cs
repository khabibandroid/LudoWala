using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerCustom : MonoBehaviour
{
    public void LoadScene(int x)
    {
        SceneManager.LoadScene(x);
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
