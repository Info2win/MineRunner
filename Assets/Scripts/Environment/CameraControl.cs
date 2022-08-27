using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform lookAt; // We are looking at to the Miner
    [SerializeField] private Vector3 offset ;
    [SerializeField] private float MAX_HEIGHT;
    [SerializeField] [Range(0.01f,1f)] public float SMOOTH_SPEED;
    public PlayerMotor motor;
    private Vector3 velocity = Vector3.zero;


    private void Start()
    {
        Vector3 desiredPostion = lookAt.position+offset;
        // transform.position = Vector3.Lerp(transform.position,desiredPostion,SMOOTH_SPEED);
        transform.position = desiredPostion;
    }

    private void LateUpdate()
    {
        if(motor.isAlive == false || motor.isRunning == false)  offset = new Vector3(0,7,-9);
        if(motor.isAlive == true && motor.isRunning == true)  offset = new Vector3(0,5,-4.5f);
        Vector3 desiredPostion = lookAt.position+offset;
        if(desiredPostion.y > MAX_HEIGHT) desiredPostion.y = MAX_HEIGHT;
        //transform.position = Vector3.Lerp(transform.position,desiredPostion,SMOOTH_SPEED);
        transform.position = Vector3.SmoothDamp(transform.position,desiredPostion,ref velocity,SMOOTH_SPEED);
    }
}
