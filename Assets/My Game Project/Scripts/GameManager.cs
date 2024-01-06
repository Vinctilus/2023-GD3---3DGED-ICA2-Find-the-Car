using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public float timeLeft;
    public float maxTime = 120f;
    public GameObject avieUI;
    public GameObject MainMenu;
    public GameObject InGame;
    public GameObject Pause;
    public GameObject End;
    public GameObject Setting;
    [Header("Gameobjecs")]
    public TrafficManager trafficmanager;
    public DayCycle dayCycle;
    public List<CarRelationConnections> cars;
    public List<DifficultyCurve> difficulties;

    [Header("Socre")]
    public float timePlus = 0;
    public float timeMinus = 0;
    public int score = 0;
    public int highscore = 0;

    public InputManager inputmanager;

    [Header("Audio")]
    public AudioManager audiomanager;




    void Update()
    {
        if (avieUI.activeSelf == false) { avieUI.SetActive(true); }

        if(avieUI== InGame) { InGameLogic(); } else { if (InGame.activeSelf) { InGame.SetActive(false); inputmanager.enabled = false; } }


        if (avieUI != MainMenu && MainMenu.activeSelf) { MainMenu.SetActive(false); }
        if (avieUI != Pause && Pause.activeSelf) { Pause.SetActive(false); }
        if (avieUI != End && End.activeSelf) { End.SetActive(false); }

    }

    public void Startgame()
    {
        score = 0;
        timeLeft = maxTime;
        NextLevel();
        ResumeGame();
        dayCycle.timerIsActiv =true;
        dayCycle.GameStart();
    }

    public void ResumeGame()
    {
        trafficmanager.CarsSetActive(true);
        inputmanager.enabled = true;
        avieUI = InGame;
        dayCycle.timerIsActiv=true;
    }

    public void GotoSetting()
    {
       
        Setting.SetActive(true);

    }
    public void ExitSetting()
    {
        
        Setting.SetActive(false);
    }

    public void GotoEnd()
    {
        highscore = Mathf.Max(score, PlayerPrefs.GetInt("Hiscore",0));
        if (highscore != PlayerPrefs.GetInt("Hiscore",0))
        {
            PlayerPrefs.SetInt("Hiscore", highscore);
            PlayerPrefs.Save();
        }
        avieUI = End;
   
        trafficmanager.CarsSetActive(false);
        dayCycle.timerIsActiv = false;
    }

    public void GotoPause()
    {
        avieUI = Pause;
        trafficmanager.CarsSetActive(false);
        dayCycle.timerIsActiv = false;
    }


    private void InGameLogic() 
    {
        if (timeLeft > 0) { timeLeft -= Time.deltaTime; }else { timeLeft = 0; GotoEnd();}

    }

    private void NextLevel()

    {
        int rendomcar = Random.Range(0, (cars.Count-1));
        int rendomdify = Random.Range(0, (difficulties.Count - 1));
        trafficmanager.creatfield(cars[rendomcar], difficulties[rendomdify], Random.Range(90, 100));
    }

    public void HitOnCar(bool CILKobj)
    {
        if (CILKobj)
        {
            score += 1;
            timeLeft += timePlus;
            timeLeft = Mathf.Min(timeLeft,maxTime);
            NextLevel();
            audiomanager.PlaySFX(audiomanager.correctSound);
        }
        else
        {
            timeLeft -= timeMinus;
            audiomanager.PlaySFX(audiomanager.wrongSound);
        }
    }
}
