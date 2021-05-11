using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine;

public class LudoGameController : PunBehaviour, IMiniGame
{

    public GameObject[] dice;
    public GameObject GameGui;
    public GameGUIController gUIController;
    public GameObject[] Pawns1;
    public GameObject[] Pawns2;
    public GameObject[] Pawns3;
    public GameObject[] Pawns4;

    public GameObject gameBoard;
    public GameObject gameBoardScaler;

    [HideInInspector]
    public int steps = 5;

    public bool nextShotPossible;
    public int SixStepsCount = 0;
    public int finishedPawns = 0;
    private int botCounter = 0;
    private List<GameObject> botPawns;
    private static LudoGameController instance;

    public static LudoGameController getInstance()
    {

        return instance;

    }

    List<string> readIDS = new List<string>();


    public void HighlightPawnsToMove(int player, int steps)
    {

        botPawns = new List<GameObject>();

        gUIController.restartTimer();


        GameObject[] pawns = GameManager.Instance.currentPlayer.pawns;

        this.steps = steps;

        if (steps == 6)
        {
            nextShotPossible = true;
            SixStepsCount++;
            if (SixStepsCount == 3)
            {
                nextShotPossible = false;
                if (GameGui != null)
                {
                    //gUIController.SendFinishTurn();
                    Invoke("sendFinishTurnWithDelay", 1.0f);
                }

                return;
            }

            // if (SixStepsCount == 2)
            //{
            //  nextShotPossible = true;
            // GameDiceController.getInstance().steps = 4;

            // if (GameGui != null)
            //   {
            //gUIController.SendFinishTurn();
            //  Invoke("sendFinishTurnWithDelay", 1.0f);

            //  }

            // return;
            // }
        }
        else
        {
            SixStepsCount = 0;
            nextShotPossible = false;
        }

        bool movePossible = false;

        int possiblePawns = 0;
        GameObject lastPawn = null;
        for (int i = 0; i < pawns.Length; i++)
        {
            bool possible = pawns[i].GetComponent<LudoPawnController>().CheckIfCanMove(steps);
            if (possible)
            {
                lastPawn = pawns[i];
                movePossible = true;
                possiblePawns++;
                botPawns.Add(pawns[i]);
            }
       
          
        }



        if (possiblePawns == 1)
        {
            if (GameManager.Instance.currentPlayer.isBot)
            {
                StartCoroutine(movePawn(lastPawn, false));
            }
            else
            {
                lastPawn.GetComponent<LudoPawnController>().MakeMove();
                //StartCoroutine(MovePawnWithDelay(lastPawn));
            }

        }
        else
        {
            if (possiblePawns == 2 && lastPawn.GetComponent<LudoPawnController>().pawnInJoint != null)
            {
                if (GameManager.Instance.currentPlayer.isBot)
                {
                    if (!lastPawn.GetComponent<LudoPawnController>().mainInJoint)
                    {
                        StartCoroutine(movePawn(lastPawn, true));
                        Debug.Log("AAA");
                    }
                    else
                    {
                        StartCoroutine(movePawn(lastPawn.GetComponent<LudoPawnController>().pawnInJoint, true));
                        Debug.Log("BBB");
                    }

                }
                else
                {
                    if (!lastPawn.GetComponent<LudoPawnController>().mainInJoint)
                    {
                        lastPawn.GetComponent<LudoPawnController>().MakeMove();
                    }
                    else
                    {
                        lastPawn.GetComponent<LudoPawnController>().pawnInJoint.GetComponent<LudoPawnController>().MakeMove();
                    }
                    //lastPawn.GetComponent<LudoPawnController>().MakeMove();
                }
            }
            else
            {
                if (possiblePawns > 0 && GameManager.Instance.currentPlayer.isBot)
                {
                    int bestScoreIndex = 0;
                    int bestScore = int.MinValue;
                    // Make bot move
                    if(Random.Range(0, 3) < 2)
                    {
                        for (int i = 0; i < botPawns.Count; i++)
                        {
                            int score = botPawns[i].GetComponent<LudoPawnController>().GetMoveScore(steps);
                            if (score > bestScore)
                            {
                                bestScore = score;
                                bestScoreIndex = i;
                            }
                        }
                    }
                    else
                    {
                        bestScoreIndex = Random.Range(0, botPawns.Count);
                    }
                    StartCoroutine(movePawn(botPawns[bestScoreIndex], true));
                }
            }
        }

        if (!movePossible)
        {
            if (GameGui != null)
            {
                Debug.Log("game controller call finish turn");
                gUIController.PauseTimers();
                Invoke("sendFinishTurnWithDelay", 1.0f);
            }
        }
    }

