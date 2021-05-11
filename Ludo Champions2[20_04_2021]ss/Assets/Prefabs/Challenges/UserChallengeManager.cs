using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

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
    public GameObject UserChallengeDetailScreen => userChallengeInfoScreen;

    ChallengeUserModel model = new ChallengeUserModel();

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

        }
        else
        {
            var resp = uwr.downloadHandler.text;
            //model = JsonUtility.FromJson<ChallengeUserModel>(resp);
            var parsemodel = JSONArray.Parse(resp);
            var jsonArray = parsemodel.AsArray;
            Debug.Log($"ChallengeData: -----> {jsonArray.Count}");

            foreach (var item in jsonArray)
            {
                Debug.Log($"{item.Key} {item.Value}");
            }
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


[Serializable]
public class ChallengeUserModel
{
    public List<ChallengeModel> challengeModels;
}

