using UnityEngine;

public class PlayerDash : BasePlayerAbility
{


    private Rigidbody2D rb;
    private Movement movementScript;
    private ForwardObjectScript forwardObject;

    //public float getDashDistance() { return DashDistance; }
    //public void setDashDistance(float value) { DashDistance = value; }


    public void Dash()
    {
        Vector3 FOpos = AbilityTemplate.gameObject.transform.localPosition;

        float rad = (AbilityTemplate.transform.eulerAngles.z + 90) * Mathf.Deg2Rad;

        Vector2 target = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        float DashSpeed = distance / duration;
        rb.MovePosition(rb.position + target.normalized * DashSpeed * Time.fixedDeltaTime);

    }

    public override bool ActivateAbility(GameObject source, GameObject Target)
    {

        rb = source.GetComponent<Rigidbody2D>();
        movementScript = source.GetComponent<Movement>();
        //print(rbArray);
        if (movementScript == null)
        {
            Debug.LogError($"No movement script attached to {this.name}");
            return false;
        }
        else
        {

            forwardObject = movementScript.GetForwardObject();
        }
        if (rb == null)
        {
            Debug.LogError($"No Rigidbody2d attached to {this.name}");
            return false;
        }
        if( base.ActivateAbility(source, Target))
        {
            movementScript.ChangeMovementPossibility(false);
            return true;
        }
        return false;

    }
    protected override void ExecuteAbility()
    {

        base.ExecuteAbility();

        
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
