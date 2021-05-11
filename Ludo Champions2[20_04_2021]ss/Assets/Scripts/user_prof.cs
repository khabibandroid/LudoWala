using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class user_prof : MonoBehaviour
{
    public string url;
    public RawImage img;

    // Start is called before the first frame update
    void Start()
    {
        url = GameManager.Instance.avatarMyUrl;
       // url = GameManager.Instance.avatarMyUrl;
        img = this.gameObject.GetComponent<RawImage>();
        StartCoroutine(Load_img());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Load_img()
    {
        WWW www = new WWW(url);
        yield return www;

        img.texture = www.texture;

    }
}
