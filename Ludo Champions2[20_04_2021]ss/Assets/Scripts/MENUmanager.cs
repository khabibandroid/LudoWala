using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MENUmanager : MonoBehaviour
{
    public void Start()
    {
        GameManager.Instance.mainmenu = this;
        PlayerPrefs.SetInt("FirstLoad", 1);
        Yourr();
        PlayTour()
;
    }

    public void Yourr()
    {
        StartCoroutine("tour");
    }

    private IEnumerator tour()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www = UnityWebRequest.Post(GameManager.Instance.BaseUrl + "api/tournment", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error downloading: " + www.error);
        }
        else
        {

            Debug.Log(www.downloadHandler.text);

            for (int i = Item_parent.transform.childCount - 1; i > 0; i--)
            {
                Destroy(Item_parent.transform.GetChild(i).gameObject);
            }

            string output = www.downloadHandler.text;

            if (output == "null" || output == "")
            {
                Debug.Log("No data found");


            }

            else
            {
                var N = JSON.Parse(output);
                if (N["status"].Value == "1")
                {
                    for (int i = 0; i < N["Data"].Count; i++)
                    {

                        GameObject row = Instantiate(Item_tour);
                        row.transform.SetParent(Item_parent.transform, false);
                        enroll_tour myscript = row.GetComponent<enroll_tour>();

                        myscript.tourid = N["Data"][i]["id"].Value;
                        myscript.tourname = N["Data"][i]["name"].Value;
                        myscript.tourjoin = "Rs " + N["Data"][i]["joining_amount"].Value + "/-";
                        myscript.tourwinning = "Rs " + N["Data"][i]["prize_amount"].Value + "/-";
                        myscript.tourtype = N["Data"][i]["game_type"] + "p";
                        myscript.tourdate = N["Data"][i]["tournment_play_date"];
                        myscript.tourtime = N["Data"][i]["join_timeleft"];
                        myscript.pays = N["Data"][i]["joining_amount"].Value;
                        myscript.TourName.text = N["Data"][i]["name"].Value;
                        ViewBtn.Add(row);
                        row.SetActive(true);
                    }
                }
                else
                {
                    Debug.Log("No tour Avilable!");
                }

            }

        }
    }

    double pay;
    double dep;
    double winn;
    double bon;
    public bool hit;

    public void joinTour()
    {
        if (hit == true)
        {
            StartCoroutine("Join_tournament");
            hit = false;
        }

    }


    private IEnumerator Join_tournament()
    {
        string outt = (ViewBtn[temp].GetComponent<enroll_tour>().pays);
        double payout = double.Parse(outt);
        Debug.Log("payout " + payout);
        Debug.Log(GameManager.Instance.Bonus + " " + GameManager.Instance.Deposites + " " + payout);

        if (GameManager.Instance.Bonus >= 0)
        {
            bon = 5;
            pay = (payout) - (bon);
        }
        if (GameManager.Instance.Deposites < payout && GameManager.Instance.Bonus >= 0)
        {
            pay = (payout) - (GameManager.Instance.Deposites);

            dep = GameManager.Instance.Deposites - bon;
        }

        if (GameManager.Instance.Deposites > payout && GameManager.Instance.Bonus <= 0)
        {
            dep = payout;
        }
        if (GameManager.Instance.Deposites > payout && GameManager.Instance.Bonus >= 0)
        {
            pay = payout - bon;
            dep = pay;
        }

        if (GameManager.Instance.Deposites <= 0)
        {
            dep = 0;
        }
        if (GameManager.Instance.Bonus <= 0)
        {
            bon = 0;
        }

        if (pay != 0.0)
        {
            winn = (payout) - ((dep) + (bon));

        }

        string gameId = ViewBtn[temp].GetComponent<enroll_tour>().tourid;
        Debug.Log("gameId: " + gameId);
        Debug.Log("deposit: " + (dep));
        Debug.Log("bonus: " + (bon));
        Debug.Log("winning: " + (winn));

        WWWForm form = new WWWForm();

        form.AddField("user_id", GameManager.Instance.UserID);
        form.AddField("game_id", gameId);
        form.AddField("winnings", winn.ToString());
        form.AddField("deposits", dep.ToString());
        form.AddField("bonus", bon.ToString());

        UnityWebRequest www = UnityWebRequest.Post(GameManager.Instance.BaseUrl + "api/join_tour", form);

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
                Debug.Log(message);
                success_panel.SetActive(true);
                success_text.text = "Tournament joined successfully!";
            }
            else if (N["status"].Value == "0")
            {
                Debug.Log("Failed!");
                success_panel.SetActive(true);
                success_text.text = N["message"].Value;
            }
        }
    }
    public void PlayTour()
    {
        StartCoroutine("play_tour");
    }

    private IEnumerator play_tour()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Instance.UserID);
        UnityWebRequest www = UnityWebRequest.Post(GameManager.Instance.BaseUrl + "api/play_games", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error downloading: " + www.error);
        }
        else
        {

            Debug.Log(www.downloadHandler.text);

            for (int i = Item_playparent.transform.childCount - 1; i > 0; i--)
            {
                Destroy(Item_playparent.transform.GetChild(i).gameObject);
            }

            string output = www.downloadHandler.text;

            if (output == "null" || output == "")
            {
                Debug.Log("No data found");


            }

            else
            {
                var N = JSON.Parse(output);
                if (N["status"].Value == "1")
                {
                    for (int i = 0; i < N["data"].Count; i++)
                    {

                        GameObject row = Instantiate(Item_play);
                        row.transform.SetParent(Item_playparent.transform, false);
                        play_tour myscript = row.GetComponent<play_tour>();

                        myscript.name.text = N["data"][i]["name"].Value;
                        myscript.gameid = N["data"][i]["game_id"].Value;
                        myscript.lost = N["data"][i]["lost"].Value;
                        myscript.type.text = N["data"][i]["type"].Value + "p";
                        myscript.timeleft = N["data"][i]["play_timeleft"].Value;
                        myscript.end_play = N["data"][i]["end_play_timeleft"].Value;
                        myscript.level.text = "Lv" + N["data"][i]["level"].Value;
                        playBtn.Add(row);
                        row.SetActive(true);

                    }

                }
                else
                {
                    Debug.Log("No tour Avilable!");
                }

            }

        }
    }
    public int live = 0;
    public int live1 = 0;
    public List<GameObject> ViewBtn, playBtn, playBtn1;
    public int temp;
    public GameObject success_panel, Item_tour, Item_parent, Item_play, Item_playparent;
    public Text success_text;

    public GameObject Loading1;
    public void logout()
    {
        Loading1.SetActive(true);
        StartCoroutine(Logout());
    }

    IEnumerator Logout()
    {
        
        yield return new WaitForSeconds(3f);
        PlayerPrefs.DeleteKey("user_ID");
        GameManager.Instance.facebookManager.destroy();
        //                GameManager.Instance.connectionLost.destroy();
        GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>().destroy();
        GameManager.Instance.avatarMy = null;
        EventCounter.LogOut = true;
        PhotonNetwork.Disconnect();

        GameManager.Instance.resetAllData();

        GameManager.Instance.playfabManager.destroy();
        Debug.LogError("logged out");
        SceneManager.LoadScene("LoginScene");


    }

}



