using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using AssemblyCSharp;
using static InitMenuScript;

public class ControlAvatars : MonoBehaviour
{
    public AudioSource playerJoin;
    public AudioSource playerLeft;
    public GameObject FailedToJoinRoomWindow;
    public GameObject FailedToJoinRoomText;
    public GameObject CancelButton;
    public GameObject startButtonPrivate;
    public GameObject RoomIDObject;
    public GameObject RoomIDText;
    public GameObject[] TemporarySlots;

    public GameObject[] OppoProgressBar;
    public GameObject bg;

    public GameObject[] OppoAvatar;
    public GameObject[] OppoAvatarImage;
    public GameObject[] InviteToJoinButtons;



    public GameObject prefab;
    private List<GameObject> avatars;
    private GameObject lastAvatar;
    private Sprite restoreSprite;
    private int addAvatars = 10;
    public bool foundPlayer = false;
    public bool playerRejected = false;
    public float speed;
    private float speed1;
    public bool foundCancel = false;
    public Sprite prite;
    private GameObject OpponentAvatar;
    public Text opponentNameText;
    public Sprite noAvatarSprite;
    public GameObject cancelGameButton;

    public GameObject menuCanvas;
    public GameObject titleCanvas;
    public GameObject matchPlayersCanvas;

    public GameObject AvatarFrameMy;
    public GameObject AvatarFrameOpponent;
    public GameObject vsText;
    public GameObject centerCoins;
    public GameObject leftcoins;
    public GameObject rightCoins;

    public GameObject oppontentCoinImage;
    public GameObject myCoinImage;
    public GameObject oppontentPayoutCoins;
    public GameObject myPayoutCoins;
    public GameObject centerPayoutCoins;

    private float rightCoinsPos = 500;
    private float leftCoinsPos = -500;
    private float centerCoinsPos = -550;
    private float avatrLeftPos = -500;
    private float avatarRightPos = 500;
    private float vsTextpos = 0;
    private AudioSource[] audioSources;
    public GameObject cantPlayNowOppo;
    public GameObject longTimeMessage;
    // Use this for initialization
    public float waitingOpponentTime = 0f;
    public GameObject messageBubbleText;
    public GameObject messageBubble;
    public bool opponentActive = true;
    public bool GameSceneLoaded = false;

    private List<int> playerDisconnectedIndexes = new List<int>();
    void Awake()
    {
        audioSources = GetComponents<AudioSource>();
    }

    void Start()
    {

        GameManager.Instance.controlAvatars = this;
    }

    public void CancelWaitingForPlayer()
    {
        Debug.Log("custom debug: cancelWaitingForPlayer, line 92, controlAvatars, called");
        PhotonNetwork.LeaveRoom();
    }


    public void ShowJoinFailed(string error)
    {
        FailedToJoinRoomWindow.SetActive(true);
    }

    public void reset()
    {
        startButtonPrivate.GetComponent<Button>().interactable = false;
        if (GameManager.Instance.type == MyGameType.Private && !GameManager.Instance.JoinedByID)
        {
            Debug.Log("Timeout infinity");
            PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeout;
        }
        else
        {
            Debug.Log("Timeout 60s");
            PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeout;
        }

        GameSceneLoaded = false;

        if (GameManager.Instance.type == MyGameType.TwoPlayer || GameManager.Instance.type == MyGameType.comp || StoreCreateTournamentData.total_player == "2" || GameManager.Instance.t_player == "2")
        {
            GameManager.Instance.requiredPlayers = 2;
        }
        else
        {
            if (StaticStrings.isFourPlayerModeEnabled || StoreCreateTournamentData.total_player == "4" || GameManager.Instance.t_player == "4")
            {
                GameManager.Instance.requiredPlayers = 4;
            }
            else
            {
                GameManager.Instance.requiredPlayers = 2;
            }

        }

        RoomIDObject.SetActive(false);

        if (GameManager.Instance.type == MyGameType.Private && !GameManager.Instance.JoinedByID || GameManager.Instance.JoinedByID)
        {
            RoomIDObject.SetActive(true);
            RoomIDText.GetComponent<Text>().text = "Fetching...";
        }
        else
        {
            RoomIDObject.SetActive(false);
        }

        CancelButton.SetActive(true);

        for (int i = 0; i < InviteToJoinButtons.Length; i++)
        {
            if (GameManager.Instance.type != MyGameType.Private || GameManager.Instance.JoinedByID)
            {
                InviteToJoinButtons[i].SetActive(false);
            }
            else
            {
                InviteToJoinButtons[i].SetActive(true);
            }

        }


        for (int i = 0; i < OppoAvatar.Length; i++)
        {
            OppoAvatar[i].SetActive(false);
        }

        if (!StaticStrings.isFourPlayerModeEnabled)
        {
            OppoAvatar[1].SetActive(false);
            OppoAvatar[2].SetActive(false);
            InviteToJoinButtons[1].SetActive(false);
            InviteToJoinButtons[2].SetActive(false);
        }

        matchPlayersCanvas.SetActive(true);
        showLongTimeMessage();



        if (GameManager.Instance.requiredPlayers == 2)
        {
            for (int i = 1; i < OppoProgressBar.Length; i++)
            {
                OppoProgressBar[i].SetActive(false);
                bg.SetActive(false);
            }
        }
        else if (StoreCreateTournamentData.total_player == "2")
        {

            for (int i = 1; i < OppoProgressBar.Length; i++)
            {
                OppoProgressBar[i].SetActive(false);

            }

        }
    }

