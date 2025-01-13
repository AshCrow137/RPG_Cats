using UnityEngine;

public class BaseAttack : BaseAbility
{
    private bool isAttacking = false;
    private bool canAttack;


    public override bool ActivateAbility(GameObject source, GameObject Target)
    {
        if (!canAttack)
        {
            return false;
        }
        if(base.ActivateAbility(source, Target))
        {
            Attack(source, Target);
            return true;
        }
        return false;
        
    }

    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();


    }
    protected virtual void Attack(GameObject source, GameObject target)
    {

        print($" {source} Attack {target} for {damage}");
        //print(Vector3.Angle(source.transform.position, target.transform.position));
        GameObject fo = abilityOwner.GetForwardObject();
        //TODO Animations 
        abilityOwner.RunAttackAnimation(fo.transform.rotation.z);

    }
    protected override void ExecuteAbility()
    {
        base.ExecuteAbility();


    }
    protected override void OnFinishAbility()
    {
        base.OnFinishAbility();
        

    }
    protected override void OnCharacterMove()
    {
        base.OnCharacterMove();
        canAttack = false;
        
    }
    protected override void OnCharacterStopMove()
    {
        base.OnCharacterStopMove();
        canAttack = true;
        
    }

    public void ChangeIsAttacking(bool isAttack)
    {
        isAttacking = isAttack;
    }
    public bool IsAttacking()
    { return isAttacking; }
    public bool CanAttack()
    { 
        return canAttack;
    }
    public bool checkDistance(Transform source, Transform target)
    {
        float distance = Vector2.Distance(source.position, target.position);
        if (distance <= Distance)
        {
            return true;
        }
        else
        {   
            print("Too far!");
            return false;

        }
    }

}
