using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Sharebutton : MonoBehaviour
{
    public Button shareBtn;

    private bool isFocus = false;
    private bool isProcessing = false;

    // Start is called before the first frame update and sign up using my referral code " + GameManager.Instance.referal + " to get Rs.10 bonus. Msg & data rates may apply."
    private void Start()
    {
        shareBtn.onClick.AddListener(TakeSSAndShare);
    }
    private void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
    }

    private void TakeSSAndShare()
    {
        SunShineNativeShare.ShareText("https://ludowala.com/  Welcome to  Ludo Wala!\n Tap here ludo.atmsoftek.usludolive.apk to download the game", "referal");
        Debug.Log("share initiated");
/*
#if UNITY_ANDROID
        if (!isProcessing)
        {
            StartCoroutine(ShareTextInAndroid());

        }
#else
        Debug.Log("No sharing set up for this plateform.");


    }
#endif*/
    }

#if UNITY_ANDROID
    public IEnumerator ShareTextInAndroid()
    {

        var shareSubject = "";
        var shareMessage = "https://ludowala.com/  Welcome to  Ludo Wala!";
        isProcessing = true;

        if(!Application.isEditor)
        {

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");

            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));


            intentObject.Call<AndroidJavaObject>("setType", "text/Plain");

            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), shareSubject);

            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareMessage);

            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share your referal code");
            currentActivity.Call("startActivity", chooser);
        }

        yield return new WaitUntil(() => isFocus);
        isProcessing = false;
    }
#endif
}
