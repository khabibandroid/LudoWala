using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowStack : MonoBehaviour
{
    List<GameObject> stack = new List<GameObject>();
    public GameObject ExitPanel;
    int currentID = 0;
    int lastBackID = 0;

    public void Back()
    {
        if (stack.Count > 0)
        {
            lastBackID = stack.Count - 1;
            GameObject gg = stack[stack.Count - 1];
            lastBackID = stack.Count - 1;
            stack.RemoveAt(stack.Count-1);
            gg.SetActive(false);
            currentID--;
            //return gg;
        }
        else
        {
            ExitPanel.SetActive(true);
            //return null;
        }
    }

    public void RemoveFromStack(int idd)
    {
        if (lastBackID == idd)
            return;
        stack.RemoveAt(idd);
        currentID--;
    }

    public int AddToStack(GameObject obj)
    {
        stack.Add(obj);
        currentID++;
        return currentID - 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
}
