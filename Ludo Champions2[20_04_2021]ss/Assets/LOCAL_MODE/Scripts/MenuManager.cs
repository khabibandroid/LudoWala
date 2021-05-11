using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace offlineplay
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject LoginPanel, ProfilePanel, HomePanel, SettingPanel, StorePanel, MultiplayerModePanel, GameRulesPanel, ContestPanel,
            PlayWithFriendsPanel, MYProfilePanel, SpinPanel, FriendListPanel, SpecialOfferPanel, CreateRoomPanel, JoinRoomLobbyPanel, JoinRoomPanel, ReferralCodePanel;
        public RoomManager Roommanager;
        private bool isOpenProfile = false;

        void Start()
        {
            Roommanager = transform.Find("RoomManager").GetComponent<RoomManager>();
            LoginPanel.SetActive(true);
            ProfilePanel.SetActive(false);
            HomePanel.SetActive(false);
            SettingPanel.SetActive(false);
            StorePanel.SetActive(false);
            MultiplayerModePanel.SetActive(false);
            if (GameManager.Instance.isLogin)
            {
                if (!GameManager.Instance.IwannaFacebookLogin && !GameManager.Instance.IwannaGoogleLogin)
                    On_Home();
            }
            GameManager.Instance.ReadSettingData();
        }
        public void On_ReferralCode()
        {
            LoginPanel.SetActive(false);
            ProfilePanel.SetActive(true);
            ReferralCodePanel.SetActive(true);
        }
        public void On_Home()
        {
            LoginPanel.SetActive(false);
            ProfilePanel.SetActive(true);
            HomePanel.SetActive(true);
        }
        public void On_Settings()
        {
            HomePanel.SetActive(false);
            SettingPanel.SetActive(true);
        }
        public void On_Store(int i)
        {
            HomePanel.SetActive(false);
            StorePanel.GetComponent<StoreManager>().openType = i;
            StorePanel.SetActive(true);
        }
        public void On_MultiplayerModes()
        {
            HomePanel.SetActive(false);
            MultiplayerModePanel.SetActive(true);
        }
        public void On_GameRules()
        {
            SettingPanel.SetActive(false);
            GameRulesPanel.SetActive(true);
        }
        public void Off_GameRules()
        {
            GameRulesPanel.GetComponent<UIPopup>().Close();
            Invoke("Return_settings", 0.4f);
        }
        void Return_settings()
        {
            On_Settings();
        }
        void Return()
        {
            On_Home();
        }
        public void On_Contest()
        {
            HomePanel.SetActive(false);
            ContestPanel.SetActive(true);
        }
        public void On_PlayWithFriends()
        {
            HomePanel.SetActive(false);
            PlayWithFriendsPanel.SetActive(true);
        }
        public void Off_PlayWithFriends()
        {
            PlayWithFriendsPanel.GetComponent<UIPopup>().Close();
            Invoke("Return", 0.4f);
        }
        public void On_MyProfile()
        {
            isOpenProfile = false;
            HomePanel.SetActive(false);
            StorePanel.SetActive(false);
            SettingPanel.SetActive(false); MultiplayerModePanel.SetActive(false); GameRulesPanel.SetActive(false); ContestPanel.SetActive(false);
            PlayWithFriendsPanel.SetActive(false); SpinPanel.SetActive(false); FriendListPanel.SetActive(false); SpecialOfferPanel.SetActive(false);
            CreateRoomPanel.SetActive(false); JoinRoomLobbyPanel.SetActive(false); JoinRoomPanel.SetActive(false);
            MYProfilePanel.SetActive(true);
        }
        public void On_MyProfile_Setting()
        {
            isOpenProfile = true;
            SettingPanel.SetActive(false);
            MYProfilePanel.SetActive(true);
        }
        public void Off_MyProfile()
        {
            if (!isOpenProfile)
                On_Home();
            else
                On_Settings();
        }
        public void On_Spin()
        {
            Roommanager.Request_Spin();
        }
        public void OpenSpin()
        {
            SpinPanel.SetActive(true);
            HomePanel.SetActive(false);
            ProfilePanel.SetActive(false);
        }
        public void On_FriendList()
        {
            FriendListPanel.SetActive(true);
            HomePanel.SetActive(false);
        }
        private void Update()
        {
            if (GameManager.Instance.settingData.music == true)
            {
                GetComponent<AudioSource>().enabled = true;
            }
            else
                GetComponent<AudioSource>().enabled = false;
        }
    }
}
