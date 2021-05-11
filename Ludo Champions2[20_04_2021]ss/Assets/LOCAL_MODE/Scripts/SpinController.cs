using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace offlineplay
{

    public class SpinController : MonoBehaviour
    {
        private MenuManager menuManager;
        private RoomManager roomManager;
        public UILabel timer;
        public bool isSpin = false;
        public spinner spinnerObj;
        public UILabel scoreText;
        private Coroutine coroutine;
        private int hr, min, sec = 1;
        void Start()
        {
            menuManager = transform.parent.GetComponent<MenuManager>();
            roomManager = menuManager.transform.Find("RoomManager").GetComponent<RoomManager>();
        }
        private void OnEnable()
        {
            spinnerObj.enabled = isSpin;
            scoreText.text = "";
        }
        public void OnExit()
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            GetComponent<UIPopup>().Close();
            menuManager.On_Home();
        }

        public void SetRemainTime(int hrs, int mins, int secs)
        {
            hr = hrs; min = mins; sec = secs;
            coroutine = StartCoroutine(CountTime());
        }

        IEnumerator CountTime()
        {
            while (true)
            {
                if (sec == 0)
                {
                    sec = 59;
                    if (min == 0)
                    {
                        min = 59;
                        if (hr > 0)
                            hr--;
                    }
                    else
                        min--;
                }
                else
                    sec--;

                timer.text = string.Format("{0:00} : {1:00} : {2:00}", hr, min, sec);
                if (hr == 0 && min == 0 && sec == 0)
                {
                    StopCoroutine(coroutine);
                    roomManager.Request_Spin();
                    GameManager.Instance.wheelSpin = true;
                }
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

}