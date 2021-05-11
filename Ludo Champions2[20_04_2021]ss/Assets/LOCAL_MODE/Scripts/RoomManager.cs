using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using Firebase.Auth;
using UnityEngine.UI;
using System.IO;
using Google;

namespace offlineplay
{

    public class RoomManager : MonoBehaviour
    {
        private SocketIOComponent socket;
        public MenuManager Menumanager;
        public SearchUserManager SManager;
        public GameObject SharePanel;
        public ReferralCodeManager referralManager;
        public PrivateJoinManager PrivateJoin;
        public ContestManager Contestmanager;
        public SpinController SpinCtrller;
        public UILabel MyPoints;
        public MultiplayerModeSettings MMS;
        public ProfileManager profileManage;
        void Start()
        {
            socket = SocketManager.Instance.GetSocketIOComponent();
            socket.On("REQ_CREATE_ROOM_RESULT", OnGetCreateRoomResult);
            socket.On("REQ_CHECK_ROOMS_RESULT", OnGetCheckRoomsResult);
            socket.On("REQ_ENTER_ROOM_RESULT", OnGetEnterRoomResult);
            socket.On("REQ_USERLIST_ROOM_RESULT", OnGetUserListResult);
            socket.On("REQ_LEAVE_ROOM_RESULT", OnGetLeaveRoomResult);
            socket.On("GET_USERINFO_RESULT", GetUserInfoResult);
            socket.On("REQ_UPDATE_USERINFO_RESULT", GetUpdateUserInfoResult);
            socket.On("REQ_ROOM_INFO_RESULT", OnGetRoomInfoResult);
            socket.On("REQ_RANK_LIST_RESULT", OnGetRankListResult);
            socket.On("REQ_SPIN_RESULT", OnGetSpinResult);
            socket.On("REQ_CHECK_REFFERAL_RESULT", OnGetRefferalResult);
            socket.On("REQ_CHECK_REFFERAL_BOUNCE_RESULT", OnGetRefferalBounceResult);
            socket.On("GET_TOURNAMENTS_RESULT", OnGetTournamentResult);
            socket.On("GET_WITHDRAWAL_RESULT", OnGetWithdrwalResult);
            GameManager.Instance.RoomID = 0;
            GameManager.Instance.PrivateRoomId = string.Empty;
            GameManager.Instance.RoomStakeMoney = 0;
            GameManager.Instance.RoomWinMoney = 0;
            GameManager.Instance.isCreateRoom = false;
            GameManager.Instance.Users.Clear();
        }
        public GameObject NoTournamentsPanel;
        public GameObject TournamentContainer;
        public GameObject TournamentInfo;
        public GameObject TournamentInfoParent;
        private Vector3 location;
        public int[] timeValue = new int[100];
        public int[] roomPrice = new int[100];
        public int[] win_price = new int[100];
        public int[] play_num = new int[100];
        public void OnGetWithdrwalResult(SocketIOEvent evt)
        {
            Debug.Log("OnGetWithdrwalResult <== SOCKET RECIEVED" + evt.data);
            string result = Global.JsonToString(evt.data.GetField("result").ToString(), "\"");
            if (result == "success")
            {
                Debug.Log("OnGetWithdrwalResult <== SOCKET RECIEVED" + evt.data);

                
                
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("username", GameManager.Instance.UserName);
                socket.Emit("REQ_USER_INFO", new JSONObject(data));


                //_ShowAndroidToastMessage("Withdrwal request success");

            }
            else
            {
                _ShowAndroidToastMessage("Withdrwal request Failed");
            }

        }