    public void ShareCode()
    {
        NativeShare share = new NativeShare();
        string shareText = StaticStrings.SharePrivateLinkMessage + " " + RoomIDText.GetComponent<Text>().text + "\n\n" + StaticStrings.SharePrivateLinkMessage2 + " ";
#if UNITY_ANDROID
        shareText += "https://play.google.com/store/apps/details?id=" + StaticStrings.AndroidPackageName;
#elif UNITY_IOS
        shareText += "https://itunes.apple.com/us/app/apple-store/id" + StaticStrings.ITunesAppID;
#endif
        //share.Share(shareText, null, null, "Share via");
    }

    public void setCancelButton()
    {
        if (GameManager.Instance.type == MyGameType.Private)
        {
            CancelButton.SetActive(true);
        }

    }

    public void updateRoomID(string id)
    {
        GameManager.Instance.privateRoomID = id;
        RoomIDText.GetComponent<Text>().text = id;
        GameManager.Instance.RoomID = id;
    }

    public void PlayerJoined(int index, string id)
    {
        Debug.LogError("began counter");
        Debug.Log("PLAYJOINED");
        TemporarySlots[index].SetActive(false);
        GameManager.Instance.currentPlayersCount++;

        if (GameManager.Instance.opponentsIDs.Contains(id))
        {
            playerJoin.Play();

            InviteToJoinButtons[index].SetActive(false);
            OppoAvatar[index].SetActive(true);
            if (GameManager.Instance.opponentsAvatars[index] != null)
                OppoAvatarImage[index].GetComponent<Image>().sprite = GameManager.Instance.opponentsAvatars[index];

            Debug.Log("Current players count: " + GameManager.Instance.currentPlayersCount);
            Debug.Log("1");
            if (GameManager.Instance.currentPlayersCount >= GameManager.Instance.requiredPlayers)
            {
                Debug.Log("2");
                if (PhotonNetwork.isMasterClient)
                {
                    Debug.Log("3");
                    hideLongTimeMessage();
                    if (!EventCounter.hasGameBegan())
                    {
                        Debug.Log("4");
                        GameManager.Instance.playfabManager.StartGame();
                        EventCounter.SetGameBegan(true);
                    }
                }
            }
            else
            {
                Debug.Log("5");
                if (PhotonNetwork.isMasterClient)
                {
                    Debug.Log("6");
                    if (!EventCounter.hasGameBegan())
                    {
                        Debug.Log("7");
                        GameManager.Instance.playfabManager.WaitForNewPlayer();
                       // EventCounter.SetGameBegan(true);
                    }
                    Debug.Log("INVOKE PLAYJOINED");
                }
            }
        }
        //StartCoroutine(SlowDownSlot(5f, index, id));

    }

