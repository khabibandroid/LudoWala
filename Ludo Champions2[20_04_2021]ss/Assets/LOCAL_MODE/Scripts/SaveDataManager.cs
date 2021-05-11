using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace offlineplay
{

    public class SaveDataManager : MonoBehaviour
    {
        private MenuManager menuManager;
        public GameObject StorePanel;
        public UIInput Username;
        public UILabel ID, Points, Level;
        public UILabel Online_played, Online_won, Friend_played, Friend_won, Tokenscaptured_mine, Tokenscaptured_opponents, Winstreaks_current, Winstreaks_best;

        private void OnEnable()
        {
            if (StorePanel.activeSelf)
                StorePanel.SetActive(false);
        }
        void Start()
        {
            menuManager = transform.parent.GetComponent<MenuManager>();
            Username.value = GameManager.Instance.UserName;
            ID.text = GameManager.Instance.UserID;
            Points.text = GameManager.Instance.Points.ToString("N0");
            //Level.text = GameManager.Instance.Level.ToString();
            Level.text = GameManager.Instance.Referral_count.ToString();
            Online_played.text = GameManager.Instance.Online_Multiplayer.played.ToString();
            Online_won.text = GameManager.Instance.Online_Multiplayer.won.ToString();
            Friend_played.text = GameManager.Instance.Friend_Multiplayer.played.ToString();
            Friend_won.text = GameManager.Instance.Friend_Multiplayer.won.ToString();
            Tokenscaptured_mine.text = GameManager.Instance.TokensCaptued.mine.ToString();
            Tokenscaptured_opponents.text = GameManager.Instance.TokensCaptued.opponents.ToString();
            Winstreaks_current.text = GameManager.Instance.WonStreaks.current.ToString();
            Winstreaks_best.text = GameManager.Instance.WonStreaks.best.ToString();
        }
        private void Update()
        {
            Level.text = GameManager.Instance.Referral_count.ToString();
        }
        public void OnExit()
        {
            GetComponent<UIPopup>().Close();
            Invoke("Return", 0.4f);
        }
        void Return()
        {
            menuManager.Off_MyProfile();
        }
    }

}