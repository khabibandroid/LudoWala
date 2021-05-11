public class Enums
{

}


public enum AdLocation
{
    GameStart,
    GameOver,
    LevelComplete,
    Pause,
    FacebookFriends,
    GameFinishWindow,
    StoreWindow,
    GamePropertiesWindow

};

public enum MyGameType
{
   TwoPlayer, ThreePlayer, FourPlayer, Private, comp

        
};

public enum MyGameMode
{
    Classic, Master, Quick
}

public enum EnumPhoton
{
    ReadyToPlay = 179,
    BeginPrivateGame = 171,
    NextPlayerTurn = 172,
    StartWithBots = 173,
    StartGame = 174,
    SendChatMessage = 175,
    TurnSkiped = 194,
    SendChatEmojiMessage = 176,
    AddFriend = 177,
    FinishedGame = 178,
    DisAtLobby = 190,
    LeavingMatchMaking = 192,
    SendTypedMessage = 165
}

public enum EnumGame
{
    DiceRoll = 50,
    PawnMove = 51,
    PawnRemove = 52,
}