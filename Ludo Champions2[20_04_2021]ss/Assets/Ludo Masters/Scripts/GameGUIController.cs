using System;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

using Photon;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static InitMenuScript;

public class GameGUIController : PunBehaviour
{
    private Sprite[] avatarSprites;

    [Header("prize")]
    public Text first;
    public Text second;
    public Text third;

    [Header("player3 Red Object")]
    public GameObject red1;
    public GameObject red2;
    public GameObject red3;
    [Header("player2 Red Object")]
    public GameObject red12;
    public GameObject red22;
    public GameObject red32;
    [Header("player 4 Red Object")]
    public GameObject red14;
    public GameObject red24;
    public GameObject red34;

    public GameObject network;

    public GameObject TIPButtonObject;
    public GameObject TIPObject;
    public GameObject firstPrizeObject;
    public GameObject SecondPrizeObject;
    public GameObject firstPrizeText;
    public GameObject secondPrizeText;

    public AudioSource WinSound;
    public AudioSource myTurnSource;
    public AudioSource oppoTurnSource;
    private bool AllPlayersReady = false;
    // LUDO
    public MultiDimensionalGameObject[] PlayersPawns;
    public GameObject[] PlayersDices;
    public GameObject[] HomeLockObjects;


    [System.Serializable]
    public class MultiDimensionalGameObject
    {
        public GameObject[] objectsArray;
    }

    public GameObject ludoBoard;
    public GameObject[] diceBackgrounds;
    public MultiDimensionalGameObject[] playersPawnsColors;
    public MultiDimensionalGameObject[] playersPawnsMultiple;
    private Color colorRed = new Color(250.0f / 255.0f, 12.0f / 255, 12.0f / 255);
    private Color32 colorIrisBlue = new Color(0, 190, 236, 255);
    private Color colorYellow = new Color(255.0f / 255.0f, 163.0f / 255, 0);
    private Color colorGreen = new Color(8.0f / 255, 174.0f / 255, 30.0f / 255);


    // END LUDO

    public GameObject GameFinishWindow;
    public GameObject ScreenShotController;
    public GameObject invitiationDialog;
    public GameObject addedFriendWindow;
    public GameObject PlayerInfoWindow;
    public GameObject ChatWindow;
    public GameObject ChatButton;
    private bool SecondPlayerOnDiagonal = true;
    private bool ThirdPlayerOnDiagonal = true;

    private List<string> PlayersIDs;
    public GameObject[] Players;
    public GameObject[] PlayersTimers;
    public GameObject[] PlayersChatBubbles;
    public GameObject[] PlayersChatBubblesText;
    public GameObject[] PlayersChatBubblesImage;
    private GameObject[] ActivePlayers;
    public GameObject[] PlayersAvatarsButton;

    [SerializeField]
    private List<Sprite> avatars;
    [SerializeField]
    private List<string> names;

    private List<PlayerObject> playerObjects;
    private int myIndex;
    private string myId;


    private Color[] borderColors = new Color[4] { Color.yellow, Color.green, Color.red, Color.blue };

    private int currentPlayerIndex;

    private int ActivePlayersInRoom;

    private Sprite[] emojiSprites;

    private string CurrentPlayerID;

    private List<PlayerObject> playersFinished = new List<PlayerObject>();


    private bool iFinished = false;
    private bool FinishWindowActive = false;

    private double firstPlacePrize;
    private double secondPlacePrize;
    private double ThirdPlacePrize;
    private double FourthPlacePrize;

    private int requiredToStart = 0;

    public bool playerMoved;

    public int diceValue;

    public GameObject LudoPawnController;

    public static GameGUIController Instance;


    public bool debugLog = false;
    public bool runInBackgroundValue = true;



    public prizeData prizedata;

    public static GameGUIController GetInstance()
    {

        return Instance;

    }

    [PunRPC]

    // Use this for initialization
    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        Instance = this;


        isServer = false;

        if (GameManager.Instance.roomOwner)
        {
            isServer = true;
        }

        requiredToStart = GameManager.Instance.requiredPlayers;
        avatarSprites = GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>().avatars;
        if (GameManager.Instance.type == MyGameType.Private)
        {
            requiredToStart = 2;
        }

      


        Debug.Log("winnings naagin: " + GameManager.Instance.first);

        PhotonNetwork.RaiseEvent((int)EnumPhoton.ReadyToPlay, 0, true, null);

        // LUDO
        // Rotate board and set colors

        int rotation = UnityEngine.Random.Range(0, 4);

        Color[] colors = null;


