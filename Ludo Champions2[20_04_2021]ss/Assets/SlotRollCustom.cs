using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotRollCustom : MonoBehaviour
{
    float speed = 200f;
    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.y >= 1600f) {
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, new Vector3(0, transform.localPosition.y + 31.25f, 0), speed*Time.deltaTime);
        }
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }
}
