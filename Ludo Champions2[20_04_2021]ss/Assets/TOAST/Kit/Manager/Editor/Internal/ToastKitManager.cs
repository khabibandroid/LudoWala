using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Toast.Kit.Common;
using Toast.Kit.Common.Multilanguage;
using Toast.Kit.Common.Util;
using Toast.Kit.Manager.Ad;
using Toast.Kit.Manager.Constant;
using Toast.Kit.Manager.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Toast.Kit.Manager.Internal
{
    internal class ToastKitManager
    {
        private static ToastKitManager instance;
        private static ToastKitServiceInstaller installer;

        public static ToastKitManager Instance
        {
            get { return instance ?? (instance = new ToastKitManager()); }
        }

        public static ToastKitServiceInstaller ServiceInstaller
        {
            get { return installer ?? (installer = new ToastKitServiceInstaller()); }
        }

        public static bool IsLock = false;

        private EditorWindow toolWindow;
        private ErrorCallback onErrorCallback;

        private readonly Dictionary<string, ServiceInfo> cachedServiceInfos = new Dictionary<string, ServiceInfo>();
        private readonly List<string> loadingServiceNames = new List<string>();
        private string showServiceName;

        public string CdnUri { get; private set; }

        public InstallInfo Install { get; private set; }

        public ServiceList Services { get; private set; }

        public ServiceInfo CurrentServiceInfo { get; private set; }

        public string ServiceLanguageName { get; private set; }

        public bool Initialized { get; private set; }


        public static string CurrentLanguageCode
        {
            get { return ToastKitMultilanguage.GetSelectLanguage(ManagerInfos.SERVICE_NAME, false); }
        }

        public void Initialize(ErrorCallback errorCallback, EditorWindow window, Rect adRect)
        {
            toolWindow = window;
            onErrorCallback = errorCallback;

            CdnUri = ManagerPaths.URI;

            EditorCoroutine.Start(SendRequest(
                ManagerPaths.CDN_INFO_FILE_NAME,
                (isError, request) =>
                {
                    if (isError == true)
                    {
                        onErrorCallback(new ManagerError(ManagerErrorCode.NETWORK, ManagerStrings.ERROR_MESSAGE_NETWORK));
                        return;
                    }

                    Action getCdnInfoSuccess = () =>
                    {
                        LoadInstallInfo();
                        LoadServiceList();

                        Advertisement.Initialize(
                            window,
                            adRect,
                            new AdvertisementConfigurations(
                                string.Format("{0}/{1}/", CdnUri, ManagerPaths.AD_FOLDER_NAME),
                                ManagerPaths.GetCachingPath(ManagerPaths.AD_FOLDER_NAME),
                                ManagerPaths.AD_FILE_NAME,
                                ToastKitMultilanguage.GetSupportLanguages(ManagerInfos.SERVICE_NAME, false),
                                false
                            ),
                            CurrentLanguageCode);

                        Advertisement.SetSelectAdvertisementInfoCallback(ToastKitManagerIndicator.SendAd);
                    };

                    if ((HttpStatusCode)request.responseCode == HttpStatusCode.NotFound)
                    {
                        getCdnInfoSuccess();
                    }
                    else
                    {
                        XmlHelper.LoadXmlFromText<CdnInfo>(
                            request.downloadHandler.text,
                            (responseCode, xmlData, message) =>
                            {
                                if (XmlHelper.ResponseCode.SUCCESS != responseCode)
                                {
                                    onErrorCallback(new ManagerError(ManagerErrorCode.CDN, ManagerStrings.ERROR_MESSAGE_CDN_INFO));
                                    return;
                                }

                                CdnUri = xmlData.uri;
                                getCdnInfoSuccess();
                            });
                    }
                }));
        }

        public void RefreshServiceData()
        {
            LoadServiceList();

            if (CurrentServiceInfo != null)
            {
                UiRefreshInfo refreshInfo = new UiRefreshInfo
                {
                    lastServiceName = CurrentServiceInfo.title
                };

                Directory.CreateDirectory(ManagerPaths.LIBRARY_PATH);
                File.WriteAllText(ManagerPaths.TEMP_REFRESH_FILE_PATH, JsonUtility.ToJson(refreshInfo));
            }

            Services = null;
            CurrentServiceInfo = null;
            ServiceLanguageName = string.Empty;
            loadingServiceNames.Clear();

            foreach (var serviceInfo in cachedServiceInfos)
            {
                ToastKitMultilanguage.Unload(ManagerInfos.GetServiceLanguageName(serviceInfo.Key), (code, message) => { });
            }
            cachedServiceInfos.Clear();

            if (toolWindow != null)
            {
                toolWindow.SendEvent(EditorGUIUtility.CommandEvent(ManagerEvents.INFO_RESET));
            }
        }

        public void CheckServiceInfoVersion(ManagerError error)
        {
            if (CurrentServiceInfo == null || error.ErrorCode == ManagerErrorCode.NETWORK)
            {
                onErrorCallback(error);
                return;
            }

            RequestServiceInfo(
                CurrentServiceInfo.title, 
                (result, newError) =>
                {
                    if (newError != null && newError.ErrorCode == ManagerErrorCode.SERVICE_INFO_NOT_CHANGE)
                    {
                        onErrorCallback(error);
                        return;
                    }

                    onErrorCallback(new ManagerError(ManagerErrorCode.SERVICE_INFO_UPDATE, ManagerStrings.SERVICE_INFO_UPDATE_COMPLETED, isOpenDialog: false));
                    RefreshServiceData();
                });
        }

        public void Clear()
        {
            foreach (var serviceInfo in cachedServiceInfos)
            {
                ToastKitMultilanguage.Unload(ManagerInfos.GetServiceLanguageName(serviceInfo.Key), (code, message) => { });
            }

            ToastKitMultilanguage.Unload(ManagerInfos.SERVICE_NAME, (code, message) => { });
            Advertisement.OnDestroy();

            cachedServiceInfos.Clear();
            installer = null;
            instance = null;
        }

        public void LoadServiceInfo(string serviceName, Action<ServiceInfo, ManagerError> callback)
        {
            if (loadingServiceNames.Contains(serviceName) == true)
            {
                return;
            }

            if (CurrentServiceInfo != null && CurrentServiceInfo.title.Equals(serviceName) == true)
            {
                callback(CurrentServiceInfo, null);
                return;
            }

            CurrentServiceInfo = null;
            showServiceName = serviceName;

            ServiceInfo serviceInfo = null;
            if (cachedServiceInfos.TryGetValue(serviceName, out serviceInfo) == true)
            {
                CurrentServiceInfo = serviceInfo;
                ServiceLanguageName = ManagerInfos.GetServiceLanguageName(serviceName);

                callback(CurrentServiceInfo, null);
            }
            else
            {
                RequestServiceInfo(serviceName, (result, error) =>
                {
                    if (error != null)
                    {
                        onErrorCallback(error);
                        callback(null, error);
                        return;
                    }

                    if (showServiceName.Contains(serviceName) == true)
                    {
                        if (CurrentServiceInfo == null)
                        {
                            ServiceLanguageName = ManagerInfos.GetServiceLanguageName(serviceName);

                            CurrentServiceInfo = result;
                            callback(CurrentServiceInfo, null);
                        }
                    }
                });
            }
        }

        private void RequestServiceInfo(string serviceName, Action<ServiceInfo, ManagerError> callback)
        {
            loadingServiceNames.Add(serviceName);

            string servicePath = ToastKitPathUtil.Combine(serviceName, ManagerPaths.SERVICE_FILE_NAME);

            Action<ServiceInfo, ManagerError> responseCallback = 
                (info, error) =>
                {
                    loadingServiceNames.Remove(serviceName);
                    callback(info, error);
                };

            EditorCoroutine.Start(SendRequest(
                servicePath,
                (isInfoError, infoRequest) =>
                {
                    if (isInfoError == true)
                    {
                        onErrorCallback(new ManagerError(
                            ManagerErrorCode.NETWORK,
                            ManagerStrings.ERROR_MESSAGE_NETWORK, isOpenDialog: false));
                        return;
                    }

                    if ((HttpStatusCode)infoRequest.responseCode != HttpStatusCode.OK)
                    {
                        responseCallback(null, new ManagerError(ManagerErrorCode.SERVICE_INFO,
                            ManagerStrings.SERVICE_INFO_NOT_FOUND, string.Format("(Service= {0}, Code= {1})", serviceName, infoRequest.responseCode), false));
                        return;
                    }

                    XmlHelper.LoadXmlFromText<ServiceInfo>(
                        infoRequest.downloadHandler.text,
                        (responseCode, xmlData, message) =>
                        {
                            if (responseCode != XmlHelper.ResponseCode.SUCCESS)
                            {
                                responseCallback(null, new ManagerError(ManagerErrorCode.SERVICE_INFO,
                                    ManagerStrings.ERROR_MESSAGE_SERVICE_INFO_LOAD_FAILED,
                                    string.Format("{0} info parsing error. (Code= {1}, Message= {2})", serviceName, responseCode, message),
                                    false));
                                return;
                            }

                            if (cachedServiceInfos.ContainsKey(serviceName) == true && 
                                string.IsNullOrEmpty(cachedServiceInfos[serviceName].infoVersion) == false && 
                                cachedServiceInfos[serviceName].infoVersion.Equals(xmlData.infoVersion) == true)
                            {
                                responseCallback(null, new ManagerError(ManagerErrorCode.SERVICE_INFO_NOT_CHANGE,
                                    ManagerStrings.ERROR_MESSAGE_SERVICE_INFO_LOAD_FAILED,
                                    string.Format("(Service= {0})", serviceName),
                                    false));
                                return;
                            }

                            string languageServiceName = ManagerInfos.GetServiceLanguageName(serviceName);
                            string languageUrl = ToastKitPathUtil.UrlCombine(CdnUri, serviceName, ManagerPaths.SERVICE_LANGUAGE_FILE_NAME);
                            ToastKitMultilanguage.Load(languageServiceName, languageUrl,
                                (result, resultMessage) =>
                                {
                                    if (result != MultilanguageResultCode.SUCCESS && result != MultilanguageResultCode.ALREADY_LOADED)
                                    {
                                        responseCallback(null, new ManagerError(ManagerErrorCode.SERVICE_INFO,
                                            ManagerStrings.ERROR_MESSAGE_SERVICE_LANGAGE_LOAD_FAILED,
                                            string.Format("(Service= {0}, Code= {1})", serviceName, result),
                                            false));
                                        return;
                                    }

                                    ToastKitMultilanguage.SelectLanguageByCode(languageServiceName, GetLanguageCode(), ((code, s) => { }));

                                    if (cachedServiceInfos.ContainsKey(serviceName) == true)
                                    {
                                        cachedServiceInfos[serviceName] = xmlData;
                                    }
                                    else
                                    {
                                        cachedServiceInfos.Add(serviceName, xmlData);
                                    }

                                    responseCallback(xmlData, null);
                                });
                        });
                }
            ));
        }

        public void ChangeLanguageCode(string nativeName)
        {
            ToastKitMultilanguage.SelectLanguageByNativeName(ManagerInfos.SERVICE_NAME, nativeName,
                (result, resultMessage) =>
                {
                    if (result != MultilanguageResultCode.SUCCESS)
                    {
                        onErrorCallback(new ManagerError(ManagerErrorCode.SETTING, ManagerStrings.ERROR_MESSAGE_CHANGE_LANGUAGE,
                            string.Format("(Code= {0})", result), false));
                        return;
                    }

                    string languageCode = GetLanguageCode();
                    foreach (var serviceInfo in cachedServiceInfos)
                    {
                        string serviceLanguageName = ManagerInfos.GetServiceLanguageName(serviceInfo.Key);
                        ToastKitMultilanguage.SelectLanguageByCode(serviceLanguageName, languageCode, (code, message) => { });
                    }

                    Advertisement.SetLanguageCode(CurrentLanguageCode);
                    ManagerInfos.LastLanguage = CurrentLanguageCode;
                });
        }

        public string GetLanguageCode()
        {
            return ToastKitMultilanguage.GetSelectLanguage(ManagerInfos.SERVICE_NAME, false);
        }

        public void DownloadServiceImage(ServiceInfo service)
        {
            if (service == null)
            {
                return;
            }

            foreach (var imageInfo in service.imageList)
            {
                string localFilepath = ManagerPaths.GetCachingPath(service.title, imageInfo.path);
                if (string.IsNullOrEmpty(localFilepath) == false && File.Exists(localFilepath) == true)
                {
                    continue;
                }

                string urlPath = ToastKitPathUtil.Combine(service.title, imageInfo.path);

                EditorCoroutine.Start(SendRequest(
                    urlPath,
                    (isError, request) =>
                    {
                        if ((HttpStatusCode)request.responseCode != HttpStatusCode.OK)
                        {
                            onErrorCallback(new ManagerError(
                                ManagerErrorCode.SERVICE_INFO,
                                ManagerStrings.ERROR_MESSAGE_SERVICE_IMAGE_GET_FAILED,
                                string.Format("(Service= {0}, File={1}, Code= {2})", service.title, imageInfo.path, request.responseCode),
                                false));
                            return;
                        }

                        string directoryName = Path.GetDirectoryName(localFilepath);
                        if (string.IsNullOrEmpty(directoryName) == false && Directory.Exists(directoryName) == false)
                        {
                            Directory.CreateDirectory(directoryName);
                        }

                        using (FileStream fs = new FileStream(localFilepath, FileMode.Create))
                        {
                            fs.Write(request.downloadHandler.data, 0, (int)request.downloadedBytes);
                        }
                    }));
            }
        }

        private void LoadServiceList()
        {
            Initialized = false;

            EditorCoroutine.Start(SendRequest(
                ManagerPaths.SERVICE_LIST_FILE_NAME,
                (isError, request) =>
                {
                    if (isError == true)
                    {
                        onErrorCallback(new ManagerError(ManagerErrorCode.NETWORK, ManagerStrings.ERROR_MESSAGE_NETWORK));
                        return;
                    }
                    
                    if ((HttpStatusCode)request.responseCode != HttpStatusCode.OK)
                    {
                        onErrorCallback(new ManagerError(
                            ManagerErrorCode.SERVICE_LIST,
                            ManagerStrings.ERROR_MESSAGE_SERVICE_LIST_GET_FAILED,
                            string.Format("(Code= {0})", request.responseCode),
                            false));
                        return;
                    }

                    XmlHelper.LoadXmlFromText<ServiceList>(
                        request.downloadHandler.text,
                        (responseCode, xmlData, message) =>
                        {
                            if (responseCode != XmlHelper.ResponseCode.SUCCESS)
                            {
                                Services = null;
                                onErrorCallback(new ManagerError(
                                    ManagerErrorCode.SERVICE_LIST,
                                    ManagerStrings.ERROR_MESSAGE_SERVICE_LIST_GET_FAILED,
                                    string.Format("parsing error (Code= {0}, Message= {1})", responseCode, message),
                                    false));
                                return;
                            }

                            Services = xmlData;
                            Initialized = true;
                        });
                }));
        }

        private void LoadInstallInfo()
        {
            try
            {
                string installPath = ToastKitPathUtil.Combine(ManagerPaths.PROJECT_DOWNLOAD_PATH, ManagerPaths.INSTALL_INFO_FILE_NAME);
                if (File.Exists(installPath) == true)
                {
                    Install = JsonUtility.FromJson<InstallInfo>(File.ReadAllText(installPath));

                    InstallInfo.Service commonInfo = Install.GetInstallInfo(ManagerPaths.COMMON_SERVICE_NAME);
                    if (commonInfo.version.Equals(ToastKitCommon.VERSION) == false)
                    {
                        commonInfo.version = ToastKitCommon.VERSION;
                    }
                }
            }
            catch (Exception e)
            {
                onErrorCallback(new ManagerError(ManagerErrorCode.SETTING, ManagerStrings.ERROR_MESSAGE_INSTALL_INFO_LOAD, e.Message, false));
            }
            finally
            {
                Install = Install ?? new InstallInfo()
                {
                    installs = new List<InstallInfo.Service>()
                    {
                        new InstallInfo.Service()
                        {
                            name = ManagerPaths.COMMON_SERVICE_NAME,
                            version = ToastKitCommon.VERSION
                        }
                    }
                };
            }
        }

        private static IEnumerator SendRequest(string path, Action<bool, UnityWebRequest> callback)
        {
            string url = ToastKitPathUtil.UrlCombine(Instance.CdnUri, path);

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.timeout = 5;

                yield return request.SendWebRequest();

                // If you use the editor coroutine, SendWebRequest() next step will work immediately.
                // Therefore, it is checked to request.isDone in the loop statement, and it is written to go to the next step when it is completed.
                while (request.isDone == false)
                {
                    yield return null;
                }

                callback(request.isNetworkError, request);
            }
        }
    }
}