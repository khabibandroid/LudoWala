using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemCheck : MonoBehaviour
{
    public Text label;

    // Start is called before the first frame update
    void Start()
    {
        Toggle toggle = gameObject.GetComponent<Toggle>();

        if (label.text == "Paytm")
        {
            toggle.interactable = MainmenuManager.instance.paytmVerified;
        }
        if (label.text == "GPAY")
        {
            toggle.interactable = MainmenuManager.instance.gpayVerified;
        }
        if (label.text == "UPI ID")
        {
            toggle.interactable = MainmenuManager.instance.upiVerified;
        }

        // if (toggle != null && gameController != null)
        {
         //   toggle.interactable = gameController.IsItemSelectable(this);
        }
    }

   
}
