using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EnterPrivateCodeDialogController : MonoBehaviour
{

    public GameObject inputField;
    public GameObject confirmationText;
    public GameObject joinButton;
    private Button join;
    private InputField field;
    public GameObject GameConfiguration;
    public GameObject failedDialog;
    public Text errorText;
    public string roomID;
    public GameObject codePanel;
    public GameObject detailPanel;
    public Text r_Id;
    public Text c_name;
    public Text t_amount;
    public Text t_player;

    void OnEnable()
    {
        if (field != null)
            field.text = "";
        if (confirmationText != null)
            confirmationText.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
        field = inputField.GetComponent<InputField>();
      // GameManager.Instance.coins = int.Parse(GameManager.Instance.Balance);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetRoomDetail() {
        StartCoroutine(roomDetail());
    }

    IEnumerator roomDetail() {
        if (string.IsNullOrEmpty(field.text))
        {

            confirmationText.SetActive(true);
            confirmationText.GetComponent<Text>().text = "Insert Code";
            yield break;
        }
        roomID = field.text;
        WWWForm form = new WWWForm();
        form.AddField("room_id", roomID);
        Debug.Log(roomID);
        var download = UnityWebRequest.Post("https://codash.tk/ludowala/index.php?user/get_room", form);
        yield return download.SendWebRequest();

        if (download.isNetworkError || download.isHttpError)
        {
            print("Error downloading: " + download.error);
        }
        else
        {

            // show the highscores
            Debug.Log(download.downloadHandler.text);
            if (download.downloadHandler.text == "null") {
                confirmationText.SetActive(true);
                confirmationText.GetComponent<Text>().text = "No room found";
            }
            else
            {
                confirmationText.SetActive(false);
                var userData = JSON.Parse(download.downloadHandler.text);
                c_name.text = "Name : " + userData["data"]["name"].Value;
                t_amount.text = "Amount : " + userData["data"]["amount"].Value;
                r_Id.text = "RoomId : " + userData["data"]["room_id"].Value;
                t_player.text = "T. Player : " + userData["data"]["type"].Value;
                GameManager.Instance.t_player = userData["data"]["type"].Value;
                Debug.Log("t_player" + GameManager.Instance.t_player);
            }

            codePanel.SetActive(confirmationText.activeSelf);
            detailPanel.SetActive(!confirmationText.activeSelf);
        }
    }
        

    public void JoinByRoomID()
    {
        GameManager.Instance.JoinedByID = true;
        GameManager.Instance.payoutCoins = 0;
        GameManager.Instance.type = MyGameType.Private;
        

        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        Debug.Log("Rooms count: " + rooms.Length);

        if (rooms.Length == 0)
        {
            Debug.Log("no rooms!");
            failedDialog.SetActive(true);
        }
        else
        {
            bool foundRoom = false;
            for (int i = 0; i < rooms.Length; i++)
            {
                if (rooms[i].Name.Equals(roomID))
                {
                    foundRoom = true;
                    if (rooms[i].CustomProperties.ContainsKey("pc"))
                    {
                        GameManager.Instance.payoutCoins = int.Parse(rooms[i].CustomProperties["pc"].ToString());
                        Debug.LogFormat("balance {0} and payout {1}", int.Parse(GameManager.Instance.Balance), GameManager.Instance.payoutCoins);
                        if (int.Parse(GameManager.Instance.Balance) >= GameManager.Instance.payoutCoins)
                        {
                            GameManager.Instance.matchPlayerObject.GetComponent<SetMyData>().MatchPlayer();
                            GameManager.Instance.isPrivateTable = true;
                            PhotonNetwork.JoinRoom(roomID);
                           
                        }
                        else
                        {
                            errorText.text = "You don't have enough coins to join room.";
                            failedDialog.SetActive(true);
                        }
                    }
                }
            }
            if (foundRoom == false)
            {
                errorText.text = "Sorry, the game doesn't exist or is already over.";
                failedDialog.SetActive(true);
            }
        }




    }
}
