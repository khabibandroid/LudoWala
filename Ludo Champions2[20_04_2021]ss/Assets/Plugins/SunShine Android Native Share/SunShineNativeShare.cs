using UnityEngine;


public class SunShineNativeShare : MonoBehaviour
{


    public static string TYPE_VIDEO = "video/*";
    public static string TYPE_AUDIO = "audio/*";
    public static string TYPE_IMAGE = "image/*";
    public static string TYPE_FILE = "file/*";
    public static string TYPE_PDF = "application/pdf*";


    private const string SHARE_PACKAGE_NAME = "com.SmileSoft.unityplugin";
    private const string SHARE_CLASS_NAME = ".ShareFragment";
    private const string TEXT_SHARE_METHOD = "ShareText";
    private const string SINGLE_FILE_SHARE_METHOD = "ShareSingleFile";
    private const string MULTIPLE_FILE_SHARE_METHOD = "ShareMultipleFileOfSameFileType";

    private const string FileProviderName = "com.SmileSoft.unityplugin.ShareProvider_test";



    public static void ShareText(string message, string shareDialogTitle)
    {

#if UNITY_ANDROID
        using (AndroidJavaObject share_android_obj = new AndroidJavaObject(SHARE_PACKAGE_NAME + SHARE_CLASS_NAME))
        {
            share_android_obj.Call(TEXT_SHARE_METHOD, message, shareDialogTitle);

        }
#endif
        Debug.Log("Native Share just work in android Platform");


    }

    public static void ShareSingleFile(string path, string fileType, string message, string shareDialogTitle)
    {
#if UNITY_ANDROID
        using (AndroidJavaObject share_android_obj = new AndroidJavaObject(SHARE_PACKAGE_NAME + SHARE_CLASS_NAME))
        {
            share_android_obj.Call(SINGLE_FILE_SHARE_METHOD, FileProviderName, path, fileType, message, shareDialogTitle);
        }

#endif
        Debug.Log("Native Share just work in android Platform");
    }

    public static void ShareMultipleFileOfSameType(string[] path, string fileType, string message, string shareDialogTitle)
    {

#if UNITY_ANDROID

        using (AndroidJavaObject share_android_obj = new AndroidJavaObject(SHARE_PACKAGE_NAME + SHARE_CLASS_NAME))
        {
            share_android_obj.Call(MULTIPLE_FILE_SHARE_METHOD, FileProviderName, path, fileType, message, shareDialogTitle);

        }

#endif

        Debug.Log("Native Share just work in android Platform");
    }

    public static void ShareMultipleFileOfMultipleType(string[] path, string message, string shareDialogTitle)
    {
#if UNITY_ANDROID
        using (AndroidJavaObject share_android_obj = new AndroidJavaObject(SHARE_PACKAGE_NAME + SHARE_CLASS_NAME))
        {

            share_android_obj.Call(MULTIPLE_FILE_SHARE_METHOD, FileProviderName, path, SunShineNativeShare.TYPE_FILE, message, shareDialogTitle);

        }
#endif

        Debug.Log("Native Share just work in android Platform");
    }


}
