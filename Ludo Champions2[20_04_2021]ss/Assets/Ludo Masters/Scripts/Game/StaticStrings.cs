namespace AssemblyCSharp
{
    public static class StaticStrings
    {

        public static string AndroidPackageName = "com.ludo.champions";
        public static string ITunesAppID = "11111111111";



        // Notifications
        public static string notificationTitle = " Ludo Wala";
        public static string notificationMessage = "Get your FREE fortune spin!";

        // Game configuration
        public static float WaitTimeUntilStartWithBots = -10.0f;// 0.9f; // Time in seconds. If after that time new player doesnt join room game will start with bots
        public static float WaitTimeUntilCloseRoom = 3000.0f;

        // Services configration IDS
        
        public static string PhotonAppID = "0f53cda7-8842-4845-9599-627e6d520b3d";
        public static string PhotonChatID = "7da6a71f-c767-40e2-9c97-a3c39451a233";

        // Admob Ads IDS
        public static string adMobAndroidID = "ca-app-pub-4216562452404378/8597103453";
        public static string adMobiOSID = "";

        // Facebook share variables
        public static string facebookShareLinkTitle = "I'm playing  Ludo Wala!. Available on Android and iOS.";

        // Share private room code
        public static string SharePrivateLinkMessage = "Join me in  Ludo Wala. My PRIVATE ROOM CODE is:";
        public static string SharePrivateLinkMessage2 = "Download  Ludo Wala from:http://ludowala.com";
        public static string ShareScreenShotText = "I finished game in  Ludo Wala. It's my score :-) Join me and download  Ludo Wala:";


        // Initial coins count for new players
        // When logged as Guest
        public static int initCoinsCountGuest = 15000;
        //When logged via Facebook
        public static int initCoinsCountFacebook = 20000;
        //When logged as Guest and then link to Facebook
        public static int CoinsForLinkToFacebook = 15000;

        // Unity Ads - reward coins count for watching video
        public static int rewardForVideoAd = 250;

        // Facebook Invite variables
        public static string facebookInviteMessage = "Come play this great game!";
        public static int rewardCoinsForFriendInvite = 250;
        public static int rewardCoinsForShareViaFacebook = 50;

        // String to add coins for testing - To add coins start game, click "Edit" button on your avatar and put that string
        // It will add 1 000 000 coins so you can test tables, buy items etc.
        public static string addCoinsHackString = "Cheat:AddCoins";



        // Hide Coins tab in shop (In-App Purchases)
        public static bool hideCoinsTabInShop = false;
        public static string runOutOfTime = "ran out of time";
        public static string waitingForOpponent = "Waiting for your opponent";

        // Other strings
        public static string youAreBreaking = "You start, good luck";
        public static string opponentIsBreaking = "is starting";
        public static string IWantPlayAgain = "I want to play again!";
        public static string cantPlayRightNow = "Can't play right now";

        // Players names for training mode
        public static string offlineModePlayer1Name = "Player 1";
        public static string offlineModePlayer2Name = "Player 2";

        // Photon configuration
        // Timeout in second when player will be disconnected when game in background
        public static float photonDisconnectTimeout = 60f; // In game scene - its better to don't change it. Player that loose focus on app will be immediately disconnected
        public static float photonDisconnectTimeout1 = 0.2f;
        public static float photonDisconnectTimeoutLong = 300.0f; // In menu scene etc. 

        // Bids Values
        public static int[] bidValues = new int[] { 10, 20, 50, 100, 250, 500, 1000, 2500 };
        public static string[] bidValuesStrings = new string[] {"10", "20", "50", "100", "250","500", "1000", "2500" };

        public static bool isFourPlayerModeEnabled = true;
        public static bool isThreePlayerModeEnabled = true;

        // Settings PlayerPrefs keys
        public static string SoundsKey = "EnableSounds";
        public static string VibrationsKey = "EnableVibrations";
        public static string NotificationsKey = "EnableNotifications";
        public static string FriendsRequestesKey = "EnableFriendsRequestes";
        public static string PrivateRoomKey = "EnablePrivateRoomRequestes";
        public static string PrefsPlayerRemovedAds = "UserRemovedAds";


        // Standard chat messages
        public static string[] chatMessages = new string[] {
            "Please don't kill",
            "Play Fast",
            "I will eat you",
            "You are good",
            "Well played",
            "Today is your day",
            "Hehehe",
            "Unlucky",
            "Thanks",
            "Yeah",
            "Remove Blockade",
            "Good Game",
            "Oops",
            "Today is my day",
            "All the best",
            "Hi",
            "Hello",
            "Nice move"
        };

        // Additional chat messages
        // Prices for chat packs
        public static int[] chatPrices = new int[] { 1000, 5000, 10000, 50000, 100000, 250000 };
        public static int[] emojisPrices = new int[] { 1000, 5000, 10000, 50000, 100000 };
        public static int[] emojimsg = new int[] {
           0,01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17
        };
        // Chat packs names
        public static string[] chatNames = new string[] { "Motivate", "Emoticons", "Cheers", "Gags", "Laughing", "Talking" };

        // Chat packs strings
        public static string[][] chatMessagesExtended = new string[][] {
            new string[] {
                "Never give up",
                "You can do it",
                "I know you have it in you!",
                "You play like a pro!",
                "You can win now!",
                "You're great!"
            },
            new string[] {
                ":)",
                ":(",
                ":o",
                ";D",
                ":P",
                ":|"
            },
            new string[] {
                "Keep it going",
                "Go opponents!",
                "Fabulastic",
                "You're awesome",
                "Best shot ever",
                "That was amazing",
            },
            new string[] {
                "OMG",
                "LOL",
                "ROFL",
                "O'RLY?!",
                "CYA",
                "YOLO"
            },
            new string[] {
                "Hahaha!!!",
                "Ho ho ho!!!",
                "Mwhahahaa",
                "Jejeje",
                "Booooo!",
                "Muuuuuuuhhh!"
            },
            new string[] {
                "Yes",
                "No",
                "I don't know",
                "Maybe",
                "Definitely",
                "Of course"
            }
        };

    }
}

