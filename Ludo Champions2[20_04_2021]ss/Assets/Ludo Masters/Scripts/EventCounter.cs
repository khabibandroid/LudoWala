using System;

public class EventCounter
{
    private static int count = 0;
    public static bool LogOut = false;
    public static int autoMovesCount = 5;
    private static bool myPawnMoved = false, myDiceRolled = false, gameBegan = false;
    private static string myID = "";
    internal static bool reconnected = false;

    public static int MyIndexInPlayerArray = -1;

    public static void ResetALLData()
    {
        //count = 0;
        if(GameManager.Instance != null)
        {
            if (GameManager.Instance.playfabManager != null)
                GameManager.Instance.playfabManager.roomOwner = false;
            GameManager.Instance.roomOwner = false;
            GameManager.Instance.resetAllData();
        }
        myPawnMoved = false;
        myDiceRolled = false;
        gameBegan = false;
        autoMovesCount = 5;
        myID = "";
        reconnected = false;
        MyIndexInPlayerArray = -1;
    }

    public static int GetCount()
    {
        count++;
        return count - 1;
    }
    public static bool IsMyPawnMoved()
    {
        return myPawnMoved;
    }
    public static bool IsMyDiceRolled()
    {
        return myDiceRolled;
    }
    public static void SetMyPawnMoved(bool b)
    {
        myPawnMoved = b;
    }
    public static void SetID(string id)
    {
        myID = id;
    }
    public static string GetID()
    {
        return myID;
    }
    public static void ResetCount()
    {
        count = 0;
    }

    public static bool hasGameBegan()
    {
        return gameBegan;
    }

    public static void SetGameBegan(bool v)
    {
        gameBegan = v;
    }

    internal static void SetMyDiceRolled(bool v)
    {
        myDiceRolled = v;
    }
}
