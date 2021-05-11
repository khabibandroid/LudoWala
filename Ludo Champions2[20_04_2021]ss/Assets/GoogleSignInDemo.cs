using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.UI;

public class GoogleSignInDemo : MonoBehaviour
{
    //public Text infoText;
    public string webClientId = "<your client id here>";

    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;


    public bool LoginSucces = false;

    public bool LoginFailed = false;

    //StartCoroutine(GameSceneManager._instance.LoadLoginScene(2, 1));

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        CheckFirebaseDependencies();
    }

    private void Update()
    {
        if (LoginSucces)
        {
            print("bool value of login succes is " + LoginSucces);
            //StartCoroutine(GameSceneManager._instance.LoadLoginScene(2, 1));
            PlayerPrefs.SetString("LoginType", "Google");
            LoginSucces = false;

        }

        if(LoginFailed)
        {
           // MenuSystem._instance.SetWarningMsg("Something went wrong!");
           // MenuSystem._instance.SetLoader(false);
            LoginFailed = false;
        }
    }


    public void SignInWithGoogle()
    {
        OnSignIn();
     //   MenuSystem._instance.SetLoader(true);
    }
    public void SignOutFromGoogle() { OnSignOut(); }


    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                    auth = FirebaseAuth.DefaultInstance;
                else
                    AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }


    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void OnSignOut()
    {
        AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
        AddToInformation("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    AddToInformation("Got Error: " + error.Status + " " + error.Message);
                    LoginFailed = true;
                    //MenuSystem._instance.SetWarningMsg(error.Message);
                }
                else
                {
                    AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                    LoginFailed = true;
                }
            }
           

        }
        else if (task.IsCanceled)
        {
            AddToInformation("Canceled");
            LoginFailed = true;
        }
        else
        {
            //StartCoroutine(GameSceneManager._instance.LoadLoginScene(2, 1));
            AddToInformation("Welcome: " + task.Result.DisplayName + "!");
            AddToInformation("Email = " + task.Result.Email);
            AddToInformation("Google ID Token = " + task.Result.IdToken);
            AddToInformation("Email = " + task.Result.Email);
            //UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            SaveUserDetails(task);
            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }

    private void SaveUserDetails(Task<GoogleSignInUser> task)
    {
        Debug.LogFormat("name : {0} email {1} imageurl {2}", task.Result.DisplayName, task.Result.Email, task.Result.ImageUrl);

    }

    private void SignInWithGoogleOnFirebase(string idToken)
    {


        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
           
            AggregateException ex = task.Exception;
            if (ex != null)
            {

                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                {

                    LoginFailed = true;
                }
                //AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                LoginSucces = true;
                AddToInformation("Sign In Successful.");
            }
        });
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        AddToInformation("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void AddToInformation(string str)
    {
        Debug.Log(str);

        //
       // MenuSystem._instance.SetLoader(false);
        //infoText.text += "\n" + str;
    }
}