    private IEnumerator MovePawnWithDelay(GameObject lastPawn)
    {
        yield return new WaitForSeconds(1.0f);

        lastPawn.GetComponent<LudoPawnController>().MakeMove();
    }

    public void sendFinishTurnWithDelay()
    {
        gUIController.SendFinishTurn();
    }

    public void Unhighlight()
    {
        for (int i = 0; i < Pawns1.Length; i++)
        {
            Pawns1[i].GetComponent<LudoPawnController>().Highlight(false);
        }

        for (int i = 0; i < Pawns2.Length; i++)
        {
            Pawns2[i].GetComponent<LudoPawnController>().Highlight(false);
        }

        for (int i = 0; i < Pawns3.Length; i++)
        {
            Pawns3[i].GetComponent<LudoPawnController>().Highlight(false);
        }

        for (int i = 0; i < Pawns4.Length; i++)
        {
            Pawns4[i].GetComponent<LudoPawnController>().Highlight(false);
        }

    }

    void IMiniGame.BotTurn(bool first)
    {
        if (first)
        {
            SixStepsCount = 0;
        }
        Invoke("RollDiceWithDelay", GameManager.Instance.botDelays[(botCounter + 1) % GameManager.Instance.botDelays.Count]);
        botCounter++;
        //throw new System.NotImplementedException();
    }


    public IEnumerator movePawn(GameObject pawn, bool delay)
    {
        if (delay)
        {
            yield return new WaitForSeconds(GameManager.Instance.botDelays[(botCounter + 1) % GameManager.Instance.botDelays.Count]);
            botCounter++;
        }
        pawn.GetComponent<LudoPawnController>().MakeMovePC();
    }

