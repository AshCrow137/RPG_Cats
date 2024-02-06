using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class PlayerMovement : Movement
    
{
    public bool Touch = false;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        //
        animator = GetComponent<Animator>();
        forwardObject = GetComponentInChildren<ForwardObjectScript>();
        if (forwardObject == null)
        {
            Debug.LogError($"No forward object attached to {this.name}"); 
        }
        animationScript = GetComponent<AnimationScript>();

    }
    protected override void Update()
    {
        if (Touch )
        {
            movement.x = FloatingJoystick.Instance.Horizontal;
            movement.y = FloatingJoystick.Instance.Vertical;
        }
        else
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");
        }
       if (animationScript != null) {
            animationScript.UpdateMovement(movement.x, movement.y, movement.sqrMagnitude);

        }
       

       
    }
    protected override void FixedUpdate()
    {
        //if (movement == Vector2.zero)
        //{
        //    base.FixedUpdate();
        //}

        //rb.MovePosition(rb.position + movement.normalized*speed*Time.fixedDeltaTime);
       
        if (canMove&& movement!=Vector2.zero) {
            setTarget(null);
            //rb.velocity = new Vector2(movement.x, movement.y) * speed;
            rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
            //forwardObject.transform.localPosition = new Vector3(movement.x, movement.y, 0);
            float angle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;
            forwardObject.transform.eulerAngles = new Vector3(0, 0, -angle);
            
        }
        else
        {
            base.FixedUpdate();
        }

    }
}
