using System;
using System.Text;
using Toast.Kit.Common.Util;
using Toast.Kit.Manager.Util;
using UnityEngine;

namespace Toast.Kit.Manager.Constant
{
    public static class ManagerPaths
    {
        public const string URI = "https://raw.githubusercontent.com/nhn/toastkit.unity/master/release/";
        public const string RELEASE_NOTES_URI = "https://github.com/nhn/toastkit.unity/blob/master/docs/Manager/ReleaseNotes.md";

        public static readonly string TOAST_KIT_ROOT = "TOAST/Kit";

        public static readonly string LANGUAGE_FILE_PATH = string.Format("{0}/Manager/Data/strings.xml", TOAST_KIT_ROOT);

        public const string INSTALL_INFO_FILE_NAME = "install.json";
        public const string CDN_INFO_FILE_NAME = "cdn.xml";
        public const string SERVICE_LIST_FILE_NAME = "servicelist.xml";
        public const string SERVICE_FILE_NAME = "service.xml";
        public const string SERVICE_LANGUAGE_FILE_NAME = "language.xml";
        public const string AD_FILE_NAME = "advertisement.xml";

        public const string COMMON_SERVICE_NAME = "Common";
        public const string AD_FOLDER_NAME = "Ad";

        public static readonly string PROJECT_ROOT_PATH = Application.dataPath.Replace("/Assets", "");
        public static readonly string PROJECT_ASSETS_PATH = Application.dataPath;
        public static readonly string PROJECT_DOWNLOAD_PATH = Application.dataPath.Replace("Assets", ManagerInfos.BRAND_NAME);

        public static readonly string CACHING_PATH = string.Format("{0}/{1}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), TOAST_KIT_ROOT);
        public static readonly string LIBRARY_PATH = Application.dataPath.Replace("/Assets", string.Format("/Library/{0}", TOAST_KIT_ROOT));
        public static readonly string TEMP_PATH = Application.dataPath.Replace("/Assets", string.Format("/Temp/{0}", TOAST_KIT_ROOT));
        public static readonly string BACKUP_PATH = ToastKitPathUtil.Combine(TEMP_PATH, "Backup");

        public static readonly string TEMP_REFRESH_FILE_PATH = ToastKitPathUtil.Combine(LIBRARY_PATH, "Refresh.json");
        
        public static string GetCachingPath(params string[] path)
        {
            StringBuilder builder = new StringBuilder(CACHING_PATH);

            for (int i = 0; i < path.Length; i++)
            {
                builder.AppendFormat("/{0}", path[i]);
            }

            return builder.ToString();
        }
    }
}