using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;

public class TypeToChat : PunBehaviour
{

    public InputField messageBox;
    public GameObject MessagePanel;
    public List<ChatMessage> chat = new List<ChatMessage>();

    void Awake()
    {
        PhotonNetwork.OnEventCall += this.OnEvent;
    }


    void OnDestroy()
    {
        PhotonNetwork.OnEventCall -= this.OnEvent;
    }

    private void OnEvent(byte eventcode, object content, int senderid)
    {
        Debug.Log("received event in type to chat: " + eventcode);
        if (eventcode == (int)EnumPhoton.SendTypedMessage)
        {
            string[] message = ((string)content).Split(';');
            Debug.Log("Received typed message in type to chat " + message[0] + " from " + message[1]);
            chat.Add(new ChatMessage(message[1], message[0]));
            string history = "";
            foreach (ChatMessage cm in chat)
                history += cm.ToString();
            Debug.Log(history);
        }
    }

    //new func to send msg typed in box
    public void SendTypedMessage()
    {
        string msg = messageBox.text;
        messageBox.text = "";
        if (string.IsNullOrEmpty(msg))
            return;
        SendMessageEvent(msg);
    }

    public void SendMessageEvent(string msg)
    {
        Debug.Log("typed message " + msg);
        /*if (!GameManager.Instance.offlineMode)
            */PhotonNetwork.RaiseEvent((int)EnumPhoton.SendTypedMessage, msg + ";" + PhotonNetwork.playerName, true, null);
        messageBox.text = "";
    }
}
[Serializable]
public class ChatMessage
{
    public string name;
    public string message;

    public ChatMessage(string name, string message)
    {
        this.name = name;
        this.message = message;
    }

    public override string ToString()
    {
        return "{ " + name + ":" + message + " }";
    }
}
