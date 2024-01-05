using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIendresuolt : MonoBehaviour
{
    [SerializeField]
    gameloic loc;
    public TextMeshProUGUI highscore;
    public TextMeshProUGUI score;

    void Update()
    {
        score.SetText(($"{loc.score}").PadLeft(8, '0'));
        highscore.SetText(($"{PlayerPrefs.GetInt("Hiscore")}").PadLeft(8, '0'));


    }
}
