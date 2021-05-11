using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoController : MonoBehaviour
{

    public GameObject window;

    public GameObject avatar;
    public GameObject playername;

    public Sprite avatarSprite;

    public GameObject TotalEarningsValue;
    public GameObject CurrentMoneyValue;
    public GameObject GamesWonValue;
    public GameObject WinRateValue;
    public GameObject TwoPlayerWinsValue;
    public GameObject FourPlayerWinsValue;
    public GameObject FourPlayerWinsText;
    public GameObject GamesPlayedValue;
    private int index;
    public Sprite defaultAvatar;

    public GameObject addFriendButton;
    public GameObject editProfileButton;
    public GameObject EditButton;




    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if (!StaticStrings.isFourPlayerModeEnabled)
        {
            FourPlayerWinsValue.SetActive(false);
            FourPlayerWinsText.SetActive(false);
        }

       

        playername.GetComponent<Text>().text = GameManager.Instance.nameMy;

        defaultAvatar = GameManager.Instance.avatarMy;
    }

    public void ShowPlayerInfo(int index)
    {
        this.index = index;
        window.SetActive(true);

        if (index == 0)
        {
       
            addFriendButton.SetActive(false);
            editProfileButton.SetActive(true);
        }
        else
        {
            addFriendButton.SetActive(true);
            editProfileButton.SetActive(false);
            Debug.Log("Player info " + index);

          
        }
    }

  
}
