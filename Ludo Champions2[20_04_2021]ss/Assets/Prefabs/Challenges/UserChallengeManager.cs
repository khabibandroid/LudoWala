using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using UnityEngine.UI;


public class UserChallengeManager : MonoBehaviour
{
    public static UserChallengeManager Instance;
    [SerializeField] private string getChallengesAPI = "https://codash.tk/ludowala/Api/AllChalange";
    [SerializeField] private string createChallengeAPI = "https://codash.tk/ludowala/Api/makeChalange";

    [Header("UserChallengeList")]
    [SerializeField] private GameObject userChallengeButtonPrefab;
    [SerializeField] private GameObject userChallengeButtonParent;

    [Space]
    [Header("ChallengeInformation")]
    [SerializeField] private GameObject userChallengeInfoScreen;
    public GameObject UserChallengeDetailScreen => userChallengeInfoScreen;
    [SerializeField] private TextMeshProUGUI EntryFee;
    [SerializeField] private TextMeshProUGUI WinAmount;

    [Space]
    [Header("CreateChallenge")]
    [SerializeField] private TMP_InputField challengeName;
    [SerializeField] private TMP_InputField bid_amount;

    public Dictionary<GameObject, ChallengeModel> challengeDict = new Dictionary<GameObject, ChallengeModel>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        StartCoroutine(ShowUserChallengeDetails());
        StartCoroutine(CreateNewChallenge());
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator CreateNewChallenge()
    {

        WWWForm formData = new WWWForm();
        formData.AddField("user_id", 556);
        formData.AddField("chalange_name", "lkhjlkjer");
        formData.AddField("bid_amount", 20);


        UnityWebRequest uwr = UnityWebRequest.Post(createChallengeAPI, formData);
        yield return uwr.SendWebRequest();

        while (!uwr.isDone)
        {
            Debug.Log($"{uwr.downloadProgress}");
        }

        if (uwr.isHttpError || uwr.isNetworkError)
        {
            Debug.Log($"isError: {uwr.isNetworkError} {uwr.isHttpError}");
        }
        else
        {
            var resp = JsonUtility.FromJson<CreateChallenge>(uwr.downloadHandler.text);

            if(resp.status == "failed")
            {
                Debug.Log($"failed: {resp.status}");
            }
            else
            {
                Debug.Log($"CreateChallengeResponse: {resp.status}");
            }
        }

    }


    #region Show Challenge Details
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
    #endregion
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

[Serializable]
public class CreateChallenge
{
    public string status;
    public string mssg;
}
