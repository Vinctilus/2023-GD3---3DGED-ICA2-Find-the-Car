using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;


public class gameloic : MonoBehaviour
{
    [Header("UI")]
    public float timeleft;
    public float MAXTIME;
    public GameObject AvieUI;
    public GameObject MainMenu;
    public GameObject InGame;
    public GameObject Pause;
    public GameObject End;
    public GameObject Setting;
    [Header("Gameobjecs")]
    public Carsmanager carmanager;
    public List<CarRealtions> Cars;
    public List<Dificulty> Dificultys;

    [Header("Socre")]
    public float timeplus = 0;
    public float timeminus = 0;
    public int score = 0;
    public int highscore = 0;

    public inputmanager controls;

    [Header("Audio")]
    public AudioManager Audiomanger;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AvieUI.activeSelf == false) { AvieUI.SetActive(true); }

        if(AvieUI== InGame) { inGame(); } else { if (InGame.activeSelf) { InGame.SetActive(false); controls.enabled = false; } }


        if (AvieUI != MainMenu && MainMenu.activeSelf) { MainMenu.SetActive(false); }
        if (AvieUI != Pause && Pause.activeSelf) { Pause.SetActive(false); }
        if (AvieUI != End && End.activeSelf) { End.SetActive(false); }

    }

    public void Startgame()
    {
        score = 0;
        timeleft = MAXTIME;
        nextlevle();
        ResumeGame();
    }

    public void ResumeGame()
    {
        carmanager.carssetactive(true);
        controls.enabled = true;
        AvieUI = InGame;
    }

    public void gotoSetting()
    {
       
        Setting.SetActive(true);

    }
    public void exitSetting()
    {
        
        Setting.SetActive(false);
    }

    public void gotoEnd()
    {
        highscore = Mathf.Max(score, PlayerPrefs.GetInt("Hiscore",0));
        if (highscore != PlayerPrefs.GetInt("Hiscore",0))
        {
            PlayerPrefs.SetInt("Hiscore", highscore);
            PlayerPrefs.Save();
        }
        AvieUI = End;
   
        carmanager.carssetactive(false);
    }

    public void gotoPouse()
    {
        AvieUI = Pause;
        carmanager.carssetactive(false);
    }


    private void inGame() 
    {
        if (timeleft > 0) { timeleft -= Time.deltaTime; }else { timeleft = 0; gotoEnd();}

    }

    private void nextlevle()

    {
        int rendomcar = Random.Range(0, (Cars.Count-1));
        int rendomdify = Random.Range(0, (Dificultys.Count - 1));
        carmanager.creatfield(Cars[rendomcar], Dificultys[rendomdify], Random.Range(90, 100));
    }

    public void chechifhasscord(bool CILKobj)
    {
        if (CILKobj)
        {
            score += 1;
            timeleft += timeplus;
            timeleft = Mathf.Min(timeleft,MAXTIME);
            nextlevle();
            Audiomanger.playSFX(Audiomanger.correctSound);
        }
        else
        {
            timeleft -= timeminus;
            Audiomanger.playSFX(Audiomanger.wrongSound);
        }
    }
}
