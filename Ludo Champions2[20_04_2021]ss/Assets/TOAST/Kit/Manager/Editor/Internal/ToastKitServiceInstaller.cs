using System;
using System.Collections.Generic;
using System.IO;
using Toast.Kit.Common.Compress;
using Toast.Kit.Common.Util;
using Toast.Kit.Manager.Constant;
using Toast.Kit.Manager.Util;
using UnityEditor;
using UnityEngine;

namespace Toast.Kit.Manager.Internal
{
    internal class PackageInstallInfo
    {
        public string serviceName;
        public string serviceVersion;
        public string packagePath;
        public string packageIntallPath;
    }

    internal class ToastKitServiceInstaller
    {
        private readonly ToastKitServiceDownloader downloader = new ToastKitServiceDownloader();
        private List<string> serviceDependencies = new List<string>();

        public ServiceInfo ProcessService { get; private set; }

        public bool IsProcessing { get { return ProcessService != null; } }
        
        public void Install(ServiceInfo service, Action<ManagerError> callback)
        {
            if (IsProcessing == true)
            {
                callback(new ManagerError(ManagerErrorCode.INSTALL, ManagerStrings.ERROR_MESSAGE_ALREADY_INSTALL));
                return;
            }
            
            if (service == null)
            {
                callback(new ManagerError(ManagerErrorCode.INSTALL, ManagerStrings.ERROR_MESSAGE_PARAMETER, "Install service info null."));
                return;
            }

            string installedVersion = ToastKitManager.Instance.Install.GetInstallVersion(service.title);
            if (string.IsNullOrEmpty(installedVersion) == false && service.version.VersionGreaterThan(installedVersion) == false)
            {
                callback(new ManagerError(ManagerErrorCode.INSTALL, ManagerStrings.ERROR_MESSAGE_ALREADY_INSTALLED));
                return;
            }

            serviceDependencies.Clear();
            foreach (var dependencyInfo in service.dependencies)
            {
                string dependencyServiceName = dependencyInfo.Key;
                if (dependencyServiceName.Equals(ManagerInfos.DEPENDENCY_UNITY_INFO_KEY) == true)
                {
                    continue;
                }

                InstallInfo.Service dependencyServiceInfo = ToastKitManager.Instance.Install.GetInstallInfo(dependencyServiceName);
                bool installable = (dependencyServiceInfo == null || string.IsNullOrEmpty(dependencyServiceInfo.version) == true) ? true : dependencyInfo.Value.version.VersionGreaterThan(dependencyServiceInfo.version);
                if (installable == true)
                {
                    if (dependencyInfo.Value.install == ServiceInstall.MANUAL)
                    {
                        callback(new ManagerError(ManagerErrorCode.INSTALL, string.Format(ManagerInfos.GetString(ManagerStrings.ERROR_MESSAGE_MANUAL_UPDATE_SERVICE), dependencyServiceName)));
                        return;
                    }
                    else
                    {
                        serviceDependencies.Add(dependencyServiceName);
                    }
                }
            }

            ToastKitManager.IsLock = true;
            ProcessService = service;

            downloader.Process(ProcessService, serviceDependencies,
                (error, downloadedList) =>
                {
                    if (error != null)
                    {
                        callback(error);
                        ProcessService = null;
                        ToastKitManager.IsLock = false;
                        return;
                    }
                    
                    EditorApplication.LockReloadAssemblies();
                    try
                    {
                        if (Directory.Exists(ManagerPaths.BACKUP_PATH) == true)
                        {
                            ToastKitFileUtil.DeleteDirectory(ManagerPaths.BACKUP_PATH);
                        }

                        foreach (var packageInfo in downloadedList)
                        {
                            string installPath = ToastKitPathUtil.Combine(Application.dataPath, packageInfo.packageIntallPath);
                            if (Directory.Exists(installPath) == true)
                            {
                                string backupPackagePath = ToastKitPathUtil.Combine(ManagerPaths.BACKUP_PATH, packageInfo.packageIntallPath);
                                string backupPackagePathMeta = string.Format("{0}.meta", backupPackagePath);
                                string installPathMeta = string.Format("{0}.meta", installPath);
                                ToastKitFileUtil.CopyDirectory(installPath, backupPackagePath, true);
                                ToastKitFileUtil.CopyFile(installPathMeta, backupPackagePathMeta);
                                ToastKitFileUtil.DeleteDirectory(installPath);
                                ToastKitFileUtil.DeleteFile(installPathMeta);
                            }

                            string packagePath = ManagerPaths.GetCachingPath(packageInfo.serviceName, packageInfo.serviceVersion, packageInfo.packagePath);

                            var responseCode = ToastKitCompress.ExtractUnityPackage(packagePath, ManagerPaths.TEMP_PATH, ManagerPaths.PROJECT_ROOT_PATH);
                            if (responseCode == CompressResultCode.SUCCESS)
                            {
                                AssetDatabase.Refresh();
                            }
                            else
                            {
                                ProcessService = null;
                                throw new Exception(string.Format("Unpack error. Code= {0}", responseCode));
                            }

                            ToastKitManager.Instance.Install.AddService(packageInfo.serviceName, packageInfo.serviceVersion);
                        }

                        ProcessService = null;

                        var refreshInfo = new UiRefreshInfo
                        {
                            lastServiceName = service.title
                        };

                        Directory.CreateDirectory(ManagerPaths.LIBRARY_PATH);
                        File.WriteAllText(ManagerPaths.TEMP_REFRESH_FILE_PATH, JsonUtility.ToJson(refreshInfo));

                        ToastKitManagerIndicator.SendInstall(service.title, service.version);

                        callback(null);
                    }
                    catch (Exception e)
                    {
                        ProcessService = null;
                        ToastKitManager.IsLock = false;
                        callback(new ManagerError(ManagerErrorCode.INSTALL, ManagerStrings.ERROR_MESSAGE_INSTALL_FAILED, e.Message));
                    }
                    EditorApplication.UnlockReloadAssemblies();
                });
        }

