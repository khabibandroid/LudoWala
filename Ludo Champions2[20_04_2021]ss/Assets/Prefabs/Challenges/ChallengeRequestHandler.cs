using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChallengeRequestHandler : MonoBehaviour
{
    [SerializeField] private Button sendRequestButton;
    [SerializeField] private TextMeshProUGUI challengeName;
    public TextMeshProUGUI ChallengeName => challengeName;
    public Button requestButton => sendRequestButton;

    // Start is called before the first frame update
    //void Start()
    //{
    //    sendRequestButton.onClick.AddListener(() =>
    //    {
    //        UserChallengeManager.Instance.UserChallengeDetailScreen.SetActive(true);
    //    });
    //}
}