        public void OnGetTournamentResult(SocketIOEvent evt)
        {
            Debug.Log("GET_TOURNAMENTS_RESULT <== SOCKET RECIEVED" + evt.data);
            int children = TournamentInfoParent.transform.childCount;
            for (int i = 0; i < children; ++i)
            {
                Destroy(TournamentInfoParent.transform.transform.GetChild(i).gameObject);
            }
            string result = Global.JsonToString(evt.data.GetField("result").ToString(), "\"");

            if (result == "success")
            {
                JSONObject obj = evt.data.GetField("data");
                int count = int.Parse(evt.data.GetField("count").ToString());
                for (int i = 0; i < obj.Count; i++)
                {
                    //TournamentInfo.transform.localScale = new Vector3(0.91114f,0.91114f,0.91114f);
                    GameObject tourtemp = Instantiate(TournamentInfo, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;

                    tourtemp.transform.SetParent(TournamentInfoParent.transform);
                    tourtemp.transform.localScale = new Vector3(0.91114f, 0.91114f, 0.91114f);
                    tourtemp.transform.localPosition = new Vector3(0, i * -175.92f, 0);
                    //Time
                    tourtemp.transform.GetChild(5).GetComponent<UILabel>().text = Global.JsonToString(obj[i].GetField("time_limit").ToString(), "\"") + " MIN";
                    tourtemp.transform.GetChild(5).GetComponent<SetTournamentTime>().value = int.Parse(Global.JsonToString(obj[i].GetField("time_limit").ToString(), "\"")) * 60;
                    tourtemp.transform.GetChild(5).GetComponent<SetTournamentTime>().listNo = i;
                    //Tourname
                    tourtemp.transform.GetChild(0).GetComponent<UILabel>().text = Global.JsonToString(obj[i].GetField("tournament_name").ToString(), "\"");
                    //Price
                    tourtemp.transform.GetChild(1).GetComponent<UILabel>().text = Global.JsonToString(obj[i].GetField("tournament_price").ToString(), "\"");
                    //PlayerNum
                    tourtemp.transform.GetChild(3).GetComponent<UILabel>().text = Global.JsonToString(obj[i].GetField("game_type").ToString(), "\"") + "P";
                    //Join
                    Debug.Log("1");
                    //ValueTime
                    timeValue[i] = int.Parse(Global.JsonToString(obj[i].GetField("time_limit").ToString(), "\"")) * 60;
                    Debug.Log("2");
                    //RoomPrice
                    roomPrice[i] = int.Parse(Global.JsonToString(obj[i].GetField("tournament_price").ToString(), "\""));
                    Debug.Log("3");
                    //WinPrice
                    win_price[i] = int.Parse(Global.JsonToString(obj[i].GetField("winning_amount").ToString(), "\""));
                    Debug.Log("4");
                    //NumPlayer
                    play_num[i] = int.Parse(Global.JsonToString(obj[i].GetField("game_type").ToString(), "\""));
                    // tourtemp.transform.Find("JoinButton").GetComponentInChildren<Button>().onClick.AddListener(() =>StartTournament(i));
                    Debug.Log("5");
                    UIButton joinButton = tourtemp.transform.GetChild(4).GetComponent<UIButton>();
                    // EventDelegate eve1 = new EventDelegate();
                    // eve1.target = this;
                    // eve1.methodName = "StartTournament";
                    // Debug.Log("5");
                    // EventDelegate.Parameter theParameter = new EventDelegate.Parameter(joinButton, "StartTournament");
                    // Debug.Log("6");
                    // eve1.parameters.SetValue(theParameter, i);
                    // Debug.Log("7");
                    // EventDelegate.Set(joinButton.onClick, eve1);
                    // Debug.Log("9");

                    EventDelegate eve1 = new EventDelegate(this, "StartTournament");
                    eve1.parameters[0].value = i;
                    EventDelegate.Set(joinButton.onClick, eve1);

                    // EventDelegate eve1 = new EventDelegate ();
                    // eve1.Set(this,"StartTournament");
                    // tourtemp.transform.GetChild(4).GetComponent<UIButton>().onClick.Clear();
                    // tourtemp.transform.GetChild(4).GetComponent<UIButton>().onClick.Add(eve1);
                    //         Debug.Log("8");
                    /*UIButton b = TournamentInfo.transform.GetChild(4).GetComponent<UIButton>();
                    EventDelegate del = new EventDelegate(this, "StartGame2P");
                    Debug.Log("TTTTTTTTTTTTTffdsdfsdfsdfsfd" + int.Parse(Global.JsonToString(obj[i].GetField("time_limit").ToString(), "\""))*60);
                    del.parameters[0].value = int.Parse(Global.JsonToString(obj[i].GetField("time_limit").ToString(), "\""))*60;
                    EventDelegate.Set(b.onClick, del);*/
                    /*if(int.Parse(Global.JsonToString(obj[i].GetField("game_type").ToString(), "\""))==2){
                        eve1.Set (this,"StartGame2P");
                    }
                    else{
                        eve1.Set (this,"StartGame4P");
                    }*/
                    // EventDelegate eve2 = new EventDelegate ();
                    // eve2.Set (this,"NameOfFunction");
                    //TournamentInfo.transform.GetChild(4).GetComponent<UIButton>().onClick.Add(eve1); 
                    //TournamentInfo.transform.GetChild(4).GetComponent<UIButton>().onClick.Add(eve2); 
                }
            }
            else
            {
                NoTournamentsPanel.SetActive(true);
                TournamentContainer.SetActive(false);
                PlayerPrefs.SetInt("isTournaments", 0);
            }

        }
        public void Test()
        {

        }
        public void StartTournament(int i)
        {
            Debug.Log("VALUE OF I is " + i);
            if (GameManager.Instance.Points >= roomPrice[i])
            {
                PlayerPrefs.SetInt("TournamentTime", timeValue[i]);

            }
            else
            {
                _ShowAndroidToastMessage("Not Enough Coins");
            }
        }
        // public void StartGame2P(){
        //     Debug.Log("rrrrrrrrrrrrrrrrrrr"+GameManager.Instance.Points);
        //     Debug.Log("rrrrrrrrrrrrrrrrrrr"+TournamentInfo.transform.GetChild(1).GetComponent<UILabel>().text);


        //     if(GameManager.Instance.Points >= int.Parse(TournamentInfo.transform.GetChild(1).GetComponent<UILabel>().text.ToString())){
        //         PlayerPrefs.SetInt("isTournaments",1);
        //         MMS.OnStartTournament2P();
        //     }else{
        //         PlayerPrefs.SetInt("isTournaments",0);
        //     }     
        // }

        // public void StartGame4P(){
        //     if(GameManager.Instance.Points >= TournamentInfo.transform.GetChild(5).GetComponent<SetTournamentTime>().value){
        //          PlayerPrefs.SetInt("isTournaments",1);
        //         MMS.OnStartTournament4P();
        //     }else{
        //         PlayerPrefs.SetInt("isTournaments",0);
        //         _ShowAndroidToastMessage("Not Enough Coinss");
        //     }  

        // }
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
        }
        public void BuyPyramid()
        {
            PlayerPrefs.SetString("CurrentTheme", "pyra");
        }
        public void BuyAladdin()
        {
            PlayerPrefs.SetString("CurrentTheme", "ala");
        }
        public void BuyWar()
        {
            PlayerPrefs.SetString("CurrentTheme", "war");
        }

