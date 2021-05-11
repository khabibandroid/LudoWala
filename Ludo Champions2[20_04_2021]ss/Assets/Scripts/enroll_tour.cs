using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System;

public class enroll_tour : MonoBehaviour
{
    [SerializeField]
    public Text _tourname, _tourwinning, _tourjoinning, _tourtype, _tourdate, _SureTotalBalance, _SureJoinAmount, TourName, timer, level;
    public Button joinBtn, SureBtn, OpenBtn;
    public GameObject TourPanel, SurePanel;

    [SerializeField]
    public string tourname, tourwinning, tourjoin, tourtype, tourdate;
    public string tourid, tourtime, pays;
   
    private void Start()
    {
       
        GameManager.Instance.enroll = this;
        joinBtn.onClick.AddListener(delegate {

            TourInfo();
        });
        OpenBtn.onClick.AddListener(delegate
        {
            Openpopup();
        });
        SureBtn.onClick.AddListener(delegate
        {
            onSureBtn();
        });

        // timer 

        showLongTimeMessage();
    }

    private void TourInfo()
    {
       
        _tourname.text = tourname;
        _tourwinning.text = tourwinning;
        _tourjoinning.text = tourjoin;
        _tourtype.text = tourtype;
        _tourdate.text = tourdate;
      
        level.text = "Lv 1";
        GameManager.Instance.Levels = "1";
        Debug.Log("Time Left: " + tourtime);

        _SureTotalBalance.text = GameManager.Instance.Balance.ToString();
        _SureJoinAmount.text = tourjoin;
        TourPanel.SetActive(true);
        GameManager.Instance.mainmenu.temp = GameManager.Instance.mainmenu.ViewBtn.IndexOf(this.gameObject);

        if (int.Parse(GameManager.Instance.mainmenu.ViewBtn[GameManager.Instance.mainmenu.temp].GetComponent<enroll_tour>().tourtime) <= int.Parse("0"))
        {

            GameManager.Instance.mainmenu.ViewBtn[GameManager.Instance.mainmenu.temp].GetComponent<enroll_tour>().SureBtn.GetComponent<Button>().interactable = false;
        }
        else  {

            GameManager.Instance.mainmenu.ViewBtn[GameManager.Instance.mainmenu.temp].GetComponent<enroll_tour>().SureBtn.GetComponent<Button>().interactable = true;
        }
    }

    private void Openpopup()
    {
        SurePanel.SetActive(true);

     
    }

    private void onSureBtn()
    {
        Debug.Log("SureBtn Hit!");
            GameManager.Instance.mainmenu.joinTour();
        GameManager.Instance.mainmenu.hit = true;
    }

 
    int time = 0;
    public void showLongTimeMessage()
    {
        Debug.Log("enroll time");
        time = int.Parse(GameManager.Instance.mainmenu.ViewBtn[GameManager.Instance.mainmenu.temp].GetComponent<enroll_tour>().tourtime);
        StartCoroutine("timers");
    }
    public int seconds;
    public int minutes;
    public int hours;
    IEnumerator timers()
    {
        time--;
        if (time > 1)
        {
            hours = Mathf.FloorToInt((float)(time) / 3600);
            minutes = Mathf.FloorToInt((float)(time - hours * 3600) / 60);
            seconds = Mathf.FloorToInt((time) % 60);
            //Level_gametime.text = "TimeLeft: " + time + " sec";
            string remainingTime = string.Format("{2:00} : {0:00} : {1:00}", minutes, seconds, hours);
            if (seconds != 0 && minutes != 0 && hours != 0)
            {
                timer.text = "TimeLeft: " + remainingTime;
            }
            else if (seconds != 0 && minutes != 0 && hours == 0)
            {
                timer.text = "TimeLeft: " + remainingTime;
            }
            else if (seconds != 0 && minutes == 0 && hours == 0)
            {
                timer.text = "TimeLeft: " + remainingTime;
            }
            else
            {
                timer.text = "TimeLeft: " + "0";
            }
        }
        if (time < -1)
        {
            timer.text = "TimeLeft: " + "0";
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine("timers");
    }
}
