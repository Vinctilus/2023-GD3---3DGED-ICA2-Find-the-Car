using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGameScreen : MonoBehaviour
{
    [Header("GameManager")]
    [SerializeField]
    GameManager loc;
    [Header("Text Fields")]
    public TextMeshProUGUI timer;
    public TextMeshProUGUI score;

    void Update()
    {
       
        timer.SetText($"{((int)loc.timeLeft) / 60}:"+ ($"{((int)loc.timeLeft) % 60}").PadLeft(2, '0'));
        score.SetText(($"{loc.score}").PadLeft(8, '0'));
    }
}
