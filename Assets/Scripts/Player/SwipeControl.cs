using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControl : MonoBehaviour
{
    // MEMBER VARIABLES
     private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown,isDragging;
     private Vector2 startTouch,swipeDelta;
     [SerializeField] GameObject introScreen;
     [SerializeField] GameObject reviveScreen;
     // INTERFACE
     public Vector2 SwipeDelta{get { return swipeDelta;}}
     public bool Tap {get {return tap;}}
     public bool SwipeLeft {get {return swipeLeft;}}
     public bool SwipeRight {get {return swipeRight;}}
     public bool SwipeDown {get {return swipeDown;}}
     public bool SwipeUp {get {return swipeUp;}}

     private void Update()
     {
        if(!introScreen.activeSelf && !reviveScreen.activeSelf)
        {
             tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
        // COMPUTER INPUTS
        if(Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDragging = true;
            startTouch = Input.mousePosition;

        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging =false;
            Reset();
        }
        // MOBILE INPUTS
        if(Input.touches.Length > 0)
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDragging = true;
                startTouch = Input.touches[0].position;
            }
            else if(Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDragging =false;
                Reset();
            }
        }
        // CALCULATE DISTANCE
        if(isDragging)
        {
            if(Input.touches.Length > 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }
        // DID WE SWIPE LARG ENOUGH ?
        if(swipeDelta.magnitude > 50)
        {
            // WHICH DIRECTION ?
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if(Mathf.Abs(x) > Mathf.Abs(y))
            {
                // LEFT
                if( x<0)
                   { swipeLeft = true;
                    Debug.Log("swipeLeft");}
                //RIGHT
                else
                    {swipeRight = true;
                    Debug.Log("swipeRight");}
            }
            else
            {
                // DOWN
                if( y<0)
                    {swipeDown = true;
                    Debug.Log("swipeDown");}
                //UP
                else
                    {swipeUp = true;
                    Debug.Log("swipeUp");}

            }
            Reset();    
        }}
     
     }
        
     private void Reset()
     {
         startTouch = swipeDelta = Vector2.zero;
         isDragging = false;
     }     
}
