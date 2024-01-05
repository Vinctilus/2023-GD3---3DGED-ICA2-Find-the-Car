using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuClickEvents : MonoBehaviour
{
    [Header("GameManager")]
    [SerializeField]
    gameloic gamemanager;

    public void StartGame()
    {
        gamemanager.Startgame();
    }
    public void PouseGame() 
    {
        gamemanager.gotoPouse(); 
    }

    public void ResumeGame()
    {
        gamemanager.ResumeGame();
    }

    public void OpenSettings() 
    {
        gamemanager.gotoSetting();
    }

    public void CloseSettings()
    {
        gamemanager.exitSetting();
    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
