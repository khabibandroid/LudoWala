using UnityEngine;

//using PlayFab.ClientModels;
using System;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using System.Collections.Generic;
using ExitGames.Client.Photon.Chat;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using AssemblyCSharp;
using System.Globalization;
using System.Collections;
using SimpleJSON;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using static InitMenuScript;

public class PlayFabManager : Photon.PunBehaviour, IChatClientListener
{
    
    private Sprite[] avatarSprites;

    public string PlayFabId;
    public string authToken;
    public bool multiGame = true;
    public bool roomOwner = false;
    private FacebookManager fbManager;
    public GameObject fbButton;
    private FacebookFriendsMenu facebookFriendsMenu;
    public ChatClient chatClient;
    private bool alreadyGotFriends = false;
    public GameObject menuCanvas;
    public GameObject MatchPlayersCanvas;
    public GameObject splashCanvas;
    public bool opponentReady = false;
    public bool imReady = false;
    public GameObject playerAvatar;
    public GameObject playerName;
    public GameObject backButtonMatchPlayers;


    public GameObject loginEmail;
    public GameObject loginPassword;
    public GameObject loginInvalidEmailorPassword;
    public GameObject loginCanvas;


    public GameObject regiterEmail;
    public GameObject registerPassword;
    public GameObject registerNickname;
    public GameObject registerInvalidInput;
    public GameObject registerCanvas;

    public GameObject resetPasswordEmail;
    public GameObject resetPasswordInformationText;

    public bool isInLobby = false;
    public bool isInMaster = false;

    void Awake()
    {

        Debug.Log("Playfab awake");
        //PlayerPrefs.DeleteAll();
        PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.PhotonCloud;
        PhotonNetwork.PhotonServerSettings.PreferredRegion = CloudRegionCode.eu;
        // PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.BestRegion;
        // PhotonNetwork.PhotonServerSettings.AppID = StaticStrings.PhotonAppID;
#if UNITY_IOS
        PhotonNetwork.PhotonServerSettings.Protocol = ConnectionProtocol.Tcp;
#else
        PhotonNetwork.PhotonServerSettings.Protocol = ConnectionProtocol.Udp;
#endif
        Debug.Log("PORT: " + PhotonNetwork.PhotonServerSettings.ServerPort);

        // PlayFabSettings.TitleId = StaticStrings.PlayFabTitleID;

        PhotonNetwork.OnEventCall += this.OnEvent;
        DontDestroyOnLoad(transform.gameObject);
    }

    void OnDestroy()
    {
        PhotonNetwork.OnEventCall -= this.OnEvent;
    }

    public void destroy()
    {
        if (this.gameObject != null)
            DestroyImmediate(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log("Playfab start");
        GameManager.Instance.playfabManager = this;

        PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong;

        fbManager = GameObject.Find("facebookManager").GetComponent<FacebookManager>();
        facebookFriendsMenu = GameManager.Instance.facebookFriendsMenu;

        avatarSprites = GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>().avatars;
    }

    void Update()
    {
        if (chatClient != null) { chatClient.Service(); }
    }



    // handle events:
    private void OnEvent(byte eventcode, object content, int senderid)
    {

        Debug.Log("Received event: " + (int)eventcode + " Sender ID: " + senderid);

        if (eventcode == (int)EnumPhoton.BeginPrivateGame || eventcode == (int)EnumPhoton.StartGame)
        {
            GameManager.Instance.opponentsIDs.Clear();
            GameManager.Instance.opponentsNames.Clear();
            GameManager.Instance.opponentsAvatarsIndex.Clear();
            GameManager.Instance.opponentsAvatars.Clear();
            BinaryFormatter formatter = new BinaryFormatter();

            MemoryStream listStream = new MemoryStream(content as byte[]);

            Dictionary<string, object> data = formatter.Deserialize(listStream) as Dictionary<string, object>;
            GameManager.Instance.opponentsIDs = data["ids"] as List<string>;
            GameManager.Instance.opponentsNames = data["name"] as List<string>;
            GameManager.Instance.opponentsAvatarsIndex = data["avatar"] as List<string>;

            for (int i = 0; i < GameManager.Instance.opponentsIDs.Count; i++)
            {
                Debug.LogFormat("Event Recieved ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^   {0} :: {1} :: {2} ", GameManager.Instance.opponentsIDs[i]
               , GameManager.Instance.opponentsNames[i]
               , GameManager.Instance.opponentsAvatarsIndex[i]);
            }
           
           

            for (int i = 0; i < GameManager.Instance.opponentsAvatarsIndex.Count; i++)
                {
                Debug.Log("****************************************   " + GameManager.Instance.opponentsAvatarsIndex[i]);

                if (GameManager.Instance.opponentsAvatarsIndex[i] == "0" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "1" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "2" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "3" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "4" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "5" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "6" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "7" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "8" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "9" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "01" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "02" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "03" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "04" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "05" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "06" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "07" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "08" ||
                    GameManager.Instance.opponentsAvatarsIndex[i] == "09" || GameManager.Instance.opponentsAvatarsIndex[i] == "10" || GameManager.Instance.opponentsAvatarsIndex[i] == "11" || GameManager.Instance.opponentsAvatarsIndex[i] == "12" || GameManager.Instance.opponentsAvatarsIndex[i] == "13" || GameManager.Instance.opponentsAvatarsIndex[i] == "14" || GameManager.Instance.opponentsAvatarsIndex[i] == "15" || GameManager.Instance.opponentsAvatarsIndex[i] == "16" || GameManager.Instance.opponentsAvatarsIndex[i] == "17" || GameManager.Instance.opponentsAvatarsIndex[i] == "18" || GameManager.Instance.opponentsAvatarsIndex[i] == "19" || GameManager.Instance.opponentsAvatarsIndex[i] == "20" || GameManager.Instance.opponentsAvatarsIndex[i] == "21")
                {
                    GameManager.Instance.opponentsAvatars.Add(GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>().avatars[int.Parse(GameManager.Instance.opponentsAvatarsIndex[i])]);
                }
                else
                {

                    GameManager.Instance.opponentsAvatars.Add(GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>().urls[i]);
                }
            }
            //StartGame(); alreay commented
            LoadGameScene();
        }
        else if (eventcode == (int)EnumPhoton.StartWithBots && senderid != PhotonNetwork.player.ID)
        {
            LoadBots();
        }
        else if (eventcode == (int)EnumPhoton.ReadyToPlay)
        {
            GameManager.Instance.readyPlayersCount++;
            //LoadGameScene();
        }

    }

    public void LoadGameWithDelay()
    {
        LoadGameScene();
    }

    public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        Debug.Log("Custom debug: mastClientSwitched called in playfabmanager");
        if (GameManager.Instance.controlAvatars != null && GameManager.Instance.type == MyGameType.Private)
        {
            /*PhotonNetwork.LeaveRoom();
            GameManager.Instance.controlAvatars.ShowJoinFailed("Room closed");*/
        }
        else
        {
            if (newMasterClient.NickName == PhotonNetwork.player.NickName)
            {
                /*if (GameManager.Instance.type == MyGameType.Private)
                {
                    Debug.Log("private opponenet left");
                    GameGUIController.GetInstance().StopAndFinishGame();
                }
                else*/
                /*{
                    Debug.Log("Im new master client");
                    WaitForNewPlayer();
                }*/
               
            }
        }

    }



