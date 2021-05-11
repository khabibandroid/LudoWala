using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [Header("Item")]
    public GameObject itemParentplayTournamentss;
    public GameObject itemPlayTournamentss;

    [Header("Position 1")]

    public Text Name1;
    public Text Position1;
    public Text GameName;
    public Text Wining1;
    public Text joining1;

    [Header("Position 2")]

    public Text Name2;
    public Text Position2;    
    public Text Wining2;
    public Text joining2;


    [Header("Position 3")]

    public Text Name3;
    public Text Position3;  
    public Text Wining3;
    public Text joining3;


    [Header("Position 4")]

    public Text Name4;
    public Text Position4;  
    public Text Wining4;
    public Text joining4;


    public static Result instance;

    void Start()
    {
        instance = this;

    }


    public void result()
    {
        

    }
   


}
