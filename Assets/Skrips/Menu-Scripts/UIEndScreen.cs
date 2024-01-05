using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEndScreen : MonoBehaviour
{
    [Header("GameManager")]
    [SerializeField]
    gameloic gamemanager;
    [Header("Text Fields")]
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI scoreText;

    void Update()
    {
        scoreText.SetText(($"{gamemanager.score}").PadLeft(8, '0'));
        highscoreText.SetText(($"{PlayerPrefs.GetInt("Hiscore")}").PadLeft(8, '0'));


    }
}
