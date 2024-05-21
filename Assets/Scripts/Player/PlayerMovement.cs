using UnityEngine;

public class PlayerMovement : Movement

{
    public bool Touch = false;


    protected override void Awake()
    {
        base.Awake();


    }
    protected override void Update()
    {
        if (Touch)
        {
            movementDirection.x = FloatingJoystick.Instance.Horizontal;
            movementDirection.y = FloatingJoystick.Instance.Vertical;
        }
        else
        {
            movementDirection.x = Input.GetAxis("Horizontal");
            movementDirection.y = Input.GetAxis("Vertical");
        }

        animationScript.UpdateMovement(movementDirection.x, movementDirection.y, movementDirection.sqrMagnitude);


    }
  
    protected override void FixedUpdate()
    {
        Move();


    }
 
}
