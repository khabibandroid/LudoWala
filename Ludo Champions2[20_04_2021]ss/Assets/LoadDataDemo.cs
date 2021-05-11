using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadDataDemo : MonoBehaviour
{
    public string url;

    public prizeData prizedata;




    private void Start()
    {
        StartCoroutine(MakeWebRequest());
    }

    IEnumerator MakeWebRequest()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if(request.isNetworkError || request.isHttpError)
        {
            Debug.Log("failed to send request" + request.error);

        }
        else
        {
            prizedata = new prizeData();
            prizedata = JsonUtility.FromJson<prizeData>(request.downloadHandler.text);
        }
    }
}


// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
[System.Serializable]
public class Datum
{
    public string winning_percentage_id;
    public string players;
    public string player_1;
    public string player_2;
    public string player_3;
    public string player_4;
    public string created_at;
    public string updated_at;
    public string deleted_at;
}

[System.Serializable]
public class prizeData
{
    public string status;
    public string message;
    public List<Datum> data;
}
