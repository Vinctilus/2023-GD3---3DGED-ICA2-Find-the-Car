using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIingame : MonoBehaviour
{
    [SerializeField]
    gameloic loc;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI score;

    void Update()
    {
       
        timer.SetText($"{((int)loc.timeleft) / 60}:"+ ($"{((int)loc.timeleft) % 60}").PadLeft(2, '0'));
        score.SetText(($"{loc.score}").PadLeft(8, '0'));
    }
}
