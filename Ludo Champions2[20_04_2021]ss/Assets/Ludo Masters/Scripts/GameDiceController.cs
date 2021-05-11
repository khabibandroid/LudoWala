using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine;
using UnityEngine.UI;

public class GameDiceController : MonoBehaviour
{

    public Sprite[] diceValueSprites;
    public GameObject arrowObject;
    public GameObject diceValueObject;
    public GameObject diceAnim;

   

    public int diceValue;

    // Use this for initialization
    public bool isMyDice = false;
    public GameObject LudoController;
    public LudoGameController controller;
    public int player = 1;
    private Button button;

    public GameObject notInteractable;
    public static GameDiceController getInstance()
    {

        return instance;

    }

    public int steps = 0;
    void Start()
    {
        button = GetComponent<Button>();
        controller = LudoController.GetComponent<LudoGameController>();
        instance = this;
        button.interactable = false;
    }

    public void AutoDiceRollCustom()
    {
        controller.nextShotPossible = false;
        controller.gUIController.PauseTimers();
        button.interactable = false;
        Debug.Log("Roll Dice");
        arrowObject.SetActive(false);
        // if (aa % 2 == 0) steps = 6;
        // else steps = 2;
        // aa++;
        steps = Random.Range(6, 7);

        RollDiceStart(steps);
        string data = steps + ";" + controller.gUIController.GetCurrentPlayerIndex;

        if (EventCounter.GetID() == "" || string.IsNullOrEmpty(EventCounter.GetID())) EventCounter.SetID(GameManager.Instance.UserID);
        if (GameManager.Instance.currentPlayer.id.Contains(GameManager.Instance.UserID))
        {
            EventCounter.SetMyDiceRolled(true);
        }

        PhotonNetwork.RaiseEvent((int)EnumGame.DiceRoll, new object[] { EventCounter.GetCount() + "_" + EventCounter.GetID(), data }, true, null);

        Debug.Log("Value: " + steps);
    }

