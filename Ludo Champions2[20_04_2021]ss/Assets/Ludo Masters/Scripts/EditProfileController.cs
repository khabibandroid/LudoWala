using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EditProfileController : MonoBehaviour
{
    public InputField editName;

    public InputField avtarurl;
    private string avatarIndex;

    public GameObject PlayerNameMain;
    public GameObject PlayerName;
    public GameObject PlayerAvatarMain;
    public GameObject AvatarMain;
    public GameObject AvatarMain1;
    private void Start()
    {

        avatarIndex = GameManager.Instance.avatarIndex;

        editName.GetComponent<InputField>().text = GameManager.Instance.nameMy;




    }

    public void Save()
    {
        GameManager.Instance.avatarIndex = avatarIndex;

        PlayerNameMain.GetComponent<Text>().text = editName.GetComponent<InputField>().text;
        GameManager.Instance.nameMy = editName.GetComponent<InputField>().text;

        PlayerName.GetComponent<Text>().text = editName.GetComponent<InputField>().text;

        PlayerAvatarMain.GetComponent<Image>().sprite = StaticGameVariablesController.instance.avatars[int.Parse(avtarurl.text)];

        AvatarMain.GetComponent<Image>().sprite = StaticGameVariablesController.instance.avatars[int.Parse(avtarurl.text)];
        AvatarMain1.GetComponent<Image>().sprite = StaticGameVariablesController.instance.avatars[int.Parse(avtarurl.text)];



    }


    public void EditProfile()
    {

        StartCoroutine(edit_profile());

    }

    IEnumerator edit_profile()
    {

        string userId = GameManager.Instance.UserID;
        string Nmetext = editName.text;
        Debug.Log("editName" + editName.text);
        Debug.Log("editavtar" + avtarurl.text);
        string avtar = avtarurl.text;

        Debug.Log("avatar " + avtar);

        WWWForm form = new WWWForm();

        form.AddField("user_id", userId);
        form.AddField("name", Nmetext);
        form.AddField("avatar", avtar);

        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?user/edit_profile", form);

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
                Debug.Log("Login Failed");


            }

            else
            {


                var N = JSON.Parse(output);

                Debug.Log(N);


            }

        }

    }

    public void ima()
    {
        avtarurl.text = "00";

    }

    public void ima1()
    {
        avtarurl.text = "01";

    }


    public void ima2()
    {
        avtarurl.text = "02";

    }

    public void ima3()
    {
        avtarurl.text = "03";

    }

    public void ima4()
    {
        avtarurl.text = "04";

    }

    public void ima5()
    {
        avtarurl.text = "05";

    }

    public void ima6()
    {
        avtarurl.text = "06";

    }

    public void ima7()
    {
        avtarurl.text = "08";

    }

    public void ima8()
    {
        avtarurl.text = "07";

    }

    public void ima9()
    {
        avtarurl.text = "09";

    }

    public void ima10()
    {
        avtarurl.text = "10";

    }



    public void ima11()
    {
        avtarurl.text = "11";

    }

    public void ima12()
    {
        avtarurl.text = "12";

    }

    public void ima13()
    {
        avtarurl.text = "13";

    }

    public void ima14()
    {
        avtarurl.text = "14";

    }

    public void ima15()
    {
        avtarurl.text = "15";

    }

    public void ima16()
    {
        avtarurl.text = "16";

    }

    public void ima17()
    {
        avtarurl.text = "18";

    }

    public void ima18()
    {
        avtarurl.text = "20";

    }

    public void ima19()
    {
        avtarurl.text = "21";

    }
}
