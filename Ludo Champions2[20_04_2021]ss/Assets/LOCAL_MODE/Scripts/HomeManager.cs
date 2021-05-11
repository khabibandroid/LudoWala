using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace offlineplay
{
    public class HomeManager : MonoBehaviour
    {
        private MenuManager menuManager;
        private GameObject loading;


        void Start()
        {
            menuManager = transform.parent.GetComponent<MenuManager>();
            loading = transform.Find("Loading").gameObject;
        }
        private void OnEnable()
        {
            GameManager.Instance.RoomID = 0;
        }
        public void OnClick_OfflineMode()
        {
            GameManager.Instance._Wifi = WIFI.offline;
            menuManager.On_MultiplayerModes();
        }
        public void OnClick_OnlineMode()
        {
            GameManager.Instance._Wifi = WIFI.online;
            menuManager.On_MultiplayerModes();
        }
        public void OnClick_VsComputerMode()
        {
            GameManager.Instance._Wifi = WIFI.vsComputer;
            menuManager.On_MultiplayerModes();
        }
        public void OnClick_PrivateRoomMode()
        {
            GameManager.Instance._Wifi = WIFI.privateRoom;

        }
        public void OnClick_Settings()
        {
            menuManager.On_Settings();
        }

    }
}