    IEnumerator SlowDownSlot(float timeSlow, int index, string id)
    {
        yield return new WaitForEndOfFrame();
        /*float speed = 35f;
        while(speed > .1f)
        {
            speed -= timeSlow * Time.deltaTime;
            Debug.Log("for slot index:"+index+", speed: "+speed);
            TemporarySlots[index].GetComponentInChildren<SlotRollCustom>().SetSpeed(speed);
            yield return new WaitForEndOfFrame();
        }*/
        /*foreach (GameObject ts in TemporarySlots)
            ts.SetActive(false);*/
        /*TemporarySlots[index].SetActive(false);
        GameManager.Instance.currentPlayersCount++;

        if (GameManager.Instance.opponentsIDs.Contains(id))
        {
            playerJoin.Play();

            InviteToJoinButtons[index].SetActive(false);
            OppoAvatar[index].SetActive(true);
            if (GameManager.Instance.opponentsAvatars[index] != null)
                OppoAvatarImage[index].GetComponent<Image>().sprite = GameManager.Instance.opponentsAvatars[index];

            Debug.Log("Current players count: " + GameManager.Instance.currentPlayersCount);

            if (GameManager.Instance.currentPlayersCount >= GameManager.Instance.requiredPlayers)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    hideLongTimeMessage();
                    if (!EventCounter.hasGameBegan())
                    {
                        GameManager.Instance.playfabManager.StartGame();
                        EventCounter.SetGameBegan(true);
                    }
                }
            }
            else
            {
                if (PhotonNetwork.isMasterClient)
                {
                    if (!EventCounter.hasGameBegan())
                    {
                        GameManager.Instance.playfabManager.WaitForNewPlayer();
                        EventCounter.SetGameBegan(true);
                    }
                    Debug.Log("INVOKE PLAYJOINED");
                }
            }
        }*/
    }

    public void PlayerDisconnected(int index)
    {
        playerLeft.Play();
        GameManager.Instance.currentPlayersCount--;
        GameManager.Instance.opponentsIDs[index] = null;
        GameManager.Instance.opponentsNames[index] = null;
        GameManager.Instance.opponentsAvatars[index] = null;
        if (GameManager.Instance.type == MyGameType.Private && !GameManager.Instance.JoinedByID)
            InviteToJoinButtons[index].SetActive(true);
        OppoAvatar[index].SetActive(false);
        Debug.Log("Current players count: " + GameManager.Instance.currentPlayersCount);
    }


    public void showLongTimeMessage()
    {
        longTimeMessage.SetActive(true);
        time = 90;
        StartCoroutine("timer");
    }

    int time = 45;
    IEnumerator timer()
    {
        time--;
        longTimeMessage.GetComponent<Text>().text = time.ToString();
        yield return new WaitForSeconds(1f);
        StartCoroutine("timer");
    }


    public void hideLongTimeMessage()
    {
        longTimeMessage.SetActive(false);
        StopCoroutine("timer");
    }

    // Update is called once per frame
    bool changedAvatar = false;
    bool startedGame = false;
    void Update()
    {
        if (!startedGame)
        {
            if (foundPlayer)
            {

                if (speed1 > 3f)
                    speed1 -= speed / 200.0f;//0.25f;

                if (speed1 < speed * (2.8f / 4.0f) && !changedAvatar)
                {
                    changedAvatar = true;
                    OpponentAvatar = lastAvatar;
                    restoreSprite = lastAvatar.GetComponent<Image>().sprite;
                    if (GameManager.Instance.avatarOpponent != null)
                        lastAvatar.GetComponent<Image>().sprite = GameManager.Instance.avatarOpponent;
                    else
                        lastAvatar.GetComponent<Image>().sprite = noAvatarSprite;
                }

                if (speed1 <= 0)
                {
                    speed1 = speed / 100.0f;
                }

                if (OpponentAvatar != null && OpponentAvatar.GetComponent<RectTransform>().anchoredPosition.y <= 0)
                {
                    speed1 = 0.0f;

                    foreach (GameObject avatar in avatars)
                    {
                        avatar.SetActive(false);
                    }


                    audioSources[1].Play();

                    OpponentAvatar.SetActive(true);
                    OpponentAvatar.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    opponentNameText.text = GameManager.Instance.nameOpponent;
                    foreach (GameObject ts in TemporarySlots)
                        ts.SetActive(false);
                    startedGame = true;




                    if (PhotonNetwork.playerList.Length < 2 || playerRejected)
                    {
                        playerDisconnected();
                    }
                    else
                    {
                        coinsAnimate();
                        AvatarFrameMy.GetComponent<Animator>().Play("MySelectorMoveOut");
                        AvatarFrameOpponent.GetComponent<Animator>().Play("OpponentFrameMoveOut");
                        vsText.GetComponent<Animator>().Play("VsTextAnim");
                        centerCoins.GetComponent<Animator>().Play("CoinsCenter");
                        leftcoins.GetComponent<Animator>().Play("OppontentCoins");
                        rightCoins.GetComponent<Animator>().Play("MyCoinsA");

                        StartCoroutine(countDownCoins(GameManager.Instance.payoutCoins));
                    }
                    GameManager.Instance.readyToAnimateCoins = true;

                }
            }



        }
    }

