using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIO;
namespace offlineplay
{
    public class LoginManager : MonoBehaviour
    {
        private MenuManager menuManager;
        [System.NonSerialized]
        public GameObject loading;
        private SocketIOComponent socket;
        public bool delete_PlayerPrefs = false;
        public RoomManager Roommanager;

        void Awake()
        {
            if (GameManager.Instance.GameVersion != "1.2")
                Application.Quit();
            menuManager = transform.parent.GetComponent<MenuManager>();
            loading = transform.Find("Loading").gameObject;
        }

        private void Start()
        {
            if (delete_PlayerPrefs)
                PlayerPrefs.DeleteAll();
            socket = SocketManager.Instance.GetSocketIOComponent();
            socket.On("GET_LOGIN_RESULT", OnGetLoginResult);
            socket.On("GET_REGISTER_RESULT", OnGetRegisterResult);
            socket.On("REQ_VALID_NAME_RESULT", OnGetValidAccountResult);

            if (PlayerPrefs.HasKey("USERNAME"))
            {
                GameManager.Instance.UserName = PlayerPrefs.GetString("USERNAME");
            }
            if (PlayerPrefs.HasKey("LOGIN"))
            {
                GameManager.Instance._Login_Mode = (LOGIN_MODE)PlayerPrefs.GetInt("LOGIN");
            }

            if (GameManager.Instance.isLogin == false)
            {
                if (PlayerPrefs.HasKey("USERNAME") && PlayerPrefs.HasKey("LOGIN"))
                {
                    if (GameManager.Instance.IwannaGoogleLogin == false && GameManager.Instance._Login_Mode == LOGIN_MODE.google)
                    {
                        OnGoogleLogin();
                    }

                    else if (GameManager.Instance._Login_Mode == LOGIN_MODE.guest)
                    {
                        print("nothing");
                    }
                    else
                    {
                        loading.SetActive(true);
                        Invoke("Valid_Account", 1.0f);
                    }
                }
            }
            else
            {
                if (GameManager.Instance.IwannaGoogleLogin)
                {
                    OnGoogleLogin();
                }
                else if (GameManager.Instance.IwannaFacebookLogin)
                {
                    print("facebook login");
                    loading.SetActive(true);
                    menuManager.GetComponent<FirebaseFacebookManager>().FacebooklogIn();
                }
            }
        }
        private void OnDestroy()
        {
            if (socket != null)
            {
                socket.Off("GET_LOGIN_RESULT", OnGetLoginResult);
                socket.Off("GET_REGISTER_RESULT", OnGetRegisterResult);
                socket.Off("REQ_VALID_NAME_RESULT", OnGetValidAccountResult);
            }
        }
        public void CheckValid()
        {

            Valid_Account();
        }
        public void Login_Guest()
        {
            GameManager.Instance._Login_Mode = LOGIN_MODE.guest;
            PlayerPrefs.SetInt("LOGIN", (int)LOGIN_MODE.guest);
            loading.SetActive(true);

            //temp commnet


            if (PlayerPrefs.HasKey("GUESTNAME"))
            {
                GameManager.Instance.UserName = PlayerPrefs.GetString("GUESTNAME");
                Valid_Account();
                StartCoroutine(WaitingLogin());
            }
            else
            {
                Register();
                StartCoroutine(WaitingLogin());
            }
        }

        #region socket emits
        private void Valid_Account()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("name", GameManager.Instance.UserName);
            JSONObject jdata = new JSONObject(data);
            socket.Emit("REQ_VALID_NAME", jdata);
        }
        private void Login()
        {
            string username = GameManager.Instance.UserName;

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("username", username);
            JSONObject jdata = new JSONObject(data);
            socket.Emit("REQ_LOGIN", jdata);
        }
        private void Register()
        {
            string signtype = "";
            Dictionary<string, string> data = new Dictionary<string, string>();
            if (GameManager.Instance._Login_Mode == LOGIN_MODE.google)
                signtype = "google";
            else if (GameManager.Instance._Login_Mode == LOGIN_MODE.facebook)
                signtype = "facebook";
            else if (GameManager.Instance._Login_Mode == LOGIN_MODE.phone)
                signtype = "phone";
            data.Add("signtype", signtype);
            data.Add("username", GameManager.Instance.UserName);
            JSONObject jdata = new JSONObject(data);
            socket.Emit("REQ_REGISTER", jdata);
        }
        #endregion

