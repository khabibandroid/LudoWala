using System;
using UnityEngine;
using ExitGames.Client.Photon.Chat;
using System.Collections.Generic;


public class GameManager
{
    public string BaseUrl = "https://codash.tk/ludowala/index.php?";
    public MENUmanager mainmenu;
    public enroll_tour enroll;
    public Login login;
    public bool readyToChangeTurn;
    public bool diceRolled;
    public int readyPlayersCount = 1;
    public int menuLoadCount = 0;
    public List<int> botDiceValues = new List<int>();
    public List<float> botDelays = new List<float>();
    public bool needToKillOpponentToEnterHome = false;
    public List<PlayerObject> playerObjects;
    public PlayerObject currentPlayer;
    public Sprite facebookAvatar = null;
    public bool trr;
    public bool priv;
    public bool yes;
    public bool no;
    public bool tours;
    public string Levels;
    public double Bonus;
    public double Deposites;
    //    public MyPlayerData myPlayerData = new MyPlayerData();
    public string privateRoomID;
    public string[] scenes = new string[] { "GameScene", "CheckersScene", "TheMillScene", "SoccerScene" };
    public string[] gamesNames = new string[] { "GOMOKU", "CHECKERS", "THE MILL", "SOCCER" };
    public string GameScene = "SoccerScene";
    private static GameManager instance;
    public int botLevel = 0;
    public List<Sprite> opponentsAvatars = new List<Sprite>() { null, null, null };
    public List<string> opponentsNames = new List<string>() { null, null, null };
    public List<string> opponentsIDs = new List<string>() { null, null, null };
    public GameObject myAvatarGameObject;
    public GameObject myNameGameObject;
    public int requiredPlayers = 4;
    public int firstPlayerInGame = 0;
    public int readyPlayers = 0;
    public int currentPlayersCount = 0;
    public bool offlineMode = false;
//    public AdsController adsController;
    public int myPlayerIndex = 0;
    public float playerTime = 20.0f; // player time in seconds
    public bool readyToAnimateCoins = false;
    public bool showTargetLines = false;
    public bool callPocketBlack = false;
    public bool callPocketAll = false;
    public bool LinkFbAccount = false;
    public bool inviteFriendActivated = false;
    public InitMenuScript initMenuScript;
    public string challengedFriendID;
    public GameObject tablesCanvas;
    public bool stopTimer = false;
    public bool ownSolids = false;
    public bool playersHaveTypes = false;
    public bool firstBallTouched = false;
    public bool wasFault = false;
    public bool validPot = false;
    public int validPotsCount = 0;

    public string faultMessage = "";
    public FacebookFriendsMenu facebookFriendsMenu;
    public GameObject matchPlayerObject;
    public GameObject backButtonMatchPlayers;
    public GameObject MatchPlayersCanvas;
    public GameObject reconnectingWindow;
    public GameControllerScript gameControllerScript;
    public FacebookManager facebookManager;
    public GameObject whiteBall;
    public bool testValue;
    public bool hasCueInHand = false;
    public GameObject FacebookLinkButton;
    public int shotPower;
    public bool ballsStriked = false;
    public List<String> ballTouchBeforeStrike = new List<String>();
    public GameObject ballHand;
    public bool iWon = false;
    public bool iLost = false;
    public bool iDraw = false;
    public bool calledPocket = false;
    public int solidPoted = 0;
    public int stripedPoted = 0;
    public bool noTypesPotedStriped = false;
    public bool noTypesPotedSolid = false;
    public GameObject usingCueText;
    public int ballTouchedBand = 0;
    public bool receivedInitPositions = false;
    public Vector3[] initPositions;
    public GameObject[] balls;
    public bool logged = false;
    public List<string> friendsIDForStatus = new List<string>();

    public string nameMy;
    public string nameMys = "sunil kumar";
    public Sprite avatarMy;
    public string avatarMyUrl;
    public GameObject dialog;

    public string nameOpponent;
    public Sprite avatarOpponent;

    public string opponentPlayFabID;
    public int offlinePlayerTurn = 1;
    public bool offlinePlayer1OwnSolid = true;
    public string facebookIDMy;
    public bool playerDisconnected = false;

