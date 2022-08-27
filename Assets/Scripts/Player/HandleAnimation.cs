using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAnimation : MonoBehaviour
{
    [SerializeField]private SwipeControl swipeControls;
    private CharacterController controller;
    [SerializeField] private PlayerMotor motor;
    public Animator anim;
    public AudioSource crash;
    public AudioSource posined;
    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();    
    }

    // Update is called once per frame
    private void Update()
    {
        bool Grounded = motor.isGrounded();
        anim.SetBool("isGrounded",Grounded);
        if(motor.isGrounded()) // if Grounded
        {

            if(swipeControls.SwipeUp)
            {
               // anim.SetTrigger("Jump");

            }
            else if (swipeControls.SwipeDown)
            {
                //StartSliding();
                //Invoke("StopSliding",motor.slidingTime);
            }
        }

        

    }
    public void diedbyGas()
    {
        posined.Play();
        anim.SetTrigger("diedbyGas");
    }
    public void Crash()
    {
        crash.Play();
        anim.SetTrigger("Death");
    }
    public void StartSliding()
        {
            anim.SetBool("isSliding",true);
        }
     public void StopSliding()
        {
            anim.SetBool("isSliding",false);
        }
 
}
