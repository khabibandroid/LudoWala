using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyRoomManagerCustom : MonoBehaviour
{

    public int entry_fee = 0;
    public Text PrizeAmount, PrizeLock;
    public GameObject RoomLock, Tick;
    public Image Ticket;
    internal float win_percentage = 0f;
    internal float prizeAmount = 0f;
    internal int playerCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        CheckEligibility(int.Parse(GameManager.Instance.Balance));
    }

    public void CheckEligibility(int wallet_balance)
    {
        bool eligible = wallet_balance > entry_fee;
        RoomLock.SetActive(!eligible);
        Ticket.enabled = eligible;
    }

    public void CalculatePrize(float winP, int pc)
    {
        win_percentage = winP;
        playerCount = pc;

        prizeAmount = ((entry_fee * playerCount) * win_percentage);
        PrizeAmount.text = prizeAmount.ToString("0.00");
        PrizeLock.text = prizeAmount.ToString("0.00");
    }

    internal void IsSelected(bool v)
    {
        Tick.SetActive(v);
        if (v)
        {
            GameManager.Instance.payoutCoins = entry_fee;
        }
    }

    void Update()
    {
        
    }
}
