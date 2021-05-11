using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OnlineMoneyRoomRateManager : MonoBehaviour
{

    internal float win_percentage = 0f;
    public MoneyRoomManagerCustom[] rooms;
    internal int selectedRoomIndex = -1;
    internal int playerCount = 2;
    internal int wallet_balance = 0;

    void Start()
    {
        StartCoroutine("FetchPercentage");
        CheckEligibility();
        SetPlayerCount(2);
    }

    IEnumerator FetchPercentage()
    {
        var url = "https://codash.tk/ludowala/index.php?api/winning_percentage";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("************************** Unable to get prize data **************************");
            foreach(MoneyRoomManagerCustom room in rooms)
            {
                room.CalculatePrize(win_percentage, playerCount);
            }
        }
        else
        {
            prizeData prizedata = new prizeData();
            prizedata = JsonUtility.FromJson<prizeData>(request.downloadHandler.text);

            if (prizedata.status == "1")
            {
                Debug.Log("************************** prize data **************************");
                if (playerCount == 2)
                {
                    int WinP1;
                    Int32.TryParse(prizedata.data[0].player_1, out WinP1);
                    float winPercentP1 = (WinP1 / 100f);
                    win_percentage = winPercentP1;
                    foreach (MoneyRoomManagerCustom room in rooms)
                    {
                        room.CalculatePrize(win_percentage, playerCount);
                    }

                    /*int WinP2;
                    Int32.TryParse(prizedata.data[0].player_2, out WinP2);
                    float winPercentP2 = (WinP2 / 100f);*/

                }
                else if (playerCount == 4)
                {
                    int WinP1;
                    Int32.TryParse(prizedata.data[0].player_1, out WinP1);
                    float winPercentP1 = (WinP1 / 100f);
                    win_percentage = winPercentP1;
                    foreach (MoneyRoomManagerCustom room in rooms)
                    {
                        room.CalculatePrize(win_percentage, playerCount);
                    }

                    /*int WinP2;
                    Int32.TryParse(prizedata.data[1].player_2, out WinP2);
                    float winPercentP2 = (WinP2 / 100f);

                    int WinP3;
                    Int32.TryParse(prizedata.data[1].player_3, out WinP3);
                    float winPercentP3 = (WinP3 / 100f);

                    int WinP4;
                    Int32.TryParse(prizedata.data[1].player_4, out WinP4);
                    float winPercentP4 = (WinP4 / 100f);*/
                }
            }

        }
    }

    public void RoomSelect(int i)
    {
        selectedRoomIndex = i;
        for(int j=0; j < rooms.Length; j++)
        {
            rooms[j].IsSelected(j==i);
        }
    }

    public void SetPlayerCount(int c)
    {
        playerCount = c;
        GameManager.Instance.type = c == 2 ? MyGameType.TwoPlayer : MyGameType.FourPlayer;
        //GameConfigurationScreen.GetComponent<GameConfigrationController>().setCreatedProvateRoom();
        StopCoroutine("FetchPercentage");
        StartCoroutine("FetchPercentage");
        if (selectedRoomIndex >= 0)
        {
            rooms[selectedRoomIndex].IsSelected(false);
            selectedRoomIndex = -1;
        }
    }

    void Update()
    {
        //get latest balance...
        //wallet_balance != latest, checkeli();
        if (wallet_balance != int.Parse(GameManager.Instance.Balance))
        {
            wallet_balance = int.Parse(GameManager.Instance.Balance);
            CheckEligibility();
            if(selectedRoomIndex >= 0 && rooms[selectedRoomIndex].entry_fee > wallet_balance)
            {
                rooms[selectedRoomIndex].IsSelected(false);
                selectedRoomIndex = -1;
            }
        }
    }

    void CheckEligibility()
    {
        foreach(MoneyRoomManagerCustom room in rooms)
        {
            room.CheckEligibility(wallet_balance);
        }
    }
    public GameObject lowbalance;
    public void ConfirmSelection()
    {
        if (selectedRoomIndex < 0)
            return;
        //confirm...
        GameManager.Instance.JoinedByID = false;
        //show pawn color selection and set game instance select = true...
        if(GameManager.Instance.select == true)
        {
            if (int.Parse(GameManager.Instance.Balance) >= GameManager.Instance.payoutCoins)
            {
                GameManager.Instance.facebookManager.startRandomGame();
                gameObject.SetActive(false);
            }
            else
            {
                lowbalance.SetActive(true);
            }
        }
    }

    void OnEnable()
    {
        GameManager.Instance.JoinedByID = false;
    }
}
