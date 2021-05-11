using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;


public class UserChallengeManager : MonoBehaviour
{
    public static UserChallengeManager Instance;
    [SerializeField] private string getChallengesAPI = "https://codash.tk/ludowala/Api/AllChalange";

    [Header("UserChallengeList")]
    [SerializeField] private GameObject userChallengeButtonPrefab;
    [SerializeField] private GameObject userChallengeButtonParent;

    [Space]
    [Header("ChallengeInformation")]
    [SerializeField] private GameObject userChallengeInfoScreen;
    [SerializeField] private TextMeshProUGUI EntryFee;
    [SerializeField] private TextMeshProUGUI WinAmount;

    public Dictionary<GameObject, ChallengeModel> challengeDict = new Dictionary<GameObject, ChallengeModel>();

    public GameObject UserChallengeDetailScreen => userChallengeInfoScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        StartCoroutine(ShowUserChallengeDetails());
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

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
            var parsemodel = JSON.Parse(resp);
            var jsonArray = parsemodel.AsArray;
            HandleJSONArray(jsonArray);
        }
    }

    private void HandleJSONArray(JSONArray jsonArray)
    {
        foreach (var item in jsonArray)
        {
            var newChallenge = Instantiate(userChallengeButtonPrefab, userChallengeButtonParent.transform);
            challengeDict.Add(newChallenge, new ChallengeModel() { chalange_id = item.Value["chalange_id"], chalange_name = item.Value["chalange_name"], username = item.Value["username"], user_id = item.Value["user_id"] });

            var requestHandler = newChallenge.GetComponent<ChallengeRequestHandler>();
            requestHandler.ChallengeName.text = item.Value["chalange_name"].ToString();
            requestHandler.requestButton.onClick.AddListener(() => ShowChallengeInfo(newChallenge));
            //Debug.Log($"{item.Value["chalange_id"]} {item.Value["chalange_name"]} {item.Value["user_id"]} {item.Value["username"]}");
        }
    }

    private void ShowChallengeInfo(GameObject currentButton)
    {
        UserChallengeDetailScreen.SetActive(true);

        if (challengeDict.TryGetValue(currentButton, out ChallengeModel selectedChallenge))
        {
            EntryFee.text = selectedChallenge.chalange_id;
            WinAmount.text = selectedChallenge.user_id;
        }
    }
}

//Challenge API db model
[Serializable]
public class ChallengeModel
{
    public string chalange_id;
    public string chalange_name;
    public string user_id;
    public string username;
}
