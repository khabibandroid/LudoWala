// <copyright file="SigninSampleScript.cs" company="Google Inc.">
// Copyright (C) 2017 Google Inc. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations
using offlineplay;
namespace SignInSample {
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using Google;
  using UnityEngine;
  using UnityEngine.UI;
    using UnityEngine.SceneManagement;
    using System.Collections;

    public class SigninSampleScript : MonoBehaviour {

    public Text statusText;

    public string webClientId = "683110527606-l4lvhn6i2vo4n3ltq0rbhlqadunhuj5m.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
    void Awake() {
      configuration = new GoogleSignInConfiguration {
            WebClientId = webClientId,
            RequestIdToken = true
      };
    }
        private void Start()
        {
            if (offlineplay.GameManager.Instance.isLogin == false)
                OnSignIn();
            else {
                if (offlineplay.GameManager.Instance.IwannaGoogleLogin == true)
                    OnSignIn();
                else
                {
                    OnSignOut();
                }
            }

        }
        public void OnSignIn() {
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;
            AddStatusText("Calling SignIn");
            GoogleSignIn.DefaultInstance.SignOut();
            StartCoroutine(abab());
      }
        IEnumerator abab()
        {
            yield return new WaitForSeconds(0.2f);
            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
        }

    public void OnSignOut() {
            Debug.Log("signout");
      AddStatusText("Calling SignOut");
      GoogleSignIn.DefaultInstance.SignOut();

            offlineplay.GameManager.Instance.UserName = "";
            offlineplay.GameManager.Instance.isLogin = false;
            offlineplay.GameManager.Instance._Login_Mode = LOGIN_MODE.guest;
            offlineplay.GameManager.Instance.AvatarImage = Resources.Load("Avatar1") as Texture;
            offlineplay.GameManager.Instance.AvatarURL = "";
            PlayerPrefs.DeleteKey("USERNAME");
            PlayerPrefs.DeleteKey("LOGIN");

            SceneManager.LoadScene("MenuScene");
        }

    public void OnDisconnect() {
      AddStatusText("Calling Disconnect");
      GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task) {
      Debug.Log("task.IsFaulted "+task.IsFaulted+ " task.IsCanceled" +task.IsCanceled);
      if (task.IsFaulted) {
      Debug.Log("task.IsFaulted ");

        using (IEnumerator<System.Exception> enumerator =
                task.Exception.InnerExceptions.GetEnumerator()) {
          if (enumerator.MoveNext()) {
            GoogleSignIn.SignInException error =
                    (GoogleSignIn.SignInException)enumerator.Current;
            AddStatusText("Got Error: " + error.Status + " " + error.Message);
                        offlineplay.GameManager.Instance.IwannaGoogleLogin = false;
                        SceneManager.LoadScene("MenuScene");
                    } else {
            AddStatusText("Got Unexpected Exception?!?" + task.Exception);
                        offlineplay.GameManager.Instance.IwannaGoogleLogin = false;
                        SceneManager.LoadScene("MenuScene");
                    }
        }
      } else if(task.IsCanceled) {
      Debug.Log("task.IsCanceled");

        AddStatusText("Canceled");
                offlineplay.GameManager.Instance.IwannaGoogleLogin = false;
                SceneManager.LoadScene("MenuScene");
            } else  {
      Debug.Log("task.success ");

        AddStatusText("Welcome: " + task.Result.DisplayName + "!");
                offlineplay.GameManager.Instance.UserName = task.Result.DisplayName;
                offlineplay.GameManager.Instance._Login_Mode = LOGIN_MODE.google;
                PlayerPrefs.SetString("USERNAME", offlineplay.GameManager.Instance.UserName);
                PlayerPrefs.SetInt("LOGIN", (int)LOGIN_MODE.google);
                offlineplay.GameManager.Instance.IwannaGoogleLogin = true;
                offlineplay.GameManager.Instance.isLogin = false;
                SceneManager.LoadScene("MenuScene"); 
      }
    }
       
    public void OnSignInSilently() {
      GoogleSignIn.Configuration = configuration;
      GoogleSignIn.Configuration.UseGameSignIn = false;
      GoogleSignIn.Configuration.RequestIdToken = true;
      AddStatusText("Calling SignIn Silently");

      GoogleSignIn.DefaultInstance.SignInSilently()
            .ContinueWith(OnAuthenticationFinished);
    }


    public void OnGamesSignIn() {
      GoogleSignIn.Configuration = configuration;
      GoogleSignIn.Configuration.UseGameSignIn = true;
      GoogleSignIn.Configuration.RequestIdToken = false;

      AddStatusText("Calling Games SignIn");

      GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
        OnAuthenticationFinished);
    }

    private List<string> messages = new List<string>();
    void AddStatusText(string text) {
      //if (messages.Count == 5) {
      //  messages.RemoveAt(0);
      //}
      //messages.Add(text);
      //string txt = "";
      //foreach (string s in messages) {
      //  txt += "\n" + s;
      //}
      statusText.text += text;
      Debug.Log(statusText.text);
    }
  }
}