    public void RollDiceWithDelay()
    {
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
        if (false || ran <= GameManager.Instance.botLevel)
        {
            for (int x = 1; x < 6; x++)
            {
                for (int i = 0; i < botPawns.Count; i++)
                {
                    int score = botPawns[i].GetComponent<LudoPawnController>().GetMoveScore(x);
                    Debug.Log(string.Format("Pawn {0} : Score {1} : DIce {2}", i, score, x));
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestScoreIndex = i;
                        num = x;
                        Debug.Log(string.Format("bestScore {0} : bestScoreIndex {1} : num {2}", bestScore, bestScoreIndex, num));
                    }
                }
            }
            Debug.Log("Number decided by before intelligence : " + num);
            if (num == 0 || bestScore <= 0)
            {
                if (false || Random.Range(1, 100) <= GameManager.Instance.botLevel)
                    num = Random.Range(3, 7);
                else
                    num = Random.Range(1, 7);
            }
            Debug.Log("Number decided by after intelligence : " + num);
        }
        else
        {
           if (LudoGameController.getInstance().SixStepsCount == 2)
            {
                nextShotPossible = true;


                num = Random.Range(1, 5);
            }
            else
            {
                num = Random.Range(1, 7);
            }
           

            Debug.Log("Number decided by random : " + num);
        }
        GameManager.Instance.currentPlayer.dice.GetComponent<GameDiceController>().RollDiceBot(num);
    }


    void IMiniGame.CheckShot()
    {
        throw new System.NotImplementedException();
    }

    void IMiniGame.setMyTurn()
    {
        SixStepsCount = 0;
        GameManager.Instance.diceShot = false;
        dice[0].GetComponent<GameDiceController>().EnableShot();
    }

    void IMiniGame.setOpponentTurn()
    {
        SixStepsCount = 0;
        GameManager.Instance.diceShot = false;
        dice[0].GetComponent<GameDiceController>().DisableShot();
        Unhighlight();
    }



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        GameManager.Instance.miniGame = this;
        PhotonNetwork.OnEventCall += this.OnEvent;
    }

    // Use this for initialization
    void Start()
    {
        // Scale gameboard
        instance = this;

        //float scalerWidth = gameBoardScaler.GetComponent<RectTransform>().rect.size.x;
        //float boardWidth = gameBoard.GetComponent<RectTransform>().rect.size.x;
        //
        //gameBoard.GetComponent<RectTransform>().localScale = new Vector2(scalerWidth / boardWidth, scalerWidth / boardWidth);

        gUIController = GameGui.GetComponent<GameGUIController>();


    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        PhotonNetwork.OnEventCall -= this.OnEvent;
    }

    private void OnEvent(byte eventcode, object content, int senderid)
    {
        Debug.Log("Received event Ludo: " + eventcode);

        if (eventcode == (int)EnumGame.DiceRoll)
        {
            object[] dataa = (object[])content;
            if (((string)dataa[0]).Split('_')[1] == EventCounter.GetID())
            {
                if (EventCounter.IsMyDiceRolled())
                {
                    if (!readIDS.Contains((string)dataa[0]))
                    {
                        readIDS.Add((string)dataa[0]);
                        gUIController.PauseTimers();
                        string[] data = ((string)dataa[1]).Split(';');
                        steps = int.Parse(data[0]);
                        int pl = int.Parse(data[1]);

                        GameManager.Instance.playerObjects[pl].dice.GetComponent<GameDiceController>().RollDiceStart(steps);
                    }
                }
            }
            else
            {
                if (!readIDS.Contains((string)dataa[0]))
                {
                    readIDS.Add((string)dataa[0]);
                    gUIController.PauseTimers();
                    string[] data = ((string)dataa[1]).Split(';');
                    steps = int.Parse(data[0]);
                    int pl = int.Parse(data[1]);

                    GameManager.Instance.playerObjects[pl].dice.GetComponent<GameDiceController>().RollDiceStart(steps);
                }
            }
            EventCounter.SetMyDiceRolled(false);
        }
        else if (eventcode == (int)EnumGame.PawnMove)
        {
            object[] dataa = (object[])content;
            if (((string)dataa[0]).Split('_')[1] == EventCounter.GetID())
            {
                if (EventCounter.IsMyPawnMoved())
                {
                    if (!readIDS.Contains((string)dataa[0]))
                    {
                        readIDS.Add((string)dataa[0]);
                        string[] data = ((string)dataa[1]).Split(';');
                        int index = int.Parse(data[0]);
                        int pl = int.Parse(data[1]);
                        steps = int.Parse(data[2]);
                        GameManager.Instance.playerObjects[pl].pawns[index].GetComponent<LudoPawnController>().MakeMovePC();
                    }
                }
            }
            else if (!readIDS.Contains((string)dataa[0]))
            {
                readIDS.Add((string)dataa[0]);
                string[] data = ((string)dataa[1]).Split(';');
                int index = int.Parse(data[0]);
                int pl = int.Parse(data[1]);
                steps = int.Parse(data[2]);
                if(EventCounter.MyIndexInPlayerArray >= 0)
                {
                    if(EventCounter.MyIndexInPlayerArray != pl)
                    {
                        GameManager.Instance.playerObjects[pl].pawns[index].GetComponent<LudoPawnController>().MakeMovePC();
                    }
                }else
                    GameManager.Instance.playerObjects[pl].pawns[index].GetComponent<LudoPawnController>().MakeMovePC();
            }
            EventCounter.SetMyPawnMoved(false);
        }
        else if (eventcode == (int)EnumGame.PawnRemove)
        {
            string data = (string)content;
            string[] messages = data.Split(';');
            int index = int.Parse(messages[1]);
            int playerIndex = int.Parse(messages[0]);

            GameManager.Instance.playerObjects[playerIndex].pawns[index].GetComponent<LudoPawnController>().GoToInitPosition(false);
        }

    }
}
