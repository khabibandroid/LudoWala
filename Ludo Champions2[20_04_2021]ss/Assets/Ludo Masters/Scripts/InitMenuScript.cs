using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using ExitGames.Client.Photon.Chat;
using UnityEngine.SceneManagement;
//using PlayFab.ClientModels;
//using PlayFab;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.Networking;
#if UNITY_ANDROID || UNITY_IOS
using UnityEngine.Advertisements;
#endif
using AssemblyCSharp;

public class InitMenuScript : MonoBehaviour
{
    public GameObject rateWindow;
    public GameObject FacebookLinkReward;
    public GameObject rewardDialogText;
    public GameObject FacebookLinkButton;
    public GameObject playerName;
    public GameObject videoRewardText;
    public GameObject playerAvatar;
    public GameObject fbFriendsMenu;
    public GameObject matchPlayer;
    public GameObject backButtonMatchPlayers;
    public GameObject MatchPlayersCanvas;
    public GameObject menuCanvas;
    public GameObject tablesCanvas;
    public GameObject gameTitle;
    public GameObject changeDialog;
    public GameObject inputNewName;
    public GameObject tooShortText;
    public GameObject coinsText;
    public GameObject coinsTextShop;
    public GameObject coinsTab;
    public GameObject TheMillButton;
    public GameObject dialog;
    // Use this for initialization
    public GameObject GameConfigurationScreen;
    public GameObject FourPlayerMenuButton;

    void Start()
    {

      //  GameManager.Instance.coins = int.Parse(GameManager.Instance.Balance);


        if (PlayerPrefs.GetInt(StaticStrings.SoundsKey, 0) == 0)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }


     //   FacebookLinkReward.GetComponent<Text>().text = "+ " + StaticStrings.CoinsForLinkToFacebook;

        if (!StaticStrings.isFourPlayerModeEnabled)
        {
            FourPlayerMenuButton.SetActive(false);
        }

        GameManager.Instance.FacebookLinkButton = FacebookLinkButton;

        GameManager.Instance.dialog = dialog;
       // videoRewardText.GetComponent<Text>().text = "+" + StaticStrings.rewardForVideoAd;
        GameManager.Instance.tablesCanvas = tablesCanvas;
        //GameManager.Instance.facebookFriendsMenu = fbFriendsMenu.GetComponent<FacebookFriendsMenu>(); ;
        GameManager.Instance.matchPlayerObject = matchPlayer;
        GameManager.Instance.backButtonMatchPlayers = backButtonMatchPlayers;
        playerName.GetComponent<Text>().text = GameManager.Instance.nameMy;
        GameManager.Instance.MatchPlayersCanvas = MatchPlayersCanvas;

        if (PlayerPrefs.GetString("LoggedType").Equals("Facebook"))
        {
           // FacebookLinkButton.SetActive(false);
        }

        if (GameManager.Instance.avatarMy != null)
            playerAvatar.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;

        GameManager.Instance.myAvatarGameObject = playerAvatar;
        GameManager.Instance.myNameGameObject = playerName;

        GameManager.Instance.coinsTextMenu = coinsText;
        GameManager.Instance.coinsTextShop = coinsTextShop;
        GameManager.Instance.initMenuScript = this;

        if (StaticStrings.hideCoinsTabInShop)
        {
            coinsTab.SetActive(false);
        }

#if UNITY_WEBGL
        coinsTab.SetActive(false);
#endif

     //   rewardDialogText.GetComponent<Text>().text = "1 Video = " + StaticStrings.rewardForVideoAd + " Coins";
        //coinsText.GetComponent<Text>().text = GameManager.Instance.myPlayerData.GetCoins() + "";



        Debug.Log("Load ad menu");
      //  AdsManager.Instance.adsScript.ShowAd(AdLocation.GameStart);

