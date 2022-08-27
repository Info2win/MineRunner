using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMotor : MonoBehaviour
{
    public SwipeControl swipeControls;
    private CharacterController controller;
    public HandleAnimation animator;
    private const float LANE_DISTANCE = 4.0f;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    public float slidingTime;
    private float verticalVelocity;
    public float speed ;
    public float rightLeftSpeed;
    private int desiredLane = 1; // 0 = LeftLane, 1 = Mid , 2 = Right
    public bool isRunning = true;
    private Transform trans;
    public float health;
    public float healthReduce;
    public float gainHealth;
    public bool isAlive =true;
    private Vector3 moveVector;
    public GameObject deathMenu;
    public GameManage manage;
    private bool doesWait=false;
    public int score;
    public int blackDiamondCount=0;
    public AudioSource backGroundAudio;
    public AudioSource moveAudio;
    public GameObject introScreen;
    public GameObject GAMEUI;
    public GameObject play;
    public TextMeshProUGUI playButtonText;
      private Vector3 reviveOffSet= new Vector3(0,0,20);
      public int reviveTime=4;
    public GameObject reviveTimerObject;
    public TextMeshProUGUI reviveTimerText;
    public GameObject reviveScreen;
    Vector3 targetPostion;
    private Collider charCollider;
 



    private void Start()
    {
       controller = GetComponent<CharacterController>();
       Time.timeScale =0;
       playButtonText = play.GetComponent<TextMeshProUGUI>();
       reviveTimerText = reviveTimerObject.GetComponent<TextMeshProUGUI>();
       
    }
    private void Update()
    {

        if(isRunning && isAlive)

        {
           
            if(!reviveScreen.activeSelf) GAMEUI.SetActive(true);
            if (swipeControls.SwipeLeft )
        {
               if(desiredLane !=0) moveAudio.Play();
                MoveLane(false);
            
        }
        if(swipeControls.SwipeRight )
           {
               if(desiredLane != 2) moveAudio.Play();
                
                MoveLane(true);
           }
        if(swipeControls.SwipeUp && isGrounded() )
            {
               animator.anim.SetTrigger("Jump");
            }
        if(swipeControls.SwipeDown)
        {
        }
        // Calculate  where we should be in the future
         targetPostion = transform.position.z * Vector3.forward;
        if(desiredLane ==0)
            targetPostion += Vector3.left * LANE_DISTANCE;
        else if (desiredLane == 2)
        targetPostion += Vector3.right  * LANE_DISTANCE;

        // Calculate move delta
        moveVector = Vector3.zero;
        moveVector.x = (targetPostion -transform.position).normalized.x *rightLeftSpeed;
        moveVector.z = speed;

        // Calculate Y
        if(isGrounded()) // if Grounded
        {
            verticalVelocity = 0.0f;
            if(swipeControls.SwipeUp)
            {
                    moveAudio.Play();
                verticalVelocity = jumpForce;
            }
            
            else if(swipeControls.SwipeDown)
            {
                    moveAudio.Play();
                    StartSliding();
                Invoke("StopSliding",slidingTime);
                

            }   
        }
        else
        {
            
            verticalVelocity -= (gravity* Time.deltaTime);
            // Fast Falling if swiped down while jumping
             if(swipeControls.SwipeDown)
            {
                    moveAudio.Play();
                    verticalVelocity = -jumpForce*2;
            }
        }
        moveVector.y = verticalVelocity;
        

        //Move the Miner
        controller.Move(moveVector*Time.deltaTime);
        

        ControlHealth();
        }
        

        
        

    // END OF THE UPDATE
    }
    public void OnReviveWithAdd()
    {
        Time.timeScale = 1;
        deathMenu.SetActive(false);
            reviveScreen.SetActive(true);
            Revive();
        
        
    }
    public void OnReviveWithTicket()
    {

        Time.timeScale = 1;
        deathMenu.SetActive(false);
            reviveScreen.SetActive(true);
            PlayerPrefs.SetInt("ticket",PlayerPrefs.GetInt("ticket")-1);
            manage.ticketText.text = PlayerPrefs.GetInt("ticket").ToString();
            Revive();
         
             
    }
    public void Revive()
    {
            backGroundAudio.Play();
            if(!isAlive)
            {
                reviveOffSet = Vector3.zero;
                health = 100;
            }
            Vector3 revivePosition = transform.position - reviveOffSet;
            revivePosition.y = -0.4440323f;
            if(desiredLane == 0) revivePosition.x = -3.8f;
            if(desiredLane == 1) revivePosition.x = 0.2f;
            if(desiredLane == 2) revivePosition.x = 3.8f;
            Debug.Log(reviveOffSet);
            gameObject.transform.position = revivePosition;
            controller.transform.position = revivePosition;
             animator.anim.SetTrigger("revive");
            
           
             
            



    }

    public void OnPlay()
    {
        introScreen.SetActive(false);
        reviveScreen.SetActive(true);
        GAMEUI.SetActive(false);
        
         if (manage.doesWaitRevive == false && reviveTime> 0 && reviveScreen.activeSelf )
        {
            manage.doesWaitRevive = true;
            StartCoroutine(ReviveTimer());
        }
                 
    }
    
    public void OnPause()
    {
        Time.timeScale =0;
        backGroundAudio.Pause();
        introScreen.SetActive(true);
        if(PlayerPrefs.GetInt("flag")==1) playButtonText.text = "CONTINUE";
        else playButtonText.text = "DEVAM ET";
    }
    
     private void StartSliding()
        {
            
            controller.height /= 4;
            controller.center = new Vector3(controller.center.x,controller.center.y/8,controller.center.z);
            animator.StartSliding();
        }
     private void StopSliding()
        {
            
            controller.height *=4 ;
            controller.center = new Vector3(controller.center.x,controller.center.y *8,controller.center.z);
            animator.StopSliding();
            
        }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Barrier":
            case "Rock":
            case "MineCartTop":
            case "MineCartBottem":
                Crash();
                break;
        }

    }

    private void Die()
    {
        deathMenu.SetActive(true);
    }

    
    private void Crash()
    {
        GAMEUI.SetActive(false);
        isRunning = false;
        animator.Crash();
         if(doesWait == false)
        {
            doesWait = true;
            StartCoroutine(WaitForFloatSeconds(3.333f)); // wait until the crash animation is done
        }
        
    }
    private void MoveLane(bool goingRight)
    {
        // Left
        if(!goingRight)
        {
            desiredLane--;
            if(desiredLane ==-1) desiredLane =0;
        }
        else
        {
            desiredLane++;
            if(desiredLane==3) desiredLane =2;
        }
            
        
    }
    public bool isGrounded()
    {
        Ray groundRay = new Ray (new Vector3(controller.bounds.center.x,controller.bounds.center.y-controller.bounds.extents.y+0.2f,controller.bounds.center.z),Vector3.down);
        return (Physics.Raycast(groundRay,0.2f+0.1f)); // if grounded return true otherwise false 
    }
    private void ControlHealth()
    {
        health  = health - (healthReduce*Time.deltaTime);
        if(health <= 0) 
        {
            GAMEUI.SetActive(false);
            isAlive = false;
            isRunning = false;
            animator.diedbyGas();
            if(doesWait == false)
        {
            doesWait = true;
            StartCoroutine(WaitForFloatSeconds(3.333f)); // wait until the diedbyGas animation is done
        }
        }
        if(health > 100) health =100;
    }
    private IEnumerator WaitForFloatSeconds(float floatTimes)
    {
        yield return new WaitForSeconds(floatTimes);
        doesWait = false;
        Die();
    }
    private IEnumerator ReviveTimer()
    {

            reviveTime--;
            reviveTimerText.text = reviveTime.ToString();
            yield return new WaitForSecondsRealtime(1);
            manage.doesWaitRevive = false;
          
    }
    
    
    

    
}
