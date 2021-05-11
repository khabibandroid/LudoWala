using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;
using Firebase.Auth;

public class Login : MonoBehaviour
{
    private Sprite[] avatarSprites;
    public GameObject intro;
  
    public GameObject login2;
 
  //  public GameObject signupSuccessPanel;


  

    public GameObject loadingPanel;
   

    //public GameObject playFab;
  
    public string NAme;
    private string useridd;
    public string Email;
    public string Mobiles;

    public static Login instance;

    public GameObject Exitpanel;

    public AudioSource sound;


    public GameObject playFab;

    public static Login Instance()
    {
        return instance;

    }

    private void Awake()
    {
        if (intro != null)
            intro.SetActive(PlayerPrefs.GetInt("FirstLoad", 0) < 1);
    }


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        GameManager.Instance.login = this;
       // Login1();
        avatarSprites = GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>().avatars;
        //   GameObject.Find("BGSound").GetComponent<AudioSource>().volume = 1;

        //PlayerPrefs.DeleteAll();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           // sound.Play();
            Exitpanel.SetActive(true);

        }
            
    }

    public void YesExit()
    {

        Application.Quit();
    }

    void Login1()
    {
        if (intro.activeInHierarchy)
        {
            StartCoroutine(callLogin1());
        }
        else
        {
            EventCounter.LogOut = false;
            loadingPanel.gameObject.SetActive(true);
            login2.SetActive(true);
            intro.SetActive(false);
            loadingPanel.gameObject.SetActive(false);
        }
    }

    IEnumerator callLogin1()
    {
        
        yield return new WaitForSeconds(2.7f);
        loadingPanel.gameObject.SetActive(true);
        
   
        //if (PlayerPrefs.HasKey("user_ID"))
       // {
  //         Debug.Log("userId " + GameManager.Instance.UserID + " playerprefb " + PlayerPrefs.GetString("user_ID"));
//
        //    playFab.GetComponent<PlayFabManager>().PlayFabId = GameManager.Instance.UserID = PlayerPrefs.GetString("user_ID");
           

         //   StartCoroutine(User_Data());
         //   GameManager.Instance.playfabManager.PhotonLogin(GameManager.Instance.UserID);
//        }
     //   else
        {
            login2.SetActive(true);
            intro.SetActive(false);
            loadingPanel.gameObject.SetActive(false);
        }
      

        //        splashPanel.SetActive(false);

        //Do Function here...
    }


    IEnumerator LateCall()
    {

        yield return new WaitForSeconds(3);
        if (PlayerPrefs.GetInt("FirstLoad", 0) < 1)
        {
            intro.SetActive(true);
        }
        
       // splashPanel.SetActive(false);

        Login1();
        //Do Function here...
    }

    bool exit = true;
    IEnumerator ExitApp()
    {
        exit = !exit;
        yield return new WaitForSeconds(1.2f);
        if (exit)
            Application.Quit();
        else
            exit = true;
    }

  


    public IEnumerator loadImageOpponent(string url)
    {

        Debug.Log("Avatar URL  is " + url);
        WWW www = new WWW(url);

        yield return www;

        GameManager.Instance.avatarMy = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f), 32);

    }



    public void LOGIN1()
            {
               
                login2.SetActive(true);
               
            }

   

   


    public static Login getInstance()
    {

        return instance;

    }

    public void guestLogin()
    {

        StartCoroutine(Guest_login());
    }
 

    public void GuestLogin()
    {

        int sprite = int.Parse("1");

        int id = Random.Range(100000, 999999);
        GameManager.Instance.avatarMyUrl = "01";

        Debug.Log("Sprite " + GameManager.Instance.avatarMyUrl);
        GameManager.Instance.nameMy = "Guest";
        GameManager.Instance.UserID = id.ToString();
        GameManager.Instance.Balance = "20000";
        GameManager.Instance.totalBalance = 20000;
        loadingPanel.SetActive(true);
        guestLogin();

    }

    IEnumerator Guest_login()
    {
        string userId = GameManager.Instance.UserID;
        string name = GameManager.Instance.nameMy;
        string avatar = GameManager.Instance.avatarMyUrl;
        string Balance = GameManager.Instance.Balance;
        

        Debug.Log("user_Social " + userId);
        Debug.Log("name_Social " + name);
        Debug.Log("avatar_Social " + avatar);
        WWWForm form = new WWWForm();


        form.AddField("user_id", userId);
        form.AddField("name", name);
        form.AddField("total_winning", Balance);
        form.AddField("avatar_url", avatar);




        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?login/social_login", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //errorMsg.text = download.error;
            Debug.Log("Error downloading: " + www.error);
            loadingPanel.SetActive(false);
        }

        else
        {
            Debug.Log(www.downloadHandler.text);

            string output = www.downloadHandler.text;

            if (output == "null" || output == "")
            {
                Debug.Log("Resgister Failed");
                loadingPanel.SetActive(false);
            }
           

            else
            {
                var N = JSON.Parse(output);

                if (N["status"].Value == "1")

                {
                    var message = N["message"].Value;

                    GameManager.Instance.UserID = N["user_id"].Value;
                    Debug.Log("message is" + message);
                   
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse("01")];

                    GameManager.key = "login";
                    if (GameManager.key.Equals("login"))
                    {
                        PlayerPrefs.SetString("user_ID", GameManager.Instance.UserID);
                        playFab.GetComponent<PlayFabManager>().PlayFabId = GameManager.Instance.UserID;
                        GameManager.Instance.playfabManager.PhotonLogin(GameManager.Instance.UserID);

                        //  MainmenuManager.getInstance().profile.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;
                    }
                    else
                    {
                        login2.SetActive(true);
                    }
                    //GameManager.Instance.playfabManager.PhotonLogin(GameManager.Instance.facebookIDMy);

                }

                else
                {
                    Debug.Log("Resgister failed");
                    loadingPanel.SetActive(false);

                }

            }

        }


    }

    public void SocialLogin()
    {

        StartCoroutine(Social_login());
    }


    IEnumerator Social_login()
    {
        string userId = GameManager.Instance.UserID;
        string name = GameManager.Instance.nameMy;
        string avatar = GameManager.Instance.avatarMyUrl;
        string Balance = GameManager.Instance.Balance;

        Debug.Log("user_Social " + userId);
        Debug.Log("name_Social " + name);
        Debug.Log("avatar_Social " + avatar);
        WWWForm form = new WWWForm();

       
        form.AddField("user_id", userId);
        form.AddField("name", name);
        form.AddField("avatar_url", avatar);
        form.AddField("total_winning", Balance);


        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?login/social_login", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //errorMsg.text = download.error;
            Debug.Log("Error downloading: " + www.error);
            loadingPanel.SetActive(false);
        }

        else
        {
            Debug.Log(www.downloadHandler.text);

            string output = www.downloadHandler.text;

            if (output == "null" || output == "")
            {
                Debug.Log("Register Failed");
                loadingPanel.SetActive(false);
            }


            else
            {
                var N = JSON.Parse(output);

                if (N["status"].Value == "1")

                {
                    var message = N["message"].Value;
                   
                    GameManager.Instance.UserID = N["user_id"].Value;
                    Debug.Log("message is" + message);

                    GameManager.key = "login";
                    if (GameManager.key.Equals("login"))
                    {
                        PlayerPrefs.SetString("user_ID", GameManager.Instance.UserID);
                        playFab.GetComponent<PlayFabManager>().PlayFabId = GameManager.Instance.UserID;
                        GameManager.Instance.playfabManager.PhotonLogin(GameManager.Instance.UserID);
                    }
                    else
                    {
                        login2.SetActive(true);
                    }
                    //GameManager.Instance.playfabManager.PhotonLogin(GameManager.Instance.facebookIDMy);

                }

                else
                {
                    Debug.Log("Resgister failed");
                    loadingPanel.SetActive(false);

                }
            }

        }

    }

   public IEnumerator User_Data()
    {



        WWWForm form = new WWWForm();

        form.AddField("user_id", GameManager.Instance.UserID);
       

        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?api/user_data", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //errorMsg.text = download.error;
            Debug.Log("Error downloading: " + www.error);
            loadingPanel.SetActive(false);
        }
        else
        {



            Debug.Log(www.downloadHandler.text);

            string output = www.downloadHandler.text;





            var N = JSON.Parse(output);
            Debug.Log(N);
            if (N["status"].Value == "1")

            {


                string userIds = N["data"]["user_id"].Value;
                string userId = N["data"]["name"].Value;
                string Avatar = N["data"]["avatar"].Value;

                string emails = N["data"]["email"].Value;
                string Mobile = N["data"]["mobile_no"].Value;
                string refer = N["data"]["referal_code"].Value;
                string address = N["data"]["address"].Value;
                string city = N["data"]["city"].Value;
                string state = N["data"]["state"].Value;
                string pin_code = N["data"]["pin_code"].Value;
                string Total_balance = N["data"]["balance"].Value;
                string emailll = N["data"]["email"].Value;


                Debug.Log("user_id is " + userIds);
                //SceneManager.LoadScene("MenuScene");
                GameManager.Instance.emaill = emailll;

                GameManager.Instance.Balance = Total_balance;
                GameManager.Instance.UserID = userIds;
                GameManager.Instance.facebookIDMy = userIds;
                GameManager.Instance.nameMy = userId;
                GameManager.Instance.avatarMyUrl = Avatar;
                GameManager.Instance.mobile = Mobile;
                GameManager.Instance.referal = refer;
                GameManager.Instance.state = state;
                GameManager.Instance.pin_code = pin_code;
                GameManager.Instance.city = city;
                GameManager.Instance.address = address;




                Debug.Log("Avatar value from userdata function is : " + Avatar);

                GameManager.Instance.avatarIndex = Avatar;

                if (Avatar == "0")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[0];
                    //GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse("0")];

                }
                else if (Avatar == "01" || Avatar == "avatars_1 (UnityEngine.Sprite)" || Avatar == "1")
                {
                    //GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse("01")];
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[1];
                }
                else if (Avatar == "02" || Avatar == "2")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[02];

                }
                else if (Avatar == "03" || Avatar == "3")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[03];

                }
                else if (Avatar == "04" || Avatar == "4")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[04];

                }
                else if (Avatar == "05" || Avatar == "5")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[05];

                }
                else if (Avatar == "06" || Avatar == "6")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[06];

                }
                else if (Avatar == "07" || Avatar == "7")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[07];

                }
                else if (Avatar == "08" || Avatar == "8")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[08];

                }
                else if (Avatar == "09" || Avatar == "9")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[09];

                }
                else if (Avatar == "10")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[10];

                }
                else if (Avatar == "11")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[11];

                }
                else if (Avatar == "12")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[12];

                }
                else if (Avatar == "13")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[13];

                }
                else if (Avatar == "14")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[14];

                }
                else if (Avatar == "15")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[15];

                }
                else if (Avatar == "16")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[16];

                }
                else if (Avatar == "17")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[17];

                }
                else if (Avatar == "18")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[18];

                }
                else if (Avatar == "19")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[19];

                }
                else if (Avatar == "20")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[20];

                }
                else if (Avatar == "21")
                {
                    GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[21];

                }
                else
                {

                    StartCoroutine(loadImageOpponent(GameManager.Instance.avatarMyUrl));
                }


            }
            else
            {


                Debug.Log("ni data");
            }

        }
    }

    [Header("login")]
    public InputField email;
    public InputField password;

    public void TogglePassView()
    {
        Debug.Log("toggling pass view: from: "+password.contentType);
        if(password.contentType == InputField.ContentType.Password)
        {
            password.contentType = InputField.ContentType.Standard;
            p_pass.contentType = InputField.ContentType.Standard;
            pc_pass.contentType = InputField.ContentType.Standard;
        }
        else
        {
            password.contentType = InputField.ContentType.Password;
            p_pass.contentType = InputField.ContentType.Password;
            pc_pass.contentType = InputField.ContentType.Password;
        }
        password.DeactivateInputField();
        password.ActivateInputField();

        p_pass.DeactivateInputField();
        p_pass.ActivateInputField();

        pc_pass.DeactivateInputField();
        pc_pass.ActivateInputField();
        Debug.Log("toggling pass view: to: " + password.contentType);
    }

    public void LOgin(int logintype)
    {
        //logintype 1,2,3 for normal ,phone and google

        loadingPanel.SetActive(true);

        if (logintype == 1)
        {
            if (email.text != "" && password.text != "")
            {
               // Debug.Log("Login Successful");
                StartCoroutine(loginss(1));


            }
            else
            {
                Debug.Log("Login Failed ");

                loadingPanel.SetActive(false);

            }
        }
        else if(logintype == 2)
        {
            string phone = PlayerPrefs.GetString("phoneNumber");

            if(phone.Length  == 10)
            {
                Debug.Log("Login Successful");
                StartCoroutine(loginss(2));
            }
            else
            {
                Debug.Log("Login Failed ");

                loadingPanel.SetActive(false);
            }

        }
        else if(logintype == 3)
        {
            string email = PlayerPrefs.GetString("userEmail");
            if(email.Length >0)
            {
                StartCoroutine(loginss(3));
            }
            else
            {
                Debug.Log("Login Failed ");

                loadingPanel.SetActive(false);
            }
        }

    }


    public void LOGIN()
    {

        //  loginss();
    }

    public IEnumerator loginss(int loginType)
    {

        yield return new WaitForSeconds(1);
        StartCoroutine(checkLogin(loginType));

    }

    public GameObject loginErrMSG;

    public IEnumerator checkLogin(int loginType)
    {
        
        string userIdtext = email.text;
        string passtext = password.text;
        Debug.Log("----------- " + userIdtext);
        Debug.Log("----------- " + passtext);

        string phoneText = PlayerPrefs.GetString("phoneNumber");
        string emailText = PlayerPrefs.GetString("userEmail");

        WWWForm form = new WWWForm();
        if (loginType == 1)
        {
            form.AddField("email", userIdtext);
            form.AddField("password", passtext);
        }
        else if(loginType == 2)
        {
            form.AddField("email", phoneText);
        }
        else if(loginType == 3)
        {
            form.AddField("email", emailText);
        }

        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?api/login", form);

        
       
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //errorMsg.text = download.error;
            Debug.Log("Error downloading: " + www.error);
            loadingPanel.SetActive(false);
        }
        else
        {

            Debug.Log(www.downloadHandler.text);

            string output = www.downloadHandler.text;
            if(output.Contains("login failed"))
            {
                loadingPanel.SetActive(false);
                loginErrMSG.SetActive(true);
            }

            if (output == "null" || output == "")
            {
                Debug.Log("Login Failed");
                loadingPanel.SetActive(false);
               
            }

            else
            {


                var N = JSON.Parse(output);

                if (N["status"].Value == "1")

                {
                    string userIds = N["data"]["user_id"].Value;
                    string userId = N["data"]["name"].Value;
                    string Avatar = N["data"]["avatar"].Value;

                    string emails = N["data"]["email"].Value;
                    string Mobile = N["data"]["mobile_no"].Value;
                    string refer = N["data"]["referal_code"].Value;
                    string address = N["data"]["address"].Value;
                    string city = N["data"]["city"].Value;
                    string state = N["data"]["state"].Value;
                    string pin_code = N["data"]["pin_code"].Value;
                    string emailll = N["data"]["email"].Value;
                    var Total_balance = N["data"]["balance"].Value;
                    var pan_status = N["data"]["pan_status"].Value;
                    var bank_status = N["data"]["bank_status"].Value;
                    var verify_mail = N["data"]["mail_verified"].Value;



                    GameManager.Instance.emaill = emailll;
                    GameManager.Instance.mobile = Mobile;
                    GameManager.Instance.Balance = Total_balance;
                    GameManager.Instance.verify_mail = verify_mail;
                    GameManager.Instance.pan_status = pan_status;
                    GameManager.Instance.bank_status = bank_status;

                    Debug.Log("avatar is " + Avatar);
                    //SceneManager.LoadScene("MenuScene");

                    GameManager.key = "login";

                   

                    GameManager.Instance.UserID = userIds;
                    GameManager.Instance.facebookIDMy = userIds;
                    GameManager.Instance.nameMy = userId;
                    GameManager.Instance.avatarMyUrl = Avatar;

                    GameManager.Instance.referal = refer;
                    GameManager.Instance.state = state;
                    GameManager.Instance.pin_code = pin_code;
                    GameManager.Instance.city = city;
                    GameManager.Instance.address = address;


                    Email = emails;

                    Mobiles = Mobile;

                    Debug.Log("GameManager.user_id" + GameManager.Instance.UserID);
                    Debug.Log("GameManager.user_id" + GameManager.key);

                    if (GameManager.key.Equals("login"))
                    {
                        PlayerPrefs.SetString("user_ID", GameManager.Instance.UserID);
                        playFab.GetComponent<PlayFabManager>().PlayFabId = GameManager.Instance.UserID;
                        GameManager.Instance.playfabManager.PhotonLogin(GameManager.Instance.UserID);
                    }
                    else
                    {
                        login2.SetActive(true);
                    }


                    Debug.Log("Avatar value from checkLogin function is :" + Avatar);
                    if (Avatar == "0" )
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "01" || Avatar == "1")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "02" || Avatar == "2")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "03" || Avatar == "3")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "04" || Avatar == "4")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "05" || Avatar == "5")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "06" || Avatar == "6")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "07" || Avatar == "7")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "08" || Avatar == "8")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "09" || Avatar == "9")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "10")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "11")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "12")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "13")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "14")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "15")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "16")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "17")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "18")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "19")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "20")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "21")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else
                    {

                        StartCoroutine(loadImageOpponent(GameManager.Instance.avatarMyUrl));
                    }


                }
                else if (N["login_status"]["status"].Value == "0")
                {

                    Debug.Log("Login failed");
                    loadingPanel.SetActive(false);
                   

                }
            }

        }

    }

    /*
    public IEnumerator checkLogin(int loginType)
    {

        string userIdtext = email.text;
        string passtext = password.text;



        WWWForm form = new WWWForm();

        form.AddField("email", userIdtext);
        form.AddField("password", passtext);

        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?api/login", form);
        //UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?api/login2", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //errorMsg.text = download.error;
            Debug.Log("Error downloading: " + www.error);
        }
        else
        {

            Debug.Log(www.downloadHandler.text);

            string output = www.downloadHandler.text;

            if (output == "null" || output == "")
            {
                Debug.Log("Login Failed");
                loadingPanel.SetActive(false);

            }

            else
            {


                var N = JSON.Parse(output);

                if (N["status"].Value == "1")

                {



                    string userIds = N["data"]["user_id"].Value;
                    string userId = N["data"]["name"].Value;
                    string Avatar = N["data"]["avatar"].Value;

                    string emails = N["data"]["email"].Value;
                    string Mobile = N["data"]["mobile_no"].Value;
                    string refer = N["data"]["referal"].Value;
                    string address = N["data"]["address"].Value;
                    string city = N["data"]["city"].Value;
                    string state = N["data"]["state"].Value;
                    string pin_code = N["data"]["pin_code"].Value;
                    string emailll = N["data"]["email"].Value;
                    var Total_balance = N["data"]["balance"].Value;
                    var pan_status = N["data"]["pan_status"].Value;
                    var bank_status = N["data"]["bank_status"].Value;
                    var verify_mail = N["data"]["mail_verified"].Value;



                    GameManager.Instance.emaill = emailll;
                    GameManager.Instance.mobile = Mobile;
                    GameManager.Instance.Balance = Total_balance;
                    GameManager.Instance.verify_mail = verify_mail;
                    GameManager.Instance.pan_status = pan_status;
                    GameManager.Instance.bank_status = bank_status;

                    Debug.Log("avatar is " + Avatar);
                    //SceneManager.LoadScene("MenuScene");

                    GameManager.key = "login";



                    GameManager.Instance.UserID = userIds;
                    GameManager.Instance.facebookIDMy = userIds;
                    GameManager.Instance.nameMy = userId;
                    GameManager.Instance.avatarMyUrl = Avatar;

                    GameManager.Instance.referal = refer;
                    GameManager.Instance.state = state;
                    GameManager.Instance.pin_code = pin_code;
                    GameManager.Instance.city = city;
                    GameManager.Instance.address = address;


                    Email = emails;

                    Mobiles = Mobile;

                    Debug.Log("GameManager.user_id" + GameManager.Instance.UserID);
                    Debug.Log("GameManager.user_id" + GameManager.key);

                    if (GameManager.key.Equals("login"))
                    {
                        PlayerPrefs.SetString("user_ID", GameManager.Instance.UserID);
                        playFab.GetComponent<PlayFabManager>().PlayFabId = GameManager.Instance.UserID;
                        GameManager.Instance.playfabManager.PhotonLogin(GameManager.Instance.UserID);
                    }
                    else
                    {
                        login2.SetActive(true);
                    }


                    Debug.Log("Avatar value from checkLogin function is :" + Avatar);
                    if (Avatar == "0")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "01" || Avatar == "1")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "02" || Avatar == "2")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "03" || Avatar == "3")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "04" || Avatar == "4")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "05" || Avatar == "5")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "06" || Avatar == "6")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "07" || Avatar == "7")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "08" || Avatar == "8")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "09" || Avatar == "9")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "10")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "11")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "12")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "13")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "14")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "15")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "16")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "17")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "18")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "19")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "20")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else if (Avatar == "21")
                    {
                        GameManager.Instance.avatarMy = StaticGameVariablesController.instance.avatars[int.Parse(N["data"]["avatar"].Value)];

                    }
                    else
                    {

                        StartCoroutine(loadImageOpponent(GameManager.Instance.avatarMyUrl));
                    }


                }
                else if (N["login_status"]["status"].Value == "0")
                {

                    Debug.Log("Login failed");
                    loadingPanel.SetActive(false);


                }
            }

        }

    }*/








    [Header("signup")]
    public InputField mobileNumber;
    public InputField p_userId;
    public InputField p_email;
    public InputField p_refer;
    public GameObject p_err;
    public InputField p_pass;
    public InputField pc_pass;
    string passss;
    public GameObject signup;
    public GameObject login;

    public void RegisterUser()
    {
        loadingPanel.SetActive(true);
        if (string.IsNullOrEmpty(p_userId.text))
        {
            p_err.SetActive(true);
            p_err.GetComponent<Text>().text = "Please provide UserId";
            loadingPanel.gameObject.SetActive(false);
            return;
        }
        if (string.IsNullOrEmpty(p_email.text))
        {
            p_err.SetActive(true);
            p_err.GetComponent<Text>().text = "enter a valid email";
            loadingPanel.gameObject.SetActive(false);
            return;
        }

        if (string.IsNullOrEmpty(p_pass.text))
        {
            p_err.SetActive(true);
            p_err.GetComponent<Text>().text = "enter a password";
            loadingPanel.gameObject.SetActive(false);
            return;
        }
        if(p_pass.text.Length < 8)
        {
            p_err.SetActive(true);
            p_err.GetComponent<Text>().text = "Password limit min 8 digits";
            loadingPanel.gameObject.SetActive(false);
            return;
        }
        if (string.IsNullOrEmpty(pc_pass.text))
        {
            p_err.SetActive(true);
            p_err.GetComponent<Text>().text = "enter a confirm password";
            loadingPanel.gameObject.SetActive(false);
            return;
        }
        if ((p_pass.text) != pc_pass.text)
        {
            p_err.SetActive(true);
            p_err.GetComponent<Text>().text = "your password did not match";
             passss = p_pass.text;
            loadingPanel.gameObject.SetActive(false);
            return;
        }

        if (string.IsNullOrEmpty(mobileNumber.text))
        {
            p_err.SetActive(true);
            p_err.GetComponent<Text>().text = "enter a mobile number";
            loadingPanel.gameObject.SetActive(false);
            return;
        }
        if (mobileNumber.text.Length < 10)
        {
            p_err.SetActive(true);
            p_err.GetComponent<Text>().text = "enter a valid mobile number";
            loadingPanel.gameObject.SetActive(false);
            return;
        }

        p_err.SetActive(false);
        StartCoroutine(RegisterUserAPI());

    }
   
    IEnumerator RegisterUserAPI()
    {
        string userIdtext = p_userId.text;
        string passtext = p_pass.text;
        string mobtext = mobileNumber.text;
        string emailtext = p_email.text;
        string reftext = p_refer.text;

        WWWForm form = new WWWForm();

        form.AddField("email", emailtext);
        form.AddField("password", passtext);
        form.AddField("name", userIdtext);
        form.AddField("mobile_no", mobtext);
        form.AddField("referal", reftext);

        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?api/registration", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //errorMsg.text = download.error;
            Debug.Log("Error downloading: " + www.error);
            loadingPanel.SetActive(false);
        }

        else
        {
            Debug.Log(www.downloadHandler.text);

            string output = www.downloadHandler.text;

            if (output == "null" || output == "")
            {
                Debug.Log("Resgister Failed");
                loadingPanel.SetActive(false);
            }


            else
            {
                var N = JSON.Parse(output);

                if (N["status"].Value == "1")

                {
                    var message = N["message"].Value;


                    Debug.Log("message is" + message);


                    loadingPanel.SetActive(true);
                    //  signupSuccessPanel.SetActive(true);
                    StartCoroutine(Anime());

                }

                else
                {
                    Debug.Log("Resgister failed");
                    p_err.SetActive(true);
                    p_err.GetComponent<Text>().text = N["message"].Value;
                    loadingPanel.SetActive(false);
                }
            }

        }

    }

    void anime()
    {

    }

    IEnumerator Anime()
    {
        loadingPanel.SetActive(false);
        yield return new WaitForSeconds(2);

        signup.SetActive(false);
        login.SetActive(true);

    
     

    }

}

