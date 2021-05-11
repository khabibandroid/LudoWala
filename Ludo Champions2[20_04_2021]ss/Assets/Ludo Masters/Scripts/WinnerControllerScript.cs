﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AssemblyCSharp;
using System;

public class WinnerControllerScript : MonoBehaviour
{

    public GameObject myObject;
    public GameObject opponentObject;
    public GameObject shareButton;

    public bool isGameScene = false;

    public Image myImage;
    public Image oppoImage;

    public Text myName;
    public Text oppoText;

    public GameObject myMessageBubble;
    public GameObject oppoMessageBubble;

    public GameObject rematchButton;

    public bool rematchRequest = false;
    public bool sentRematch = false;

    public GameObject ChatMessagesList;
    public GameObject ChatMessageButtonPrefab;

    public GameObject ChatMessagesObject;

    public GameObject prizeText;
    private AudioSource[] audioSources;
    public GameObject reardShareText;

    public bool messageDialogVisible = false;
    // Use this for initialization
    void Start()
    {



        audioSources = GetComponents<AudioSource>();

        if (GameManager.Instance.playerDisconnected)
        {
            GameManager.Instance.playerDisconnected = false;
            if (!isGameScene)
            {
                rematchButton.SetActive(false);
            }
        }



        if (!isGameScene)
        {


            PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong; ;

          //  if (GameManager.Instance.payoutCoins > StoredUserData.coins)
           // {
           //     rematchButton.SetActive(false);
           // }

            if (reardShareText != null)
                reardShareText.GetComponent<Text>().text = "+" + StaticStrings.rewardCoinsForShareViaFacebook;

            // if (StaticStrings.showAdOnGameOverScene)
            //     AdsManager.Instance.adsScript.ShowAd();

            rematchRequest = false;
            sentRematch = false;

            if (GameManager.Instance.iWon)
            {
                myObject.GetComponent<Animator>().Play("WinnerOpponentAnimation");
                audioSources[0].Play();

            }
            else if (GameManager.Instance.iDraw)
            {
                myObject.GetComponent<Animator>().Play("WinnerOpponentAnimation");
                opponentObject.GetComponent<Animator>().Play("WinnerOpponentAnimation");
                audioSources[0].Play();

            }
            else
            {
                opponentObject.GetComponent<Animator>().Play("WinnerOpponentAnimation");
                audioSources[1].Play();
            }

            if (GameManager.Instance.avatarMy != null)
                myImage.sprite = GameManager.Instance.avatarMy;
            if (GameManager.Instance.avatarOpponent != null)
                oppoImage.sprite = GameManager.Instance.avatarOpponent;

            myName.text = Login.getInstance().NAme;
            oppoText.text = GameManager.Instance.nameOpponent;

            double prizeCoins = GameManager.Instance.payoutCoins * 2;

            if (prizeCoins >= 1000)
            {
                if (prizeCoins >= 1000000)
                {
                    if (prizeCoins % 1000000.0f == 0)
                    {
                        prizeText.GetComponent<Text>().text = (prizeCoins / 1000000.0f).ToString("0") + "M";

                    }
                    else
                    {
                        prizeText.GetComponent<Text>().text = (prizeCoins / 1000000.0f).ToString("0.0") + "M";

                    }

                }
                else
                {
                    if (prizeCoins % 1000.0f == 0)
                    {
                        prizeText.GetComponent<Text>().text = (prizeCoins / 1000.0f).ToString("0") + "k";
                    }
                    else
                    {
                        prizeText.GetComponent<Text>().text = (prizeCoins / 1000.0f).ToString("0.0") + "k";
                    }

                }
            }
            else
            {
                prizeText.GetComponent<Text>().text = prizeCoins + "";
            }
        }

        for (int i = 0; i < StaticStrings.chatMessages.Length; i++)
        {
            GameObject button = Instantiate(ChatMessageButtonPrefab);
            button.transform.GetChild(0).GetComponent<Text>().text = StaticStrings.chatMessages[i];
            button.transform.parent = ChatMessagesList.transform;
            button.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
            string index = StaticStrings.chatMessages[i];
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            button.GetComponent<Button>().onClick.AddListener(() => SendMessageEvent(index));
        }



    }



    void OnDestroy()
    {
        removeOnEventCall();
    }

