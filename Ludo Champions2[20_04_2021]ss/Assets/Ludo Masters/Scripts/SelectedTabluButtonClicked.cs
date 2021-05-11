using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectedTabluButtonClicked : MonoBehaviour
{

    public int tableNumber;
    public int fee;

    // Use this for initialization

    void Start()
    {
        Debug.Log("start");
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        gameObject.GetComponent<Button>().onClick.AddListener(startGame);
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void startGame()
    {
        if (GameManager.Instance.type == MyGameType.comp)
        {
            GameManager.Instance.type = MyGameType.comp;
            GameManager.Instance.GameScene = "GameScene";
            GameManager.Instance.requiredPlayers = tableNumber;

            Debug.Log("Fee: " + fee + "  Coins: " + GameManager.Instance.totalBalance);
            if (int.Parse(GameManager.Instance.Balance) >= fee)
            {
                if (GameManager.Instance.inviteFriendActivated)
                {
                    GameManager.Instance.tableNumber = tableNumber;
                    GameManager.Instance.payoutCoins = fee;
                    GameManager.Instance.initMenuScript.backToMenuFromTableSelect();
                    GameManager.Instance.playfabManager.challengeFriend(GameManager.Instance.challengedFriendID, "" + fee + ";" + tableNumber);

                }
                if (GameManager.Instance.offlineMode)
                {
                    GameManager.Instance.payoutCoins = fee;
                    if (!GameManager.Instance.gameSceneStarted)
                    {
                        SceneManager.LoadScene(GameManager.Instance.GameScene);
                        GameManager.Instance.gameSceneStarted = true;
                    }
                }
                else
                {
                    GameManager.Instance.tableNumber = tableNumber;
                    GameManager.Instance.payoutCoins = fee;
                    GameManager.Instance.facebookManager.startRandomGame();
                }

            }
            else
            {
                //  GameManager.Instance.game.lowbalance.SetActive(true);
            }

            //            GameManager.Instance.dialog.SetActive(true);

        }


        else
        {
            GameManager.Instance.GameScene = "GameScene";
            GameManager.Instance.requiredPlayers = tableNumber;

            Debug.Log("Fee: " + fee + "  Coins: " + GameManager.Instance.totalBalance);
            if (int.Parse(GameManager.Instance.Balance) >= fee)
            {
                if (GameManager.Instance.inviteFriendActivated)
                {
                    GameManager.Instance.tableNumber = tableNumber;
                    GameManager.Instance.payoutCoins = fee;
                    GameManager.Instance.initMenuScript.backToMenuFromTableSelect();
                    GameManager.Instance.playfabManager.challengeFriend(GameManager.Instance.challengedFriendID, "" + fee + ";" + tableNumber);

                }
                if (GameManager.Instance.offlineMode)
                {
                    GameManager.Instance.payoutCoins = fee;
                    if (!GameManager.Instance.gameSceneStarted)
                    {
                        SceneManager.LoadScene(GameManager.Instance.GameScene);
                        GameManager.Instance.gameSceneStarted = true;
                    }
                }
                else
                {
                    GameManager.Instance.tableNumber = tableNumber;
                    GameManager.Instance.payoutCoins = fee;
                    GameManager.Instance.facebookManager.startRandomGame();
                }

            }
            else
            {
                // GameManager.Instance.game.lowbalance.SetActive(true);
            }

        }




    }


    public void comp()
    {
        GameManager.Instance.type = MyGameType.comp;
    }
}