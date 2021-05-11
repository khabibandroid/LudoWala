using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCoinsFrameValue : MonoBehaviour
{

    private int currentValue = 0;
    private Text text;
    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        InvokeRepeating("CheckAndUpdateValue", 0.2f, 0.2f);
    }

    private void CheckAndUpdateValue()
    {
        if (currentValue != int.Parse(GameManager.Instance.Balance))
        {
            currentValue = int.Parse(GameManager.Instance.Balance);
            if (currentValue != 0)
            {
                text.text = int.Parse(GameManager.Instance.Balance).ToString("0,0", CultureInfo.InvariantCulture)+" Coins";
            }
            else
            {
                text.text = "0";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
