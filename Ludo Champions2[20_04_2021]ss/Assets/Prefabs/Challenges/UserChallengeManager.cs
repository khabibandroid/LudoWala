using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;


public class UserChallengeManager : MonoBehaviour
{
    //OnlineMoney Room Rate Manager 

    [SerializeField] private OnlineMoneyRoomRateManager OnlineMoneyRoomRate;



    private static UserChallengeManager instance;
    [SerializeField] private string getChallengesAPI = "https://codash.tk/ludowala/Api/AllChalange";
    [Header("UserChallengeList")]
    [SerializeField] private GameObject userChallengeButtonPrefab;
    [SerializeField] private GameObject userChallengeButtonParent;
    [Space]
    [Header("ChallengeInformation")]
    [SerializeField] private GameObject userChallengeInfoScreen;
    [SerializeField] private TextMeshProUGUI EntryFee;
    [SerializeField] private TextMeshProUGUI WinAmount;
    [SerializeField] private GameObject userChallengeOfflineStatusScreen;
    public Dictionary<GameObject, ChallengeModel> challengeDict = new Dictionary<GameObject, ChallengeModel>();
    public GameObject UserChallengeDetailScreen => userChallengeInfoScreen;
    public GameObject UserChallengeCoinScreen;
    public List<string> UserIdList = new List<string>();
    private string currentUserID;
    private string currentUserName;
    public int bid_Amount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = new UserChallengeManager();
        }

    }



    // Start is called before the first frame update
    void Start()
    {
    }



    public void OnChallengeClicked()
    {
        // PhotonNetwork.FindFriends(new string[] { "553","Siva"});
        StartCoroutine(ShowUserChallengeDetails());
    }


    private IEnumerator ShowUserChallengeDetails()
    {
        var uwr = UnityWebRequest.Get(getChallengesAPI);
        yield return uwr.SendWebRequest();

        while (!uwr.isDone)
        {
            Debug.Log($"{uwr.downloadProgress}");
        }

        if (uwr.isHttpError || uwr.isNetworkError)
        {
            Debug.Log(uwr.isNetworkError);
        }
        else
        {
            var resp = uwr.downloadHandler.text;
            Debug.LogError($"Response: {resp}");
            //var resp = "[{ 'chalange_id':'9','chalange_name':'newName','user_id':'557','username':'Siva'},{ 'chalange_id':'8','chalange_name':'MyChallenge','user_id':'553','username':'Ash'}]";
            var parsemodel = JSON.Parse(resp);
            var jsonArray = parsemodel[0]["userData"].AsArray;
            StartCoroutine(HandleJSONArray(jsonArray));
        }
    }

    IEnumerator HandleJSONArray(JSONArray jsonArray)
    {
        UserIdList.Clear();
        Debug.LogError("JSON ARRAY COUNT Before :" + jsonArray.Count + "  " + UserIdList.Count);
        foreach (var item in jsonArray)
        {
            UserIdList.Add(item.Value["user_id"]);
        }
        PhotonNetwork.FindFriends(UserIdList.ToArray());

        yield return new WaitForSeconds(2);

        Debug.LogError("JSON ARRAY COUNT  After:" + jsonArray.Count + "  " + UserIdList.Count);

        foreach (var item2 in jsonArray)
        {
            //TODO: PlayFabManager Method  Missing
            //if (GameManager.Instance.playfabManager.IsUserIDOnline(item2.Value["user_id"], item2.Value["username"]))
            {
                var newChallenge = Instantiate(userChallengeButtonPrefab, userChallengeButtonParent.transform);
                challengeDict.Add(newChallenge, new ChallengeModel() { username = item2.Value["username"], user_id = item2.Value["user_id"], email = item2.Value["email"], bid_amount = item2.Value["bid_amount"]});
                //chalange_id = item.Value["chalange_id"], bid_amount = item.Value["bid_amount"], 

                var requestHandler = newChallenge.GetComponent<ChallengeRequestHandler>();
                requestHandler.ChallengeName.text = item2.Value["username"] + "test...";
                requestHandler.requestButton.onClick.AddListener(() => ShowChallengeInfo(newChallenge)); // newChallenge
            }
        }
    }

    public void OnChallengePopupClosed()
    {
        if (userChallengeButtonParent.transform.childCount > 0)
        {
            userChallengeButtonParent.transform.DestroyChildren();
        }
    }

    private void ShowChallengeInfo(GameObject currentButton)  // 
    {
        UserChallengeDetailScreen.SetActive(true);

        if (challengeDict.TryGetValue(currentButton, out ChallengeModel selectedChallenge))
        {
            // EntryFee.text = selectedChallenge.bid_amount;
            //WinAmount.text = selectedChallenge.bid_amount + 50;
            // UserChallengeDetailScreen.GetComponentInChildren<Button>().onClick.AddListener(() => OnInnerSendRequestClicked(selectedChallenge.user_id));
            //UserChallengeCoinScreen.GetComponentInChildren<Button>().onClick.AddListener(() => OnInnerSendRequestClicked(selectedChallenge.user_id));
            currentUserID = selectedChallenge.user_id;
            currentUserName = selectedChallenge.username;
            bid_Amount = int.Parse(selectedChallenge.bid_amount);

        }
    }

    public void OnInnerSendRequestClicked()
    {
        Debug.LogError($"OnInnerSendRequestClicked: {currentUserID}");
        //PhotonNetwork.FindFriends(new string[] { currentUserID });
        //Invoke("CheckUserIsOnline", 2f);

        if (int.Parse(GameManager.Instance.Balance) > bid_Amount)
        {
            StartCoroutine(CheckUserIsOnline());
        }
        else
        {
            OnlineMoneyRoomRate.lowbalance.SetActive(true);
        }
    }

    private IEnumerator CheckUserIsOnline()
    {
        yield return new WaitForSeconds(2);
        //TODO: PlayFabManager Method  Missing
        //if (/*GameManager.Instance.playfabManager.IsUserIDOnline(currentUserID, currentUserName)*/ true)
        if (PhotonNetwork.FindFriends(new string[] { currentUserID }))
        {
            //Debug.Log("COMING TO INNER SEND REQUEST ...." + currentUserID);
            GameManager.Instance.playfabManager.CreatePrivateRoom();
            GameManager.Instance.isPrivateTable = true;
            GameManager.Instance.matchPlayerObject.GetComponent<SetMyData>().MatchPlayer();
            GameManager.Instance.playfabManager.challengeFriend(currentUserID, GameManager.Instance.payoutCoins + ";" + GameManager.Instance.privateRoomID);
            // Debug.Log("COMING TO INNER SEND ROOM ID ...." + GameManager.Instance.privateRoomID+ " ROOM ID "+GameManager.Instance.RoomID);
        }
        else
        {
            userChallengeOfflineStatusScreen.SetActive(true);
        }
    }
}

//Challenge API db model
[Serializable]
public class ChallengeModel
{
    public string chalange_id;
    public string bid_amount;
    public string user_id;
    public string username;
    public string email;
}