    public void StartGame()
    {
        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;

        CancelInvoke("StartGameWithBots");
        Invoke("startGameScene", 3.0f);
    }

    private IEnumerator waitAndStartGame()
    {
        Debug.Log("required " + GameManager.Instance.requiredPlayers);
        while (GameManager.Instance.readyPlayers < GameManager.Instance.requiredPlayers - 1 || !imReady /*|| (!GameManager.Instance.roomOwner && !GameManager.Instance.receivedInitPositions)*/)
        {
            yield return 0;
        }
        startGameScene();
        GameManager.Instance.readyPlayers = 0;
        opponentReady = false;
        imReady = false;
    }

    public void startGameScene()
    {
        if (GameManager.Instance.currentPlayersCount >= GameManager.Instance.requiredPlayers || GameManager.Instance.type == MyGameType.Private)
        {

            LoadGameScene();
            Debug.Log("opponentsIDs : " + GameManager.Instance.opponentsIDs.Count);
            Debug.Log("opponentsNames : " + GameManager.Instance.opponentsNames.Count);
            Debug.Log("opponentsAvatars : " + GameManager.Instance.opponentsAvatars.Count);
            Debug.Log("opponentsAvatarsIndex : " + GameManager.Instance.opponentsAvatarsIndex.Count);
            GameManager.Instance.opponentsIDs.Insert(0, GameManager.Instance.UserID);
            if (GameManager.Instance.nameMy == "")
                GameManager.Instance.nameMy.Insert(0, "Player");
            else
                GameManager.Instance.opponentsNames.Insert(0, GameManager.Instance.nameMy);
            GameManager.Instance.opponentsAvatars.Insert(0, GameManager.Instance.avatarMy);

            GameManager.Instance.opponentsAvatarsIndex.Insert(0,  GameManager.Instance.avatarIndex);
            Debug.Log("startGameScene GameManager.Instance.opponentsAvatarsIndex " + GameManager.Instance.avatarIndex);

            for (int i = 0; i < GameManager.Instance.opponentsAvatarsIndex.Count; i++)
                Debug.Log("**************!!!!!!!!!!!!!!!!!     "+GameManager.Instance.opponentsAvatarsIndex[i]);

            for (int i = 0; i < GameManager.Instance.opponentsIDs.Count; i++)
            {
                Debug.LogFormat("Event Sending ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^   {0} :: {1} :: {2} ", GameManager.Instance.opponentsIDs[i]
               , GameManager.Instance.opponentsNames[i]
               , GameManager.Instance.opponentsAvatarsIndex[i]);
            }

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("ids", GameManager.Instance.opponentsIDs);
            data.Add("name", GameManager.Instance.opponentsNames);
            data.Add("avatar", GameManager.Instance.opponentsAvatarsIndex);
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream listStream = new MemoryStream();
            formatter.Serialize(listStream, data);
            byte[] listBytes = listStream.ToArray();
            //RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            if (GameManager.Instance.type == MyGameType.Private)
            {
                
                PhotonNetwork.RaiseEvent((int)EnumPhoton.BeginPrivateGame, listBytes, true, null);
            }
            else
            {
                PhotonNetwork.RaiseEvent((int)EnumPhoton.StartGame, listBytes, true, null);
            }

        }
        else
        {
            if (PhotonNetwork.isMasterClient)
                WaitForNewPlayer();
        }
    }


