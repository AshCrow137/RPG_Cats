using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{

    protected static Rigidbody2D rb;
    protected static Animator animator;
    [SerializeField]
    protected  ForwardObjectScript forwardObject;
    protected static AnimationScript animationScript;
    protected bool canMove = true;

    protected Vector2 movement;

    //Movement parameters
    [SerializeField]
    protected float speed;

    protected Transform target;

    public  UnityEvent OnMove = new UnityEvent();
    public  UnityEvent OnMovementFinished = new UnityEvent();
    protected virtual void Awake()
    {
        forwardObject = GetComponentInChildren<ForwardObjectScript>();
        if (forwardObject == null)
        {
            Debug.LogError($"No forward object attached to {this.name}");
        }
    }
    protected virtual void Update()
    {

    }
    protected virtual void FixedUpdate()
    {
        if (canMove && movement != Vector2.zero)
        {
            OnMove.Invoke();

        }
        else
        {
            OnMovementFinished.Invoke();
        }

    }
    public virtual void MoveToTarget(Transform target)
    {
        if (canMove && target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
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
