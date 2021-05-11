using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInternet : MonoBehaviour
{
    public GameObject blocker;
    void Awake()
    {
        if (!GameManager.Instance.offlineMode && GameManager.Instance.type != MyGameType.comp)
            StartCoroutine("CheckConnection");
    }

    IEnumerator CheckConnection()
    {
        while (true)
        {
            blocker.SetActive(Application.internetReachability == NetworkReachability.NotReachable);
            if (EventCounter.GetID() == "") EventCounter.SetID(GameManager.Instance.UserID);
            yield return new WaitForSeconds(1f);
        }
    }

}
