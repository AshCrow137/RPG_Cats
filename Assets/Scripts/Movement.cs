using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Movement : MonoBehaviour
{
    
    protected static Rigidbody2D rb;
    protected static Animator animator;
    protected static ForwardObjectScript forwardObject;
    protected static AnimationScript animationScript;
    protected bool canMove = true;

    protected Vector2 movement;

    //Movement parameters
    [SerializeField]
    protected float speed;

    protected Transform target;
    private void Awake()
    {
        forwardObject = GetComponentInChildren<ForwardObjectScript>();
        if (forwardObject== null)
        {
            Debug.LogError($"No forward object attached to {this.name}");
        }
    }
    protected virtual void Update()
    {

    }
    protected virtual void FixedUpdate()
    {
        if (canMove && target!= null) 
        {
            transform.position = Vector3.MoveTowards(transform.position,target.position,speed* Time.deltaTime);
        }
    }
    public ForwardObjectScript GetForwardObject()
    {

        return forwardObject;
    }
    public float getSpeed()
    {
        return speed;
    }
    public void changeSpeed(float amount)
    {
        speed += amount;
    }
    public void ChangeMovementPossibility(bool value)
    {
        canMove = value;
    }
    public bool GetMovementPossibility()
    {
        return canMove;
    }
    public float GetCurrentSpeed()
    {
        return movement.sqrMagnitude;
    }
    public Vector2 getMovement()
    {
        return movement;
    }
    public void setTarget(Transform newtarget)
    {
        target = newtarget;

       
       
    }
}