    Text oppontent;
    Text my;
    Text center;
    Image opImage;
    Image myImage;
    Image lc;
    private void coinsAnimate()
    {

        oppontent = oppontentPayoutCoins.GetComponent<Text>();
        my = myPayoutCoins.GetComponent<Text>();
        center = centerPayoutCoins.GetComponent<Text>();
        opImage = oppontentCoinImage.GetComponent<Image>();
        myImage = myCoinImage.GetComponent<Image>();
        lc = leftcoins.GetComponent<Image>();

        my.color = new Color(my.color.r, my.color.g, my.color.b, 1);
        oppontent.color = new Color(oppontent.color.r, oppontent.color.g, oppontent.color.b, 1);
        opImage.color = new Color(opImage.color.r, opImage.color.g, opImage.color.b, 1);
        myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, 1);

        if (GameManager.Instance.payoutCoins >= 1000)
        {
            if (GameManager.Instance.payoutCoins >= 1000000)
            {
                if (GameManager.Instance.payoutCoins % 1000000.0f == 0)
                {
                    my.text = (GameManager.Instance.payoutCoins / 1000000.0f).ToString("0") + "M";
                    oppontent.text = (GameManager.Instance.payoutCoins / 1000000.0f).ToString("0") + "M";
                }
                else
                {
                    my.text = (GameManager.Instance.payoutCoins / 1000000.0f).ToString("0.0") + "M";
                    oppontent.text = (GameManager.Instance.payoutCoins / 1000000.0f).ToString("0.0") + "M";
                }

            }
            else
            {
                if (GameManager.Instance.payoutCoins % 1000.0f == 0)
                {
                    my.text = (GameManager.Instance.payoutCoins / 1000.0f).ToString("0") + "k";
                    oppontent.text = (GameManager.Instance.payoutCoins / 1000.0f).ToString("0") + "k";
                }
                else
                {
                    my.text = (GameManager.Instance.payoutCoins / 1000.0f).ToString("0.0") + "k";
                    oppontent.text = (GameManager.Instance.payoutCoins / 1000.0f).ToString("0.0") + "k";
                }

            }
        }
        else
        {
            oppontent.text = GameManager.Instance.payoutCoins + "";
            my.text = GameManager.Instance.payoutCoins + "";
        }

