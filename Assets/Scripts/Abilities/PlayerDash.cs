using UnityEngine;

public class PlayerDash : BasePlayerAbility
{


    private Rigidbody2D rb;
    private Movement movementScript;
    private ForwardObjectScript forwardObject;

    //public float getDashDistance() { return DashDistance; }
    //public void setDashDistance(float value) { DashDistance = value; }

    private void Start()
    {

    }
    public void Dash()
    {

        //rb.velocity = rb.velocity * DashDistance;
        Vector3 FOpos = forwardObject.gameObject.transform.localPosition;
        //Vector2 target = new Vector2 ( FOpos.x, FOpos.y) ;
        //Vector2 target = movementScript.getMovement();
        float rad = (forwardObject.transform.eulerAngles.z + 90) * Mathf.Deg2Rad;

        Vector2 target = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        float DashSpeed = abilityDistance / executeTime;
        rb.MovePosition(rb.position + target.normalized * DashSpeed * Time.fixedDeltaTime);

    }

    public override void ActivateAbility(GameObject source, GameObject Target)
    {

        rb = source.GetComponent<Rigidbody2D>();
        movementScript = source.GetComponent<Movement>();
        print(rb);
        if (movementScript == null)
        {
            Debug.LogError($"No movement script attached to {this.name}");
        }
        else
        {

            forwardObject = movementScript.GetForwardObject();
        }
        if (rb == null)
        {
            Debug.LogError($"No Rigidbody2d attached to {this.name}");
        }
        base.ActivateAbility(source, Target);

    }
    protected override void ExecuteAbility()
    {

        base.ExecuteAbility();

        movementScript.ChangeMovementPossibility(false);
        Dash();


    }
    protected override void OnFinishAbility()
    {
        base.OnFinishAbility();
        movementScript.ChangeMovementPossibility(true);
    }
    public override GameObject GetTarget()
    {
        return gameObject;
    }
}