        public void BuyDefault()
        {
            PlayerPrefs.SetString("CurrentTheme", "def");
        }

        public GameObject internetPanel;
        private void OnDestroy()
        {
            socket.Off("REQ_CREATE_ROOM_RESULT", OnGetCreateRoomResult);
            socket.Off("REQ_CHECK_ROOMS_RESULT", OnGetCheckRoomsResult);
            socket.Off("REQ_ENTER_ROOM_RESULT", OnGetEnterRoomResult);
            socket.Off("REQ_USERLIST_ROOM_RESULT", OnGetUserListResult);
            socket.Off("REQ_LEAVE_ROOM_RESULT", OnGetLeaveRoomResult);
            socket.Off("GET_USERINFO_RESULT", GetUserInfoResult);
            socket.Off("REQ_UPDATE_USERINFO_RESULT", GetUpdateUserInfoResult);
            socket.Off("REQ_ROOM_INFO_RESULT", OnGetRoomInfoResult);
            socket.Off("REQ_RANK_LIST_RESULT", OnGetRankListResult);
            socket.Off("REQ_SPIN_RESULT", OnGetSpinResult);
            socket.Off("REQ_CHECK_REFFERAL_RESULT", OnGetRefferalResult);
            socket.Off("REQ_CHECK_REFFERAL_BOUNCE_RESULT", OnGetRefferalBounceResult);
            socket.Off("GET_TOURNAMENTS_RESULT", OnGetTournamentResult);
            socket.Off("GET_WITHDRAWAL_RESULT", OnGetWithdrwalResult);

        }
        public void Update()
        {
            if (internetPanel == null)
                return;
            if (Application.internetReachability == NetworkReachability.NotReachable && !internetPanel.activeInHierarchy)
            {
                internetPanel.SetActive(true);
                // Time.timeScale = 0;

                //Debug.Log("Error. Check internet connection!");
            }
            else if (Application.internetReachability != NetworkReachability.NotReachable && internetPanel.activeInHierarchy)
            {
                internetPanel.SetActive(false);
                // Time.timeScale = 1;
                //DoNothing
            }
        }