        if (PlayerPrefs.GetInt("GamesPlayed", 1) % 8 == 0 && PlayerPrefs.GetInt("GameRated", 0) == 0)
        {
            if(rateWindow)
            rateWindow.SetActive(true);
            PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 1) + 1);
        }

    }


    public void QuitApp()
    {
        PlayerPrefs.SetInt("GameRated", 1);
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + StaticStrings.AndroidPackageName);
#elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/id" + StaticStrings.ITunesAppID);
#endif
        //Application.Quit();
    }



    public void RefreshRoomDataCustom()
    {
        //EventCounter.ResetALLData();
    }
   

    public void ShowGameConfiguration(int index)
    {
      
        switch (index)
        {
            case 0:
                GameManager.Instance.type = MyGameType.TwoPlayer;
                GameConfigurationScreen.GetComponent<GameConfigrationController>().setCreatedProvateRoom();
                break;
            case 1:
                GameManager.Instance.type = MyGameType.ThreePlayer;
                GameConfigurationScreen.GetComponent<GameConfigrationController>().setCreatedProvateRoom();
                break;
            case 2:
                GameManager.Instance.type = MyGameType.FourPlayer;
                GameConfigurationScreen.GetComponent<GameConfigrationController>().setCreatedProvateRoom();
                break;
            case 3:
                GameManager.Instance.type = MyGameType.Private;
                GameConfigurationScreen.GetComponent<GameConfigrationController>().setCreatedProvateRoom();
                break;
            case 4:
                GameManager.Instance.type = MyGameType.comp;
                GameConfigurationScreen.GetComponent<GameConfigrationController>().setCreatedProvateRoom();
                break;
        }
        GameConfigurationScreen.SetActive(true);
     //  AdsManager.Instance.adsScript.ShowAd(AdLocation.GamePropertiesWindow);
    }
	public void ShowGametypeConfiguration()
	{

        if (GameManager.Instance.mainmenu.ViewBtn[GameManager.Instance.mainmenu.temp].GetComponent<enroll_tour>().tourid == "2p")
        {
            GameManager.Instance.type = MyGameType.TwoPlayer;
            GameConfigurationScreen.GetComponent<GameConfigrationController>().setCreatedProvateRoom();

        }
       else if (GameManager.Instance.mainmenu.ViewBtn[GameManager.Instance.mainmenu.temp].GetComponent<enroll_tour>().tourid == "3p")
        {
            GameManager.Instance.type = MyGameType.ThreePlayer;
            GameConfigurationScreen.GetComponent<GameConfigrationController>().setCreatedProvateRoom();

        }
       else if (GameManager.Instance.mainmenu.ViewBtn[GameManager.Instance.mainmenu.temp].GetComponent<enroll_tour>().tourid == "4p")
        {
            GameManager.Instance.type = MyGameType.FourPlayer;
            GameConfigurationScreen.GetComponent<GameConfigrationController>().setCreatedProvateRoom();

        }

    }

	public void TakeScreenshot()
    {
        ScreenCapture.CaptureScreenshot("TestScreenshot.png");
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void showAdStore()
    {
      //  AdsManager.Instance.adsScript.ShowAd(AdLocation.StoreWindow);
    }

    public void backToMenuFromTableSelect()
    {
        GameManager.Instance.offlineMode = false;
        tablesCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        gameTitle.SetActive(true);
    }

    public void showSelectTableScene(bool challengeFriend)
    {
        if (!challengeFriend)
            GameManager.Instance.inviteFriendActivated = false;

   //   AdsManager.Instance.adsScript.ShowAd(AdLocation.GameStart);
        if (GameManager.Instance.offlineMode)
        {
            TheMillButton.SetActive(false);
        }
        else
        {
            TheMillButton.SetActive(true);
        }
        menuCanvas.SetActive(false);
        tablesCanvas.SetActive(true);
        gameTitle.SetActive(false);
    }

    public void playOffline()
    {
        //GameManager.Instance.tableNumber = 0;
        GameManager.Instance.offlineMode = true;
        GameManager.Instance.roomOwner = true;
        showSelectTableScene(false);
        SceneManager.LoadScene(GameManager.Instance.GameScene);
    }

    public void switchUser()
    {
        GameManager.Instance.playfabManager.destroy();
        GameManager.Instance.facebookManager.destroy();
        GameManager.Instance.connectionLost.destroy();

//        GameObject.Find("BGSound").GetComponent<BGSound>().destroy();
        GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>().destroy();
        GameManager.Instance.avatarMy = null;
        PhotonNetwork.Disconnect();

        PlayerPrefs.DeleteAll();
        GameManager.Instance.resetAllData();
        LocalNotification.ClearNotifications();

      
        //GameManager.Instance.myPlayerData.GetCoins() = 0;
        SceneManager.LoadScene("LoginScene");
    }

    public void showChangeDialog()
    {
        changeDialog.SetActive(true);
    }

    public void changeUserName()
    {
        Debug.Log("Change Nickname");

        string newName = inputNewName.GetComponent<Text>().text;
        if (newName.Equals(StaticStrings.addCoinsHackString))
        {
            
            changeDialog.SetActive(false);
        }
        else
        {
            /* if (newName.Length > 0)
             {
                 UpdateUserTitleDisplayNameRequest displayNameRequest = new UpdateUserTitleDisplayNameRequest()
                 {
                     //DisplayName = newName
                     DisplayName = GameManager.Instance.playfabManager.PlayFabId
                 };

                 PlayFabClientAPI.UpdateUserTitleDisplayName(displayNameRequest, (response) =>
                 {
                     Dictionary<string, string> data = new Dictionary<string, string>();
                     data.Add("PlayerName", newName);
                     UpdateUserDataRequest userDataRequest = new UpdateUserDataRequest()
                     {
                         Data = data,
                         Permission = UserDataPermission.Public
                     };

                     PlayFabClientAPI.UpdateUserData(userDataRequest, (result1) =>
                     {
                         Debug.Log("Data updated successfull ");
                         Debug.Log("Title Display name updated successfully");
                         PlayerPrefs.SetString("GuestPlayerName", newName);
                         PlayerPrefs.Save();
                         GameManager.Instance.nameMy = newName;
                         playerName.GetComponent<Text>().text = newName;
                     }, (error1) =>
                     {
                         Debug.Log("Data updated error " + error1.ErrorMessage);
                     }, null);

                 }, (error) =>
                 {
                     Debug.Log("Title Display name updated error: " + error.Error);

                 }, null);

                 changeDialog.SetActive(false);
             }
             else
             {
                 tooShortText.SetActive(true);
             }*/
        }
        



    }

    public void startQuickGame()
    {
       
    }

    public void startQuickGameTableNumer(int tableNumer, int fee)
    {
        GameManager.Instance.payoutCoins = fee;
        GameManager.Instance.tableNumber = tableNumer;
    
    }

    public void showFacebookFriends()
    {

//        AdsManager.Instance.adsScript.ShowAd(AdLocation.FacebookFriends);
       
    }

    public void setTableNumber()
    {
        GameManager.Instance.tableNumber = Int32.Parse(GameObject.Find("TextTableNumber").GetComponent<Text>().text);
    }

    public void ShowGameConfigurationPrivate(int index, int player)
    {
        switch (index)
        {
            case 3:
                GameManager.Instance.JoinedByID = true;
                GameManager.Instance.type = MyGameType.Private;


                break;

        }

        GameConfigurationScreen.GetComponent<GameConfigrationController>().startGamePrivate(player);
        //  AdsManager.Instance.adsScript.ShowAd(AdLocation.GamePropertiesWindow);
    }


    [Header("Create tournament")]
    public InputField createtournamentAmount;
    public GameObject errorMsg;
    public Toggle twoPlayer;
    public Toggle FourPlayer;
    int totalPlayer;
    public void craeteTournament()
    {
        Debug.Log("GameManager.Instance.Balance" + GameManager.Instance.Balance);
        if (string.IsNullOrEmpty(createtournamentAmount.text))
        {
            errorMsg.SetActive(true);
            errorMsg.GetComponent<Text>().text = "Enter amount";
            return;
        }
        else /*if (int.Parse(createtournamentAmount.text) == 0) {
            errorMsg.SetActive(true);
            errorMsg.GetComponent<Text>().text = "Invalid amount";
            return;
        }
        else*/ if (int.Parse(createtournamentAmount.text) > int.Parse(GameManager.Instance.Balance))
        {
            errorMsg.SetActive(true);
            errorMsg.GetComponent<Text>().text = "Insufficent amount";
            return;
        }
        else
        {
            errorMsg.SetActive(false);
        }
        GameObject.Find("createTournamentScreen").SetActive(false);
        StartCoroutine(craeteTournament_());
    }

    IEnumerator craeteTournament_()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Instance.UserID);
        form.AddField("amount", createtournamentAmount.text);
        if (twoPlayer.isOn)
        {
            totalPlayer = 2;
        }
        else
        {
            if (FourPlayer.isOn)
            {
                totalPlayer = 4;
            }
        }
        form.AddField("type", totalPlayer.ToString());
        var download = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?user/roomcode", form);
        yield return download.SendWebRequest();

        if (download.isNetworkError || download.isHttpError)
        {
            print("Error downloading: " + download.error);
        }
        else
        {
            // show the highscores
            // Debug.Log(download.downloadHandler.text);



            string userDetails = download.downloadHandler.text;
            Debug.Log("" + userDetails);

            var values = JSON.Parse(userDetails);
            if (values["status"] == "1")
            {


                StoreCreateTournamentData.user_id = values["data"]["user_id"];

                StoreCreateTournamentData.total_player = values["data"]["type"];
                StoreCreateTournamentData.amount = values["data"]["amount"];
                // StoreCreateTournamentData.created = values["data"]["created"];
                // StoreCreateTournamentData.updated = values["data"]["updated"];
                StoreCreateTournamentData.room_name = values["data"]["room_id"];
                Debug.Log("StoreCreateTournamentData.user_id " + StoreCreateTournamentData.user_id);
                //Debug.Log("StoreCreateTournamentData.u_tour_id " + StoreCreateTournamentData.u_tour_id);
                Debug.Log("StoreCreateTournamentData.total_player " + StoreCreateTournamentData.total_player);
                Debug.Log("StoreCreateTournamentData.amount " + StoreCreateTournamentData.amount);
                // Debug.Log("StoreCreateTournamentData.created" + StoreCreateTournamentData.created);
                Debug.Log("StoreCreateTournamentData.room_name " + StoreCreateTournamentData.room_name);
                GameManager.Instance.type = MyGameType.Private;
                GameManager.Instance.isPrivateTable = true;
                ShowGameConfigurationPrivate(3, int.Parse(StoreCreateTournamentData.total_player));
            }
            else
            {
                errorMsg.SetActive(true);
                errorMsg.GetComponent<Text>().text = values["msg"];
            }
        }
    }

    public class StoreCreateTournamentData
    {
        public static string u_tour_id;
        public static string user_id;
        public static string total_player;
        public static string amount;
        public static string created;
        public static string updated;
        public static string room_name;

    }


}
