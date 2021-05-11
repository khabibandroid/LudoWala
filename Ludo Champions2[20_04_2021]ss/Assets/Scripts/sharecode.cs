using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine;
using UnityEngine.UI;

public class sharecode : MonoBehaviour
{
    public Button shareBtn;
    public GameObject RoomIDText;
    private bool isFocus = false;
    private bool isProcessing = false;

    // Start is called before the first frame update
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

#if UNITY_ANDROID
        if (!isProcessing)
        {
            StartCoroutine(ShareTextInAndroid());

        }
#else
        Debug.Log("No sharing set up for this plateform.");


#endif
    }

#if UNITY_ANDROID
    public IEnumerator ShareTextInAndroid()
    {

        var shareSubject = "This is friends room code. ";
        var shareMessage = StaticStrings.SharePrivateLinkMessage + " " + RoomIDText.GetComponent<Text>().text + "\n\n" ;

        isProcessing = true;

        if (!Application.isEditor)
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