        public void CreateRoom()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("username", GameManager.Instance.UserName);
            string room_title = "";
            if (GameManager.Instance._Wifi == WIFI.online)
                room_title = GameManager.Instance.UserName + " has set a challenge";
            else if (GameManager.Instance._Wifi == WIFI.privateRoom)
            {
                int myRandomNo = UnityEngine.Random.Range(1000000, 9999999);
                room_title = myRandomNo.ToString();
                GameManager.Instance.PrivateRoomId = room_title;
            }
            data.Add("room_title", room_title);
            data.Add("seat_limit", ((int)GameManager.Instance._GamePlayType).ToString());
            data.Add("status", "ready");
            data.Add("game_mode", GameManager.Instance._GameMode.ToString());
            data.Add("wifi_mode", GameManager.Instance._Wifi.ToString());
            data.Add("stake_money", GameManager.Instance.RoomStakeMoney.ToString());
            data.Add("win_money", GameManager.Instance.RoomWinMoney.ToString());
            JSONObject jdata = new JSONObject(data);
            socket.Emit("REQ_CREATE_ROOM", jdata);
        }

        private void OnGetCreateRoomResult(SocketIOEvent evt)
        {
            Debug.Log("REQ_CREATE_ROOM_RESULT <== SOCKET RECIEVED" + evt.data);

            string result = Global.JsonToString(evt.data.GetField("result").ToString(), "\"");
            int roomID = int.Parse(Global.JsonToString(evt.data.GetField("roomID").ToString(), "\""));
            if (result == "success")
            {
                GameManager.Instance.RoomID = roomID;
                if (GameManager.Instance._Wifi == WIFI.online)
                {
                    Join_Room();
                }
                else if (GameManager.Instance._Wifi == WIFI.privateRoom)
                {
                    GameManager.Instance.PrivateRoomId += roomID.ToString();
                    SharePanel.SetActive(true);
                }
            }
        }
        public void Join_Room()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("username", GameManager.Instance.UserName);
            data.Add("roomID", GameManager.Instance.RoomID.ToString());
            data.Add("photo", "https://cdn.onlinewebfonts.com/svg/img_329115.png");
            JSONObject jdata = new JSONObject(data);
            socket.Emit("REQ_JOIN_ROOM", jdata);
        }

        private void OnGetEnterRoomResult(SocketIOEvent evt)
        {
            Debug.Log("REQ_ENTER_ROOM_RESULT <== SOCKET RECIEVED" + evt.data);

            string result = Global.JsonToString(evt.data.GetField("result").ToString(), "\"");
            if (result == "success")
            {
                GameManager.Instance.Points -= GameManager.Instance.RoomStakeMoney;
                SManager.searching = true;
                UpdateUserInfo();
            }
            else
            {
                print("You can't enter room because you are in this room");
            }
        }

        List<string> players = new List<string>();
        List<string> playerPhotos = new List<string>();
        private void OnGetUserListResult(SocketIOEvent evt)
        {
            Debug.Log("REQ_USERLIST_ROOM_RESULT <== SOCKET RECIEVED" + evt.data);

            int roomid = int.Parse(Global.JsonToString(evt.data.GetField("roomid").ToString(), "\""));
            if (roomid == GameManager.Instance.RoomID)
            {
                JSONObject UserArray = evt.data.GetField("userlist");
                if (UserArray == null)
                    return;

                players.Clear(); playerPhotos.Clear();
                GameManager.Instance.Users.Clear();
                for (int i = 0; i < UserArray.Count; i++)
                {
                    JSONObject jsonItem = UserArray[i];
                    string username = Global.JsonToString(jsonItem.GetField("username").ToString(), "\"");
                    string photourl = Global.JsonToString(jsonItem.GetField("photo").ToString(), "\"");
                    int points = int.Parse(Global.JsonToString(jsonItem.GetField("points").ToString(), "\""));
                    int level = int.Parse(Global.JsonToString(jsonItem.GetField("level").ToString(), "\""));
                    if (!GameManager.Instance.UserName.Equals(username))
                    {
                        players.Add(username); playerPhotos.Add(photourl);
                    }
                    UserInfo info = new UserInfo();
                    info.name = username;
                    info.points = points;
                    info.level = level;
                    GameManager.Instance.Users.Add(info);
                }
                if (GameManager.Instance._Wifi == WIFI.online)
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        StartCoroutine(match_player(i));
                    }
                }

            }
        }

        IEnumerator match_player(int i)
        {
            yield return new WaitForSeconds(1.0f);
            SManager.OnFindPlayer(i, players[i], playerPhotos[i]);
        }

        public void LeaveRoom()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["roomid"] = "" + GameManager.Instance.RoomID;
            data["username"] = GameManager.Instance.UserName;
            socket.Emit("REQ_LEAVE_ROOM", new JSONObject(data));
        }
        private void OnGetLeaveRoomResult(SocketIOEvent evt)
        {
            Debug.Log("REQ_LEAVE_ROOM_RESULT <== SOCKET RECIEVED" + evt.data);

            string username = Global.JsonToString(evt.data.GetField("username").ToString(), "\"");
            string message = Global.JsonToString(evt.data.GetField("message").ToString(), "\"");

            if (players.Contains(username))
            {
                int index = players.IndexOf(username);
                players.RemoveAt(index); playerPhotos.RemoveAt(index);
                if (SManager.gameObject.activeSelf)
                    SManager.OnLeavePlayer(index);
            }
        }
        public void RequestRoomInfo()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("roomID", GameManager.Instance.RoomID.ToString());
            JSONObject jdata = new JSONObject(data);

            socket.Emit("REQ_ROOM_INFO", jdata);
        }
        private void OnGetRoomInfoResult(SocketIOEvent evt)
        {
            Debug.Log("REQ_ROOM_INFO_RESULT <== SOCKET RECIEVED" + evt.data);

            int playercount = int.Parse(evt.data.GetField("seatlimit").ToString());
            if (playercount == 0)
                return;
            string gamemode = Global.JsonToString(evt.data.GetField("gamemode").ToString(), "\"");
            if (gamemode == "classic")
                GameManager.Instance._GameMode = GameMode.classic;
            else if (gamemode == "arrow")
            {
                GameManager.Instance._GameMode = GameMode.arrow;
            }
            else if (gamemode == "quick")
                GameManager.Instance._GameMode = GameMode.quick;
            GameManager.Instance.RoomStakeMoney = int.Parse(Global.JsonToString(evt.data.GetField("stakemoney").ToString(), "\""));
            GameManager.Instance.RoomWinMoney = int.Parse(Global.JsonToString(evt.data.GetField("winmoney").ToString(), "\""));
            GamePlayType count = (GamePlayType)playercount;
            GameManager.Instance._GamePlayType = count;
            PrivateJoin.joinStart();
        }
        public void Request_RankList(string type)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("rank_type", type);
            JSONObject jdata = new JSONObject(data);
            socket.Emit("REQ_RANK_LIST", jdata);
        }

        private void OnGetRankListResult(SocketIOEvent evt)
        {
            Debug.Log("REQ_RANK_LIST_RESULT <== SOCKET RECIEVED" + evt.data);

            string result = Global.JsonToString(evt.data.GetField("result").ToString(), "\"");
            if (result.Equals("success"))
            {
                JSONObject UserArray = evt.data.GetField("users");
                if (UserArray == null)
                    return;
                Contestmanager.InitUI(UserArray);
            }
        }

        public void AddGameHistory(int point)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("username", GameManager.Instance.UserName);
            data.Add("points", point.ToString());
            JSONObject jdata = new JSONObject(data);
            socket.Emit("REQ_GAME_HIST", jdata);
        }

        public void Request_Spin()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("username", GameManager.Instance.UserName);
            JSONObject jdata = new JSONObject(data);
            socket.Emit("REQ_SPIN", jdata);
        }
        private void OnGetSpinResult(SocketIOEvent evt)
        {
            Debug.Log("REQ_SPIN_RESULT <== SOCKET RECIEVED" + evt.data);

            string result = Global.JsonToString(evt.data.GetField("result").ToString(), "\"");

            if (result == "success")
            {
                GameManager.Instance.wheelSpin = true;
                SpinCtrller.timer.text = "";
                SpinCtrller.isSpin = true;
                Menumanager.OpenSpin();
            }
            else
            {
                GameManager.Instance.wheelSpin = false;
                SpinCtrller.isSpin = false;
                Menumanager.OpenSpin();
                int hr = int.Parse(evt.data.GetField("hours").ToString());
                int min = int.Parse(evt.data.GetField("minutes").ToString());
                int sec = int.Parse(evt.data.GetField("seconds").ToString());
                SpinCtrller.SetRemainTime(hr, min, sec);
            }
        }

        #region GET USERINFO
        public void GetUserInfo(string username)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("username", username);
            socket.Emit("REQ_USER_INFO", new JSONObject(data));
        }

        private void GetUserInfoResult(SocketIOEvent evt)
        {
            Debug.Log("GET_USERINFO_RESULT <== SOCKET RECIEVED" + evt.data);

            string result = Global.JsonToString(evt.data.GetField("result").ToString(), "\"");
            if (result.Equals("success"))
            {
                string name = Global.JsonToString(evt.data.GetField("username").ToString(), "\"");
                string photoURL = Global.JsonToString(evt.data.GetField("photo").ToString(), "\"");
                int points = int.Parse(Global.JsonToString(evt.data.GetField("points").ToString(), "\""));
                int level = int.Parse(Global.JsonToString(evt.data.GetField("level").ToString(), "\""));
                int referralCount = int.Parse(Global.JsonToString(evt.data.GetField("referral_count").ToString(), "\""));
            }
        }
        #endregion

        #region UPDATE USER PROFILE
        public void UpdateUserInfo()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("username", GameManager.Instance.UserName);
            data.Add("points", GameManager.Instance.Points.ToString());
            data.Add("level", GameManager.Instance.Level.ToString());
            data.Add("online_played", GameManager.Instance.Online_Multiplayer.played.ToString());
            data.Add("online_won", GameManager.Instance.Online_Multiplayer.won.ToString());
            data.Add("friend_played", GameManager.Instance.Friend_Multiplayer.played.ToString());
            data.Add("friend_won", GameManager.Instance.Friend_Multiplayer.won.ToString());
            data.Add("tokenscaptured_mine", GameManager.Instance.TokensCaptued.mine.ToString());
            data.Add("tokenscaptured_opponents", GameManager.Instance.TokensCaptued.opponents.ToString());
            data.Add("wonstreaks_current", GameManager.Instance.WonStreaks.current.ToString());
            data.Add("wonstreaks_best", GameManager.Instance.WonStreaks.best.ToString());
            JSONObject jdata = new JSONObject(data);
            socket.Emit("REQ_UPDATE_USERINFO", jdata);
        }
        private void GetUpdateUserInfoResult(SocketIOEvent evt)
        {
            Debug.Log("REQ_UPDATE_USERINFO_RESULT <== SOCKET RECIEVED" + evt.data);


        }
        #endregion

        #region Match Way

        public void Check_Match_Way()
        {

            Debug.Log("REACHED HERE");
            int val = (int)(GameManager.Instance._GamePlayType);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("seat_limit", val.ToString());
            data.Add("game_mode", GameManager.Instance._GameMode.ToString());
            data.Add("wifi_mode", GameManager.Instance._Wifi.ToString());
            data.Add("stake_money", GameManager.Instance.RoomStakeMoney.ToString());
            data.Add("win_money", GameManager.Instance.RoomWinMoney.ToString());
            JSONObject jdata = new JSONObject(data);
            socket.Emit("REQ_CHECK_ROOMS", jdata);
        }
        private void OnGetCheckRoomsResult(SocketIOEvent evt)
        {
            Debug.Log("REQ_CHECK_ROOMS_RESULT <== SOCKET RECIEVED" + evt.data);

            string result = Global.JsonToString(evt.data.GetField("result").ToString(), "\"");

            int IsBotOn = int.Parse(Global.JsonToString(evt.data.GetField("isBotsActive").ToString(), "\""));
            int botID = int.Parse(Global.JsonToString(evt.data.GetField("botId").ToString(), "\""));
            PlayerPrefs.SetInt("IsBotOn", IsBotOn);
            PlayerPrefs.SetInt("BotID", botID);
            if (result == "success")
            {
                int roomID = int.Parse(Global.JsonToString(evt.data.GetField("roomID").ToString(), "\""));

                GameManager.Instance.RoomID = roomID;

                Join_Room();
            }
            else
            {
                CreateRoom();
            }
        }
        #endregion

        #region Check referral code
        public void Check_Referral_Code(string code)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("username", GameManager.Instance.UserName);
            data.Add("referral", code);
            JSONObject jdata = new JSONObject(data);
            socket.Emit("REQ_CHECK_REFFERAL", jdata);
        }
        private void OnGetRefferalResult(SocketIOEvent evt)
        {
            Debug.Log("REQ_CHECK_REFFERAL_RESULT <== SOCKET RECIEVED" + evt.data);

            print(evt.name + " " + evt.data);
            string result = Global.JsonToString(evt.data.GetField("result").ToString(), "\"");
            referralManager.SetResult(result);
        }
        public void Check_Referral_Bounce()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("username", GameManager.Instance.UserName);
            JSONObject jdata = new JSONObject(data);
            socket.Emit("REQ_CHECK_REFFERAL_BOUNCE", jdata);
        }
        private void OnGetRefferalBounceResult(SocketIOEvent evt)
        {
            //Debug.Log("REQ_CHECK_REFFERAL_BOUNCE_RESULT <== SOCKET RECIEVED" + evt.data);

            string result = Global.JsonToString(evt.data.GetField("result").ToString(), "\"");

            if (result == "success")
            {
                int add_points = int.Parse(evt.data.GetField("bounce").ToString());
                GameManager.Instance.Referral_count = int.Parse(evt.data.GetField("referCount").ToString());

                if (add_points > 0)
                {
                    GameManager.Instance.Points += add_points;
                    Menumanager.ProfilePanel.GetComponent<ProfileManager>().Counting(GameManager.Instance.Points, GameManager.Instance.Points - add_points);
                }
            }
        }
        #endregion
    }

}