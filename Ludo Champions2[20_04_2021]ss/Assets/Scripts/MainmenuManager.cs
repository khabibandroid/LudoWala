using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainmenuManager : MonoBehaviour
{
    public GameObject wallet;
    public GameObject home;
    public GameObject leaderboard;
    public GameObject contast;
    public GameObject Loading;

    public string selectColor;

    public GameObject wallet1;
    public GameObject home1;
    public GameObject leaderboard1;
    public GameObject contast1;

    public GameObject tournament;
    public GameObject result;


    [Header("User Profile")]
    public Text name;
    public Text mobile;
    public Text p_name;
    public Text prof_Name;
    public Text prof_Mobile;
    public Text prof_Email;

    [Header("referal")]
    public Text referal;


    [Header("update Address")]
    public InputField address;
    public InputField state;
    public InputField city;
    public InputField pin_code;

    public Text Addresss;
    public Text States;
    public Text Citys;
    public Text Pin_codes;


    [Header("Game info")]

    public Text total_matches;
    public Text total_played;
    public Text total_win;
    public Text skip_matches;
    public Text matches;
    public Text wins;


    [Header("Profile")]

    public Text Name;

    public Text Mobile;
    public Text WalletProfileName;

    public InputField O_pass;
    public InputField N_pass;
    public GameObject updatepopup;
    public Text updatepopuptext;

    public GameObject Exitpanel;
    public GameObject slide_image;
    public GameObject Profile_image;
    public GameObject Profile_imagemain;

    public AudioSource sound;

    public static MainmenuManager instance;


    public bool paytmVerified;
    public bool gpayVerified;
    public bool upiVerified;


    public static MainmenuManager getInstance()
    {

        return instance;

    }

    public void yellows()
    {
        GameManager.Instance.select = true;
        selectColor = "yellow";
    }
    public void BLue()
    {
        GameManager.Instance.select = true;
        selectColor = "Blue";
    }
    public void reds()
    {
        GameManager.Instance.select = true;
        selectColor = "red";
    }
    public void greens()
    {
        GameManager.Instance.select = true;
        selectColor = "Green";
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        PlayerPrefs.SetInt("FirstLoad", 1);
        slide_image.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;
        Profile_image.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;
        Profile_imagemain.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;

        Application.runInBackground = true;
        //        total_balances.text = GameManager.Instance.Balance.ToString();
        //        total_balance_with.text = GameManager.Instance.Balance.ToString();
        //        total_balance_Cash.text = GameManager.Instance.totalBalance.ToString();
        Withdraw_history();

        getinfo();
        StartCoroutine(User_Data());
        user_profile();
        StartCoroutine(Balance());
        StartCoroutine(updated_profile());

        InvokeRepeating("UpdateBalance", 10, 10);
    }

    void UpdateBalance()
    {
        balance();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // sound.Play();
            Exitpanel.SetActive(false);

        }


    }



    private void Awake()
    {

        //Application.runInBackground = true;
    }


    public void user_profile()
    {
        name.text = GameManager.Instance.nameMy;
        name.text = GameManager.Instance.nameMy;
        mobile.text = GameManager.Instance.mobile;
        p_name.text = GameManager.Instance.nameMy;
        prof_Name.text = GameManager.Instance.nameMy;
        p_name.text = GameManager.Instance.nameMy;
        prof_Name.text = GameManager.Instance.nameMy;
        prof_Mobile.text = GameManager.Instance.mobile;
        prof_Email.text = GameManager.Instance.emaill;

        referal.text = GameManager.Instance.referal;
        Addresss.text = GameManager.Instance.address;
        Citys.text = GameManager.Instance.city;
        States.text = GameManager.Instance.state;
        Pin_codes.text = GameManager.Instance.pin_code;
    }

    public void YesExit()
    {

        Application.Quit();
    }


    public void Wallet()
    {

        StartCoroutine(updated_profile());
        StartCoroutine(walletss());
    }

    IEnumerator walletss()
    {
        yield return new WaitForSeconds(1);
        wallet.SetActive(true);

        leaderboard.SetActive(false);
        contast.SetActive(false);
        home.SetActive(false);
        wallet1.SetActive(true);
        home1.SetActive(false);
        leaderboard1.SetActive(false);
        contast1.SetActive(false);

        Loading.SetActive(false);

    }


    public void Home()
    {

        StartCoroutine(homess());
    }

    IEnumerator homess()
    {
        yield return new WaitForSeconds(1);
        wallet.SetActive(false);
        home.SetActive(true);
        leaderboard.SetActive(false);
        contast.SetActive(false);
        wallet1.SetActive(false);
        home1.SetActive(true);
        leaderboard1.SetActive(false);
        contast1.SetActive(false);

       
        Loading.SetActive(false);
    }

    public void Leaderboard()
    {

        StartCoroutine(updated_profile());
        StartCoroutine(leaderss());
        StartCoroutine(User_Data());

    }

    IEnumerator leaderss()
    {
        yield return new WaitForSeconds(1);
       // wallet.SetActive(false);
    //    home.SetActive(false);
        leaderboard.SetActive(true);
  //      contast.SetActive(false);

//        wallet1.SetActive(false);
//        home1.SetActive(false);
      //  leaderboard1.SetActive(true);
    //    contast1.SetActive(false);

        Loading.SetActive(false);
    }


    public void Contast()
    {


        StartCoroutine(contastss());
    }

    IEnumerator contastss()
    {
        yield return new WaitForSeconds(1);
        wallet.SetActive(false);
        home.SetActive(false);
        leaderboard.SetActive(false);
        contast.SetActive(true);


        wallet1.SetActive(false);
        home1.SetActive(false);
        leaderboard1.SetActive(false);
        contast1.SetActive(true);

        Loading.SetActive(false);
     
    }


    public GameObject CommingSoon;


    public void Help()
    {

        Application.OpenURL("http://www.ludodreams.com/");

    }
    public void CallSupport()
    {

        Application.OpenURL("https://ludovegas.com/");

    }
    public void TermsS()
    {
        Application.OpenURL("https://ludovegas.com/terms.html");

    }
    public void howplay()
    {
        Application.OpenURL("http://ludodreams.com/index.html#easy-paly");

    }
    public void howwork()
    {
        Application.OpenURL("http://ludodreams.com/");

    }
    public void Privacy()
    {
        Application.OpenURL("https://ludovegas.com/privacy.html");

    }
    public void Legal()
    {

        Application.OpenURL("http://ludodreams.com/");

    }
    public void About()
    {
        Application.OpenURL("https://ludo.atmsoftek.usindex.html");

    }

    public void Update_profile()
    {
        StartCoroutine(update_profile());
        Debug.Log("Clicked!");
    }


    IEnumerator update_profile()
    {
        Debug.Log("hit!");
        // string userId = E_name.text;
        string oldpass = O_pass.text;
        string pass = N_pass.text;
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Instance.UserID);
        // form.AddField("userID", userId);
        form.AddField("password", pass);
        form.AddField("old_password", oldpass);

        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?user/update", form);

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





            var N = JSON.Parse(output);


            if (N["status"].Value == "1")

            {


                var message = N["message"].Value;


                updatepopup.SetActive(true);

                //   Anime();


                Debug.Log(message);

            }
            else if (N["status"].Value == "0")
            {

                updatepopup.SetActive(true);
                updatepopuptext.text = "UPDATE FAILED!";
                Anime();

            }


        }


    }

    IEnumerator Anime()
    {

        yield return new WaitForSeconds(3);

        updatepopup.SetActive(false);




    }

    public void Updated_profile()
    {
        StartCoroutine(updated_profile());

    }

    IEnumerator updated_profile()
    {

        string userId = GameManager.Instance.UserID;

        WWWForm form = new WWWForm();

        form.AddField("user_id", userId);


        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?user/user_byid", form);

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





            var N = JSON.Parse(output);


            if (N["status"].Value == "1")

            {


                var message = N["message"].Value;

                var response = N["response"].Value;

                string names = N["response"][0]["name"].Value;


                string emails = N["response"][0]["email"].Value;
                string Mobiles = N["response"][0]["mobile_no"].Value;

                Name.text = names;

                Mobile.text = Mobiles;
                WalletProfileName.text = names;

                Debug.Log(message);
//                Debug.Log(response);

            }
            else
            {

                Debug.Log("fail!!!");

            }


        }

    }

    public void Updated_address()
    {
        StartCoroutine(updated_address());
    }

    IEnumerator updated_address()
    {
        var State = state.text;
        var City = city.text;
        var Address = address.text;
        var Pin_code = pin_code.text;


        WWWForm form = new WWWForm();

        form.AddField("state", State);
        form.AddField("city", City);
        form.AddField("address", Address);
        form.AddField("pin_code", Pin_code);

        form.AddField("user_id", GameManager.Instance.UserID);
        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?user/update_address", form);

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


            


            var N = JSON.Parse(output);


            if (N["status"].Value == "1")

            {


                var message = N["message"].Value;


                Debug.Log(message);


            }
            else
            {

                Debug.Log("fail!!!");

            }


        }

    }



   

    public void getinfo()
    {

        StartCoroutine(Game_Info());
    }

    public IEnumerator Game_Info()
    {


        WWWForm form = new WWWForm();

        form.AddField("user_id", GameManager.Instance.UserID);

        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?user/game_info", form);

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
                Debug.Log("no data found");

            }

            else
            {
                var N = JSON.Parse(output);



                {



                    string matchess = N["joined"].Value;
                    string played = N["played"].Value;
                    string winss = N["winned"].Value;

                    string skip = N["lefted"].Value;


                    total_matches.text = matchess;
                    matches.text = matchess;
                    total_played.text = played;
                    total_win.text = winss;
                    wins.text = winss;
                    skip_matches.text = skip;

                    GameManager.Instance.Total_matches = matchess;
                    GameManager.Instance.Total_play = played;
                    GameManager.Instance.Wins = winss;
                    GameManager.Instance.Skip = skip;



                }
            }

        }

    }

    [Header("verify text")]



    public GameObject c_verifie;
    public GameObject C_withdraw;
    public GameObject W_verifie;
    public GameObject W_withdraw;


    public GameObject mail_verify;

    [Header("balance ")]


    public Text total_balances;
    public Text winnning_balances;
    public Text add_balances;
    public Text total_balance_with;
    public Text total_balance_Cash;
    public Text Bonus;
    public Text winnning_balancess;
    IEnumerator User_Data()
    {
        WWWForm form = new WWWForm();

        form.AddField("user_id", GameManager.Instance.UserID);

        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?api/user_data", form);

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
            var N = JSON.Parse(output);

            if (N["status"].Value == "1")

            {
                GameManager.Instance.UserID = N["data"]["user_id"].Value;
           

                if (N["data"]["pan_status"].Value == "0" && N["data"]["bank_status"].Value == "0" || GameManager.Instance.pan_status == "0" && GameManager.Instance.bank_status == "0")
                {
                    c_verifie.SetActive(true);
                    C_withdraw.SetActive(false);
                    W_verifie.SetActive(true);
                    W_withdraw.SetActive(false);

                }


                else if (N["data"]["pan_status"].Value == "0" && N["data"]["bank_status"].Value == "1" || GameManager.Instance.pan_status == "0" && GameManager.Instance.bank_status == "1")
                {
                    c_verifie.SetActive(true);
                    C_withdraw.SetActive(false);
                    W_verifie.SetActive(true);
                    W_withdraw.SetActive(false);

                }

                else if (N["data"]["pan_status"].Value == "1" && N["data"]["bank_status"].Value == "0" || GameManager.Instance.pan_status == "1" && GameManager.Instance.bank_status == "0")
                {
                    c_verifie.SetActive(true);
                    C_withdraw.SetActive(false);
                    W_verifie.SetActive(true);
                    W_withdraw.SetActive(false);
                }

                else if (N["data"]["pan_status"].Value == "1" && N["data"]["bank_status"].Value == "1" || GameManager.Instance.pan_status == "1" && GameManager.Instance.bank_status == "1")
                {
                    c_verifie.SetActive(false);
                    C_withdraw.SetActive(true);
                    W_verifie.SetActive(false);
                    W_withdraw.SetActive(true);

                }


                if (N["data"]["paytm_number"].Value != "")
                    paytmVerified = true;
                else
                    paytmVerified = false;


                if ( N["data"]["gpay_number"].Value != "")
                    gpayVerified = true;
                else
                    gpayVerified = false;


                if ( N["data"]["upi_id"].Value != "")
                    upiVerified = true;
                else
                    upiVerified = false;
            }

        }
    }

    [Header("notification")]
    public Text Message;
    public Text Date;

    public GameObject noti;
    public GameObject P_noti;

    public void Notification()
    {

        StartCoroutine(notification());
    }


    IEnumerator notification()
    {


        WWWForm form = new WWWForm();

        form.AddField("user_id", GameManager.Instance.UserID);


        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?user/notification", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //errorMsg.text = download.error;
            Debug.Log("Error downloading: " + www.error);
        }
        else
        {

            /* for (int i = noti.transform.childCount - 1; i > 0; i--)
             {
                 Destroy(P_noti.transform.GetChild(i).gameObject);
             }
             */
            Debug.Log(www.downloadHandler.text);

            string output = www.downloadHandler.text;

            if (output == "" || output == "null")
            {

                Debug.Log("No Notification found");

                noti.SetActive(false);
                P_noti.SetActive(true);
            }

            else
            {



                var N = JSON.Parse(output);

                if (N["status"].Value == "1")
                {
                    /* for (int i = 0; i < N.Count; i++)
                     {

                         GameObject row = Instantiate(noti);

                         row.transform.SetParent(P_noti.transform, false);*/
                    var message = N["message"].Value;
                    var date = N["starting_date"].Value;
                    var name = N["name"].Value;

                    Message.text = message + ":" + "\n" + name;
                    Date.text = "Date: " + date;

                    P_noti.SetActive(false);
                    noti.SetActive(true);
                    // row.SetActive(true);
                    // }
                }

                else
                {

                    Debug.Log("No data found");
                }
            }

        }

    }
    [Header("Withdraw")]
    public InputField amount;
    public GameObject Success;
    public Text request;
    public Dropdown withdrawOptions;


    public void withdraw()
    {
        Debug.Log(GameManager.Instance.Balance);
        if(amount.text == "" || amount.text == string.Empty)
        //if(string.IsNullOrEmpty(amount.text))
        {
            Success.SetActive(true);
            request.text = "Invalid amount!\n " + "amount can't be 0 ";
            return;

        }


        if(int.Parse(amount.text) < 10)
        {
            Success.SetActive(true);
            request.text = "Invalid amount!\n " + "minimum amount accept 10/- ";
            return;
        }
        if(int.Parse(amount.text) > int.Parse(GameManager.Instance.Balance))
        {

            Success.SetActive(true);
            request.text = "Invalid amount!";
            return;
        }


        if(withdrawOptions.value == 0) // None
        {
            Success.SetActive(true);
            request.text = "Invalid Withdrawl Option";
            return;
        }
       
        StartCoroutine(With_draw());

    }

    IEnumerator With_draw()
    {
        var Amount = amount.text;

        
        
        WWWForm form = new WWWForm();

        form.AddField("amount", Amount);
        form.AddField("user_id", GameManager.Instance.UserID);
       // form.AddField("method_id", "");

        if (withdrawOptions.value == 0)//paytm
            form.AddField("withdraw_method", "");

        if (withdrawOptions.value == 1)//paytm
            form.AddField("withdraw_method", "paytm");

        if (withdrawOptions.value == 2)//paytm
            form.AddField("withdraw_method", "gpay");

        if (withdrawOptions.value == 3)//paytm
            form.AddField("withdraw_method", "upi");



        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?user/payment_widthdrawal", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //errorMsg.text = download.error;
            Debug.Log("Error downloading: " + www.error);
            Debug.Log("Request fail");
            Success.SetActive(true);
            request.text = "Request Failed!\n"+www.error.ToString();
        }
        else
        {



            Debug.Log(www.downloadHandler.text);

            string output = www.downloadHandler.text;




            var N = JSON.Parse(output);

            if (N["status"] == "1")
            {
                var message = N["message"].Value;
                Success.SetActive(true);
                request.text = "Your withdraw request summited Successfully! \n Please wait 24 hours for Withdraw!!!";
                Debug.Log(message);

            }
            else
            {
                Debug.Log("Request fail");
                Success.SetActive(true);
                request.text = "Request Failed!";
            }

        }
    }


    [Header("Add_history")]
    public Text Tnsmsg;
    public Text TnxAmount;
    public Text TnxID;

    public GameObject Addhs;
    public GameObject P_Addhs;
    public GameObject No_trasn;

    public void add_history()
    {
        StartCoroutine(Add_History());


    }

    IEnumerator Add_History()
    {



        WWWForm form = new WWWForm();

        form.AddField("user_id", GameManager.Instance.UserID);

        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?user/payment_history", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //errorMsg.text = download.error;
            Debug.Log("Error downloading: " + www.error);
        }
        else
        {


            for (int i = P_Addhs.transform.childCount - 1; i > 0; i--)
            {
                Debug.Log("Enter here ::");
                Destroy(P_Addhs.transform.GetChild(i).gameObject);
            }

            Debug.Log(www.downloadHandler.text);

            string output = www.downloadHandler.text;


            if (output == "null" || output == "")
            {

                Debug.Log("No Data found");

            }
            else
            {

                var N = JSON.Parse(output);

                if (N["status"] == "1")
                {
                    for (int i = 0; i < N["data"].Count; i++)
                    {

                        GameObject row = Instantiate(Addhs);

                        row.transform.SetParent(P_Addhs.transform, false);
                        payment_history myscript = row.GetComponent<payment_history>();

                        var message = N["data"]["message"].Value;
                        var tnxid = N["data"][i]["txnid"].Value;
                        var tnxmsg = N["data"][i]["txnmsg"].Value;
                        var tnxamount = N["data"][i]["txnamount"].Value;


                        myscript.W_msg.text = tnxmsg;
                        myscript.w_amount.text = tnxamount;
                        myscript.W_id.text = tnxid;
                        No_trasn.SetActive(false);



                        //Tnsmsg.text = tnxmsg;
                        //TnxAmount.text = tnxamount;
                        //TnxID.text = tnxid;
                        //No_trasn.SetActive(false);
                        Debug.Log(message);
                        row.SetActive(true);

                    }



                }
                else
                {

                    No_trasn.SetActive(true);
                    Debug.Log("Request fail");

                }


            }
        }
    }

    [Header("Withdraw_history")]

    public GameObject withdra;
    public GameObject P_withdra;
    public GameObject no_trans;

    public void Withdraw_history()
    {
        StartCoroutine(withdraw_History());


    }

    IEnumerator withdraw_History()
    {



        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Instance.UserID);


        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?api/widthdrawal", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //errorMsg.text = download.error;
            Debug.Log("Error downloading: " + www.error);
        }
        else
        {


            for (int i = P_withdra.transform.childCount - 1; i > 0; i--)
            {
                Destroy(P_withdra.transform.GetChild(i).gameObject);
            }

            Debug.Log(www.downloadHandler.text);

            string output = www.downloadHandler.text;





            var N = JSON.Parse(output);

            if (N["status"] == "1")
            {

                for (int i = 0; i < N["data"].Count; i++)
                {

                    GameObject row = Instantiate(withdra);

                    row.transform.SetParent(P_withdra.transform, false);
                    payment_history myscript = row.GetComponent<payment_history>();

                    var message = N["data"]["message"].Value;
                    var tnxid = N["data"][i]["id"].Value;
                    var tnxmsg = N["data"][i]["status"].Value;
                    var tnxamount = N["data"][i]["amount"].Value;

                    myscript.W_msg.text = tnxmsg;
                    myscript.w_amount.text = tnxamount;
                    myscript.W_id.text = tnxid;
                    no_trans.SetActive(false);
                    Debug.Log(message);
                    row.SetActive(true);
                }

            }
            else
            {
                Debug.Log("Request fail");
                no_trans.SetActive(true);
            }


        }
    }

    [Header("Balance")]
    public Text totalBalance;
    public Text WinningBalance;
    public Text W_AddBalance;
    public Text W_totalBalance;
    public Text W_WinningBalance;
    public Text WW_AddBalance;
    public Text WW_totalBalance;
    public Text WW_WinningBalance;
    public Text TOTALBAL;

    public Text winni, depoo, bonu;
    

    public void balance()
    {
        StartCoroutine(Balance());


    }

    IEnumerator Balance()
    {

        print("((((((((((((((   " + GameManager.Instance.UserID);

        WWWForm form = new WWWForm();

        form.AddField("user_id", GameManager.Instance.UserID);

        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?user/balance", form); //+ GameManager.Instance.UserID, form);

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

            var N = JSON.Parse(output);

            if (N["status"] == "1")
            {
            
                var z = N["Data"]["total_bonus"].Value;
                var x = N["Data"]["total_deposit"].Value;
                var y = N["Data"]["total_winning"].Value;

                var balance = N["Data"]["balance"].Value;
               

                W_AddBalance.text = double.Parse(x).ToString();
//                WW_AddBalance.text = double.Parse(N["Data"]["total_bonus"].Value).ToString();
                GameManager.Instance.Deposites = double.Parse(x);
                GameManager.Instance.Bonus = double.Parse(z);
                bonu.text = "Rs " + z;
                depoo.text = "Rs " + x;     
                winni.text = "Rs " + Mathf.FloorToInt(float.Parse(y)).ToString();    // Lalit
                totalBalance.text = winni.text;

                int totalBalanceFromServer = Mathf.FloorToInt(float.Parse(y)) + int.Parse(x);

                GameManager.Instance.Balance = totalBalanceFromServer.ToString();

                W_totalBalance.text = "Rs " + totalBalanceFromServer.ToString(); //Lalit   // To Show At Balance panel
//                WW_totalBalance.text = c.ToString();
                TOTALBAL.text = totalBalanceFromServer.ToString();
            }
            else
            {
                Debug.Log("Request fail");

            }


        }
    }

    [Header("leaderboard")]
    public GameObject Parent_month;
    public GameObject Items_month;
    public Text ser_no;
    public Text winnings;
    public Text Names;

    public void leaderboard_month()
    {
        StartCoroutine(leader_month());

    }


    IEnumerator leader_month()
    {


        WWWForm form = new WWWForm();



        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala//index.php?user/lead_week", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //errorMsg.text = download.error;
            Debug.Log("Error downloading: " + www.error);
        }
        else
        {


            for (int i = Parent_month.transform.childCount - 1; i > 0; i--)
            {
                Destroy(Parent_month.transform.GetChild(i).gameObject);
            }

            Debug.Log(www.downloadHandler.text);

            string output = www.downloadHandler.text;

            if (output == "null" || output == "")
            {
                Debug.Log(" No Contast found.");
            }

            else
            {
                var N = JSON.Parse(output);


                if (N["status"].Value == "1")
                {

                    for (int i = 0; i < N["Data"].Count; i++)



                    {

                        GameObject row = Instantiate(Items_month);

                        row.transform.SetParent(Parent_month.transform, false);

                        lead_month myscript = row.GetComponent<lead_month>();

                        myscript.name.text = N["Data"][i]["name"].Value;
                       
                        myscript.win.text = N["Data"][i]["winration"].Value;
                        myscript.ser_no.text = (i + 1).ToString();
                        row.SetActive(true);
                    }
                }
                else
                {

                    Debug.Log("Ni data");

                }


            }
        }
    }

}
