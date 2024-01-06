using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuClickEvents : MonoBehaviour
{
    [Header("GameManager")]
    [SerializeField]
    GameManager gamemanager;

    public void StartGame()
    {
        gamemanager.Startgame();
    }
    public void PouseGame() 
    {
        gamemanager.GotoPause(); 
    }

    public void ResumeGame()
    {
        gamemanager.ResumeGame();
    }

    public void OpenSettings() 
    {
        gamemanager.GotoSetting();
    }

    public void CloseSettings()
    {
        gamemanager.ExitSetting();
    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
