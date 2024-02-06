using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseAttack : BaseAbility
{
    private bool isAttacking = false;
    private GameObject attackSource;
    private GameObject attackTarget;
    private bool canAttack;

    public override void ActivateAbility(GameObject source, GameObject Target)
    {
        base.ActivateAbility(source, Target);
        attackSource = source;
        attackTarget = Target;
        Movement movement = source.GetComponent<Movement>();
        if (movement == null) 
        {
            Debug.LogError($"{this.gameObject} has no attachment Movement component!");
            return;
        }

        if (canAttack) 
        {
            isAttacking = true;
            movement.setTarget(null);
            Attack(source,Target);
        }
        else
        {
            print("Too far!");
            isAttacking = false;
            movement.setTarget(Target.transform);
        }
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (attackTarget != null&&attackSource!=null) 
        {
            checkDistance(attackSource.transform, attackTarget.transform);
        }
        
    }
    protected virtual void Attack(GameObject source,GameObject target)
    {
        print($"Attack {target} for {damage}");
        //TODO Animations 

    }
    protected override void executeAbility()
    {
        base.executeAbility();

        
    }
    protected override void OnFinishAbility()
    {
        base.OnFinishAbility();
        
    }

    public void ChangeIsAttacking(bool isAttack)
    {
        isAttacking=isAttack;
    }
    public bool IsAttacking()
    { return isAttacking; }
    private void checkDistance(Transform source,Transform target)
    {
        float distance = Vector2.Distance(source.position, target.position);
        
        if (distance <= abilityDistance && !isAttacking)
        {
            canAttack = true;
            ActivateAbility(attackSource, attackTarget);
           
        }
        else 
        {
            canAttack=false;
            
        }
        
    }
}
