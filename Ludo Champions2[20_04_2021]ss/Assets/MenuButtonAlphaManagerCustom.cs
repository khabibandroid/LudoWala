using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonAlphaManagerCustom : MonoBehaviour
{
    [Range(0f, 1f)]
    public float alpha = 1f;
    public Image _self, _image;
    public Text _text;

    // Update is called once per frame
    void Update()
    {
        if(alpha < 0.75f)
        {
            alpha = Mathf.Lerp(alpha, 0f, 0.1f);
        }
        _self.color = new Color(_self.color.r, _self.color.g, _self.color.b, alpha);
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, alpha);
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, alpha);
    }
}