    public void SendMessageEvent(string index)
    {
        Debug.Log("Button Clicked " + index);
        if (!GameManager.Instance.offlineMode)
            PhotonNetwork.RaiseEvent(193, index, true, null);
        ChatMessagesObject.GetComponent<Animator>().Play("hideMessageDialog");
        messageDialogVisible = false;

        if (isGameScene)
        {
            myMessageBubble.SetActive(true);
            myMessageBubble.transform.GetChild(0).GetComponent<Text>().text = index;
            if (isGameScene)
            {
                CancelInvoke("hideMyMessageBubble");
                Invoke("hideMyMessageBubble", 6.0f);
            }
        }

    }

    public void loadMenuScene()
    {
        Debug.Log("custom debug: loadMenuScene, line 189, WinnerControllerScript, called");
        // if (GameManager.Instance.offlineMode && StaticStrings.showAdWhenLeaveGame)
        //     AdsManager.Instance.adsScript.ShowAd();
        SceneManager.LoadScene("MenuScene");
        Debug.Log("Timeout 6");
        PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong; ;
        if (!GameManager.Instance.offlineMode)
            PhotonNetwork.RaiseEvent(194, 1, true, null);
        removeOnEventCall();

        GameManager.Instance.cueController.removeOnEventCall();
        PhotonNetwork.LeaveRoom();

        GameManager.Instance.playfabManager.roomOwner = false;
        GameManager.Instance.roomOwner = false;
        GameManager.Instance.resetAllData();

    }

    public void sendRematchRequest()
    {
        if (!rematchRequest)
        {
            sentRematch = true;
            Debug.Log("Send message");
            if (!GameManager.Instance.offlineMode)
                PhotonNetwork.RaiseEvent(195, 1, true, null);
            myMessageBubble.SetActive(true);
            myMessageBubble.transform.GetChild(0).GetComponent<Text>().text = StaticStrings.IWantPlayAgain;
            rematchButton.SetActive(false);
        }
        else
        {
            Debug.Log("Send message");
            if (!GameManager.Instance.offlineMode)
                PhotonNetwork.RaiseEvent(195, 1, true, null);
            rematchButton.SetActive(false);
            GameManager.Instance.resetAllData();
            GameManager.Instance.GameScene = "GameScene";
            if (!GameManager.Instance.gameSceneStarted)
            {
                SceneManager.LoadScene(GameManager.Instance.GameScene);
                GameManager.Instance.gameSceneStarted = true;
            }
            removeOnEventCall();
        }
    }

    public void sendMessageButton()
    {
        ChatMessagesObject.GetComponent<Animator>().Play("showMessagesDialog");
        messageDialogVisible = true;

    }

    void Awake()
    {
        PhotonNetwork.OnEventCall += this.OnEvent;

    }

    public void removeOnEventCall()
    {
        PhotonNetwork.OnEventCall -= this.OnEvent;
    }

    // Multiplayer data received
    private void OnEvent(byte eventcode, object content, int senderid)
    {
        Debug.Log("Received message");
        if (eventcode == 195)
        {
            if (sentRematch)
            {
                GameManager.Instance.resetAllData();
                GameManager.Instance.GameScene = "GameScene";
                if (!GameManager.Instance.gameSceneStarted)
                {
                    SceneManager.LoadScene(GameManager.Instance.GameScene);
                    GameManager.Instance.gameSceneStarted = true;
                }
                removeOnEventCall();
            }
            else
            {
                rematchRequest = true;
               // if (GameManager.Instance.payoutCoins <= StoredUserData.coins)
               // {
               //     oppoMessageBubble.SetActive(true);
               //     oppoMessageBubble.transform.GetChild(0).GetComponent<Text>().text = StaticStrings.IWantPlayAgain;
                //}
            }
        }
        else if (eventcode == 194)
        {
            rematchButton.SetActive(false);
            oppoMessageBubble.SetActive(true);
            oppoMessageBubble.transform.GetChild(0).GetComponent<Text>().text = StaticStrings.cantPlayRightNow;

        }
        else if (eventcode == 193)
        {
            string index = (string)content;
            Debug.Log("INDEX: " + index);
            oppoMessageBubble.SetActive(true);
            oppoMessageBubble.transform.GetChild(0).GetComponent<Text>().text = index;
            if (isGameScene)
            {
                CancelInvoke("hideOppoMessageBubble");
                Invoke("hideOppoMessageBubble", 6.0f);
            }

        }
    }

    public void hideOppoMessageBubble()
    {
        oppoMessageBubble.SetActive(false);
    }

    public void hideMyMessageBubble()
    {
        myMessageBubble.SetActive(false);
    }
}
