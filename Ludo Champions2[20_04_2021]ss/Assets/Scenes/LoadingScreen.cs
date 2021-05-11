using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;

    [SerializeField]
    private GameObject loading_Bar_holder, SignUP, playFab, loading ,intro;

    [SerializeField]
    private Image loading_Bar_progress;

    [SerializeField]
    private float progress_Value, progress_Multiplier_1, progress_Multiplier_2, Load_Level_Time;

    private bool hit;

    private string ver;


    public GameObject phoneLoginScreen;
    public bool InitLogin;
  
   private void Awake()
    {
       // PlayerPrefs.DeleteAll();
        MakeSingleton();
    }

  


    private void Start()
    {
       
        LoadLevel();
        hit = true;
       // StartCoroutine("version");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void upp()
    {
        Application.OpenURL("http://ludotrips.com/");
    }

    private void Update()
    {
      // if(InitLogin)
            ShowLoadingScreen();

    }
    
   private void MakeSingleton()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            
        }
    }

    public void LoadLevel()
    {
        progress_Value = 0f;
        loading_Bar_holder.SetActive(true);
       
    }

    private void ShowLoadingScreen()
    {
        if(progress_Value < 1f)
        {
            progress_Value += progress_Multiplier_1 * progress_Multiplier_2;
            loading_Bar_progress.fillAmount = progress_Value;
        }

        if(progress_Value > 1f)
        {
            progress_Value = 1.1f;
            // loading_Bar_progress.fillAmount = 0f;
            if (hit == true)
            {
                hit = false;
                //if (ver == Application.version)
                {
               //     updates.SetActive(false);
                    if (PlayerPrefs.HasKey("user_ID"))
                    {
                        Debug.Log("userId " + GameManager.Instance.UserID + "\n playerprefb " + PlayerPrefs.GetString("user_ID"));
                        playFab.GetComponent<PlayFabManager>().PlayFabId = GameManager.Instance.UserID = PlayerPrefs.GetString("user_ID");
                        StartCoroutine(GameManager.Instance.login.User_Data());
                        GameManager.Instance.playfabManager.PhotonLogin(GameManager.Instance.UserID);
                    }
                    else
                    {
                        SignUP.SetActive(false);
                        if(intro != null) intro.SetActive(false);
                    }
                }
               // else if (ver != Application.version)
                {
                 //   updates.SetActive(true);
                }
              //  else
                {
                  //  loading.SetActive(true);
                //    StartCoroutine("version");
                }
            }
        }
    }

    private IEnumerator version()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www = UnityWebRequest.Post(GameManager.Instance.BaseUrl + "version", form);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error Download: " + www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            string output = www.downloadHandler.text;
            if (output == "null" || output == "" || output == null)
            {
                Debug.Log("Login Failed");
            }
            else
            {
                var N = JSON.Parse(output);
                if (N["status"].Value == "1")
                {
                    ver =  N["version"].Value;
                }
                else
                {
                  
                }
            }
        }
    }
}
