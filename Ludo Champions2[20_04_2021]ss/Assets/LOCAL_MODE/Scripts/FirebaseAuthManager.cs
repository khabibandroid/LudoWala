﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Google;
using System.Threading.Tasks;

public class FirebaseAuthManager : MonoBehaviour
{
    public UILabel logText;
    FirebaseAuth auth;
    FirebaseUser user;
    public UILabel email;
    public UILabel password;


    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        InitializeFirebase();
    }

    #region Email Login
    void InitializeFirebase()
    {
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }
    public void LoginWithEmail() 
    {
        if (string.IsNullOrEmpty(email.text) || string.IsNullOrEmpty(password.text))
        {
            logText.text = "Please input email or password";
            return;
        }
        DebugLog(email.text + "/" + password.text);
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }


            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.Log(string.Format("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId));

        });
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                Debug.Log("displayName = " + user.DisplayName ?? "");
                Debug.Log("emailAddress = " + user.Email ?? "");
                Debug.Log("photoUrl = " + user.PhotoUrl ?? "");
            }
        }
    }

    private void SendVerifyEmail()
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            user.SendEmailVerificationAsync().ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SendEmailVerificationAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("Email sent successfully.");
            });
        }
    }
    #endregion

    #region Google Login
    public void LoginWithGoogle()
    {
        if (GoogleSignIn.Configuration == null)
        {
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
               
                WebClientId = "1075626014473-hjh3l3gmja01n1sass415mk1r2u6a5p7.apps.googleusercontent.com"
            };
            GoogleSignIn.Configuration.UseGameSignIn = false;
        }

        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();
        signIn.ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                signInCompleted.SetCanceled();
            }
            else if (task.IsFaulted)
            {
                signInCompleted.SetException(task.Exception);
            }
            else
            {

                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                DebugLog(((Task<GoogleSignInUser>)task).Result.DisplayName);
                auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
                {
                    if (authTask.IsCanceled)
                    {
                        signInCompleted.SetCanceled();
                    }
                    else if (authTask.IsFaulted)
                    {
                        signInCompleted.SetException(authTask.Exception);
                    }
                    else
                    {
                        signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
                    }
                });
            }
        });
    }
    #endregion

    #region Extra Methods
    public void OnClearLog()
    {
        logText.text = string.Empty;
    }

    public void DebugLog(string log)
    {
        logText.text += log + '\n';
    }
    #endregion

    public void OnSignOut()
    {
        DebugLog("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
        DebugLog("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }
}
