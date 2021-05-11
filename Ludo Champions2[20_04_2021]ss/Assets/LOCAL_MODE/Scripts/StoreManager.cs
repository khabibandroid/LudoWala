using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace offlineplay
{

    public class StoreManager : MonoBehaviour
    {
        private MenuManager menuManager;
        public GameObject SpecialOffer;
        public GameObject MYProfilePanel;
        [System.NonSerialized]
        public int openType = 0;
        public UIToggle[] types;
        public GameObject phonenumberinput, withdrwalinput, accountnumberinput, ifscinput, withbtn, googlePayBtn, paytmBtn, PhonepayBtn, bankBtn;
        void Start()
        {
            menuManager = transform.parent.GetComponent<MenuManager>();
            GetComponent<Animator>().enabled = true;

        }
        private void OnEnable()
        {
            // SpecialOffer.SetActive(true);
            // transform.localScale = Vector3.zero;
            // GetComponent<Animator>().enabled = false;
            // for (int i = 0; i < types.Length; i++)
            // {
            //     if(i == openType) types[i].Set(true); else types[i].Set(false);
            // }
            // if(MYProfilePanel.activeSelf)
            //     MYProfilePanel.SetActive(false);
        }
        public void OnExit()
        {
            withbtn.SetActive(false);
            withdrwalinput.SetActive(false);
            phonenumberinput.SetActive(false);
            accountnumberinput.SetActive(false);
            ifscinput.SetActive(false);
            googlePayBtn.SetActive(true);
            paytmBtn.SetActive(true);
            PhonepayBtn.SetActive(true);
            bankBtn.SetActive(true);

            GetComponent<UIPopup>().Close();
            Invoke("Return", 0.4f);
        }
        void Return()
        {
            menuManager.On_Home();
        }
        public void Off_SpecialOffer()
        {
            SpecialOffer.SetActive(false);

            GetComponent<UIPopup>().Open();
        }


    }

}