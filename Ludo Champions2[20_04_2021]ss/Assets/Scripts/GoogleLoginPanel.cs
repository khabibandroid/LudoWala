using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Google;
using Firebase;
using System.Threading.Tasks;
using System;

public class GoogleLoginPanel : MonoBehaviour
{
    [SerializeField]
    private InputField idTokenField;

    [SerializeField]
    private InputField assesTokenField;

    [SerializeField]
    private Text messageText;


    [SerializeField]
    private InputField EmailInputField;

    public bool LoginSucces = false;

    public bool LoginFailed = false;

    private FirebaseAuth firebaseAuth;



    public string webClientId = "<your client id here>";

   
    private GoogleSignInConfiguration configuration;


    public GameObject waitscreen;

    private void Awake() 
    {
       
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        firebaseAuth = FirebaseAuth.DefaultInstance;
        
    }
   

    public void SignInWithGoogle()
    {
        //if(EmailInputField.text != string.Empty || EmailInputField.text != null)
            OnSignIn();
        
    }
    private void OnSignIn()
    {

        waitscreen.SetActive(true);
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        //AddToInformation("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
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
                    waitscreen.SetActive(false);
                    //MenuSystem._instance.SetWarningMsg(error.Message);
                }
                else
                {
                    AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                    LoginFailed = true;
                    waitscreen.SetActive(false);
                }
            }


        }
        else if (task.IsCanceled)
        {
            AddToInformation("Canceled");
            LoginFailed = true;
            waitscreen.SetActive(false);
        }
        else
        {
            //StartCoroutine(GameSceneManager._instance.LoadLoginScene(2, 1));
            AddToInformation("Welcome: " + task.Result.DisplayName + "!");
            AddToInformation("Email = " + task.Result.Email);
            PlayerPrefs.SetString("userEmail", task.Result.Email);
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
        PlayerPrefs.SetString("userEmail",task.Result.Email);
    }


    private void SignInWithGoogleOnFirebase(string idToken)
    {


        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        firebaseAuth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {

            AggregateException ex = task.Exception;
            if (ex != null)
            {

                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                {

                    waitscreen.SetActive(false);
                    LoginFailed = true;
                }
                //AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                LoginSucces = true;
                AddToInformation("Sign In Successful.");
                
                GameManager.Instance.login.LOgin(3);
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


    public void LoginButton()
    {
        Login(idTokenField.text, assesTokenField.text);
        //GoogleAuthProvider googleAuthProvider = GoogleAuthProvider.;
    }

    private void Login(string idToken, string assesToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, assesToken);

        firebaseAuth.SignInWithCredentialAsync(credential).ContinueWith(task => {

            if (task.IsCanceled)
            {
                messageText.text = "Sign is canceled";

                Debug.LogError("Sign is canceled");
                return;
            }

            if (task.IsFaulted)
            {
                messageText.text = "Sign is faulted";

                Debug.LogError("Sign is faulted");
                return;
            }

            FirebaseUser user = task.Result;

            messageText.text = "Sign is successfully !";
            Debug.Log("Sign is successfully");
        });
    }
}