    public GameObject invitationDialog;
    public ChatClient chatClient;

    public int coinsCount;
    public bool roomOwner = false;
    public float linesLength = 5.0f;
    public int avatarMoveSpeed = 15;
    public bool opponentDisconnected = false;
    public CueController cueController;
    public GameObject friendButtonMenu;
    public GameObject smallMenu;
    public PlayFabManager playfabManager;
    public float messageTime = 0;

    public int tableNumber = 0;
    public AudioSource[] audioSources;
    public int calledPocketID = 0;
    public GameObject coinsTextMenu;
    public GameObject coinsTextShop;
    public int cueIndex = 0;
    public int cuePower = 0;
    public int cueAim = 0;
    public int cueTime = 0;
    public IAPController IAPControl;
    public GameObject cueObject;
    public List<string[]> friendsStatuses = new List<string[]>();
    public int opponentCueIndex = 0;
    public int opponentCueTime = 0;
    public ControlAvatars controlAvatars;
  //  public InterstitialAdsControllerScript interstitialAds;
//    public AdMobObjectController adsScript;
    public ConnectionLostController connectionLost;
    public bool opponentActive = true;
    public IMiniGame miniGame;

    public bool myTurnDone = false;

	public string UserID;
	public string RoomID;
    public string GameID;
    public string PlayerName;
    public string PlayerAvatar;
    public string emaill;
    public string referal;
    public string mobile;
    public bool isPrivateTable = false;
    public string avatarIndex;
    public  string _avatarKey = "avatar";
    public string NAME;

    public bool select;

    public string Total_matches;
    public string Total_play;
    public string Wins;
    public string Skip;

    public double totalBalance;
    public Sprite opponentMy;
    public string opponentMyUrl;
    public string t_player;

    public string first;
    public string second;
    public string third;

    public string address;
    public string city;
    public string pin_code;
    public string state;
    public string status;

    public string verify_mail;
    public string pan_status;
    public string bank_status;
	public string Balance;

	public static string user_id;
    public static string key;

    public List<string> opponentsAvatarsIndex = new List<string>() { null, null, null };

   // public string GetAvatarIndex()
    //{
        //return StoreUserData.avatar_Id;
    //}

    public string invitationID = "";
    public MyGameMode mode;
    public MyGameType type;
    public bool isMyTurn = false;
    public bool diceShot = false;
    public string[] PlayersIDs;
    public bool gameSceneStarted = false;
    // Game settings

    // 50, 100, 500, 2500, 10 000, 50 000, 100 000, 250 000, 500 000, 2 500 000, 5 000 000, 10 000 000, 15 000 000
    public double payoutCoins = 15000000;
    public string bonus;
    public int index;
    public string newplayer;

    public int coins;
    public bool JoinedByID = false;

    public void resetAllData()
    {
        readyPlayersCount = 1;
        gameSceneStarted = false;
        opponentsIDs = new List<string>() { null, null, null };
        opponentsAvatars = new List<Sprite>() { null, null, null };
        opponentsNames = new List<string>() { null, null, null };
        opponentsAvatarsIndex = new List<string>() { null, null, null };
       
        readyToChangeTurn = false;
        diceRolled = false;
       
        currentPlayersCount = 0;
        myTurnDone = false;
        opponentActive = true;
        readyToAnimateCoins = false;
        opponentDisconnected = false;
        offlinePlayerTurn = 1;
        offlinePlayer1OwnSolid = true;
        offlineMode = false;
        solidPoted = 0;
        stripedPoted = 0;
        messageTime = 0.0f;
        stopTimer = false;
        ownSolids = false;
        playersHaveTypes = false;
        firstBallTouched = false;
        wasFault = false;
        validPot = false;
        validPotsCount = 0;
        faultMessage = "";
        hasCueInHand = false;
        ballsStriked = false;
        ballTouchBeforeStrike = new List<String>();
        PlayersIDs = null;



        ballTouchedBand = 0;
        receivedInitPositions = false;
    }




    private GameManager() { }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    public void resetTurnVariables()
    {
        stopTimer = false;
    }


    
}