        #region socket events
        private void OnGetValidAccountResult(SocketIOEvent evt)
        {
            string result = Global.JsonToString(evt.data.GetField("result").ToString(), "\"");
            if (result.Equals("success"))
                Register();
            else
                Login();
        }
        private void OnGetLoginResult(SocketIOEvent evt)
        {
            string result = Global.JsonToString(evt.data.GetField("result").ToString(), "\"");
            if (result.Equals("success"))
            {
                GameManager.Instance.UserName = Global.JsonToString(evt.data.GetField("username").ToString(), "\"");
                GameManager.Instance.UserID = Global.JsonToString(evt.data.GetField("userid").ToString(), "\"");
                GameManager.Instance.AvatarURL = Global.JsonToString(evt.data.GetField("photo").ToString(), "\"");
                GameManager.Instance.Points = int.Parse(evt.data.GetField("points").ToString());
                GameManager.Instance.Level = int.Parse(evt.data.GetField("level").ToString());
                GameManager.Instance.Referral_count = int.Parse(evt.data.GetField("referral_count").ToString());

                JSONObject online_multiplayer = evt.data.GetField("online_multiplayer");
                GameManager.Instance.Online_Multiplayer.played = int.Parse(online_multiplayer.GetField("played").ToString());
                GameManager.Instance.Online_Multiplayer.won = int.Parse(online_multiplayer.GetField("won").ToString());
                JSONObject friend_multiplayer = evt.data.GetField("friend_multiplayer");
                GameManager.Instance.Friend_Multiplayer.played = int.Parse(friend_multiplayer.GetField("played").ToString());
                GameManager.Instance.Friend_Multiplayer.won = int.Parse(friend_multiplayer.GetField("won").ToString());
                JSONObject tokens_captured = evt.data.GetField("tokens_captured");
                GameManager.Instance.TokensCaptued.mine = int.Parse(tokens_captured.GetField("mine").ToString());
                GameManager.Instance.TokensCaptued.opponents = int.Parse(tokens_captured.GetField("opponents").ToString());
                JSONObject won_streaks = evt.data.GetField("won_streaks");
                GameManager.Instance.WonStreaks.current = int.Parse(won_streaks.GetField("current").ToString());
                GameManager.Instance.WonStreaks.best = int.Parse(won_streaks.GetField("best").ToString());
                GameManager.Instance.Referral_code = Global.JsonToString(evt.data.GetField("referral_code").ToString(), "\"");
                loading.SetActive(false);
                LoginSuccess();

            }
            else
                Debug.Log("login error");
        }

        private void OnGetRegisterResult(SocketIOEvent evt)
        {
            string result = Global.JsonToString(evt.data.GetField("result").ToString(), "\"");
            GameManager.Instance.UserName = Global.JsonToString(evt.data.GetField("username").ToString(), "\"");
            GameManager.Instance.UserID = Global.JsonToString(evt.data.GetField("userid").ToString(), "\"");
            GameManager.Instance.Points = int.Parse(evt.data.GetField("points").ToString());
            GameManager.Instance.Referral_code = Global.JsonToString(evt.data.GetField("referral_code").ToString(), "\"");
            loading.SetActive(false);
            if (result == "success")
            {
                PlayerPrefs.SetString("USERNAME", GameManager.Instance.UserName);
                if (GameManager.Instance.UserName.Contains("Guest"))
                    PlayerPrefs.SetString("GUESTNAME", GameManager.Instance.UserName);
                SignupSuccess();
            }
            else
                Debug.Log("register error");

        }
        #endregion

        void LoginSuccess()
        {
            GameManager.Instance.isLogin = true;
            menuManager.On_Home();
            GameManager.Instance.IwannaGoogleLogin = false;
            GameManager.Instance.IwannaFacebookLogin = false;
        }
        void SignupSuccess()
        {
            GameManager.Instance.isLogin = true;
            menuManager.On_ReferralCode();
            GameManager.Instance.IwannaGoogleLogin = false;
            GameManager.Instance.IwannaFacebookLogin = false;
        }

        IEnumerator WaitingLogin()
        {
            yield return new WaitForSeconds(10.0f);
            loading.SetActive(false);
            GameManager.Instance.UserName = "Player1";
            LoginSuccess();
        }
        public void OnGoogleLogin()
        {
            SceneManager.LoadScene("SnsLogin");
        }
    }

    public enum LOGIN_MODE
    {
        facebook,
        google,
        guest,
        phone
    }
}
