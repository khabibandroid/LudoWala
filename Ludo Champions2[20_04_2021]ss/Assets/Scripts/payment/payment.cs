using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class payment : MonoBehaviour
{

    public InputField add;
    public InputField paytm_no;
    public GameObject Panel;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Add_money()
    {
        Debug.Log(GameManager.Instance.UserID);
        Application.OpenURL("https://codash.tk/ludowala/index.php?user/PaytmGateway/" + GameManager.Instance.UserID + "/" + add.text);

    }

    public void Patm_NO()
    {
        StartCoroutine(NO());
    }

    IEnumerator NO()
    {

        Debug.Log("live userid " + GameManager.Instance.UserID);
        Debug.Log("live number " + paytm_no.text);

        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Instance.UserID);
        form.AddField("number", paytm_no.text);


        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?user/paytm_no", form);

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
                Debug.Log(" No Contast found.");
            }

            else
            {
                var N = JSON.Parse(output);


                if (N["status"].Value == "1")
                {
                    Debug.Log("Number add successfully");
                    Panel.SetActive(true);

                }
                else
                {
                    Panel.SetActive(true);
                    Panel.GetComponent<Text>().text = "Success Failed!";
                }


            }
        }
    }


}
