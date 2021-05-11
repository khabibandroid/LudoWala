using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System;
using SocketIO;
namespace offlineplay
{
    public class ImagePicker : MonoBehaviour
    {
        public UIMaskedTexture image;
        public UIMaskedTexture avatar;
        private byte[] bytes;
        private SocketIOComponent socket;

        private void Start()
        {
            socket = SocketManager.Instance.GetSocketIOComponent();
            socket.On("UPLOAD_USER_PHOTO_RESULT", Upload_User_Photo_result);
        }
        private void OnDestroy()
        {
            socket.Off("UPLOAD_USER_PHOTO_RESULT", Upload_User_Photo_result);
        }
        public void PUTImage()
        {
#if UNITY_ANDROID
            using (var plugin = new AndroidJavaClass("com.mycompany.images.Plugin"))
            {
                plugin.CallStatic("launch");
            }
#endif

        }

        void OnImageReceived(string path)
        {
            Texture2D texture = null;
            byte[] bytes = File.ReadAllBytes(path);
            texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            image.mainTexture = avatar.mainTexture =
            GameManager.Instance.AvatarImage = (Texture)texture;

            if (!string.IsNullOrEmpty(GameManager.Instance.UserName))
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                string byteArray = Convert.ToBase64String(bytes);
                data.Add("userid", GameManager.Instance.UserID);
                data.Add("photo_data", byteArray);
                socket.Emit("UPLOAD_USER_PHOTO", new JSONObject(data));
            }
        }
        private void Upload_User_Photo_result(SocketIOEvent evt)
        {
            print("Photo upload success!");
        }
    }
}
