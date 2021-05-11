using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdatePlayerTimer : MonoBehaviour
{
    public float playerTime;
    public GameObject timerObject;
    public Image timer;
    private bool timeSoundsStarted;
    public AudioSource[] audioSources;
    public GameObject GUIController;
    public bool myTimer;

    public bool oppo1;
    public bool oppo2;
    public bool oppo3;
    public bool paused = false;
    public bool halted = false;
  
    // Use this for initialization
    void Start()
    {
        timer = gameObject.GetComponent<Image>();
        GameManager.Instance.priv = true;
      
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        timer = gameObject.GetComponent<Image>();
    }

    public void Pause(bool stay)
    {
        if (!stay) { paused = true; }
        audioSources[0].Stop();
    }
    public void Pause()
    {
        paused = true;
        audioSources[0].Stop();
    }
    public void Play()
    {
        paused = false;
        if (!audioSources[0].isPlaying)
        {
            audioSources[0].Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!halted)
        {
            if (!paused)
                Play();
            if(GameManager.Instance.yes)
            updateClock();
        }
        else
        {
            Pause(true);
        }
    }

    bool autoMoved = false;

    public void restartTimer()
    {
        paused = false;
        timer.fillAmount = 1.0f;
        autoMoved = false;
    }


    void OnDisable()
    {
        if (timer != null)
        {
            timer.fillAmount = 1.0f;
            paused = false;
            audioSources[0].Stop();
        }
    }


    int autoMoveCounter = 5;



  

    private void updateClock()
    {
        float minus;

        playerTime = GameManager.Instance.playerTime;
        if (GameManager.Instance.offlineMode)
            playerTime = GameManager.Instance.playerTime + GameManager.Instance.cueTime;
        minus = 1.0f / playerTime * Time.deltaTime;

        timer.fillAmount -= minus;
       

        if (timer.fillAmount < 0.25f && !timeSoundsStarted)
        {
            audioSources[0].Play();
            timeSoundsStarted = true;
            
        }

        if (System.Math.Abs(timer.fillAmount) <= 0.15f && GameManager.Instance.yes == true)
        {
            if (myTimer && Application.internetReachability != NetworkReachability.NotReachable)
            {
                if(autoMoveCounter > 0)
                {
                    LudoPawnController _lpc = GameObject.Find("LudoPawn").GetComponent<LudoPawnController>();
                    if (_lpc.highlight.activeInHierarchy)
                    {
                        //move highlighted pawn
                        _lpc.MakeMove();
                        autoMoved = true;
                        autoMoveCounter--;
                        EventCounter.autoMovesCount--;
                    }
                    else
                    {
                        _lpc = GameObject.Find("LudoPawn (1)").GetComponent<LudoPawnController>();
                        if (_lpc.highlight.activeInHierarchy)
                        {
                            //move highlighted pawn
                            _lpc.MakeMove();
                            autoMoved = true;
                            autoMoveCounter--;
                            EventCounter.autoMovesCount--;
                        }
                        else
                        {
                            _lpc = GameObject.Find("LudoPawn (2)").GetComponent<LudoPawnController>();
                            if (_lpc.highlight.activeInHierarchy)
                            {
                                //move highlighted pawn
                                _lpc.MakeMove();
                                autoMoved = true;
                                autoMoveCounter--;
                                EventCounter.autoMovesCount--;
                            }
                            else
                            {
                                _lpc = GameObject.Find("LudoPawn (3)").GetComponent<LudoPawnController>();
                                if (_lpc.highlight.activeInHierarchy)
                                {
                                    //move highlighted pawn
                                    _lpc.MakeMove();
                                    autoMoved = true;
                                    autoMoveCounter--;
                                    EventCounter.autoMovesCount--;
                                }
                                else
                                {
                                    if (!autoMoved)
                                    {
                                        GameDiceController _gdc = GameObject.Find("Dice1").GetComponent<GameDiceController>();
                                        if (_gdc.isMyDice)
                                        {
                                            _gdc.AutoDiceRollCustom();
                                            autoMoved = true;
                                            autoMoveCounter--;
                                            EventCounter.autoMovesCount--;
                                        }
                                        else
                                        {
                                            _gdc = GameObject.Find("Dice2").GetComponent<GameDiceController>();
                                            if (_gdc.isMyDice)
                                            {
                                                _gdc.AutoDiceRollCustom();
                                                autoMoved = true;
                                                autoMoveCounter--;
                                                EventCounter.autoMovesCount--;
                                            }
                                            else
                                            {
                                                _gdc = GameObject.Find("Dice3").GetComponent<GameDiceController>();
                                                if (_gdc.isMyDice)
                                                {
                                                    _gdc.AutoDiceRollCustom();
                                                    autoMoved = true;
                                                    autoMoveCounter--;
                                                    EventCounter.autoMovesCount--;
                                                }
                                                else
                                                {
                                                    _gdc = GameObject.Find("Dice4").GetComponent<GameDiceController>();
                                                    if (_gdc.isMyDice)
                                                    {
                                                        _gdc.AutoDiceRollCustom(); autoMoved = true;
                                                        autoMoveCounter--;
                                                        EventCounter.autoMovesCount--;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("exiting coz auto moves left = " + autoMoveCounter);
                    PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 1) + 1);
                    GameGUIController.GetInstance().LeaveGame(true);

                    //  PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong;

                    //GameManager.Instance.cueController.removeOnEventCall();
                    // PhotonNetwork.LeaveRoom();

                    GameManager.Instance.playfabManager.roomOwner = false;
                    GameManager.Instance.roomOwner = false;
                    GameManager.Instance.resetAllData();
                    SceneManager.LoadScene("MenuScene");
                    return;
                }
            }
            else
            {
                if (!autoMoved && System.Math.Abs(timer.fillAmount) <= 0f && GameManager.Instance.yes == true)
                {
                    if (false) { }
                    else
                    {
                        GameManager.Instance.yes = false;
                        Debug.Log("TIME 0");
                        if (GameManager.Instance.currentPlayer.timerMissed == 1)
                        {
                            Debug.Log("Skip turn out");
                            PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 1) + 1);
                            GameGUIController.GetInstance().LeaveGame(true);

                            //  PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong;

                            //GameManager.Instance.cueController.removeOnEventCall();
                            // PhotonNetwork.LeaveRoom();

                            GameManager.Instance.playfabManager.roomOwner = false;
                            GameManager.Instance.roomOwner = false;
                            GameManager.Instance.resetAllData();
                            SceneManager.LoadScene("MenuScene");
                            return;
                        }
                        if (myTimer)
                        {
                            GameManager.Instance.currentPlayer.timerMissed++;

                            //GameManager.Instance.currentPlayer.AvatarObject.GetComponent<PlayerAvatarController>().chance[GameManager.Instance.currentPlayer.timerMissed].sprite = GameManager.Instance.currentPlayer.AvatarObject.GetComponent<PlayerAvatarController>().red;
                        }
                        audioSources[0].Stop();
                        GameManager.Instance.stopTimer = true;
                        if (!GameManager.Instance.offlineMode)
                        {
                            if (myTimer)
                            {
                                Debug.Log("Timer call finish turn");

                                GUIController.GetComponent<GameGUIController>().SendSkipedTurn();
                            }

                            else if (oppo1)
                            {
                                GameGUIController.GetInstance().ChangeTurn();

                                GameGUIController.GetInstance().cout1++;
                            }
                            else if (oppo2)
                            {
                                GameGUIController.GetInstance().ChangeTurn();

                                GameGUIController.GetInstance().cout++;
                            }
                            else if (oppo3)
                            {
                                GameGUIController.GetInstance().ChangeTurn();

                                GameGUIController.GetInstance().cout2++;
                            }
                            else
                            {
                                if (GameManager.Instance.priv == true)
                                {
                                    GameManager.Instance.priv = false;

                                    Debug.Log("Opponent Timer call finish turn");
                                    GUIController.GetComponent<GameGUIController>().panel.SetActive(false);
                                }
                                else if (GameManager.Instance.priv == false)
                                {
                                    Debug.Log("Opponent Timer call finish turn");
                                    GUIController.GetComponent<GameGUIController>().panel.SetActive(true);
                                }


                            }
                            //PhotonNetwork.RaiseEvent(9, null, true, null);
                        }
                        else
                        {
                            GameManager.Instance.wasFault = true;
                            GameManager.Instance.cueController.setTurnOffline(true);
                        }

                        //showMessage("You " + StaticStrings.runOutOfTime);

                        /*if (!GameManager.Instance.offlineMode)
                        {
                            GameManager.Instance.cueController.setOpponentTurn();
                        }*/
                    }

                }
            }


        }



    }
}
