using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNewLevel : MonoBehaviour
{
    public float GAME_SPEED = 1;
    [SerializeField] private float GAME_SPEED_UP_AMOUNT;
    public float GAME_SPEED_UP_AFTER_SECONDS;
    public GameObject[] mines;
    public PlayerMotor motor;
    public HandleAnimation animator;
    public GameManage manage;
    private float zPos = 137.9f;
    private bool doesCreateSection = false;
    private bool doesSpeedUp = false;
    private int mineNum;
    [SerializeField] private float afterSecondsCreateNewLevel ;
    
    private void Start()
    {
        motor.gainHealth = 6 * GAME_SPEED ;
        motor.slidingTime = 1.32f / GAME_SPEED;
        motor.speed =  7 * GAME_SPEED;
        motor.rightLeftSpeed = 10 * GAME_SPEED;
        motor.healthReduce = 2 * GAME_SPEED;
        animator.anim.SetFloat("speed",GAME_SPEED);
        animator.anim.SetFloat("runningSpeed",GAME_SPEED);
        afterSecondsCreateNewLevel = 15 / GAME_SPEED;
    }
    private void Update()
    {
        if(doesCreateSection == false && motor.isRunning)
        {
            doesCreateSection = true;
            StartCoroutine(GenerateMine(afterSecondsCreateNewLevel));
        }
        if(doesSpeedUp == false && motor.isRunning  )
        {
            doesSpeedUp = true;
            StartCoroutine(SpeedUp());
        }
        

    }
    private IEnumerator SpeedUp()
    {
        GAME_SPEED += GAME_SPEED_UP_AMOUNT;
        motor.gainHealth = 6 * GAME_SPEED ;
        motor.slidingTime = 1.32f / GAME_SPEED;
        motor.speed =  7 * GAME_SPEED;
        if (GAME_SPEED < 2.4f ) motor.rightLeftSpeed = 10 * GAME_SPEED;
        motor.healthReduce = 2 * GAME_SPEED;
        afterSecondsCreateNewLevel = 15 / GAME_SPEED;
        if(GAME_SPEED < 2.4f) animator.anim.SetFloat("runningSpeed",GAME_SPEED);
        animator.anim.SetFloat("speed",GAME_SPEED);
        yield return new WaitForSeconds(GAME_SPEED_UP_AFTER_SECONDS);

         if (GAME_SPEED > 6)
        {
            
            GAME_SPEED_UP_AMOUNT = 0.01f;
            
        }
        else if (GAME_SPEED > 5)
        {
            GAME_SPEED_UP_AMOUNT = 0.03f;

        }
        
        else if(GAME_SPEED > 4)
        {
            GAME_SPEED_UP_AMOUNT = 0.06f;

        }
        else if (GAME_SPEED > 3)
        {
            GAME_SPEED_UP_AMOUNT = 0.09f;

        }
        else if(GAME_SPEED > 2)
        {
            GAME_SPEED_UP_AMOUNT = 0.12f;
      
        }
        
        doesSpeedUp = false;
    }
    private IEnumerator GenerateMine(float seconds)
    {
        mineNum = Random.Range(0,5);
        Instantiate(mines[mineNum], new Vector3(0,0,zPos),Quaternion.identity);
        zPos += 137.9f;
        yield return new WaitForSeconds(seconds);
        doesCreateSection = false;
    } 

    
    
}
