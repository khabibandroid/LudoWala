using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace offlineplay
{
    public class LanguageController : MonoBehaviour
    {
        private MenuManager menuManager;

        void Start()
        {
            menuManager = transform.parent.GetComponent<MenuManager>();
        }
        public void OnExit()
        {
            GetComponent<UIPopup>().Close();
            Invoke("Return", 0.4f);
        }
        void Return()
        {
            menuManager.On_Settings();
        }
    }
}