    public void SetDiceValue()
    {
        Debug.Log("Set dice value called");
        diceValueObject.GetComponent<Image>().sprite = diceValueSprites[steps - 1];
        diceValueObject.SetActive(true);
        diceAnim.SetActive(false);
        controller.gUIController.restartTimer();
        if (isMyDice)
            controller.HighlightPawnsToMove(player, steps);
        if (GameManager.Instance.currentPlayer.isBot)
        {
            controller.HighlightPawnsToMove(player, steps);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableShot()
    {
        if (GameManager.Instance.currentPlayer.isBot)
        {
            GameManager.Instance.miniGame.BotTurn(false);
            notInteractable.SetActive(false);
        }
        else
        {

            controller.gUIController.myTurnSource.Play();
            notInteractable.SetActive(false);
            button.interactable = true;
            arrowObject.SetActive(true);
        }
    }

    public void DisableShot()
    {
        notInteractable.SetActive(true);
        button.interactable = false;
        arrowObject.SetActive(false);
    }

    public void EnableDiceShadow()
    {
        notInteractable.SetActive(true);
    }

    public void DisableDiceShadow()
    {
        notInteractable.SetActive(false);
    }
    int aa = 0;
    int bb = 0;
    private static GameDiceController instance;
    private List<GameObject> botPawns;
    public void RollDice()
    {
        /*  if (isMyDice)
          {

              controller.nextShotPossible = false;
              controller.gUIController.PauseTimers();
              button.interactable = false;
              Debug.Log("Roll Dice");
              arrowObject.SetActive(false);

              botPawns = new List<GameObject>();

              bool movePossible = false;

              int possiblePawns = 0;
              GameObject lastPawn = null;
              GameObject[] pawns = GameManager.Instance.currentPlayer.pawns;
              for (int i = 0; i < pawns.Length; i++)
              {
                  bool possible = false;
                  for (int x = 1; x < 6; x++)
                  {
                      possible = pawns[i].GetComponent<LudoPawnController>().CheckIfCanMove(x);
                      if (possible)
                          break;
                  }
                  if (possible)
                  {
                      lastPawn = pawns[i];
                      movePossible = true;
                      possiblePawns++;
                      botPawns.Add(pawns[i]);
                  }
              }
              int num = 0;
              int ran = Random.Range(1, 100);
              int bestScoreIndex = 0;
              int bestScore = int.MinValue;


              if (ran <= 100)
              {
                  for (int x = 1; x < 6; x++)
                  {
                      for (int i = 0; i < botPawns.Count; i++)
                      {
                          int score = botPawns[i].GetComponent<LudoPawnController>().GetMoveScore(x);
                          Debug.Log(string.Format("Pawn {0} : Score {1} : DIce {2}", i, score, x));

                          if (score == 400 || score == 700 || score == -100)
                          {
                              bestScore = score;
                              bestScoreIndex = i;
                              if (LudoGameController.getInstance().SixStepsCount == 2)
                              {
                                  LudoGameController.getInstance().nextShotPossible = true;
                                  if (x == 6)
                                  {
                                      num = Random.Range(1,5);
                                      Debug.Log("numb: " + num);
                                  }
                                  else
                                  {
                                      if (x == 1)
                                      {

                                          num = Random.Range(2,7);
                                          Debug.Log("numb: " + num);

                                      }
                                      if (x == 2)
                                      {
                                          num = Random.Range(3,7);
                                          Debug.Log("numb: " + num);
                                      }
                                      if (x == 3)
                                      {
                                          num = Random.Range(4,7);
                                          Debug.Log("numb: " + num);
                                      }
                                      if (x == 4)
                                      {
                                          num = Random.Range(1,3);
                                          Debug.Log("numb: " + num);
                                      }
                                      if (x == 5)
                                      {
                                          num = Random.Range(1, 4);
                                          Debug.Log("numb: " + num);
                                      }

                                      if (x == 6)
                                      {
                                          num = Random.Range(1, 5);
                                          Debug.Log("numb: " + num);
                                      }
                                  }
                              }
                              else
                              {
                                  if (x == 1)
                                  {

                                      num = Random.Range(2,7);
                                      Debug.Log("numb: " + num);

                                  }
                                  if (x == 2)
                                  {
                                      num = Random.Range(3,7);
                                      Debug.Log("numb: " + num);
                                  }
                                  if (x == 3)
                                  {
                                      num = Random.Range(4,7);
                                      Debug.Log("numb: " + num);
                                  }
                                  if (x == 4)
                                  {
                                      num = Random.Range(1, 3);
                                      Debug.Log("numb: " + num);
                                  }
                                  if (x == 5)
                                  {
                                      num = Random.Range(1, 4);
                                      Debug.Log("numb: " + num);
                                  }

                                  if (x == 6)
                                  {
                                      num = Random.Range(1, 5);
                                      Debug.Log("numb: " + num);
                                  }
                                  Debug.Log(string.Format("Joint not happen\n" + "bestScore {0} : bestScoreIndex {1} : num {2}", bestScore, bestScoreIndex, num));
                              }
                          }
                          if (score > bestScore && score != 400 && score != 700 && score != -100)
                          {
                              bestScore = score;
                              bestScoreIndex = i;
                              if (LudoGameController.getInstance().SixStepsCount == 2)
                              {
                                  LudoGameController.getInstance().nextShotPossible = true;
                                  if (x == 6)
                                  {
                                      num = Random.Range(1,5);
                                      Debug.Log("numb: " + num);
                                  }
                                  else
                                  {
                                      num = Random.Range(1, 5);
                                      Debug.Log("numb: " + num);
                                  }
                              }
                              else
                              {
                                  num = x;
                              }
                              Debug.Log(string.Format("bestScore {0} : bestScoreIndex {1} : num {2}", bestScore, bestScoreIndex, num));
                          }
                      }
                  }
                  Debug.Log("MINE  Number decided by before intelligence : " + num);
                  if (num == 0 || bestScore <= 0)
                  {

                      if (Random.Range(0, 100) <= 100)
                      {
                          if (LudoGameController.getInstance().SixStepsCount == 2)
                          {
                              LudoGameController.getInstance().nextShotPossible = true;
                              if (num == 6)
                              {
                                  num = Random.Range(1, 5);
                                  Debug.Log("numb: " + num);
                              }
                              else
                              {
                                  num = Random.Range(1, 5);
                                  Debug.Log("numb: " + num);
                              }
                          }
                          else
                          {
                              num = Random.Range(1, 7);
                              Debug.Log("numb: " + num);
                          }


                      }

                      else
                      {
                          if (LudoGameController.getInstance().SixStepsCount == 2)
                          {
                              LudoGameController.getInstance().nextShotPossible = true;
                              if (num == 6)
                              {
                                  num = Random.Range(2, 5);
                                  Debug.Log("numb: " + num);
                              }
                          }
                          else
                          {
                              num = Random.Range(2, 7);
                          }
                      }
                  }
                  Debug.Log("MINE  Number decided by after intelligence : " + num);
              }
              else
              {
                  if (LudoGameController.getInstance().SixStepsCount == 2)
                  {
                      LudoGameController.getInstance().nextShotPossible = true;


                      num = Random.Range(3, 5);
                  }
                  else
                  {
                      num = Random.Range(2, 7);
                  }


                  Debug.Log("MINE Number decided by random : " + num);
              }




              // if (aa % 2 == 0) steps = 6;
              // else steps = 2;
              // aa++;


              RollDiceStart(num);
              string data = num + ";" + controller.gUIController.GetCurrentPlayerIndex;
              PhotonNetwork.RaiseEvent((int)EnumGame.DiceRoll, data, true, null);

              Debug.Log("Value: " + num);

          }*/
        if (isMyDice)
        {

            controller.nextShotPossible = false;
            controller.gUIController.PauseTimers();
            button.interactable = false;
            Debug.Log("Roll Dice");
            arrowObject.SetActive(false);
            // if (aa % 2 == 0) steps = 6;
            // else steps = 2;
            // aa++;

            steps = Random.Range(1, 7) ;

            RollDiceStart(steps);
            string data = steps + ";" + controller.gUIController.GetCurrentPlayerIndex;

            if (EventCounter.GetID() == "" || string.IsNullOrEmpty(EventCounter.GetID())) EventCounter.SetID(GameManager.Instance.UserID);
            if (GameManager.Instance.currentPlayer.id.Contains(GameManager.Instance.UserID))
            {
                EventCounter.SetMyDiceRolled(true);
            }

            PhotonNetwork.RaiseEvent((int)EnumGame.DiceRoll, new object[] { EventCounter.GetCount() + "_" + EventCounter.GetID(), data }, true, null);

            Debug.Log("Value: " + steps);


            /*

            steps = Random.Range(1, 7);

            RollDiceStart(steps);
            string data = steps + ";" + controller.gUIController.GetCurrentPlayerIndex;
            PhotonNetwork.RaiseEvent((int)EnumGame.DiceRoll, data, true, null);

            Debug.Log("Value: " + steps);

            */
        }
    }
    public void RollDiceBot(int value)
    {

        controller.nextShotPossible = false;
        controller.gUIController.PauseTimers();

        Debug.Log("Roll Dice bot");

        // if (bb % 2 == 0) steps = 6;
        // else steps = 2;
        // bb++;


        steps = value;

        RollDiceStart(steps);


    }

    public void RollDiceStart(int steps)
    {
      
        GetComponent<AudioSource>().Play();
        this.steps = steps;
        diceValueObject.SetActive(false);
        diceAnim.SetActive(true);
        diceAnim.GetComponent<Animator>().Play("RollDiceAnimation");
    }
}
