using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoMovesDotsManager : MonoBehaviour
{
    public GameObject[] greenDots;
    public Text MoveCountText;

    private void Update()
    {
        greenDots[0].SetActive(EventCounter.autoMovesCount >= 5);
        greenDots[1].SetActive(EventCounter.autoMovesCount >= 4);
        greenDots[2].SetActive(EventCounter.autoMovesCount >= 3);
        greenDots[3].SetActive(EventCounter.autoMovesCount >= 2);
        greenDots[4].SetActive(EventCounter.autoMovesCount >= 1);

        MoveCountText.text = "Auto Moves Left: " + EventCounter.autoMovesCount;
    }
}
