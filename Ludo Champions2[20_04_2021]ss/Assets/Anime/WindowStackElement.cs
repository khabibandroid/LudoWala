using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowStackElement : MonoBehaviour
{
    public WindowStack ws;
    int id = -1;
    private void OnEnable()
    {
        id = ws.AddToStack(gameObject);
    }

    private void OnDisable()
    {
        ws.RemoveFromStack(id);
    }
}