        public void Uninstall(ServiceInfo service, Action<ManagerError> callback)
        {
            if (IsProcessing == true)
            {
                callback(new ManagerError(ManagerErrorCode.UNINSTALL, ManagerStrings.ERROR_MESSAGE_ALREADY_INSTALL));
                return;
            }

            ToastKitManager.IsLock = true;
            ProcessService = service;

            EditorApplication.LockReloadAssemblies();
            try
            {
                bool deletedScriptFile = false;
                foreach (var package in service.packageList)
                {
                    if (string.IsNullOrEmpty(package.installPath) == true)
                    {
                        callback(new ManagerError(ManagerErrorCode.UNINSTALL, ManagerStrings.ERROR_MESSAGE_INSTALL_PATH_NOT_FOUND, package.installPath));
                        continue;
                    }

                    string path = ToastKitPathUtil.Combine(Application.dataPath, package.installPath);
                    string pathMeta = string.Format("{0}.meta", path);

                    if (Directory.Exists(path) == true)
                    {
                        if (deletedScriptFile == false)
                        {
                            deletedScriptFile = ToastKitFileUtil.IsScriptFile(path);
                        }

                        ToastKitFileUtil.DeleteDirectory(path);
                    }

                    if (File.Exists(pathMeta) == true)
                    {
                        ToastKitFileUtil.DeleteFile(pathMeta);
                    }
                }

                if (deletedScriptFile == false)
                {
                    ToastKitManager.IsLock = false;
                }

                ToastKitManager.Instance.Install.RemoveService(service);

                var refreshInfo = new UiRefreshInfo
                {
                    lastServiceName = service.title
                };

                Directory.CreateDirectory(ManagerPaths.LIBRARY_PATH);
                File.WriteAllText(ManagerPaths.TEMP_REFRESH_FILE_PATH, JsonUtility.ToJson(refreshInfo));
                AssetDatabase.Refresh();

                ProcessService = null;

                ToastKitManagerIndicator.SendRemove(service.title, service.version);

                callback(null);
            }
            catch (Exception e)
            {
                ProcessService = null;
                ToastKitManager.IsLock = false;
                callback(new ManagerError(ManagerErrorCode.UNINSTALL, ManagerStrings.ERROR_MESSAGE_REMOVE_FAILED, e.Message));
            }
            EditorApplication.UnlockReloadAssemblies();
        }
    }
}