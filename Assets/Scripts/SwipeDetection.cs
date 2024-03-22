using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeDetection : MonoBehaviour
{
    public static SwipeDetection instance;
    public delegate void Swipe(Vector2 direction);
    public event Swipe swipePerformed;
    [SerializeField] private InputAction position, press;
    [SerializeField] private float swipeThreashold;
    private Vector2 inititialPos;
    private Vector2 currentPos => position.ReadValue<Vector2>();

    private void Awake()
    {
        position.Enable();
        press.Enable();
        press.performed += _ => { inititialPos = currentPos; };
        press.canceled += _ => DetectSwipe();
        instance = this;
    }

    private void DetectSwipe()
    {
        Vector2 delta = currentPos - inititialPos;
        Vector2 direction = Vector2.zero;
        //to use for strict direction
        /*if((Mathf.Abs(delta.x)) > swipeThreashold)
        {
            direction.x = Mathf.Clamp(delta.x, -1, 1);
            
        }
        if ((Mathf.Abs(delta.y)) > swipeThreashold)
        {
            direction.y = Mathf.Clamp(delta.y, -1, 1);
        }*/
        if (direction != Vector2.zero && swipePerformed != null)
            swipePerformed(direction);
    }
}
