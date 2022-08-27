using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveForward : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3;
    private Transform target;
    

private void Start()
{
    target = gameObject.transform;
}
    private void Update()
    {
        target.position += Vector3.forward*moveSpeed*Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,target.position,moveSpeed*Time.deltaTime);
    }
}