        center.text = "0";

    }

    private IEnumerator countDownCoins(double count)
    {

        Debug.Log("STAET");

        StartCoroutine(waitSecs(5));
        Debug.Log("END");
        int loops = 50;
        double minus = count / loops;
        double current = count;
        double centerCurrent = 0;

        float minusAlpha = 1.0f / loops;

        yield return new WaitForSeconds(2);

        audioSources[2].Play();

        for (int i = 0; i < loops; i++)
        {


            my.color = new Color(my.color.r, my.color.g, my.color.b, my.color.a - minusAlpha);
            oppontent.color = new Color(oppontent.color.r, oppontent.color.g, oppontent.color.b, oppontent.color.a - minusAlpha);
            opImage.color = new Color(opImage.color.r, opImage.color.g, opImage.color.b, opImage.color.a - minusAlpha);
            myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, myImage.color.a - minusAlpha);

            current -= minus;
            centerCurrent += minus * 2;


            if (count >= 1000)
            {
                if (count >= 1000000)
                {
                    my.text = (current / 1000000.0f).ToString("0.0") + "M";
                    oppontent.text = (current / 1000000.0f).ToString("0.0") + "M";


                    center.text = (centerCurrent / 1000000.0f).ToString("0.0") + "M";


                }
                else
                {
                    my.text = (current / 1000.0f).ToString("0.0") + "k";
                    oppontent.text = (current / 1000.0f).ToString("0.0") + "k";
                    if (centerCurrent >= 1000000)
                    {

                        center.text = (centerCurrent / 1000000.0f).ToString("0.0") + "M";


                    }
                    else
                    {

                        center.text = (centerCurrent / 1000.0f).ToString("0.0") + "k";


                    }
                }
            }
            else
            {
                my.text = current + "";
                oppontent.text = current + "";
                if (centerCurrent >= 1000)
                {

                    center.text = (centerCurrent / 1000.0f).ToString("0.0") + "k";


                }
                else
                {
                    center.text = centerCurrent + "";
                }
            }



            if (current > 0)
            {
                //				StartCoroutine (waitSecs (1));
                yield return new WaitForSeconds(0.04f);
                //yield return new WaitForSeconds (1f);
            }

        }

        if (centerCurrent >= 1000)
        {
            if (centerCurrent >= 1000000)
            {
                if (centerCurrent % 1000000.0f == 0)
                {
                    center.text = (centerCurrent / 1000000.0f).ToString("0") + "M";
                }
            }
            else
            {
                if (centerCurrent % 1000.0f == 0)
                {
                    center.text = (centerCurrent / 1000.0f).ToString("0") + "k";
                }
            }
        }

        float alpha = 1.0f;



        for (int i = 0; i < 20; i++)
        {

            alpha -= (1 / 20.0f);
            //Color c1 = leftcoins.GetComponent <Image> ().color;
            //c1.a = alpha;
            //Debug.Log ("AAAA + " + c1.a);
            lc.color = new Color(1, 1, 1, alpha);

            Color c2 = rightCoins.GetComponent<Image>().color;
            c2.a = alpha;
            rightCoins.GetComponent<Image>().color = c2;

            yield return new WaitForSeconds(0.01f);
        }



        startGame();
    }



    private IEnumerator waitSecs(float milis)
    {
        yield return new WaitForSeconds(milis);
    }

    public void playerDisconnected()
    {
        Debug.Log("custom debug: playerDIsconnected, line 591, controlAvatars, called");
        StopAllCoroutines();
        rightCoins.SetActive(false);
        leftcoins.SetActive(false);
        cantPlayNowOppo.SetActive(true);
        PhotonNetwork.LeaveRoom();
        Invoke("cancelGame", 5.0f);
    }

    private void cancelGame()
    {
        cantPlayNowOppo.SetActive(false);
        matchPlayersCanvas.SetActive(false);
        PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong; ;
        Debug.Log("Timeout 1");
        //reset ();
    }

    public void StartGamePrivate()
    {
        //PhotonNetwork.RaiseEvent((int)EnumPhoton.BeginPrivateGame, null, true, null);
        GameManager.Instance.playfabManager.StartGame();
    }

    private void startGame()
    {
        GameObject.Find("PlayFabManager").GetComponent<PlayFabManager>().imReady = true;
        if (!GameManager.Instance.offlineMode)
        {

            PhotonNetwork.RaiseEvent(199, GameManager.Instance.cueIndex + "-" + GameManager.Instance.cueTime, true, null);
        }

        if (PhotonNetwork.playerList.Length < 2)
        {
            playerDisconnected();
        }

        //SceneManager.LoadScene ("GameScene");
        //reset ();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            PhotonNetwork.SendOutgoingCommands();
            Debug.Log("Application pause");
        }
        else
        {
            PhotonNetwork.SendOutgoingCommands();
            Debug.Log("Application resume");
        }
    }

    public void hideMessageBubble()
    {
        messageBubble.GetComponent<Animator>().Play("HideBubble");
    }

    public IEnumerator updateMessageBubbleText()
    {
        yield return new WaitForSeconds(1.0f * 2);
        waitingOpponentTime -= 1;
        if (!GameManager.Instance.opponentDisconnected)
            messageBubbleText.GetComponent<Text>().text = StaticStrings.waitingForOpponent + " " + waitingOpponentTime;
        if (waitingOpponentTime > 0 && !opponentActive && !GameManager.Instance.opponentDisconnected)
        {
            StartCoroutine(updateMessageBubbleText());
        }
    }

    public void test()
    {

    }

    public void cancelMatching()
    {
        //		if (!foundPlayer && PhotonNetwork.otherPlayers.Length == 0) {
        //			PhotonNetwork.LeaveRoom ();
        //			matchPlayersCanvas.SetActive (false);
        //		}
        cancelGameButton.SetActive(false);
        Invoke("cancelGameInvoke", 3.0f);
    }

    public void cancelGameInvoke()
    {
        Debug.Log("Length: " + PhotonNetwork.otherPlayers.Length);
        if (!foundPlayer && PhotonNetwork.otherPlayers.Length == 0)
        {

            PhotonNetwork.LeaveRoom();
            matchPlayersCanvas.SetActive(false);
            PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong;
            Debug.Log("Timeout 2");
            GameManager.Instance.playfabManager.imReady = false;
            //GameObject.Find ("PlayFabManager").GetComponent <PlayFabManager> ().imReady = false;
        }
    }


}
