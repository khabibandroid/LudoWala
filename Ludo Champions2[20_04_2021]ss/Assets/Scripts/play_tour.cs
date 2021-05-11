using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class play_tour : MonoBehaviour
{
    [SerializeField]
    public Text Level_gameName, Level_gametime, Level_gametype, name, type, game_level, level, _level, _type,  _tourname;

    [SerializeField]
    public string timeleft, gameid, lost, end_play;

    public Button playBtn, OpenBtn;
    int time = 0;
    public GameObject Info;
    public int seconds;
    public int minutes;
    public int hours;

    private void Start()
    {
        playBtn.onClick.AddListener(delegate {

            TourInfo();
        });
        OpenBtn.onClick.AddListener(delegate
        {
            Openpopup();
            TourInfo();
        });
        showLongTimeMessage();
    }
    private void TourInfo()
    {
        GameManager.Instance.tours = true;
        Level_gameName.text = name.text;
        Level_gametype.text = type.text;
        game_level.text = level.text;
        _level.text = level.text;
        _type.text = type.text;
        _tourname.text = name.text;
        GameManager.Instance.GameID = gameid;
       
        GameManager.Instance.tours = true;
        if( level.text == "Lv1")
        {
            GameManager.Instance.Levels = "1";
        }
        if (level.text == "Lv2")
        {
            GameManager.Instance.Levels = "2";
        }
        if (level.text == "Lv3")
        {
            GameManager.Instance.Levels = "3";
        }
        if (level.text == "Lv4")
        {
            GameManager.Instance.Levels = "4";
        }
        if (level.text == "Lv5")
        {
            GameManager.Instance.Levels = "5";
        }
        if (level.text == "Lv6")
        {
            GameManager.Instance.Levels = "6";
        }
        if (level.text == "Lv7")
        {
            GameManager.Instance.Levels = "7";
        }
        GameManager.Instance.mainmenu.live = GameManager.Instance.mainmenu.playBtn.IndexOf(this.gameObject);

        if (int.Parse(GameManager.Instance.mainmenu.playBtn[GameManager.Instance.mainmenu.live].GetComponent<play_tour>().timeleft) <= int.Parse("0") && (GameManager.Instance.mainmenu.playBtn[GameManager.Instance.mainmenu.live].GetComponent<play_tour>().lost == "no") && (GameManager.Instance.mainmenu.playBtn[GameManager.Instance.mainmenu.live].GetComponent<play_tour>().lost == "no") && (int.Parse(GameManager.Instance.mainmenu.playBtn[GameManager.Instance.mainmenu.live].GetComponent<play_tour>().end_play) > int.Parse("0")))
        {

            GameManager.Instance.mainmenu.playBtn[GameManager.Instance.mainmenu.live].GetComponent<play_tour>().playBtn.GetComponent<Button>().interactable = true;
        }
        else
        {
            GameManager.Instance.mainmenu.playBtn[GameManager.Instance.mainmenu.live].GetComponent<play_tour>().playBtn.GetComponent<Button>().interactable = false;
        }

    }

    private void Openpopup()
    {
        Info.SetActive(true);
    }

    public void showLongTimeMessage()
    {
       
            time = int.Parse(GameManager.Instance.mainmenu.playBtn[GameManager.Instance.mainmenu.live].GetComponent<play_tour>().timeleft);
        
        StartCoroutine("timers");
    }

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
                Level_gametime.text = "TimeLeft: " + remainingTime;

            }
            else if (seconds != 0 && minutes != 0 && hours <= 0)
            {
                Level_gametime.text = "TimeLeft: " + remainingTime;

            }
            else if (seconds != 0 && minutes == 0 && hours <= 0)
            {
                Level_gametime.text = "TimeLeft: " + remainingTime;

            }
            else
            {
                Level_gametime.text = "TimeLeft: " + "0";

            }
           
        }
        if(time < -1)
        {
            Level_gametime.text = "TimeLeft: " + "0";
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine("timers");
    }
}
