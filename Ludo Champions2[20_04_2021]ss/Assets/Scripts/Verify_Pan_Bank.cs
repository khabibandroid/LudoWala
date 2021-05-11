using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Verify_Pan_Bank : MonoBehaviour
{


    [Header("Verify bank")]

    public InputField Paytm_Field;
    public InputField Gpay_Field;
    public InputField Upi_Field;

    public GameObject infoPanel;
    public Text infoText;


    private void Start()
    {
       
    }

 


    public void bank()
    {
        if(Paytm_Field.text != null)
         StartCoroutine(bank_verify());
    }

    IEnumerator bank_verify()
    {
        string paytm = Paytm_Field.text;
        string gpay  = Gpay_Field.text;
        string upi   = Upi_Field.text;

        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Instance.UserID);
        form.AddField("paytm_number", paytm);
        form.AddField("gpay_number", gpay);
        form.AddField("upi_id", upi);

        //form.AddField("isfc_code", pan);
        //form.AddField("holder_name", namepan);
        //form.AddField("account_no", dob);



        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?user/update_bank", form);

        //Your summited data in under process
        infoPanel.SetActive(true);
        infoText.text = "Your submited data in under process";


        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //errorMsg.text = download.error;
            Debug.Log("Error downloading: " + www.error);
            infoText.text = "" + www.error;

        }
        else
        {

            Debug.Log(www.downloadHandler.text);

            string output = www.downloadHandler.text;

            var N = JSON.Parse(output);


            if (N["status"].Value == "1")
            {
                var message = N["message"].Value;
                Debug.Log(message);
                infoText.text = "Data submitted successfully!";
            }
            else
            {
                Debug.Log("fail!!!");

                infoText.text = "Data submission Failed";
            }


        }

    }


}