        if (MainmenuManager.getInstance().selectColor == "red")
        {
            colors = new Color[] { colorRed, colorIrisBlue, colorYellow, colorGreen };
            ludoBoard.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -180.0f);
        }
        else if (MainmenuManager.getInstance().selectColor == "Blue")
        {
            colors = new Color[] { colorIrisBlue, colorYellow, colorGreen, colorRed };
            ludoBoard.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -90.0f);
        }
        else if (MainmenuManager.getInstance().selectColor == "yellow")
        {
            colors = new Color[] { colorYellow, colorGreen, colorRed, colorIrisBlue };

        }
        else if (MainmenuManager.getInstance().selectColor == "Green")
        {
            colors = new Color[] { colorGreen, colorRed, colorIrisBlue, colorYellow };
            ludoBoard.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -270.0f);
        }
        else
        {
            ludoBoard.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -180.0f);
            colors = new Color[] { colorRed, colorGreen, colorYellow, colorIrisBlue };
        }

        for (int i = 0; i < diceBackgrounds.Length; i++)
        {
            diceBackgrounds[i].GetComponent<Image>().color = colors[i];
        }

        for (int i = 0; i < playersPawnsColors.Length; i++)
        {
            for (int j = 0; j < playersPawnsColors[i].objectsArray.Length; j++)
            {
                playersPawnsColors[i].objectsArray[j].GetComponent<Image>().color = colors[i];
                playersPawnsMultiple[i].objectsArray[j].GetComponent<Image>().color = colors[i];
            }
        }
        playerMoved = false;


        // END LUDO




        // Update player data in playfab
        Dictionary<string, string> data = new Dictionary<string, string>();


        currentPlayerIndex = 0;
        emojiSprites = GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>().emoji;
        myId = GameManager.Instance.UserID;
        if (EventCounter.GetID() == "") EventCounter.SetID(myId);
        playerObjects = new List<PlayerObject>();
        avatars = GameManager.Instance.opponentsAvatars;
        // int sprites = UnityEngine.Random.Range(0, avatarSprites.Length - 1);
        //  avatars.Insert(0, avatarSprites[sprites]);



        names = GameManager.Instance.opponentsNames;
        /// names.Insert(0, GameManager.Instance.nameMy);



        PlayersIDs = new List<string>();
        for (int i = 0; i < GameManager.Instance.opponentsIDs.Count; i++)
        {
            if (GameManager.Instance.opponentsIDs[i] != null)
            {
                Debug.Log("opponents id: " + GameManager.Instance.opponentsIDs[i]);
                PlayersIDs.Add(GameManager.Instance.opponentsIDs[i]);
            }
        }
        // PlayersIDs.Insert(0, GameManager.Instance.UserID);
        Debug.Log(names.Count + " : " + PlayersIDs.Count + " : " + avatars.Count);


        for (int i = 0; i < PlayersIDs.Count; i++)
        {
            playerObjects.Add(new PlayerObject(names[i], PlayersIDs[i], avatars[i]));

            Debug.Log("names : " + names[i]);
            Debug.Log("PlayersIDs : " + PlayersIDs[i]);
            Debug.Log("avatars : " + avatars[i]);

        }
        Debug.Log("PlayersIDs.Count : " + PlayersIDs);
        // Bubble sort
        /* for (int i = 0; i < PlayersIDs.Count; i++)
         {
             for (int j = 0; j < PlayersIDs.Count - 1; j++)
             {
                 if (string.Compare(playerObjects[j].id, playerObjects[j + 1].id) == 1)
                 {
                     // swaap ids
                     PlayerObject temp = playerObjects[j + 1];
                     playerObjects[j + 1] = playerObjects[j];
                     playerObjects[j] = temp;
                 }
             }
         }*/
        for (int i = 0; i < PlayersIDs.Count; i++)
        {
            Debug.Log(playerObjects[i].id);
        }

        ActivePlayersInRoom = PlayersIDs.Count;

        if (PlayersIDs.Count == 2)
        {
            if (SecondPlayerOnDiagonal)
            {
                Players[1].SetActive(false);
                Players[3].SetActive(false);
                ActivePlayers = new GameObject[2];
                ActivePlayers[0] = Players[0];
                ActivePlayers[1] = Players[2];

                // LUDO
                for (int i = 0; i < PlayersPawns[1].objectsArray.Length; i++)
                {
                    PlayersPawns[1].objectsArray[i].SetActive(false);
                }


                for (int i = 0; i < PlayersPawns[3].objectsArray.Length; i++)
                {
                    PlayersPawns[3].objectsArray[i].SetActive(false);
                }

                // END LUDO
            }
            else
            {

                // LUDO
                for (int i = 0; i < PlayersPawns[2].objectsArray.Length; i++)
                {
                    PlayersPawns[2].objectsArray[i].SetActive(false);
                }

                for (int i = 0; i < PlayersPawns[3].objectsArray.Length; i++)
                {
                    PlayersPawns[3].objectsArray[i].SetActive(false);
                }

                // END LUDO
                Players[2].SetActive(false);
                Players[3].SetActive(false);
                ActivePlayers = new GameObject[2];
                ActivePlayers[0] = Players[0];
                ActivePlayers[1] = Players[1];
            }
        }
        /*  else if (PlayersIDs.Count == 3)
          {
              if (ThirdPlayerOnDiagonal)
              {

                  Players[3].SetActive(false);
                  ActivePlayers = new GameObject[3];
                  ActivePlayers[0] = Players[0];
                  ActivePlayers[1] = Players[1];
                  ActivePlayers[2] = Players[2];

                  // LUDO


                  for (int i = 0; i < PlayersPawns[3].objectsArray.Length; i++)
                  {
                      PlayersPawns[3].objectsArray[i].SetActive(false);
                  }

                  // END LUDO
              }
              else
              {

                  // LUDO
                  for (int i = 0; i < PlayersPawns[2].objectsArray.Length; i++)
                  {
                      PlayersPawns[2].objectsArray[i].SetActive(false);
                  }



                  // END LUDO
                  Players[2].SetActive(false);

                  ActivePlayers = new GameObject[3];
                  ActivePlayers[0] = Players[0];
                  ActivePlayers[1] = Players[1];
                  ActivePlayers[2] = Players[3];
              }
          } */
        else
        {
            ActivePlayers = Players;
        }



        int startPos = 0;
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id == GameManager.Instance.UserID)
            {
                startPos = i;
                break;
            }
        }
        int index = 0;
        bool addedMe = false;
        myIndex = startPos;
        GameManager.Instance.myPlayerIndex = myIndex;
        for (int i = startPos; ;)
        {
            if (i == startPos && addedMe) break;

            if (PlayersIDs.Count == 2 && SecondPlayerOnDiagonal)
            {
                if (addedMe)
                {
                    playerObjects[i].timer = PlayersTimers[2];
                    playerObjects[i].ChatBubble = PlayersChatBubbles[2];
                    playerObjects[i].ChatBubbleText = PlayersChatBubblesText[2];
                    playerObjects[i].ChatbubbleImage = PlayersChatBubblesImage[2];
                    string id = playerObjects[i].id;
                    PlayersAvatarsButton[2].GetComponent<Button>().onClick.RemoveAllListeners();
                    PlayersAvatarsButton[2].GetComponent<Button>().onClick.AddListener(() => ButtonClick(id));

                    // LUDO
                    playerObjects[i].dice = PlayersDices[2];
                    playerObjects[i].pawns = PlayersPawns[2].objectsArray;

                    for (int k = 0; k < playerObjects[i].pawns.Length; k++)
                    {
                        playerObjects[i].pawns[k].GetComponent<LudoPawnController>().setPlayerIndex(i);
                    }
                    playerObjects[i].homeLockObjects = HomeLockObjects[2];

                    // END LUDO
                }
                else
                {
                    GameManager.Instance.myPlayerIndex = i;
                    playerObjects[i].timer = PlayersTimers[index];
                    playerObjects[i].ChatBubble = PlayersChatBubbles[index];
                    playerObjects[i].ChatBubbleText = PlayersChatBubblesText[index];
                    playerObjects[i].ChatbubbleImage = PlayersChatBubblesImage[index];
                    string id = playerObjects[i].id;

                    // LUDO
                    playerObjects[i].dice = PlayersDices[index];
                    playerObjects[i].pawns = PlayersPawns[index].objectsArray;

                    for (int k = 0; k < playerObjects[i].pawns.Length; k++)
                    {
                        playerObjects[i].pawns[k].GetComponent<LudoPawnController>().setPlayerIndex(i);
                    }
                    playerObjects[i].homeLockObjects = HomeLockObjects[index];
                    // END LUDO
                }
            }
            else
            {

                playerObjects[i].timer = PlayersTimers[index];
                playerObjects[i].ChatBubble = PlayersChatBubbles[index];
                playerObjects[i].ChatBubbleText = PlayersChatBubblesText[index];
                playerObjects[i].ChatbubbleImage = PlayersChatBubblesImage[index];

                // LUDO
                playerObjects[i].dice = PlayersDices[index];
                playerObjects[i].pawns = PlayersPawns[index].objectsArray;

                for (int k = 0; k < playerObjects[i].pawns.Length; k++)
                {
                    playerObjects[i].pawns[k].GetComponent<LudoPawnController>().setPlayerIndex(i);
                }
                playerObjects[i].homeLockObjects = HomeLockObjects[index];
                // END LUDO

                string id = playerObjects[i].id;
                if (index != 0)
                {
                    PlayersAvatarsButton[index].GetComponent<Button>().onClick.RemoveAllListeners();
                    PlayersAvatarsButton[index].GetComponent<Button>().onClick.AddListener(() => ButtonClick(id));
                }

            }




            playerObjects[i].AvatarObject = ActivePlayers[index];
            ActivePlayers[index].GetComponent<PlayerAvatarController>().Name.GetComponent<Text>().text = playerObjects[i].name;
            if (playerObjects[i].avatar != null)
            {
                ActivePlayers[index].GetComponent<PlayerAvatarController>().Avatar.GetComponent<Image>().sprite = playerObjects[i].avatar;
            }

            index++;

            if (i < PlayersIDs.Count - 1)
            {
                i++;
            }
            else
            {
                i = 0;
            }

            addedMe = true;
        }

        currentPlayerIndex = GameManager.Instance.firstPlayerInGame;
        Debug.Log("currentPlayerIndex : " + currentPlayerIndex);
        Debug.Log("playerObjects : " + playerObjects.Count);
        GameManager.Instance.currentPlayer = playerObjects[currentPlayerIndex];




        // SetTurn();

        // // if (myIndex == 0)
        // // {
        // //     SetMyTurn();
        // //     playerObjects[0].dice.GetComponent<GameDiceController>().DisableDiceShadow();
        // // }
        // // else
        // // {
        // //     SetOpponentTurn();
        // //     playerObjects[currentPlayerIndex].dice.GetComponent<GameDiceController>().DisableDiceShadow();
        // // }





        GameManager.Instance.playerObjects = playerObjects;

        // // Check if all players are still in room - if not deactivate
        // for (int i = 0; i < playerObjects.Count; i++)
        // {
        //     bool contains = false;
        //     if (!playerObjects[i].id.Contains("_BOT"))
        //     {
        //         for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
        //         {
        //             if (PhotonNetwork.playerList[j].NickName.Equals(playerObjects[i].id))
        //             {
        //                 contains = true;
        //                 break;
        //             }
        //         }

        //         if (!contains)
        //         {
        //             setPlayerDisconnected(i);
        //         }
        //     }
        // }

        // CheckPlayersIfShouldFinishGame();

        // Set prizes. 
        // For 2 players 1st get 2x payout and 2nd 0. 
        // For 4 players 1st get 4x payout and 2nd gets payout

        CalculateWinningAmount();

        


        // LUDO

        // Enable home locks

        if (GameManager.Instance.mode == MyGameMode.Quick || GameManager.Instance.mode == MyGameMode.Master)
        {
            for (int i = 0; i < GameManager.Instance.playerObjects.Count; i++)
            {
                GameManager.Instance.playerObjects[i].homeLockObjects.SetActive(true);
            }
            GameManager.Instance.needToKillOpponentToEnterHome = true;
        }
        else
        {
            GameManager.Instance.needToKillOpponentToEnterHome = false;
        }
        //GameManager.Instance.needToKillOpponentToEnterHome = false;

        // END LUDO

        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id.Contains("_BOT"))
            {
                GameManager.Instance.readyPlayersCount++;
            }
        }

        GameManager.Instance.playerObjects = playerObjects;

        // Check if all players are still in room - if not deactivate
        for (int i = 0; i < playerObjects.Count; i++)
        {
            bool contains = false;
            if (!playerObjects[i].id.Contains("_BOT"))
            {
                for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
                {
                    if (PhotonNetwork.playerList[j].NickName.Equals(playerObjects[i].id))
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    GameManager.Instance.readyPlayersCount++;
                    Debug.Log("Ready players: " + GameManager.Instance.readyPlayersCount);
                    setPlayerDisconnected(i);
                }
            }
        }

        CheckPlayersIfShouldFinishGame();

        StartCoroutine(waitForPlayersToStart());

        if(GameManager.Instance.mainmenu)
        game_id = GameManager.Instance.mainmenu.ViewBtn[GameManager.Instance.mainmenu.temp].GetComponent<enroll_tour>().tourid;

        user_id = GameManager.Instance.UserID;
        room_id = PhotonNetwork.room.Name;
        Debug.Log(room_id);

    }

    

    //Dinesh
    private void CalculateWinningAmount()
    {
        StartCoroutine(CalculateWinningAmountRoutine());

        /*if (ActivePlayersInRoom == 2)
        {

            firstPlacePrize = GameManager.Instance.payoutCoins + (80 * GameManager.Instance.payoutCoins) / 100;
            firstPrizeText.GetComponent<Text>().text = (GameManager.Instance.payoutCoins + (80 * GameManager.Instance.payoutCoins) / 100).ToString();
            Debug.Log("********** PRIZE " + firstPrizeText.GetComponent<Text>().text);
            secondPlacePrize = 0;
        }

        else if (ActivePlayersInRoom == 4)
        {
            firstPlacePrize = (double)3.6 * GameManager.Instance.payoutCoins;
            firstPrizeText.GetComponent<Text>().text = ((double)3.6 * GameManager.Instance.payoutCoins).ToString();
            Debug.Log(firstPrizeText.GetComponent<Text>().text);
            secondPlacePrize = 0;
        }
        else
        {
            firstPlacePrize = (int)3.6 * GameManager.Instance.payoutCoins;
        }

        //  firstPrizeText.GetComponent<Text>().text = firstPlacePrize + "";
        secondPrizeText.GetComponent<Text>().text = secondPlacePrize + "";

        if (secondPlacePrize == 0)
        {
            SecondPrizeObject.SetActive(false);
            firstPrizeObject.GetComponent<RectTransform>().anchoredPosition = SecondPrizeObject.GetComponent<RectTransform>().anchoredPosition;
        }*/




    }

    double insertAmountP1, insertAmountP2, insertAmountP3, insertAmountP4;

    IEnumerator CalculateWinningAmountRoutine()
    {
        var url = "https://codash.tk/ludowala/index.php?api/winning_percentage";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("************************** Unable to get prize data **************************");
            firstPlacePrize = 0;
            secondPlacePrize = 0;
            ThirdPlacePrize = 0;
            firstPrizeText.GetComponent<Text>().text = firstPlacePrize.ToString();
            secondPrizeText.GetComponent<Text>().text = secondPlacePrize.ToString();
            insertAmountP1 = insertAmountP2 = insertAmountP3 = insertAmountP4 = 0;
        }
        else
        {
            prizedata = new prizeData();
            prizedata = JsonUtility.FromJson<prizeData>(request.downloadHandler.text);

            if (prizedata.status == "1")
            {
                Debug.Log("************************** prize data **************************");
                if (ActivePlayersInRoom == 2)
                {
                    int WinP1;
                    Int32.TryParse(prizedata.data[0].player_1, out WinP1);
                    float winPercentP1 = (WinP1 / 100f);
                    firstPlacePrize = ((GameManager.Instance.payoutCoins *ActivePlayersInRoom )* winPercentP1 );
                    firstPrizeText.GetComponent<Text>().text = firstPlacePrize.ToString("0.00");
                    insertAmountP1 = firstPlacePrize;


                    int WinP2;
                    Int32.TryParse(prizedata.data[0].player_2, out WinP2);
                    float winPercentP2 = (WinP2 / 100f);
                    secondPlacePrize = ((GameManager.Instance.payoutCoins * ActivePlayersInRoom) * winPercentP2 );
                    secondPrizeText.GetComponent<Text>().text = secondPlacePrize.ToString("0.00");
                    insertAmountP2 = secondPlacePrize;

                    Debug.Log("************************** data 1 ************************* "  + WinP1 + " "+ winPercentP1
                        + " " + firstPlacePrize  + " --- " + GameManager.Instance.payoutCoins + "----" + winPercentP1);


                    Debug.Log("************************** data 2 ************************* " + WinP2 + " " + winPercentP2
                       + " " + secondPlacePrize);

                }
                else if (ActivePlayersInRoom == 4)
                {
                    int WinP1;
                    Int32.TryParse(prizedata.data[1].player_1, out WinP1);
                    float winPercentP1 = (WinP1 / 100f);
                    firstPlacePrize = (GameManager.Instance.payoutCoins * winPercentP1 * ActivePlayersInRoom);
                    firstPrizeText.GetComponent<Text>().text = firstPlacePrize.ToString("0.00");
                    insertAmountP1 = firstPlacePrize;


                    int WinP2;
                    Int32.TryParse(prizedata.data[1].player_2, out WinP2);
                    float winPercentP2 = (WinP2 / 100f);
                    secondPlacePrize = (GameManager.Instance.payoutCoins * winPercentP2 * ActivePlayersInRoom);
                    secondPrizeText.GetComponent<Text>().text = secondPlacePrize.ToString("0.00");
                    insertAmountP2 = secondPlacePrize;


                    int WinP3;
                    Int32.TryParse(prizedata.data[1].player_3, out WinP3);
                    float winPercentP3 = (WinP3 / 100f);
                    ThirdPlacePrize = (GameManager.Instance.payoutCoins * winPercentP3 * ActivePlayersInRoom);
                    //secondPrizeText.GetComponent<Text>().text = secondPlacePrize.ToString();
                    insertAmountP3 = ThirdPlacePrize;


                    int WinP4;
                    Int32.TryParse(prizedata.data[1].player_4, out WinP4);
                    float winPercentP4 = (WinP4 / 100f);
                    FourthPlacePrize = (GameManager.Instance.payoutCoins * winPercentP4 * ActivePlayersInRoom);
                    insertAmountP4 = FourthPlacePrize; 
                }
            }

        }
    }
    //Dinesh



    public IEnumerator loadImageOpponent(string url)
    {
        WWW www = new WWW(url);

        yield return www;

        GameManager.Instance.opponentMy = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f), 32);

    }
    private IEnumerator waitForPlayersToStart()
    {
        Debug.Log("Waiting for players " + GameManager.Instance.readyPlayersCount + " - " + requiredToStart);

        yield return new WaitForSeconds(0.1f);


        if (GameManager.Instance.readyPlayersCount < requiredToStart)
        {
            StartCoroutine(waitForPlayersToStart());
        }
        else
        {
            AllPlayersReady = true;
            SetTurn();

            StartCoroutine(result_insert());
            // if (myIndex == 0)
            // {
            //     SetMyTurn();
            //     playerObjects[0].dice.GetComponent<GameDiceController>().DisableDiceShadow();
            // }
            // else
            // {
            //     SetOpponentTurn();
            //     playerObjects[currentPlayerIndex].dice.GetComponent<GameDiceController>().DisableDiceShadow();
            // }

        }


    }

    public int GetCurrentPlayerIndex
    {
        get
        {
            return currentPlayerIndex;
        }
        set
        {
            currentPlayerIndex = value;
        }
    }

    public void TIPButton()
    {
        if (TIPObject.activeSelf)
        {
            TIPObject.SetActive(false);
        }
        else
        {
            TIPObject.SetActive(true);
        }
    }

    public void FacebookShare()
    {
        if (PlayerPrefs.GetString("LoggedType").Equals("Facebook"))
        {

            Uri myUri = new Uri("https://play.google.com/store/apps/details?id=" + StaticStrings.AndroidPackageName);
#if UNITY_IPHONE
            myUri = new Uri("https://itunes.apple.com/us/app/apple-store/id" + StaticStrings.ITunesAppID);
#endif

            /*  FB.ShareLink(
                  myUri,
                  StaticStrings.facebookShareLinkTitle,
                  callback: ShareCallback
              );*/
        }
    }

    /* private void ShareCallback(IShareResult result)
     {
         if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
         {
             Debug.Log("ShareLink Error: " + result.Error);
         }
         else if (!String.IsNullOrEmpty(result.PostId))
         {
             // Print post identifier of the shared content
             Debug.Log(result.PostId);
         }
         else
         {
             // Share succeeded without postID
             GameManager.Instance.playfabManager.addCoinsRequest(StaticStrings.rewardCoinsForShareViaFacebook);
             Debug.Log("ShareLink success!");
         }
     }*/

    public void StopAndFinishGame()
    {
        StopTimers();
        SetFinishGame(PhotonNetwork.player.NickName, true);
        ShowGameFinishWindow();
    }

    public void ShareScreenShot()
    {

#if UNITY_ANDROID
        string text = StaticStrings.ShareScreenShotText;
        text = text + " " + "https://play.google.com/store/apps/details?id=" + StaticStrings.AndroidPackageName;
        //  ScreenShotController.GetComponent<NativeShare>().ShareScreenshotWithText(text);
#elif UNITY_IOS
        string text = StaticStrings.ShareScreenShotText;
        text = text + " " + "https://itunes.apple.com/us/app/apple-store/id" + StaticStrings.ITunesAppID;
        ScreenShotController.GetComponent<NativeShare>().ShareScreenshotWithText(text);
#endif


    }

    public void ShowGameFinishWindow()
    {
        if (!FinishWindowActive)
        {

            //  AdsManager.Instance.adsScript.ShowAd(AdLocation.GameFinishWindow);
            FinishWindowActive = true;

            List<PlayerObject> otherPlayers = new List<PlayerObject>();

            for (int i = 0; i < playerObjects.Count; i++)
            {
                // PlayerAvatarController controller = playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>();
                //  if (controller.Active && !controller.finished)
                //  {
                //      otherPlayers.Add(playerObjects[i]);
                //  }
            }

            GameFinishWindow.GetComponent<GameFinishWindowController>().showWindow(playersFinished, otherPlayers, firstPlacePrize, secondPlacePrize, ThirdPlacePrize);
        }
    }




    private void ButtonClick(string id)
    {

        int index = 0;

        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id == id)
            {
                index = i;
                break;
            }
        }

        CurrentPlayerID = id;

        if (playerObjects[index].AvatarObject.GetComponent<PlayerAvatarController>().Active)
        {
            //PlayerInfoWindow.GetComponent<PlayerInfoController>().ShowPlayerInfo(playerObjects[index].avatar, playerObjects[index].name, playerObjects[index].data);
        }

    }

    public void AddFriendButtonClick()
    {
        if (!CurrentPlayerID.Contains("_BOT"))
        {
            /* AddFriendRequest request = new AddFriendRequest()
             {
                 FriendPlayFabId = CurrentPlayerID,
             };

             PlayFabClientAPI.AddFriend(request, (result) =>
             {
                 PhotonNetwork.RaiseEvent((int)EnumPhoton.AddFriend, PhotonNetwork.playerName + ";" + GameManager.Instance.nameMy + ";" + CurrentPlayerID, true, null);
                 addedFriendWindow.SetActive(true);
                 Debug.Log("Added friend successfully");
             }, (error) =>
             {
                 addedFriendWindow.SetActive(true);
                 Debug.Log("Error adding friend: " + error.Error);
             }, null);*/
        }
        else
        {
            Debug.Log("Add Friend - It's bot!");
            addedFriendWindow.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (debugLog)
        {
            runInBackgroundValue = Application.runInBackground;
        }

        if (!Application.runInBackground)
        {
            Application.runInBackground = true;

            if (debugLog)
            {
                Debug.Log("Re-Setting Application.runInBackground to TRUE at: " + Time.time);
            }
        }
    }

    public bool IsPlayerInGame()
    {
        return playerObjects[currentPlayerIndex].inGame;
    }

    public void FinishedGame()
    {
        if (GameManager.Instance.currentPlayer.id == PhotonNetwork.player.NickName)
        {
            SetFinishGame(GameManager.Instance.currentPlayer.id, true);
        }
        else
        {
            SetFinishGame(GameManager.Instance.currentPlayer.id, false);
        }

        // SetFinishGame(PhotonNetwork.player.NickName, true);
    }

    public int cout = -1;
    public int cout1 = -1;
    public int cout2 = -1;

    public void ChangeTurn()
    {
        if (PhotonNetwork.isMasterClient)
        {
            // MasterServerController._instance.StructureData();
            if (EventCounter.GetID() == "") EventCounter.SetID(myId);
            PhotonNetwork.RaiseEvent((int)EnumPhoton.TurnSkiped, new object[] { EventCounter.GetCount() + "_" + EventCounter.GetID(), currentPlayerIndex }, true, new RaiseEventOptions { CachingOption = EventCaching.AddToRoomCacheGlobal });
            setCurrentPlayerIndex(currentPlayerIndex);

        }

        /*if (cout == 0)
        {
            red1.GetComponent<Image>().sprite = GameManager.Instance.currentPlayer.AvatarObject.GetComponent<PlayerAvatarController>().red;

        }
        if (cout == 1)
        {
            red2.GetComponent<Image>().sprite = GameManager.Instance.currentPlayer.AvatarObject.GetComponent<PlayerAvatarController>().red;

        }
        if (cout == 2)
        {
            red3.GetComponent<Image>().sprite = GameManager.Instance.currentPlayer.AvatarObject.GetComponent<PlayerAvatarController>().red;

        }
        if (cout1 == 0)
        {
            red12.GetComponent<Image>().sprite = GameManager.Instance.currentPlayer.AvatarObject.GetComponent<PlayerAvatarController>().red;

        }
        if (cout1 == 1)
        {
            red22.GetComponent<Image>().sprite = GameManager.Instance.currentPlayer.AvatarObject.GetComponent<PlayerAvatarController>().red;

        }
        if (cout1 == 2)
        {
            red32.GetComponent<Image>().sprite = GameManager.Instance.currentPlayer.AvatarObject.GetComponent<PlayerAvatarController>().red;

        }
        if (cout2 == 0)
        {
            red14.GetComponent<Image>().sprite = GameManager.Instance.currentPlayer.AvatarObject.GetComponent<PlayerAvatarController>().red;

        }
        if (cout2 == 1)
        {
            red24.GetComponent<Image>().sprite = GameManager.Instance.currentPlayer.AvatarObject.GetComponent<PlayerAvatarController>().red;

        }
        if (cout2 == 2)
        {
            red34.GetComponent<Image>().sprite = GameManager.Instance.currentPlayer.AvatarObject.GetComponent<PlayerAvatarController>().red;

        }*/
        SetTurn();
    }

    private void SetFinishGame(string id, bool me)
    {
        if (!me || !iFinished)
        {
            Debug.Log("SET FINISH");
            ActivePlayersInRoom--;

            int index = GetPlayerPosition(id);



            playersFinished.Add(playerObjects[index]);


            PlayerAvatarController controller = playerObjects[index].AvatarObject.GetComponent<PlayerAvatarController>();
            controller.Name.GetComponent<Text>().text = "";
            controller.Active = false;
            controller.finished = true;

            playerObjects[index].dice.SetActive(false);

            int positions = playersFinished.Count;
            if (positions == 1)
            {
                controller.Crown.SetActive(true);
            }

            if (me)
            {
                PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong;
                iFinished = true;
                if (ActivePlayersInRoom >= 0)
                {
                    PhotonNetwork.RaiseEvent((int)EnumPhoton.FinishedGame, id, true, new RaiseEventOptions { CachingOption = EventCaching.AddToRoomCacheGlobal });
                    Debug.Log("set finish call finish turn");
                    ShowGameFinishWindow();
                    SendFinishTurn();
                    position = positions.ToString();
                    // StartCoroutine(result_insert());

                }


                Debug.Log("position: " + position);
                position = positions.ToString();
                StartCoroutine(result_insert1());

                // game_id = MENUmanager.getInstance().playBtn[MENUmanager.getInstance().temps].GetComponent<MyContast>().GameID;
                //  user_id = Login.Instance().USERID.text;
                // room_id = PhotonNetwork.room.Name;
                // Debug.Log(room_id);

                if (positions == 1)
                {
                    WinSound.Play();
                    Dictionary<string, string> data = new Dictionary<string, string>();

                    if (GameManager.Instance.type == MyGameType.TwoPlayer)
                    {

                        //data.Add(MyPlayerData.TwoPlayerWinsKey, (GameManager.Instance.myPlayerData.GetTwoPlayerWins() + 1).ToString());
                    }
                    else if (GameManager.Instance.type == MyGameType.ThreePlayer)
                    {
                        // data.Add(MyPlayerData.FourPlayerWinsKey, (GameManager.Instance.myPlayerData.GetFourPlayerWins() + 1).ToString());
                    }
                    else if (GameManager.Instance.type == MyGameType.FourPlayer)
                    {
                        // data.Add(MyPlayerData.FourPlayerWinsKey, (GameManager.Instance.myPlayerData.GetFourPlayerWins() + 1).ToString());
                    }
                    // GameManager.Instance.myPlayerData.UpdateUserData(data);
                }
                else if (positions == 2)
                {
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    //  data.Add(MyPlayerData.CoinsKey, (GameManager.Instance.myPlayerData.GetCoins() + secondPlacePrize).ToString());
                    //   data.Add(MyPlayerData.TotalEarningsKey, (GameManager.Instance.myPlayerData.GetTotalEarnings() + secondPlacePrize).ToString());
                    // GameManager.Instance.myPlayerData.UpdateUserData(data);
                }
                else if (positions == 3)
                {
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    //  data.Add(MyPlayerData.CoinsKey, (GameManager.Instance.myPlayerData.GetCoins() + secondPlacePrize).ToString());
                    //   data.Add(MyPlayerData.TotalEarningsKey, (GameManager.Instance.myPlayerData.GetTotalEarnings() + secondPlacePrize).ToString());
                    // GameManager.Instance.myPlayerData.UpdateUserData(data);
                }
            }

            else if (GameManager.Instance.currentPlayer.isBot && PhotonNetwork.isMasterClient)
            {
                SendFinishTurn();
            }
            else
            {
                ShowGameFinishWindow();
            }



            controller.setPositionSprite(positions);


            CheckPlayersIfShouldFinishGame();
        }
    }

    public int GetPlayerPosition(string id)
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id.Equals(id))
            {
                return i;
            }
        }
        return -1;
    }

    public void SendFinishTurn()
    {

        if (!FinishWindowActive && ActivePlayersInRoom > 1)
        {
            if (GameManager.Instance.currentPlayer.isBot)
            {
                BotDelay();
            }
            else
            {
                if (EventCounter.GetID() == "") EventCounter.SetID(myId);
                PhotonNetwork.RaiseEvent((int)EnumPhoton.NextPlayerTurn, new object[] { EventCounter.GetCount() + "_" + EventCounter.GetID(), myIndex }, true, new RaiseEventOptions { CachingOption = EventCaching.AddToRoomCacheGlobal });

                //currentPlayerIndex = (myIndex + 1) % playerObjects.Count;

                Debug.Log("PLAYER BEFORE: " + currentPlayerIndex);

                setCurrentPlayerIndex(myIndex);

                Debug.Log("PLAYER AFTER: " + currentPlayerIndex + " isbot: " + GameManager.Instance.currentPlayer.isBot);

                SetTurn();
                //SetOpponentTurn();

                GameManager.Instance.miniGame.setOpponentTurn();
            }
        }
    }

    public void SendSkipedTurn()
    {

        if (!FinishWindowActive && ActivePlayersInRoom > 1)
        {
            if (GameManager.Instance.currentPlayer.isBot)
            {
                BotDelay();
            }
            else
            {
                if (EventCounter.GetID() == "") EventCounter.SetID(myId);
                PhotonNetwork.RaiseEvent((int)EnumPhoton.TurnSkiped, new object[] { EventCounter.GetCount() + "_" + EventCounter.GetID(), myIndex }, true, new RaiseEventOptions { CachingOption = EventCaching.AddToRoomCacheGlobal });

                //currentPlayerIndex = (myIndex + 1) % playerObjects.Count;

                Debug.Log("PLAYER BEFORE: " + currentPlayerIndex);

                setCurrentPlayerIndex(myIndex);

                Debug.Log("PLAYER AFTER: " + currentPlayerIndex + " isbot: " + GameManager.Instance.currentPlayer.isBot);

                SetTurn();
                //SetOpponentTurn();

                GameManager.Instance.miniGame.setOpponentTurn();
            }
        }
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        PhotonNetwork.OnEventCall += this.OnEvent;
    }


    void OnDestroy()
    {
        PhotonNetwork.OnEventCall -= this.OnEvent;
    }

    List<string> readIDS = new List<string>();

    private void OnEvent(byte eventcode, object content, int senderid)
    {
        Debug.Log("received event: " + eventcode);
        if (eventcode == (int)EnumPhoton.NextPlayerTurn)
        {
            object[] dataa = (object[])content;
            if (!readIDS.Contains((string)dataa[0]))
            {
                readIDS.Add((string)dataa[0]);
                if (playerObjects[(int)dataa[1]].AvatarObject.GetComponent<PlayerAvatarController>().Active &&
                currentPlayerIndex == (int)dataa[1])
                {
                    if (!FinishWindowActive)
                    {
                        setCurrentPlayerIndex((int)dataa[1]);

                        SetTurn();
                    }
                }
            }
        }
        else if (eventcode == (int)EnumPhoton.TurnSkiped)
        {
            object[] dataa = (object[])content;
            if (!readIDS.Contains((string)dataa[0]))
            {
                readIDS.Add((string)dataa[0]);
                GameManager.Instance.currentPlayer.timerMisseds++;
                GameManager.Instance.currentPlayer.AvatarObject.GetComponent<PlayerAvatarController>().chance[GameManager.Instance.currentPlayer.timerMisseds].sprite = GameManager.Instance.currentPlayer.AvatarObject.GetComponent<PlayerAvatarController>().red;
                GameManager.Instance.priv = true;
                if (playerObjects[(int)dataa[1]].AvatarObject.GetComponent<PlayerAvatarController>().Active &&
                    currentPlayerIndex == (int)dataa[1])
                {
                    if (!FinishWindowActive)
                    {
                        setCurrentPlayerIndex((int)dataa[1]);

                        SetTurn();
                    }
                }
            }
        }
        else if (eventcode == (int)EnumPhoton.SendChatMessage)
        {
            string[] message = ((string)content).Split(';');
            Debug.Log("Received message " + message[0] + " from " + message[1]);
            for (int i = 0; i < playerObjects.Count; i++)
            {
                if (playerObjects[i].id.Equals(message[1]))
                {
                    playerObjects[i].ChatBubbleText.SetActive(true);
                    playerObjects[i].ChatbubbleImage.SetActive(false);
                    playerObjects[i].ChatBubbleText.GetComponent<Text>().text = message[0];
                    playerObjects[i].ChatBubble.GetComponent<Animator>().Play("MessageBubbleAnimation");
                }
            }
        }
        else if (eventcode == (int)EnumPhoton.SendChatEmojiMessage)
        {
            string[] message = ((string)content).Split(';');
            Debug.Log("Received message " + message[0] + " from " + message[1]);
            for (int i = 0; i < playerObjects.Count; i++)
            {
                if (playerObjects[i].id.Equals(message[1]))
                {
                    playerObjects[i].ChatBubbleText.SetActive(false);
                    playerObjects[i].ChatbubbleImage.SetActive(true);
                    int index = int.Parse(message[0]);

                    if (index > emojiSprites.Length - 1)
                    {
                        index = emojiSprites.Length;
                    }
                    playerObjects[i].ChatbubbleImage.GetComponent<Image>().sprite = emojiSprites[index];
                    playerObjects[i].ChatBubble.GetComponent<Animator>().Play("MessageBubbleAnimation");
                }
            }
        }
        else if (eventcode == (int)EnumPhoton.AddFriend)
        {
            if (PlayerPrefs.GetInt(StaticStrings.FriendsRequestesKey, 0) == 0)
            {
                string[] data = ((string)content).Split(';');
                if (PhotonNetwork.playerName.Equals(data[2]))
                    invitiationDialog.GetComponent<PhotonChatListener2>().showInvitationDialog(data[0], data[1], null);
            }
            else
            {
                Debug.Log("Invitations OFF");
            }

        }
        else if (eventcode == (int)EnumPhoton.FinishedGame)
        {
            string message = (string)content;
            SetFinishGame(message, false);

        }
        else if (eventcode == 151)
         { // Opponent paused game
             if (isServer)
               //  ShotPowerIndicator.anim.Play("ShotPowerAnimation");
             GameManager.Instance.opponentActive = false;
             GameManager.Instance.stopTimer = true;
            GameManager.Instance.gameControllerScript.showMessage(StaticStrings.waitingForOpponent + " " + StaticStrings.photonDisconnectTimeout);
         }
         else if (eventcode == 152)
         { // Opponent resumed game
              if (canShowControllers && isServer && !shotMyTurnDone)
                //  ShotPowerIndicator.anim.Play("MakeVisible");
             GameManager.Instance.opponentActive = true;

                 if ((isServer && !shotMyTurnDone) || !isServer)
                 GameManager.Instance.stopTimer = false;
                 GameManager.Instance.gameControllerScript.hideBubble();

            }
        else
        {
            GameManager.Instance.priv = false;
        }

    }

    private void ShowAllControllers()
    {
        if (canShowControllers)
        {
            Debug.Log("Showing controllers");
        }
    }

    [HideInInspector]
    public bool isServer;
    private bool shotMyTurnDone = true;
    private bool canShowControllers = true;
    public float playerTime;
    public Image timer;
    private void SetMyTurn()
    {
        GameManager.Instance.isMyTurn = true;

        if (GameManager.Instance.miniGame != null)
            GameManager.Instance.miniGame.setMyTurn();

        isServer = true;
        StartTimer();
    }

    private void BotTurn()
    {
        oppoTurnSource.Play();
        //GameManager.Instance.currentPlayer = playerObjects[currentPlayerIndex];
        GameManager.Instance.isMyTurn = false;
        Debug.Log("Bot Turn");
        StartTimer();

        GameManager.Instance.miniGame.BotTurn(true);

        //Invoke("BotDelay", 2.0f);

    }

    private void SetTurn()
    {
        Debug.Log("SET TURN CALLED");
        GameManager.Instance.yes = true;
        for (int i = 0; i < playerObjects.Count; i++)
        {
            playerObjects[i].dice.GetComponent<GameDiceController>().EnableDiceShadow();
        }

        playerObjects[currentPlayerIndex].dice.GetComponent<GameDiceController>().DisableDiceShadow();

        GameManager.Instance.currentPlayer = playerObjects[currentPlayerIndex];

        if (playerObjects[currentPlayerIndex].id == myId)
        {
            SetMyTurn();
        }
        else if (playerObjects[currentPlayerIndex].isBot)
        {
            BotTurn();
        }
        else
        {
            SetOpponentTurn();
        }
    }

    private void BotDelay()
    {
        if (!FinishWindowActive)
        {
            setCurrentPlayerIndex(currentPlayerIndex);
            SetTurn();
        }

    }


    private void setCurrentPlayerIndex(int current)
    {

        while (true)
        {
            current = current + 1;
            currentPlayerIndex = (current) % playerObjects.Count;
            GameManager.Instance.currentPlayer = playerObjects[currentPlayerIndex];
            if (playerObjects[currentPlayerIndex].AvatarObject.GetComponent<PlayerAvatarController>().Active) break;
        }

    }

    private void SetOpponentTurn()
    {
        Debug.Log("Opponent turn");
        oppoTurnSource.Play();
        isServer = false;
        GameManager.Instance.isMyTurn = false;
        /*if (playerObjects[currentPlayerIndex].id.Contains("_BOT"))
        {
            BotTurn();
        }*/

        StartTimer();
    }

    private void StartTimer()
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (i == currentPlayerIndex)
            {
                playerObjects[currentPlayerIndex].timer.SetActive(true);
            }
            else
            {
                playerObjects[i].timer.SetActive(false);
            }
        }
    }

    public void StopTimers()
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            playerObjects[i].timer.SetActive(false);
        }
    }

    public void PauseTimers()
    {
        playerObjects[currentPlayerIndex].timer.GetComponent<UpdatePlayerTimer>().Pause();
    }

    public void restartTimer()
    {
        playerObjects[currentPlayerIndex].timer.GetComponent<UpdatePlayerTimer>().restartTimer();
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)   //GAMESCENE// this gets call when other player leave /DINESH
    {
        Debug.Log("Custom debug: onphotonplayerdis called in gameguicontroller");
        //if (true || EventCounter.hasGameBegan())
            //return;
        Debug.Log("Custom debug: onphotonplayerdis called in gameguicontroller part 2");
        Debug.Log("Player disconnected: " + otherPlayer.NickName);

        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id.Equals(otherPlayer.NickName))
            {
                setPlayerDisconnected(i);
                break;
            }
        }

        CheckPlayersIfShouldFinishGame();
    }

    // public void CheckPlayersIfShouldFinishGame()
    // {
    //     if (!FinishWindowActive)
    //     {
    //         if ((ActivePlayersInRoom == 1 && !iFinished) || ActivePlayersInRoom == 1)
    //         {

    //             StopAndFinishGame();
    //         }

    //         if (iFinished && ActivePlayersInRoom == 1 && CheckIfOtherPlayerIsBot())
    //         {
    //             AddBotToListOfWinners();
    //             StopAndFinishGame();
    //         }
    //     }
    // }


    public void CheckPlayersIfShouldFinishGame()
    {
        Debug.Log("Custom debug: checkplayerifshouldfinishgame called in gameguicontroller");
        if (!FinishWindowActive)
        {
            if ((ActivePlayersInRoom == 1 && !iFinished))
            {
                //if(false && !EventCounter.hasGameBegan()) StopAndFinishGame();
                StopAndFinishGame();
                return;
            }

            if (ActivePlayersInRoom == 0)
            {
                StopAndFinishGame();
                return;
            }

            if (iFinished && ActivePlayersInRoom == 1 && CheckIfOtherPlayerIsBot())
            {
                AddBotToListOfWinners();
                StopAndFinishGame();
                return;
            }

            if (ActivePlayersInRoom > 1 && iFinished)
            {
                TIPButtonObject.SetActive(true);
            }



        }
    }
    public void AddBotToListOfWinners()
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id.Contains("_BOT") && playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().Active)
            {
                playersFinished.Add(playerObjects[i]);
            }
        }
    }

    public bool CheckIfOtherPlayerIsBot()
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id.Contains("_BOT") && playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().Active)
            {
                playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().finished = true;
                return true;
            }
        }
        return false;
    }

    public void setPlayerDisconnected(int i)
    {
        requiredToStart--;
        if (!FinishWindowActive)
        {
            if (!playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().finished)
                ActivePlayersInRoom--;

            Debug.Log("Active players: " + ActivePlayersInRoom);
            if (currentPlayerIndex == i && ActivePlayersInRoom > 1)
            {
                setCurrentPlayerIndex(currentPlayerIndex);
                if (AllPlayersReady)
                    SetTurn();
            }

            Debug.Log("za petla");
            playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().PlayerLeftRoom();

            // LUDO
            playerObjects[i].dice.SetActive(false);
            if (!playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().finished)
            {
                for (int j = 0; j < playerObjects[i].pawns.Length; j++)
                {
                    // playerObjects[i].pawns[j].SetActive(false);
                    playerObjects[i].pawns[j].GetComponent<LudoPawnController>().GoToInitPosition(false);
                }
            }
            // END LUDO
        }
    }
    public void LeaveGame2(bool finishWindow)
    {
        Debug.Log("custom debug: LeaveGame2, line 1653, gameGUI, called");
        if (!iFinished || finishWindow)
        {

            PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 1) + 1);
            SceneManager.LoadScene("MenuScene");
            PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong;

            //GameManager.Instance.cueController.removeOnEventCall();
            PhotonNetwork.LeaveRoom();

            GameManager.Instance.playfabManager.roomOwner = false;
            GameManager.Instance.roomOwner = false;
            GameManager.Instance.resetAllData();
            position = "1";
            StartCoroutine(result_insert1());

        }
        else
        {

            ShowGameFinishWindow();
        }
    }
    public void LeaveGame(bool finishWindow)
    {
       // PhotonNetwork.RaiseEvent((int)EnumPhoton.FinishedGame, EventCounter.GetID(), true, null);
        Debug.Log("custom debug: LeaveGame, line 1679, gameGUI, called");
        if (!iFinished || finishWindow)
        {

            PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 1) + 1);
            SceneManager.LoadScene("MenuScene");
            PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong;

            //GameManager.Instance.cueController.removeOnEventCall();
            PhotonNetwork.LeaveRoom();

            GameManager.Instance.playfabManager.roomOwner = false;
            GameManager.Instance.roomOwner = false;
            GameManager.Instance.resetAllData();
            position = "4";
            StartCoroutine(result_insert1());

        }
        else
        {

            ShowGameFinishWindow();
        }
    }
    public void LeaveGame1(bool finishWindow)
    {
        Debug.Log("custom debug: LeaveGame1, line 1705, gameGUI, called");
        if (!iFinished || finishWindow)
        {

            PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 1) + 1);
            SceneManager.LoadScene("MenuScene");
            PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong;

            //GameManager.Instance.cueController.removeOnEventCall();
            PhotonNetwork.LeaveRoom();

            GameManager.Instance.playfabManager.roomOwner = false;
            GameManager.Instance.roomOwner = false;
            GameManager.Instance.resetAllData();
         

        }
        else
        {

            ShowGameFinishWindow();
        }
    }

    public void ShowHideChatWindow()
    {
        if (!ChatWindow.activeSelf)
        {
            ChatWindow.SetActive(true);
            ChatButton.GetComponent<Text>().text = "X";
        }
        else
        {
            ChatWindow.SetActive(false);
            ChatButton.GetComponent<Text>().text = "CHAT";
        }
    }


    [Header("Result_integrate")]
    public string game_id;
    public string room_id;
    public string position;
    public string user_id;

    public void Result_insert()
    {


    }

    IEnumerator result_insert()
    {

        Debug.Log("user id: " + GameManager.Instance.UserID);
        Debug.Log("payoutCoins: " + GameManager.Instance.payoutCoins);

        string game;
        int bal;

       
      
        WWWForm form = new WWWForm();

        form.AddField("user_id", GameManager.Instance.UserID);
        form.AddField("amount",GameManager.Instance.payoutCoins.ToString());

        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?api/deduct_balance", form);

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




                GameManager.Instance.trr = false;



                Debug.Log(message);

            }
            else
            {
                Debug.Log("Failed!");

            }


        }

    }
    IEnumerator result_insert1()
    {

        Debug.Log("user id: " + GameManager.Instance.UserID);
        Debug.Log("room id: " + GameManager.Instance.RoomID);
        Debug.Log("position: " + position);
        Debug.Log("win amount: " + GameManager.Instance.payoutCoins);

        string game;
        if (GameManager.Instance.trr == true)
        {
            game = GameManager.Instance.GameID;
            Debug.Log("game id0: " + game);

        }
        else
        {
            game = UnityEngine.Random.Range(00000000, 99999999).ToString();
            Debug.Log("game id: " + game);
        }


        double winamounts;
        if (position == "1")
        {
            if (GameManager.Instance.type == MyGameType.TwoPlayer || GameManager.Instance.type == MyGameType.comp || StoreCreateTournamentData.total_player == "2" || GameManager.Instance.t_player == "2")
            {
                //winamounts = GameManager.Instance.payoutCoins + (80 * GameManager.Instance.payoutCoins / 100);
                //winamounts = GameManager.Instance.payoutCoins + insertAmountP1;
                winamounts = insertAmountP1;
                Debug.Log("Position1 " + winamounts);
            }
            else
            {
                //winamounts = (int)(3.6 * GameManager.Instance.payoutCoins);
                //winamounts = GameManager.Instance.payoutCoins + insertAmountP1;
                winamounts = insertAmountP1;
                Debug.Log("Position1 " + winamounts);
            }
        }
        else if (position == "2")
        {
            //winamounts = 0;
            //winamounts = GameManager.Instance.payoutCoins + insertAmountP2;
            winamounts = insertAmountP2;
            Debug.Log("Position2 " + winamounts);
        }

        else if (position == "3")
        {
            //winamounts = 0;
            //winamounts = GameManager.Instance.payoutCoins + insertAmountP3;
            winamounts =  insertAmountP3;
            Debug.Log("Position3 " + winamounts);
        }

        else 
        {
            //winamounts = 0;
            //winamounts = GameManager.Instance.payoutCoins + insertAmountP4;
            winamounts =insertAmountP4;
            Debug.Log("win amount4: " + winamounts);
        }

        WWWForm form = new WWWForm();

        form.AddField("user_id", GameManager.Instance.UserID);
        if (GameManager.Instance.trr == true)
        {
            form.AddField("game_id", game);
        }
        form.AddField("position", position);
        form.AddField("room_id", GameManager.Instance.RoomID);
        form.AddField("amount", winamounts.ToString());

        UnityWebRequest www = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?api/result_insert", form);

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





                GameManager.Instance.trr = false;


                Debug.Log(message);

            }
            else
            {
                Debug.Log("Failed!");

            }


        }

    }
    public GameObject panel;
 
}
