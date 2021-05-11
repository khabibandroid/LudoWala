using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeRequestHandler : MonoBehaviour
{
    [SerializeField] private Button sendRequestButton;

    // Start is called before the first frame update
    void Start()
    {
        sendRequestButton.onClick.AddListener(() =>
        {
            UserChallengeManager.Instance.UserChallengeDetailScreen.SetActive(true);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