    public void LoadGameScene()
    {
        GameManager.Instance.GameScene = "GameScene";

        if (!GameManager.Instance.gameSceneStarted)
        {
            SceneManager.LoadScene(GameManager.Instance.GameScene);
            GameManager.Instance.gameSceneStarted = true;
        }
        Debug.Log("Loading Game Scene **********");
    }



    public void WaitForNewPlayer()
    {
        Debug.Log("WaitForNewPlayer");
        if (PhotonNetwork.isMasterClient && GameManager.Instance.type != MyGameType.Private)
        {
            if (GameManager.Instance.type == MyGameType.comp)
            {
                Debug.Log("START INVOKE");
                CancelInvoke("StartGameWithBots");
                Invoke("StartGameWithBots", StaticStrings.WaitTimeUntilStartWithBots);

            }
            else
            {
                Debug.Log("START INVOKE");
                CancelInvoke("StartGameWithBots");
                Invoke("StartGameWithBots", StaticStrings.WaitTimeUntilCloseRoom);
            }
        }
    }

    public void StartGameWithBots()
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (PhotonNetwork.room.PlayerCount < GameManager.Instance.requiredPlayers)
            {
                Debug.Log("Master Client");
                // PhotonNetwork.RaiseEvent((int)EnumPhoton.StartWithBots, null, true, null);
                LoadBots();
            }
        }
        else
        {
            Debug.Log("Not Master client");
        }
    }

    public void LoadBots()
    {
        Debug.Log("Close room - add bots");
        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;

        if (PhotonNetwork.isMasterClient)
        {
            Invoke("AddBots", 3.0f);
        }
        else
        {
            AddBots();
        }

    }

    public void AddBots()
    {
        // Add Bots here

        Debug.Log("Add Bots with delay");

        if (PhotonNetwork.room.PlayerCount < GameManager.Instance.requiredPlayers)
        {

            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.RaiseEvent((int)EnumPhoton.StartWithBots, null, true, null);
            }

            for (int i = 0; i < GameManager.Instance.requiredPlayers - 1; i++)
            {
                if (GameManager.Instance.opponentsIDs[i] == null)
                {
                    StartCoroutine(AddBot(i));
                }
            }
        }
    }



    List<string> BotName = new List<string>() {
        "Akrosh",
        "Neera",
        "Avinash",
        "Kalpit",
        "Syed",
        "Mohsin",
        "Javed",
        "Monu",
        "Akram",
        "Loki",
        "Krishna",
        "Mukesh",
        "LalChand",
        "Vipin",
        "Salman",
        "Shaeed",
        "Girajj",
        "Puneet",
        "Yash",
        "Sohail",
        "Yusuf",
        "Sahzad",
        "Hari",
        "Rochin",
        "Akhil",
        "Akash",
        "Nikhil",
        "Abhi",
        "Chandu",
        "Shiv",
        "Mahdev",
        "Vishnu",
        "Bhola",
        "Raju",
        "Tushar",
        "Mahesh",
        "Ramesh",
        "Suresh",
        "Mohit",
        "Sohib",
        "Raj",
        "Akshay",
        "Sourabh",
        "Hemant",
        "Ajay",
        "Alok",
        "Rahul",
        "Manish",
        "Aslam",
        "Sharukh",
        "Yunush",
        "Surender",
        "Pukhraj",
        "Pramit",
        "Mayank",
        "Varun",
        "Deepak",
        "Deepankar",
        "Munna",
        "Vinni",
        "Binni",
        "Kosi",
        "Vimal",
        "Gurpreet",
        "Guru",
        "Gurri",
        "Gurvinder",
        "Monu",
        "Sonu",
        "Golu",
        "Baban",
        "Babal",
        "Shovik",
        "Navin",
        "Charan",
        "Shivam",
        "Neeraj",
        "Arvind",
        "Narender",
        "Lokender",
        "Kuldeep",
        "Virender",
        "Vijender",
        "Nitin",
        "Ankit",
        "Rajkumar",
        "Deepak",
        "Kamal",
        "Love",
        "Anshul",
        "Vinod",
        "Rishab",
        "Brijesh",
        "Badal",
        "Dilip",
        "Rinku",
        "Dinesh",
        "Rakesh",
        "Anil",
        "Aarif",
        "pooja",
        "priya",
        "parul",
        "sakshi",
        "Anjli",
        "susila",
        "Rinki",
        "monika",
        "priya",
        "Riya",
        "rohini",
        "mohini",
        "deepika",
        "Rani",
        "kanika",
        "komal",
        "megaha",
        "kushboo",
        "ragini"
    };


    public IEnumerator AddBot(int i)
    {
        yield return new WaitForSeconds(i + UnityEngine.Random.Range(0.0f, 0.9f));

        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("Sprite " + avatarSprites);
            int sprite = UnityEngine.Random.Range(0, avatarSprites.Length - 1);

            GameManager.Instance.opponentsAvatars[i] = avatarSprites[sprite];
            GameManager.Instance.opponentsAvatarsIndex[i] = sprite.ToString();
            GameManager.Instance.opponentsIDs[i] = "_BOT" + i;
            if(GameManager.Instance.type==MyGameType.comp)
            {
                GameManager.Instance.opponentsNames[i] = "CPU";
                GameManager.Instance.NAME = "CPU";
            }
            else {
                GameManager.Instance.opponentsNames[i] = BotName[UnityEngine.Random.Range(0, BotName.Count)];
                GameManager.Instance.NAME = BotName[UnityEngine.Random.Range(0, BotName.Count)];
            }

            Debug.Log("Name: " + GameManager.Instance.opponentsNames[i]);
            GameManager.Instance.controlAvatars.PlayerJoined(i, "_BOT" + i);
            string content = sprite + ";" + i + ";" + GameManager.Instance.opponentsNames[i];
            PhotonNetwork.RaiseEvent((int)EnumPhoton.StartWithBots, content, true, RaiseEventOptions.Default);
        }

    }


    public void startScene() => StartCoroutine(loadSceneMenu());


    private IEnumerator loadSceneMenu()
    {
        yield return new WaitForSeconds(0.1f);

        if (isInMaster && isInLobby)
        {
            SceneManager.LoadScene("MenuScene");
        }
        else
        {
            StartCoroutine(loadSceneMenu());
        }

    }


    private string androidUnique()
    {
        AndroidJavaClass androidUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityPlayerActivity = androidUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject unityPlayerResolver = unityPlayerActivity.Call<AndroidJavaObject>("getContentResolver");
        AndroidJavaClass androidSettingsSecure = new AndroidJavaClass("android.provider.Settings$Secure");
        return androidSettingsSecure.CallStatic<string>("getString", unityPlayerResolver, "android_id");
    }

    // #######################  PHOTON  ##########################


    public void PhotonLogin(string id)
    {
        this.PlayFabId = id;
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
        PhotonNetwork.AuthValues.AddAuthParameter("username", this.PlayFabId);
        PhotonNetwork.AuthValues.UserId = this.PlayFabId;
        PhotonNetwork.ConnectUsingSettings("1.4");
        PhotonNetwork.playerName = this.PlayFabId;
        connectToChat();
    }


    public void connectToChat()
    {
        Debug.Log("");

        chatClient = new ChatClient(this);
        GameManager.Instance.chatClient = chatClient;
        ExitGames.Client.Photon.Chat.AuthenticationValues authValues = new ExitGames.Client.Photon.Chat.AuthenticationValues();
        authValues.UserId = this.PlayFabId;
        authValues.AuthType = ExitGames.Client.Photon.Chat.CustomAuthenticationType.Custom;
        authValues.AddAuthParameter("username", this.PlayFabId);
        authValues.AddAuthParameter("Token", authToken);
        chatClient.Connect(StaticStrings.PhotonChatID, "1.4", authValues);

        Debug.Log("Token" + authToken);
    }


    public override void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {

        Debug.Log("Custom properties changed: " + DateTime.Now.ToString());
    }


    public void OnConnected()
    {
        Debug.Log("Photon Chat connected!!!");
        chatClient.Subscribe(new string[] { "invitationsChannel" });
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("Custom debug: onphotonplayerdis called in playfabmanager");
        if (true || EventCounter.hasGameBegan())
            return;
        Debug.Log("Custom debug: onphotonplayerdis called in playfabmanager part 2");
        GameManager.Instance.opponentDisconnected = true;

        GameManager.Instance.invitationID = "";

        if (GameManager.Instance.controlAvatars != null)
        {
            Debug.Log("PLAYER DISCONNECTED " + player.NickName);
            if (PhotonNetwork.room.PlayerCount > 1)
            {
                GameManager.Instance.controlAvatars.startButtonPrivate.GetComponent<Button>().interactable = true;
            }
            else
            {
                GameManager.Instance.controlAvatars.startButtonPrivate.GetComponent<Button>().interactable = false;
            }
            PhotonNetwork.room.IsOpen = true;
            PhotonNetwork.room.IsVisible = true;
            WaitForNewPlayer();
            CancelInvoke("startGameScene");
            int index = GameManager.Instance.opponentsIDs.IndexOf(player.NickName);
            //PhotonNetwork.room.IsOpen = true;
            GameManager.Instance.controlAvatars.PlayerDisconnected(index);
        }
    }

    public void showMenu()
    {
        menuCanvas.gameObject.SetActive(true);

        playerName.GetComponent<Text>().text = GameManager.Instance.nameMy;

        if (GameManager.Instance.avatarMy != null)
            playerAvatar.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;

        splashCanvas.SetActive(false);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("Subscribed to CHAT - set online status!");
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }


    public void challengeFriend(string id, string message)
    {
        //if (GameManager.Instance.invitationID.Length == 0 || !GameManager.Instance.invitationID.Equals(id))
        //{
        chatClient.SendPrivateMessage(id, "INVITE_TO_PLAY_PRIVATE;" + /*id + this.PlayFabId + ";" +*/ GameManager.Instance.nameMy + ";" + message);
        GameManager.Instance.invitationID = id;
        Debug.Log("Send invitation to: " + id);
        // }
    }

    string roomname;
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        if (!sender.Equals(this.PlayFabId))
        {
            if (message.ToString().Contains("INVITE_TO_PLAY_PRIVATE"))
            {
                GameManager.Instance.invitationID = sender;

                string[] messageSplit = message.ToString().Split(';');
                string whoInvite = messageSplit[1];
                string payout = messageSplit[2];
                string roomID = messageSplit[3];
                GameManager.Instance.payoutCoins = int.Parse(payout);
                GameManager.Instance.invitationDialog.GetComponent<PhotonChatListener>().showInvitationDialog(0, whoInvite, payout, roomID, 0);
            }
        }

        if ((GameManager.Instance.invitationID.Length == 0 || !GameManager.Instance.invitationID.Equals(sender)))
        {

        }
        else
        {
            GameManager.Instance.invitationID = "";
        }
    }

    public void join()
    {
        PhotonNetwork.JoinRoom(roomname);
    }

    public void DebugReturn(DebugLevel level, string message)
    {

    }

    public void OnChatStateChange(ChatState state)
    {

    }

    private bool rejoin; // reset it to false elsewhere
    
    public override void OnConnectionFail(DisconnectCause cause)
    {
        if (PhotonNetwork.Server == ServerConnection.GameServer)
        {
            switch (cause)
            {
                // add other disconnect causes that could happen while joined
                case DisconnectCause.DisconnectByClientTimeout:
                    rejoin = true;
                    break;
                default:
                    rejoin = true;
                    break;
            }
        }
    }


    public override void OnDisconnectedFromPhoton()  // call at user end which was dissconnected / DINESH  need to add reconnect code here
    {
        Debug.Log("Disconnected from photon");
        //GameGUIController.GetInstance().network.SetActive(true);
        // StartCoroutine(CheckReconnection((value)=> { if (!value) { switchUser(); } }));   // Lalit
        //if (rejoin)
        {
            if (!EventCounter.LogOut)
            {
                if (!PhotonNetwork.ReconnectAndRejoin())
                {
                    Debug.Log("ReconnectAndRejoin");
                    Debug.LogError("Error trying to reconnect and rejoin");
                    // PhotonNetwork.Reconnect();
                    // switchUser();
                    if (PhotonNetwork.Reconnect())
                    {
                        EventCounter.reconnected = true;
                        Debug.Log("Successful reconnected!", this);
                    }
                    else
                    {
                        Debug.Log("Failed reconnecting and joining!!", this);
                        switchUser();
                    }
                }
            }
            else
            {
                Debug.Log("Reconnect");
               // PhotonNetwork.Reconnect();
                Debug.LogError("rejoin");
                EventCounter.reconnected = true;
            }
        }
    }

    public void DisconnecteFromPhoton()
    {
        PhotonNetwork.Disconnect();
    }

    public void switchUser()
    {
        GameManager.Instance.playfabManager.destroy();
        GameManager.Instance.facebookManager.destroy();
       // GameManager.Instance.connectionLost.destroy();
        GameManager.Instance.avatarMy = null;
        GameManager.Instance.logged = false;
        GameManager.Instance.resetAllData();
        SceneManager.LoadScene("LoginScene");
    }

    public void OnDisconnected()
    {
        Debug.Log("Chat disconnected - Reconnect");
        connectToChat();
    }



    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {

    }

    public void OnUnsubscribed(string[] channels)
    {

    }


    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("STATUS UPDATE CHAT!");
        Debug.Log("Status change for: " + user + " to: " + status);

        bool foundFriend = false;
        for (int i = 0; i < GameManager.Instance.friendsStatuses.Count; i++)
        {
            string[] friend = GameManager.Instance.friendsStatuses[i];
            if (friend[0].Equals(user))
            {
                GameManager.Instance.friendsStatuses[i][1] = "" + status;
                foundFriend = true;
                break;
            }
        }

        if (!foundFriend)
        {
            GameManager.Instance.friendsStatuses.Add(new string[] { user, "" + status });
        }

        if (GameManager.Instance.facebookFriendsMenu != null)
            GameManager.Instance.facebookFriendsMenu.updateFriendStatus(status, user);
    }

    public override void OnConnectedToMaster()
    {
        isInMaster = true;
        Debug.Log("Connected to master");
        
        if (SceneManager.GetActiveScene().name != "MenuScene")
        {
            startScene();
        }

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"Joined lobby: CountOfPlayersInLobby: {PhotonNetwork.countOfPlayersOnMaster} PlayerName: {PhotonNetwork.playerName} Player: {PhotonNetwork.player}");
        isInLobby = true;
    }


    public override void OnReceivedRoomListUpdate()
    {
        Debug.LogError($"RoomListUpdateAfterJoinedLobby");
        RoomInfo[] roomList = PhotonNetwork.GetRoomList();
        foreach (var item in roomList)
        {
            Debug.LogError($"RoomName: {item.Name}, PlayerCount: {item.PlayerCount}, CustomProperties {item.CustomProperties}");
        }
    }




    public void JoinRoomAndStartGame()
    {
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {
            {"m", GameManager.Instance.mode.ToString() +  GameManager.Instance.type.ToString() + GameManager.Instance.payoutCoins.ToString()}
         };

        StartCoroutine(TryToJoinRandomRoom(expectedCustomRoomProperties));
        //PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    private int _Maxplayers;
    public void JoinRoomAndStartGamePrivate(int player)
    {

        _Maxplayers = (byte)player;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {
            {"m", GameManager.Instance.mode.ToString() +  GameManager.Instance.type.ToString() + GameManager.Instance.payoutCoins.ToString() }
        };



        setupBotForJoinOrCreateRoom(player);
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, (byte)_Maxplayers);

        //PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }
    public void setupBotForJoinOrCreateRoom(int players)
    {

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomPropertiesForLobby = new String[] { "m", "v" };
        roomOptions.PlayerTtl = 1000 * 60 * 60;
        roomOptions.EmptyRoomTtl = 300000;




        string BotMoves = generateBotMoves();

        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {
            { "m", GameManager.Instance.mode.ToString() +  GameManager.Instance.type.ToString() + GameManager.Instance.payoutCoins.ToString()},
            {"bt", BotMoves},
            {"fp", 0}
        };

        Debug.Log("Create Room: " + GameManager.Instance.mode.ToString() + GameManager.Instance.type.ToString() + GameManager.Instance.payoutCoins.ToString());
        roomOptions.MaxPlayers = (byte)players;
        //roomOptions.IsVisible = true;
    }


    public IEnumerator TryToJoinRandomRoom(ExitGames.Client.Photon.Hashtable roomOptions)
    {
        while (true)
        {
            if (isInLobby && isInMaster)
            {
                PhotonNetwork.JoinRandomRoom(roomOptions, 0);
                break;
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
    }


    public void OnPhotonRandomJoinFailed()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.PlayerTtl = 1000 * 60 * 60;
        roomOptions.EmptyRoomTtl = 300000;
        roomOptions.CustomRoomPropertiesForLobby = new String[] { "m", "v" };




        string BotMoves = generateBotMoves();

        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {
            { "m", GameManager.Instance.mode.ToString() +  GameManager.Instance.type.ToString() + GameManager.Instance.payoutCoins.ToString()},
            {"bt", BotMoves},
            {"fp", UnityEngine.Random.Range(0, GameManager.Instance.requiredPlayers)}
         };

        Debug.Log("*************************************Create Room: " + GameManager.Instance.mode.ToString() + GameManager.Instance.type.ToString() + GameManager.Instance.payoutCoins.ToString());
        roomOptions.MaxPlayers = (byte)GameManager.Instance.requiredPlayers;
        //roomOptions.IsVisible = true;

        StartCoroutine(TryToCreateGameAfterFailedToJoinRandom(roomOptions));

    }



    public string generateBotMoves()
    {
        // Generate BOT moves
        string BotMoves = "";
        int BotCount = 100;
        // Generate dice values
        for (int i = 0; i < BotCount; i++)
        {
            BotMoves += (UnityEngine.Random.Range(1, 7)).ToString();
            if (i < BotCount - 1)
            {
                BotMoves += ",";
            }
        }

        BotMoves += ";";

        // Generate delays
        float minValue = GameManager.Instance.playerTime / 10;
        if (minValue < 1.5f) minValue = 1.5f;
        for (int i = 0; i < BotCount; i++)
        {
            BotMoves += (UnityEngine.Random.Range(minValue, GameManager.Instance.playerTime / 8)).ToString();
            if (i < BotCount - 1)
            {
                BotMoves += ",";
            }
        }
        return BotMoves;
    }

    public void extractBotMoves(string data)
    {
        GameManager.Instance.botDiceValues = new List<int>();
        GameManager.Instance.botDelays = new List<float>();
        string[] d1 = data.Split(';');


        string[] diceValues = d1[0].Split(',');
        for (int i = 0; i < diceValues.Length; i++)
        {
            GameManager.Instance.botDiceValues.Add(int.Parse(diceValues[i]));
        }

        string[] delays = d1[1].Split(',');
        for (int i = 0; i < delays.Length; i++)
        {
            GameManager.Instance.botDelays.Add(float.Parse(delays[i]));
        }
    }

    public override void OnLeftLobby()
    {
        isInLobby = false;
        isInMaster = false;
    }

    public IEnumerator TryToCreateGameAfterFailedToJoinRandom(RoomOptions roomOptions)
    {
        roomOptions.PlayerTtl = 1000 * 60 * 60;
        roomOptions.EmptyRoomTtl = 300000;
        while (true)
        {
            if (isInLobby && isInMaster)
            {
                PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);


                break;
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
    }


    public override void OnJoinedRoom()
    {

        Debug.Log("OnJoinedRoom");


        if (PhotonNetwork.room.CustomProperties.ContainsKey("bt"))
        {
            extractBotMoves(PhotonNetwork.room.CustomProperties["bt"].ToString());
        }

        if (PhotonNetwork.room.CustomProperties.ContainsKey("fp"))
        {
            GameManager.Instance.firstPlayerInGame = int.Parse(PhotonNetwork.room.CustomProperties["fp"].ToString());
        }
        else
        {
            GameManager.Instance.firstPlayerInGame = 0;
        }



        GameManager.Instance.avatarOpponent = null;


        Debug.Log("Players in room " + PhotonNetwork.room.PlayerCount);
        Debug.Log("Players in roomid " + PhotonNetwork.room.Name);

        GameManager.Instance.RoomID = PhotonNetwork.room.Name;

        GameManager.Instance.currentPlayersCount = 1;


        if (PhotonNetwork.room.PlayerCount == 1)
        {
            GameManager.Instance.roomOwner = true;
            WaitForNewPlayer();
        }
        else if (PhotonNetwork.room.PlayerCount >= GameManager.Instance.requiredPlayers)
        {
            PhotonNetwork.room.IsOpen = false;
            PhotonNetwork.room.IsVisible = false;
        }

        if (!roomOwner)
        {
            GameManager.Instance.backButtonMatchPlayers.SetActive(false);

            for (int i = 0; i < PhotonNetwork.otherPlayers.Length; i++)
            {

                int ii = i;
                int index = GetFirstFreeSlot();
                GameManager.Instance.opponentsIDs[index] = PhotonNetwork.otherPlayers[ii].NickName;

                /*  GetUserDataRequest getdatarequest = new GetUserDataRequest()
                  {
                      PlayFabId = PhotonNetwork.otherPlayers[ii].NickName,

                  };*/

                string otherID = PhotonNetwork.otherPlayers[ii].NickName;
                StartCoroutine(GetUserProfile(index, otherID));



                /*  PlayFabClientAPI.GetUserData(getdatarequest, (result) =>
                  {
                      Dictionary<string, UserDataRecord> data = result.Data;

                      if (data.ContainsKey("LoggedType"))
                      {
                          if (data["LoggedType"].Value.Equals("Facebook"))
                          {
                              bool fbAvatar = true;
                              int avatarIndex = 0;
                              if (!data[MyPlayerData.AvatarIndexKey].Value.Equals("fb"))
                              {
                                  fbAvatar = false;
                                  avatarIndex = int.Parse(data[MyPlayerData.AvatarIndexKey].Value.ToString());
                              }
                              getOpponentData(data, index, fbAvatar, avatarIndex, otherID);
                          }
                          else
                          {
                              if (data.ContainsKey("PlayerName"))
                              {
                                  GameManager.Instance.opponentsNames[index] = data["PlayerName"].Value;
                                  //GameManager.Instance.controlAvatars.PlayerJoined(index);
                                  bool fbAvatar = true;
                                  int avatarIndex = 0;
                                  if (!data[MyPlayerData.AvatarIndexKey].Value.Equals("fb"))
                                  {
                                      fbAvatar = false;
                                      avatarIndex = int.Parse(data[MyPlayerData.AvatarIndexKey].Value.ToString());
                                  }
                                  getOpponentData(data, index, fbAvatar, avatarIndex, otherID);
                              }
                              else
                              {
                                  Debug.Log("ERROR");
                              }
                          }
                      }
                      else
                      {
                          Debug.Log("ERROR");
                      }

                  }, (error) =>
                  {
                      Debug.Log("Get user data error: " + error.ErrorMessage);
                  }, null);*/
            }
        }
    }




    public void CreatePrivateRoom()
    {
        GameManager.Instance.JoinedByID = false;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;


        string roomName = "";
        for (int i = 0; i < 8; i++)
        {
            roomName = roomName + UnityEngine.Random.Range(0, 10);
        }

        roomOptions.CustomRoomPropertiesForLobby = new String[] { "pc" };
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {
            { "pc", GameManager.Instance.payoutCoins}
         };
        Debug.Log("Private room name: " + roomName);
        roomOptions.PlayerTtl = 1000 * 60 * 60;
        roomOptions.EmptyRoomTtl = 300000;
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }


    public void CreatePrivateRoomById()
    {
        GameManager.Instance.JoinedByID = true;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = byte.Parse(StoreCreateTournamentData.total_player);

        string roomName = StoreCreateTournamentData.room_name;
        //for (int i = 0; i < 8; i++)
        //{
        //    roomName = roomName + UnityEngine.Random.Range(0, 10);
        //}
        GameManager.Instance.payoutCoins = int.Parse(StoreCreateTournamentData.amount);
        roomOptions.CustomRoomPropertiesForLobby = new String[] { "pc" };
        roomOptions.PlayerTtl = 1000 * 60 * 60;
        roomOptions.EmptyRoomTtl = 300000;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {
            { "pc", GameManager.Instance.payoutCoins},


            { "fp", UnityEngine.Random.Range(0, int.Parse(StoreCreateTournamentData.total_player))}
         };
        Debug.Log("Private room name: " + roomName);
        Debug.Log("Private room players: " + int.Parse(StoreCreateTournamentData.total_player));
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }


    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        roomOwner = true;
        GameManager.Instance.roomOwner = true;
        GameManager.Instance.currentPlayersCount = 1;
        GameManager.Instance.controlAvatars.updateRoomID(PhotonNetwork.room.Name);
        GameManager.Instance.RoomID = PhotonNetwork.room.Name;
        Debug.Log("Room ID: " + PhotonNetwork.room.Name);
    }

    public override void OnLeftRoom()
    {
        if (true || EventCounter.hasGameBegan())
            return;
        Debug.Log("OnLeftRoom called");
        roomOwner = false;
        GameManager.Instance.roomOwner = false;
        GameManager.Instance.resetAllData();
    }

    public int GetFirstFreeSlot()
    {
        int index = 0;
        for (int i = 0; i < GameManager.Instance.opponentsIDs.Count; i++)
        {
            if (GameManager.Instance.opponentsIDs[i] == null)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Failed to create room");
        CreatePrivateRoom();
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Failed to join room");

        if (GameManager.Instance.type == MyGameType.Private)
        {
            if (GameManager.Instance.controlAvatars != null)
            {
                GameManager.Instance.controlAvatars.ShowJoinFailed(codeAndMsg[1].ToString());
            }
        }
        else
        {
            GameManager.Instance.facebookManager.startRandomGame();
        }
    }

    private void GetPlayerDataRequest(string playerID)
    {

    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        if (SceneManager.GetActiveScene().name.Equals("MenuScene"))
        {
            CancelInvoke("StartGameWithBots");

            Debug.Log("New player joined " + newPlayer.NickName);
            Debug.Log("Players Count: " + GameManager.Instance.currentPlayersCount);



            if (PhotonNetwork.room.PlayerCount >= GameManager.Instance.requiredPlayers)
            {
                PhotonNetwork.room.IsOpen = false;
                PhotonNetwork.room.IsVisible = true;
            }

            int index = GetFirstFreeSlot();
            GameManager.Instance.index = GetFirstFreeSlot();
            GameManager.Instance.newplayer = newPlayer.NickName;

            GameManager.Instance.opponentsIDs[index] = newPlayer.NickName;
            StartCoroutine(GetUserProfile(index, newPlayer.NickName));

        }


    }






    public IEnumerator GetUserProfile(int index, string id)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", id);
        var download = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?user/user_byid", form);
        download.chunkedTransfer = false;
        yield return download.SendWebRequest();

        if (download.isNetworkError || download.isHttpError)
        {
            Debug.Log("Error downloading: " + download.error);
        }

        else
            // show the highscores
            Debug.Log(download.downloadHandler.text);
        var data = JSON.Parse(download.downloadHandler.text);
        Debug.Log(data["response"][0]["avatar"].Value);
        GameManager.Instance.opponentMyUrl = data["response"][0]["avatar"].Value;

        if (data["response"][0]["name"].Value == "")
            GameManager.Instance.opponentsNames[index] = "ludo_dream";

        else
            GameManager.Instance.opponentsNames[index] = data["response"][0]["name"].Value;

        if (GameManager.Instance.opponentMyUrl == "0" || GameManager.Instance.opponentMyUrl == "1" || GameManager.Instance.opponentMyUrl == "00" || GameManager.Instance.opponentMyUrl == "01" || GameManager.Instance.opponentMyUrl == "02" || GameManager.Instance.opponentMyUrl == "03" || GameManager.Instance.opponentMyUrl == "04" || GameManager.Instance.opponentMyUrl == "05" || GameManager.Instance.opponentMyUrl == "06" || GameManager.Instance.opponentMyUrl == "07" || GameManager.Instance.opponentMyUrl == "08" || GameManager.Instance.opponentMyUrl == "09" || GameManager.Instance.opponentMyUrl == "10" || GameManager.Instance.opponentMyUrl == "11" || GameManager.Instance.opponentMyUrl == "12" || GameManager.Instance.opponentMyUrl == "13" || GameManager.Instance.opponentMyUrl == "14" || GameManager.Instance.opponentMyUrl == "15" || GameManager.Instance.opponentMyUrl == "16" || GameManager.Instance.opponentMyUrl == "17" || GameManager.Instance.opponentMyUrl == "18" || GameManager.Instance.opponentMyUrl == "19" || GameManager.Instance.opponentMyUrl == "20" || GameManager.Instance.opponentMyUrl == "21")
        {
            Debug.Log("GameManager.Instance.opponentMyUrl " + GameManager.Instance.opponentMyUrl);


            GameManager.Instance.opponentsAvatars[index] = StaticGameVariablesController.instance.avatars[int.Parse(data["response"][0]["avatar"].Value)];


            if (GameManager.Instance.opponentsAvatarsIndex.Count > index)
                GameManager.Instance.opponentsAvatarsIndex[index] = data["response"][0]["avatar"].Value;
            else
                GameManager.Instance.opponentsAvatarsIndex.Add(data["response"][0]["avatar"].Value);


            GameManager.Instance.controlAvatars.PlayerJoined(index, id);
        }
        else
        {
            // show the highscores


            StartCoroutine(loadImageOpponent(GameManager.Instance.opponentMyUrl, index, id));



            if (GameManager.Instance.opponentsAvatarsIndex.Count > index)
                GameManager.Instance.opponentsAvatarsIndex[index] = data["response"][0]["avatar"].Value;
            else
                GameManager.Instance.opponentsAvatarsIndex.Add(data["response"][index]["avatar"].Value);


        }
    }



    public IEnumerator loadImageOpponent(string url, int index, string id)
    {
        WWW www = new WWW(url);

        yield return www;



        GameManager.Instance.opponentsAvatars[index] = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f), 32);
        GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>().urls[index] = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f), 32);
        GameManager.Instance.controlAvatars.PlayerJoined(index, id);
      
    }


    // Lalit // disconnect
    //private void OnApplicationPause(bool isPaused)
    //{
    //    if (isPaused)
    //    {
    //        PhotonNetwork.Disconnect();
    //        StartCoroutine(CheckReconnection((value)=> { }));
    //    }
    //}

    //IEnumerator CheckReconnection(System.Action<bool> callback)
    //{
    //    while (PhotonNetwork.networkingPeer.PeerState == PeerStateValue.Disconnected)
    //    {
    //        yield return new WaitForEndOfFrame();
    //    }
    //    if (!PhotonNetwork.ReconnectAndRejoin())
    //    {
    //        //if (PhotonNetwork.Reconnect())
    //        //{

    //        //    Debug.Log("Successful reconnected!", this);
    //        //}
    //        //else
    //        //{
    //        //    Debug.Log("Failed reconnecting and joining!!", this);
    //        //}
    //        callback(false);
    //    }
    //    else
    //    {
    //        Debug.Log("Successful reconnected and joined!", this);
    //        callback(true);
    //    }
    //}


    
}
