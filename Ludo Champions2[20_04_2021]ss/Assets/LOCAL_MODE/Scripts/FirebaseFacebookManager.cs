using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using Firebase.Auth;
using UnityEngine.UI;
using System;
using System.IO;
using Google;
namespace offlineplay
{
    public class FirebaseFacebookManager : MonoBehaviour
    {
        public LoginManager loginmanager;

        private FirebaseAuth auth;
        PhoneAuthProvider provider;
        private bool isLoginSuccess = false;
        private string username;

        public string webClientId = "<your client id here>";
        private GoogleSignInConfiguration configuration;

        private void Awake()
        {
            configuration = new GoogleSignInConfiguration
            {
                WebClientId = webClientId,
                RequestIdToken = true
            };

            if (!FB.IsInitialized)
                FB.Init();
            else
                FB.ActivateApp();

            auth = FirebaseAuth.DefaultInstance;
        }
        public void CheckFBStatus()
        {
            if (FB.IsInitialized)
            {
                FB.LogOut();
            }
        }
        public void FacebooklogIn()
        {
            CheckFBStatus();
            loginmanager.loading.SetActive(true);
            FB.LogInWithReadPermissions(callback: OnLogIn);
        }

        private void OnLogIn(ILoginResult result)
        {
            if (FB.IsLoggedIn)
            {
                AccessToken tocken = result.AccessToken;

                Credential credential = FacebookAuthProvider.GetCredential(tocken.TokenString);
                accessToken(credential);
            }
            else
            {

                loginmanager.loading.SetActive(false);
            }
        }

        public void accessToken(Credential credential)
        {
            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    loginmanager.loading.SetActive(false);
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    loginmanager.loading.SetActive(false);
                    return;
                }

                FirebaseUser newUser = task.Result;
                Debug.Log(newUser.Email);
                username = newUser.DisplayName;
                isLoginSuccess = true;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            });
            StartCoroutine(CheckLoginResult());
        }
        IEnumerator CheckLoginResult()
        {
            while (true)
            {
                if (isLoginSuccess)
                {
                    GameManager.Instance.UserName = username;
                    GameManager.Instance._Login_Mode = LOGIN_MODE.facebook;
                    PlayerPrefs.SetString("USERNAME", GameManager.Instance.UserName);
                    PlayerPrefs.SetInt("LOGIN", (int)LOGIN_MODE.facebook);
                    loginmanager.CheckValid();
                    isLoginSuccess = false;
                }
                yield return null;
            }
        }

        public void Logout()
        {
            print("FB Logout");
            GameManager.Instance.IwannaFacebookLogin = false;
            GameManager.Instance.UserName = "";

            PlayerPrefs.DeleteKey("USERNAME");
            PlayerPrefs.DeleteKey("LOGIN");
            FB.LogOut();

        }
        public void GoogleLogout()
        {
            Debug.Log("signout");

            GoogleSignIn.DefaultInstance.SignOut();

            GameManager.Instance.UserName = "";
            GameManager.Instance._Login_Mode = LOGIN_MODE.guest;
            GameManager.Instance.AvatarImage = Resources.Load("Avatar1") as Texture;
            GameManager.Instance.AvatarURL = "";
            PlayerPrefs.DeleteKey("USERNAME");
            PlayerPrefs.DeleteKey("LOGIN");
        }

        public UILabel MyText;
        string verificationId = "";

        private void Start()
        {
            provider = PhoneAuthProvider.GetInstance(auth);
        }
        public void OpenPhoneLogin()
        {
            phoneloginPanel.SetActive(true);
            phonenumberPanel.SetActive(true);
            phonenumberPanel.transform.Find("Input_phone").GetComponent<UIInput>().value = "";
            verificodePanel.SetActive(false);
            verificodePanel.transform.Find("Input_Code").GetComponent<UIInput>().value = "";
            verificationId = "";
        }
        public void LoginPhone(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) || !phoneNumber.Contains("+"))
                return;

            phonenumberPanel.SetActive(false);
            verificodePanel.SetActive(true);


            uint phoneAuthTimeoutMs = 60000;
            provider.VerifyPhoneNumber(phoneNumber, phoneAuthTimeoutMs, null,
            verificationCompleted: (credential) =>
            {

                MyText.text += "verfication completed";
                auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("SignInWithCredentialAsync encountered an error: " +
                                       task.Exception);
                        return;
                    }

                });
            },
            verificationFailed: (error) =>
            {
                MyText.text += "error";
            },
            codeSent: (id, token) =>
            {
                MyText.text += "SMS Has been sent " + id;
                verificationId = id;
            },
            codeAutoRetrievalTimeOut: (id) =>
            {
                MyText.text += "timeout";
            });
        }

        public void VerficationEnter(string verificationCode)
        {
            if (string.IsNullOrEmpty(verificationId) && string.IsNullOrEmpty(verificationId))
                return;
            Credential credential = provider.GetCredential(verificationId, verificationCode);
            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("SignInWithCredentialAsync encountered an error: " +
                                   task.Exception);
                    return;
                }

                FirebaseUser newUser = task.Result;

                GameManager.Instance.UserName = newUser.PhoneNumber;
                GameManager.Instance._Login_Mode = LOGIN_MODE.phone;
                PlayerPrefs.SetString("USERNAME", GameManager.Instance.UserName);
                PlayerPrefs.SetInt("LOGIN", (int)LOGIN_MODE.phone);
                loginmanager.CheckValid();
            });
        }
        public GameObject phoneloginPanel;
        public GameObject phonenumberPanel;
        public GameObject verificodePanel;
        public UILabel Seconds;

        IEnumerator TimeoutCount()
        {
            Seconds.text = "60s";
            int i = 60;
            while (i > 0)
            {
                i--;
                Seconds.text = i.ToString() + "s";
                yield return new WaitForSeconds(1.0f);
            }
        }
        public void CloseVerifyWindow()
        {
            verificodePanel.SetActive(false);
            phonenumberPanel.SetActive(true);
        }
        public void ClosePhonenumberWindow()
        {
            phoneloginPanel.SetActive(false);
        }
    }
}