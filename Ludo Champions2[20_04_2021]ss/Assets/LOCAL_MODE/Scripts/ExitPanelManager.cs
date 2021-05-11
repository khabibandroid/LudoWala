using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPanelManager : MonoBehaviour
{
    public GameObject Blackbg;
    private void OnEnable()
    {
        Blackbg.SetActive(true);
    }
    private void OnDisable()
    {
        Blackbg.SetActive(false);
    }
    public void ClosePanel()
    {

    }
    public void KeepPlay()
    {
        gameObject.SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
