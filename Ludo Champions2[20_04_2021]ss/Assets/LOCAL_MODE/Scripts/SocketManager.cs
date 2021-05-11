using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace offlineplay
{

    public class SocketManager : MonoBehaviour
    {

        public static SocketManager Instance { get; private set; }
        void Awake()
        {
            //Unity singleton 
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public SocketIO.SocketIOComponent GetSocketIOComponent()
        {
            return GetComponent<SocketIO.SocketIOComponent>();
        }
    }

}