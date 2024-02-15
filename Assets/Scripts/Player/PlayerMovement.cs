using UnityEngine;

public class PlayerMovement : Movement

{
    public bool Touch = false;


    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();

        //
        animator = GetComponent<Animator>();
     
        animationScript = GetComponent<AnimationScript>();

    }
    protected override void Update()
    {
        if (Touch)
        {
            movement.x = FloatingJoystick.Instance.Horizontal;
            movement.y = FloatingJoystick.Instance.Vertical;
        }
        else
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");
        }
        if (animationScript != null)
        {
            animationScript.UpdateMovement(movement.x, movement.y, movement.sqrMagnitude);

        }



    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (canMove && movement != Vector2.zero)
        {
            if (target!=null)
            {
                setTarget(null);
            }
            
            rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
            float angle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;
            forwardObject.transform.eulerAngles = new Vector3(0, 0, -angle);

        }
        else
        {
            //MoveToTarget();
        }

    }
    public override void MoveToTarget(Transform target)
    {
        if(movement==Vector2.zero)
        {
            base.MoveToTarget(target);
        }
        
    }
}
