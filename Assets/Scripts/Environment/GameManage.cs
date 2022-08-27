using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using TMPro;
using System;
using UnityEngine.UI;


public class GameManage : MonoBehaviour
{
    private bool doesWait;
    public bool doesWaitRevive;
    public int time;
    public GameObject timerObject;
    public GameObject continueMenu;
    public GameObject ticket;
    private RectTransform continueMenuRectTransform;
    private TextMeshProUGUI timer;
    public PlayerMotor motor;
    public GameObject highestScore;
    private TextMeshProUGUI highestScoreText;
    public GameObject blackDiamonds;
    public GameObject ticketObject;
    public TextMeshProUGUI ticketText;
    private TextMeshProUGUI blackDiamondsText;
    public bool breakCorountine;
    public GameObject ContinueWithAdObject;
    public GameObject ContinueWithTicketObject;
    public GameObject playObject;
    public Image language;
    public Sprite turkish;
    public Sprite english;
    private TextMeshProUGUI playText;
    private TextMeshProUGUI continueWithAdText;
    private TextMeshProUGUI continueWithTicketText;
    public bool isShowingAd;
    public AudioSource deniedAudio;
    public AdsInitializer ads;
    [SerializeField] private int forcedAdAfterDeath;

  
    private void Awake()
    {
        highestScoreText = highestScore.GetComponent<TextMeshProUGUI>();
        blackDiamondsText = blackDiamonds.GetComponent<TextMeshProUGUI>();
        ticketText = ticketObject.GetComponent<TextMeshProUGUI>();
        continueWithAdText = ContinueWithAdObject.GetComponent<TextMeshProUGUI>();
        continueWithTicketText = ContinueWithTicketObject.GetComponent<TextMeshProUGUI>();
        playText = playObject.GetComponent<TextMeshProUGUI>();
        highestScoreText.text = PlayerPrefs.GetInt("highestScore").ToString();
        blackDiamondsText.text = PlayerPrefs.GetInt("blackDiamonds").ToString();
        ticketText.text = PlayerPrefs.GetInt("ticket").ToString();
         if(PlayerPrefs.GetInt("flag")==1)  English();
        else Turkish();
   
    }
    private void Start()
    {
        timer = timerObject.GetComponent<TextMeshProUGUI>();
        continueMenuRectTransform = continueMenu.GetComponent<RectTransform>();
       
    }

    private void Update()
    {
        if(!motor.isAlive || !motor.isRunning)
        {
            if(PlayerPrefs.GetInt("ticket") < 1) // if there is no tickets remove the use tickets to continue button
        {
            continueMenuRectTransform.sizeDelta = new Vector2(600,300); // adjust the background
            ticket.SetActive(false);
        }
        if (doesWait == false && time > 0 && motor.deathMenu.activeSelf && !isShowingAd)
        {
            doesWait = true;
            StartCoroutine(Timer());
            Time.timeScale =0;
            motor.backGroundAudio.Pause();
        }
       
        
        if(time ==0)
        {
            OnDeath();
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");

        }

        }
         if (doesWaitRevive == false && motor.reviveTime> 0 && motor.reviveScreen.activeSelf )
        {
            doesWaitRevive = true;
            StartCoroutine(ReviveTimer());
        }
        if(motor.reviveTime == 0 && !motor.reviveScreen.activeSelf)
        {
            motor.isRunning = true;
            motor.isAlive = true;
            motor.reviveTime = 4;
            time = 6;
            Time.timeScale =1;
            motor.backGroundAudio.Play();
        }
        
        
    }
   

    public void OnDeath()
    {
       if(motor.score > PlayerPrefs.GetInt("highestScore"))
       {
           PlayerPrefs.SetInt("highestScore",motor.score);
           highestScoreText.text = (motor.score).ToString();
       }
       PlayerPrefs.SetInt("blackDiamonds",PlayerPrefs.GetInt("blackDiamonds")+ motor.blackDiamondCount);
    // FORCED ADS
        PlayerPrefs.SetInt("deathCount",PlayerPrefs.GetInt("deathCount")+1);
        ads.ShowForcedAd();
         
    }
    public void OnBuyTicket()
    {
        int countBD = PlayerPrefs.GetInt("blackDiamonds");
        if( countBD >= 301)
        {
            PlayerPrefs.SetInt("blackDiamonds", countBD-301);
            PlayerPrefs.SetInt("ticket",PlayerPrefs.GetInt("ticket")+1);
            blackDiamondsText.text = PlayerPrefs.GetInt("blackDiamonds").ToString();
            ticketText.text = PlayerPrefs.GetInt("ticket").ToString();
        }
        else
        {
            deniedAudio.Play();
        }
    }
    public void Turkish()
    {
        language.sprite = turkish;
        playText.text = "OYNA";
        continueWithAdText.text = "Devam Etmek İçin\nReklam İzle";
        continueWithTicketText.text = "Devam Etmek İçin\nBilet Kullan";       
    }
    public void English()
    {
        language.sprite = english;
        playText.text ="PLAY";
        continueWithAdText.text = "Continue\nBy Playing Ad";
        continueWithTicketText.text = "Continue\nBy Using Ticket";

    }

    public void OnFlag()
    {
        if(PlayerPrefs.GetInt("flag")==1) 
        {
            PlayerPrefs.SetInt("flag",0);
            Turkish();
        }
        else
        {
            PlayerPrefs.SetInt("flag",1);
            English();
        }

    }
    
    private IEnumerator Timer()
    {
        
        time--;  
        timer.text = time.ToString();
        yield return new WaitForSecondsRealtime(1);
        doesWait = false;

    }
    private IEnumerator ReviveTimer()
    {

            motor.reviveTime--;
            motor.reviveTimerText.text = motor.reviveTime.ToString();
            yield return new WaitForSecondsRealtime(1);
            doesWaitRevive = false;
            if (motor.reviveTime == 0) motor.reviveScreen.SetActive(false);

        
            
            
            
        
    } 

}

