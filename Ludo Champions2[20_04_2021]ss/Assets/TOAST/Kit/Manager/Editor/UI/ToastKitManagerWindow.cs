using System;
using Toast.Kit.Common.Log;
using Toast.Kit.Common.Multilanguage;
using Toast.Kit.Manager.Ad;
using Toast.Kit.Manager.Constant;
using Toast.Kit.Manager.Internal;
using Toast.Kit.Manager.Ui.Helper;
using UnityEditor;
using UnityEngine;

namespace Toast.Kit.Manager.Ui
{
    internal class ToastKitManagerWindow : EditorWindow
    {
        public static ToastKitManagerWindow window;

        private ToastKitServiceList serviceList;
        private ToastKitServiceDetail serviceDetail;

        private string[] languages;
        private int selectedLanguageIndex;

        private bool openedImageViewer;
        private bool openedInfoViewer;


        public static void OpenWindow()
        {
            LanguageLoad(() =>
            {
                window = GetWindow<ToastKitManagerWindow>();
            }, false);
        }

        private static void LanguageLoad(Action callback, bool opened = true)
        {
            ToastKitMultilanguage.Load(
                ManagerInfos.SERVICE_NAME,
                ManagerPaths.LANGUAGE_FILE_PATH,
                (result, resultMsg) =>
                {
                    if (result != MultilanguageResultCode.SUCCESS && result != MultilanguageResultCode.ALREADY_LOADED)
                    {
                        ToastKitLogger.Error(string.Format("Language load failed. (type= {0})", result), ManagerInfos.SERVICE_NAME, typeof(ToastKitManagerWindow));
                        return;
                    }

                    callback();
                });
        }

        private void OnEnable()
        {
            minSize = new Vector2(800, 380);
            Initialize();
        }

        private void OnDestroy()
        {
            ToastKitManager.Instance.Clear();
        }

        private void Initialize()
        {
            titleContent = ManagerUi.GetContent(ManagerStrings.WINDOW_TITLE);

            EditorApplication.playModeStateChanged -= OnPlaymodeChanged;
            EditorApplication.playModeStateChanged += OnPlaymodeChanged;

            serviceList = new ToastKitServiceList(this);
            serviceDetail = new ToastKitServiceDetail(this);

            ToastKitManager.Instance.Initialize(OnErrorCallback, this,
                new Rect(0, 0, ManagerUiDefine.LEFT_FRAME_WIDTH, ManagerUiDefine.AD_FRAME_HEIGHT));

            if (ToastKitMultilanguage.IsLoadService(ManagerInfos.SERVICE_NAME) == true)
            {
                languages = ToastKitMultilanguage.GetSupportLanguages(ManagerInfos.SERVICE_NAME, true);
                if (languages != null)
                {
                    string lastLanguage = ManagerInfos.LastLanguage;
                    if (string.IsNullOrEmpty(lastLanguage) == false)
                    {
                        ToastKitManager.Instance.ChangeLanguageCode(ManagerInfos.LastLanguage);
                    }

                    selectedLanguageIndex = GetSelectLanguageIndex(ToastKitMultilanguage.GetSelectLanguage(ManagerInfos.SERVICE_NAME, true));
                }
                else
                {
                    languages = new[] { ManagerUiDefine.EMPTY_LANGUAGES_VALUE };
                    selectedLanguageIndex = 0;
                }
            }
            else
            {
                languages = new[] { ManagerUiDefine.EMPTY_LANGUAGES_VALUE };
                selectedLanguageIndex = 0;

                Reload();
            }
        }

