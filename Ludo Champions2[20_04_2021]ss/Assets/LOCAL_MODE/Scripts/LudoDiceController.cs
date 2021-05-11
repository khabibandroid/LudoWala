using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace offlineplay
{
    public class LudoDiceController : MonoBehaviour
    {
        public GameObject diceInit;
        private GameObject diceRoll;
        private GameObject diceValue;
        private LudoRoundController RoundController;
        public bool isTurn = false;
        public bool isRolled = false;
        public int value = 1;
        private List<int> diceHistory = new List<int>();


        void Awake()
        {
            print("B");
            diceRoll = transform.Find("diceRoll").gameObject;
            diceValue = transform.Find("diceValue").gameObject;
            RoundController = GameObject.Find("LudoRoundController").GetComponent<LudoRoundController>();
            OnDisable_DiceInit();
            Dice_Init();
            diceInit.GetComponent<UIEventTrigger>().onClick.Add(new EventDelegate(Dice_Roll));
        }
        private void Update()
        {

            if (GameManager.Instance._GamePlayType == GamePlayType.five || GameManager.Instance._GamePlayType == GamePlayType.six)
            {
                if (GameObject.Find("UI Root/Camera").GetComponent<GameUIController>().isOpenAnyPanel)
                    diceRoll.GetComponent<SpriteRenderer>().enabled = false;
                else
                    diceRoll.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        public void Dice_Roll()
        {
            print("D");
            if (isTurn && !isRolled)
            {
                RoundController.GetComponent<UIPlaySound>().audioClip = RoundController.token_arrow;
                RoundController.GetComponent<UIPlaySound>().Play();
                if (diceRoll.activeSelf)
                    return;
                diceInit.SetActive(false);
                diceRoll.SetActive(true);
                if (GameManager.Instance._Wifi == WIFI.online || GameManager.Instance._Wifi == WIFI.privateRoom)
                {
                    if (RoundController.TurnId == 0)
                        RoundController.Roll_Event();
                }
            }
        }
        public void Dice_RandomValue()
        {
            print("E");
            if (GameManager.Instance._Wifi != WIFI.online && GameManager.Instance._Wifi != WIFI.privateRoom)
            {
                value = Random.Range(6, 7);

                diceHistory.Add(value);
                if (diceHistory.Count > 2)
                {
                    if (value == 6)
                    {
                        RoundController.GetComponent<UIPlaySound>().audioClip = RoundController.token_start;
                        RoundController.GetComponent<UIPlaySound>().Play();
                        bool isDoubleSix = false;
                        for (int i = 0; i < diceHistory.Count; i++)
                        {
                            if (diceHistory[i] == 6)
                                isDoubleSix = true;
                            else
                                break;
                        }
                        if (isDoubleSix)
                        {
                            value = Random.Range(1, 5);
                        }
                    }
                    diceHistory.Clear();
                }
            }
            RoundController.MoveSteps = value;
            diceValue.GetComponent<UISprite>().spriteName = string.Format("Dice{0}", value);
            diceValue.SetActive(true);
            RoundController.PlayerControllers[RoundController.TurnId].MoveSteps = value;

            RoundController.PlayerControllers[RoundController.TurnId].CheckMove();
            isRolled = true;
        }

        public void Dice_Init()
        {
            print("F");
            diceInit.SetActive(true); diceRoll.SetActive(false);
            diceValue.GetComponent<UISprite>().alpha = 1.0f;
            diceValue.SetActive(false);
            isRolled = false;
        }

        public void DiceValue_Disable()
        {
            print("G");
            diceInit.SetActive(false); diceRoll.SetActive(false);
            diceValue.SetActive(true);
            diceValue.GetComponent<UISprite>().alpha = 0.5f;
            isRolled = false;
        }

        public void NoneActive_DiceInit_Click()
        {
            print("H");
            diceInit.GetComponent<BoxCollider>().enabled = false;
        }
        public void Active_DiceInit_Click()
        {
            print("I");
            diceInit.GetComponent<BoxCollider>().enabled = true;
        }
        public void OnDisable_DiceInit()
        {
            print("J");
            diceInit.GetComponent<UISprite>().alpha = 0.5f;
        }
        public void OnEnable_DiceInit()
        {
            print("K");
            diceInit.GetComponent<UISprite>().alpha = 1.0f;
            Dice_Init();
        }

    }
}
