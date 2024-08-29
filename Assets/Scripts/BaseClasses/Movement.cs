using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    protected  Rigidbody2D rb;

    protected  ForwardObjectScript forwardObject;
    protected  AnimationScript animationScript;
    protected bool canMove = true;

   
    protected Vector2 movementDirection;

    //Movement parameters
    [SerializeField]
    protected float speed;

    //protected Transform target;
    [HideInInspector]
    public  UnityEvent OnMove = new UnityEvent();
    [HideInInspector]
    public  UnityEvent OnMovementFinished = new UnityEvent();

  
    protected virtual void Awake()
    {
        forwardObject = GetComponentInChildren<ForwardObjectScript>();
        if (forwardObject == null)
        {
            Debug.LogError($"No forward object attached to {this.name}");
        }
        rb = GetComponent<Rigidbody2D>();


        animationScript = GetComponent<AnimationScript>();
        if (!animationScript)
        {
            Debug.LogError("Missing AnimationScript");
        }
    }
    protected virtual void Update()
    {
        

    }
    protected virtual void FixedUpdate()
    {
       
        
    }
    protected virtual void Move()
    {

        //if (target != null)
        //{
        //    setTarget(null);
        //}
        if (canMove && movementDirection != Vector2.zero)
        {
            OnMove.Invoke();
            rb.MovePosition(rb.position + movementDirection.normalized * speed * Time.fixedDeltaTime);
            RotateForwardObject(movementDirection);
        }
        else
        {
            OnMovementFinished.Invoke();

        }




    }
    protected void RotateForwardObject(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        forwardObject.transform.eulerAngles = new Vector3(0, 0, -angle);
    }
    public virtual void MoveToTarget(Vector3 target)
    {
        
        Vector2 rotationDirection = new Vector2 (target.x, target.y) - new Vector2(transform.position.x, transform.position.y);
       

        if (canMove && rotationDirection != Vector2.zero )
        {

            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            RotateForwardObject(rotationDirection);
            animationScript.UpdateMovement(rotationDirection.x, rotationDirection.y, rotationDirection.sqrMagnitude);

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
        return movementDirection.sqrMagnitude;
    }
    public Vector2 getMovement()
    {
        return movementDirection;
    }
    //public void setTarget(Transform newtarget)
    //{
    //    target = newtarget;
    //}
}