        private void OnPlaymodeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                Reload();
            }
        }

        private void Reload()
        {
            LanguageLoad(() =>
            {
                Initialize();
                Repaint();
            });
        }

        private void OnGUI()
        {
            DoToolbarGUI();

            if (ToastKitManager.Instance.Initialized == false)
            {
                GUILayout.BeginArea(new Rect(position.width * 0.5f - 100, (position.height - ManagerUiDefine.COPYRIGHT_HEIGHT) * 0.5f - 50, 400, 100));
                EditorGUILayout.BeginHorizontal();
                {
                    ManagerUi.LabelValue(ManagerUiIcon.StatusWheel);
                    ManagerUi.Label(ManagerStrings.SERVICE_LIST_LOADING, ManagerUiStyle.IconLabel);
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
            else
            {
                DoManagerGUI();
            }

            DoCopyrightGUI();

            if (Event.current.type == EventType.ExecuteCommand)
            {
                switch (Event.current.commandName)
                {
                    case ManagerEvents.IMAGE_SELECT:
                        {
                            openedImageViewer = true;
                            Repaint();
                            break;
                        }
                    case ManagerEvents.CHANGE_SERVICE:
                        {
                            serviceDetail.LoadService(serviceList.SelectedService);
                            break;
                        }
                    case ManagerEvents.INFO_RESET:
                        {
                            serviceList.Clear();
                            serviceDetail.Clear();
                            break;
                        }
                }
            }

            if (Event.current.type == EventType.MouseDown)
            {
                openedImageViewer = false;
                openedInfoViewer = false;
                Repaint();
            }
        }

        private void Update()
        {
            serviceList.Update();
            serviceDetail.Update();

            if (EditorUtility.scriptCompilationFailed == true)
            {
                ToastKitManager.IsLock = false;
            }
        }

        private void DoToolbarGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, position.width, ManagerUiDefine.TOOLBAR_HEIGHT));
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                ManagerUi.Label(ManagerStrings.SERVICE_LIST);

                EditorGUI.BeginChangeCheck();
                {
                    selectedLanguageIndex = ManagerUi.PopupValue(selectedLanguageIndex, languages, ManagerUiStyle.ToolbarPopup, GUILayout.Width(100));
                }
                if (EditorGUI.EndChangeCheck() == true)
                {
                    ToastKitManager.Instance.ChangeLanguageCode(GetSelectLanguageCode());
                }

                if (ManagerUi.InfoButton() == true)
                {
                    openedInfoViewer = true;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            ManagerUi.DrawVerticalSplitter(0, ManagerUiDefine.TOOLBAR_HEIGHT, position.width);
        }

        private void DoManagerGUI()
        {
            EditorGUI.BeginDisabledGroup(ToastKitManager.IsLock);

            EditorGUI.BeginDisabledGroup(openedImageViewer || openedInfoViewer);
            Rect contentRect = new Rect(0, ManagerUiDefine.TOOLBAR_HEIGHT, position.width, position.height - ManagerUiDefine.TOOLBAR_HEIGHT - ManagerUiDefine.COPYRIGHT_HEIGHT);
            GUILayout.BeginArea(contentRect);
            {
                Rect listRect = new Rect(0, 0, ManagerUiDefine.LEFT_FRAME_WIDTH, position.height - ManagerUiDefine.AD_FRAME_HEIGHT);
                GUILayout.BeginArea(listRect);
                {
                    serviceList.OnGUI(listRect);
                }
                GUILayout.EndArea();

                float adY = contentRect.height - ManagerUiDefine.AD_FRAME_HEIGHT;
                ManagerUi.DrawVerticalSplitter(0, adY, ManagerUiDefine.LEFT_FRAME_WIDTH);

                Rect adRect = new Rect(0, adY, ManagerUiDefine.LEFT_FRAME_WIDTH, ManagerUiDefine.AD_FRAME_HEIGHT);
                GUILayout.BeginArea(adRect);
                {
                    Advertisement.Draw();
                }
                GUILayout.EndArea();

                ManagerUi.DrawHorizontalSplitter(ManagerUiDefine.LEFT_FRAME_WIDTH, 0, position.height);

                Rect detailRect = new Rect(
                    ManagerUiDefine.LEFT_FRAME_WIDTH + ManagerUiDefine.DETAIL_MARGIN,
                    0,
                    position.width - ManagerUiDefine.LEFT_FRAME_WIDTH - ManagerUiDefine.DETAIL_MARGIN,
                    position.height);
                GUILayout.BeginArea(detailRect);
                {
                    if (serviceDetail != null)
                    {
                        serviceDetail.OnGUI(detailRect);
                    }
                }
                GUILayout.EndArea();
            }
            GUILayout.EndArea();
            EditorGUI.EndDisabledGroup();

            if (openedImageViewer == true)
            {
                GUI.DrawTexture(new Rect(0, 0, position.width, position.height), ManagerUiStyle.OverlayTexture);

                if (serviceDetail.SelectedImageInfo != null)
                {
                    GUILayout.BeginArea(contentRect);
                    DoImageViewerGUI(contentRect);
                    GUILayout.EndArea();
                }
            }

            if (openedInfoViewer == true)
            {
                GUILayout.BeginArea(contentRect);
                {
                    GUI.DrawTexture(new Rect(0, 0, position.width, position.height), ManagerUiStyle.OverlayTexture);

                    BeginWindows();
                    {
                        ManagerUi.Window(
                            1,
                            new Rect(
                                (contentRect.width * 0.5f) - (ManagerUiDefine.INFO_WINDOW_WIDTH * 0.5f),
                                (contentRect.height * 0.5f) - (ManagerUiDefine.INFO_WINDOW_HEIGHT * 0.5f),
                                ManagerUiDefine.INFO_WINDOW_WIDTH,
                                ManagerUiDefine.INFO_WINDOW_HEIGHT),
                            DoInformationGUI,
                            ManagerStrings.INFO_TITLE);
                    }
                    EndWindows();
                }
                GUILayout.EndArea();
            }

            EditorGUI.EndDisabledGroup();
        }

        private void DoCopyrightGUI()
        {
            GUILayout.BeginArea(new Rect(0, position.height - ManagerUiDefine.COPYRIGHT_HEIGHT, position.width, ManagerUiDefine.COPYRIGHT_HEIGHT));
            EditorGUILayout.BeginVertical(ManagerUiStyle.CopyrightBox);
            {
                ManagerUi.Label(ManagerStrings.COPYRIGHT, ManagerUiStyle.CopyrightLabel);
            }
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private double printCopyCompleteTime;

        private void DoInformationGUI(int unusedWindowId)
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    ManagerUi.Label(ManagerStrings.VERSION, GUILayout.Width(ManagerUiDefine.INFO_SUBJECT_WIDTH));

                    EditorGUILayout.BeginVertical();
                    {
                        ManagerUi.LabelValue(ToastKitManagerVersion.VERSION);

                        if (ManagerUi.LabelButton(ManagerInfos.GetString(ManagerStrings.INFO_RELEASE_NOTES)) == true)
                        {
                            Application.OpenURL(ManagerPaths.RELEASE_NOTES_URI);
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                {
                    ManagerUi.Label(ManagerStrings.INFO_SUPPORT_MAIL, GUILayout.Width(ManagerUiDefine.INFO_SUBJECT_WIDTH));
                    ManagerUi.LabelValue(ManagerStrings.SUPPORT_MAIL);

                    if (ManagerUi.Button(ManagerStrings.INFO_SUPPORT_MAIL_COPY, EditorStyles.miniButton) == true)
                    {
                        GUIUtility.systemCopyBuffer = ManagerStrings.SUPPORT_MAIL;

                        printCopyCompleteTime = EditorApplication.timeSinceStartup + ManagerUiDefine.TOAST_TIMER;
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (printCopyCompleteTime > EditorApplication.timeSinceStartup)
                {
                    ManagerUi.Label(ManagerStrings.INFO_SUPPORT_MAIL_COPY_COMPLETED, ManagerUiStyle.ToastLabel);
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DoImageViewerGUI(Rect contentRect)
        {
            Texture2D texture = serviceDetail.GetImage();

            float imageWidth = 0;
            float imageHeight = 0;

            if (texture.height > contentRect.height)
            {
                imageHeight = contentRect.height - ManagerUiDefine.IMAGE_VIEWER_MIN_HEIGHT_PADDING;
                imageWidth = texture.width * (imageHeight / texture.height);
            }
            else
            {
                imageWidth = texture.width;
                imageHeight = texture.height;
            }

            Rect imageRect = new Rect((position.width - imageWidth) * 0.5f, (position.height - imageHeight) * 0.5f, imageWidth, imageHeight);
            Rect textRect = new Rect(imageRect.x, imageRect.y - 30f, imageWidth, 100);

            EditorGUI.DrawPreviewTexture(imageRect, texture);
            ManagerUi.LabelValue(textRect, serviceDetail.SelectedImageInfo.title, ManagerUiStyle.ImageTitleLabel);
        }

        private string GetSelectLanguageCode()
        {
            if (selectedLanguageIndex >= languages.Length)
            {
                return string.Empty;
            }

            return languages[selectedLanguageIndex];
        }

        private int GetSelectLanguageIndex(string languageCode)
        {
            for (int i = 0; i < languages.Length; i++)
            {
                if (languages[i].Equals(languageCode) == true)
                {
                    return i;
                }
            }

            return ManagerUiDefine.LANGUAGE_NOT_FOUND;
        }

        private void OnErrorCallback(ManagerError error)
        {
            ManagerUi.ErrorDialog(error);
        }
    }
}