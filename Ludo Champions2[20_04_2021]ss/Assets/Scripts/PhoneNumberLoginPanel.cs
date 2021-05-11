using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;


public class PhoneNumberLoginPanel : MonoBehaviour
{
    private event PhoneAuthProvider.CodeSent codeSent;
    private event PhoneAuthProvider.VerificationCompleted verificationCompleted;
    private event PhoneAuthProvider.VerificationFailed verificationFailed;

    [SerializeField]
    private RectTransform numberPanel;

    [SerializeField]
    private RectTransform passwordPanel;

    [Space]

    [SerializeField]
    private InputField phoneNumberField;

    [SerializeField]
    private InputField passwordField;

    [Space]

    [SerializeField]
    private uint timeoutDurationMs = 1000;

    [SerializeField]
    private string textPhoneNumber;

    private FirebaseAuth firebaseAuth;

    private PhoneAuthProvider phoneAuthProvider;
    private ForceResendingToken forceResendingToken;

    [SerializeField]
    private string verificationId;

    private string lastPhoneNumber = "null";

    public GameObject PhoneLoginScreen;

    public GameObject EnterPhoneNumberScreen;
    public GameObject EnterPasswordScreen;

    public GameObject waitScreen;

    public LoadingScreen loadingScreenManager;

    string enteredPhoneNumber;
    private void Awake() 
    {
        codeSent += CodeBeenSent;
        verificationCompleted += VerificationBeenCompleted;
        verificationFailed += VerificationBeenFailed;

        firebaseAuth = FirebaseAuth.GetAuth(FirebaseApp.Create());//Firebase.Auth.FirebaseAuth.DefaultInstance;//
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            SendMessageToPhoneNumber(textPhoneNumber);
    }

    public void SendMessageButton()
    {
        string phoneNumber = phoneNumberField.text;
        
        if (CheckPhoneNumberText(phoneNumber))
        {
            waitScreen.SetActive(true);
            print(phoneNumber);
            if (phoneNumber != lastPhoneNumber)
                forceResendingToken = null;

            lastPhoneNumber = phoneNumber;

            phoneAuthProvider = SendMessageToPhoneNumber("+91" +phoneNumber);
            enteredPhoneNumber = phoneNumber;
        }
    }

    public void EnterPasswordButton() 
    {
        waitScreen.SetActive(true);
        if (phoneAuthProvider != null)
        {
            Credential credential = phoneAuthProvider.GetCredential(verificationId, passwordField.text);

            firebaseAuth.SignInWithCredentialAsync(credential).ContinueWith(task => 
            {
                if (task.IsFaulted)
                {
                    waitScreen.SetActive(false);
                    Debug.LogError(task.Exception);
                    return;
                }
                // loadingScreenManager.LoadLevel();
                // loadingScreenManager.InitLogin = true;

                FirebaseUser user = task.Result;
                
                waitScreen.SetActive(true);
                PhoneLoginScreen.SetActive(false);
                PlayerPrefs.SetString("phoneNumber",enteredPhoneNumber);
                GameManager.Instance.login.LOgin(2);
                Debug.Log("Signed successfully");



            });

            }
    }

    public void ResendPasswordButton() 
    {
        if (forceResendingToken != null)
            forceResendingToken.Dispose();
    }

    private PhoneAuthProvider SendMessageToPhoneNumber(string phoneNumber)
    {
        Debug.Log("SendMessageToPhoneNumber");
        waitScreen.SetActive(true);

        PhoneAuthProvider phoneAuthProvider = PhoneAuthProvider.GetInstance(firebaseAuth);

        phoneAuthProvider.VerifyPhoneNumber(
            phoneNumber,
            timeoutDurationMs,
            forceResendingToken,
            verificationCompleted,
            verificationFailed,
            codeSent);

        

        return phoneAuthProvider;
    }

    private bool CheckPhoneNumberText(string text) 
    {
        bool result = true;

        if (result)
            for (int i = 0; i < text.Length; i++)
                if(!System.Char.IsDigit(text[i]))
                {
                    result = false;
                    break;
                }

        return result;
    }

    private void CodeBeenSent(string verificationId, ForceResendingToken forceResendingToken) 
    {
        Debug.Log("code been send");

        Debug.Log(verificationId.ToString());

        this.forceResendingToken = forceResendingToken;
        this.verificationId = verificationId;

        EnterPhoneNumberScreen.SetActive(false);
        EnterPasswordScreen.SetActive(true);
        waitScreen.SetActive(false);

    }

    private void VerificationBeenCompleted(Credential credential) 
    {
        Debug.Log("Verification been completed");
        waitScreen.SetActive(false);
    }

    private void VerificationBeenFailed(string error)
    {
        waitScreen.SetActive(false);
        Debug.LogError("VerificationBeenFailed : " + error);
    }
}
