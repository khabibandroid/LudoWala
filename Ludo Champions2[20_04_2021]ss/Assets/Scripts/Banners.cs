using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Banners : MonoBehaviour
{
   

    // Start is called before the first frame update

    private void Awake()
    {
       
     
    }

    void Start()
    {

        StartCoroutine(ads_banner());

    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator ads_banner()
    {
        WWWForm form = new WWWForm();

    //http://ludowala.com/adminphp

        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?user/banner", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //errorMsg.text = download.error;
            Debug.Log("Error downloading: " + www.error);
        }
        else
        {

            Debug.Log(www.downloadHandler.text);

            string output = www.downloadHandler.text;

            if (output == "null" || output == "")
            {
                Debug.Log("Auto Login Failed");

            }

            else
            {
                var N = JSON.Parse(output);

                Debug.Log("Banner " + N);

                var banner1 = N[0]["file_name"].Value;

                storebanner.Instance.Banner1 = banner1;
               
                Debug.Log("Banner1 " + banner1);

                var banner2 = N[1]["file_name"].Value;

                storebanner.Instance.Banner2 = banner2;

                Debug.Log("Banner2 " + banner2);

            }

        }

    }

}
public class storebanner
{
    public string Banner1;
    public string Banner2;
    public static storebanner instance;


    public static storebanner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new storebanner();
            }
            return instance;
        }
    }
}

