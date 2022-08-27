using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [SerializeField] private float jumpHeight = 1.0f;

    private Transform target;
    public SwipeControl swipeControls;

    public Animator animator;

    
    
     void Update()
    {
        target = gameObject.transform;
        bool isLeftStepping = animator.GetBool("isLeftStepping");
        bool isRightStepping = animator.GetBool("isRightStepping");
        bool isJumping = animator.GetBool("isJumping");
        bool isSliding = animator.GetBool("isSliding");
        
        if(swipeControls.SwipeLeft)
        {   
            target.position += Vector3.left*4;
            
            if(!isLeftStepping)
            {   
                animator.SetBool("isLeftStepping",true);
               // gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,target.position,4);
               StartCoroutine();
                animator.SetBool("isLeftStepping",false);
            }

            
        }
        if(swipeControls.SwipeRight)
           { 
               target.position += Vector3.right*4;
               gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,target.position,4);
               if(!isRightStepping)
               animator.SetBool("isRightStepping",true);
           }
        if(swipeControls.SwipeUp)
            {
                target.position += Vector3.up*jumpHeight;
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,target.position,jumpHeight);
                if(!isJumping)
                animator.SetBool("isJumping",true);
            }
        if(swipeControls.SwipeDown)
        {
            target.position = new Vector3(gameObject.transform.position.x,0,gameObject.transform.position.z);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,target.position,gameObject.transform.position.y-0);
            if(!isSliding)
            animator.SetBool("isSliding",true);
        }
            

        
    }

    IEnumerator StartCoroutine()
    {
        yield return new WaitForSeconds(2);
    }
    
}
