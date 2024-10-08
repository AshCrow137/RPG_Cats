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
            movementDirection = FloatingJoystick.Instance.Direction;
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
