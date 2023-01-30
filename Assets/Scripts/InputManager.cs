using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void GemTapped(Gem gem);
    public delegate void Swipe(SwipeDirection swipeDirection);

    public static Swipe OnSwipe;
    public static GemTapped OnGemTapped;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;

    [SerializeField] private float swipeThreshold;
    
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            touchEndPos = Input.mousePosition;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
                Single.NegativeInfinity, LayerMask.GetMask("Gem"));
            if (hit != null)
            {
                Debug.Log(hit.transform.name);
                OnGemTapped(hit.transform.GetComponent<Gem>());
            }
        }

        // if (Input.GetMouseButton(0))
        //     touchEndPos = Input.mousePosition;
        //
        // if (Input.GetMouseButtonUp(0))
        // {
        //     touchEndPos = Input.mousePosition;
        //     DetectSwipe();
        // }
    }

    private void DetectSwipe()
    {
        Vector2 dragVector = touchEndPos - touchStartPos;
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        SwipeDirection swipeDirection;
        if (positiveX > positiveY && positiveX > swipeThreshold)
        {
            swipeDirection = (dragVector.x > 0) ? SwipeDirection.Right : SwipeDirection.Left;
            OnSwipe(swipeDirection);
        }
        else if (positiveY > positiveX && positiveY > swipeThreshold)
        {
            swipeDirection = (dragVector.y > 0) ? SwipeDirection.Up : SwipeDirection.Down;
            OnSwipe(swipeDirection);
        }
    }
    
    public enum SwipeDirection
    {
        Up,
        Down,
        Right,
        Left,
        
    }
}
