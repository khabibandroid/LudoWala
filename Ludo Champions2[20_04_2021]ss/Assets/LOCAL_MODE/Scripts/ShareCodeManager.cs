using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace offlineplay
{

    public class ShareCodeManager : MonoBehaviour
    {
        public UILabel joinNumber;

        private void OnEnable()
        {
            GameManager.Instance.isCreateRoom = true;
            joinNumber.text = "";
            joinNumber.text = GameManager.Instance.PrivateRoomId;
        }

        public void OnShare()
        {
#if UNITY_ANDROID
            /*if (joinNumber.text != "")
                NativeShare.ShareText(joinNumber.text);*/
#elif UNITY_IOS
        /*if (joinNumber.text != "")
            NativeShare.ShareText(joinNumber.text);*/
#endif
        }
    }
}