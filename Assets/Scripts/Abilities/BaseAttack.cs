using System.Collections;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class BaseAttack : BaseAbility
{
    private bool isAttacking = false;
    private bool startMoveToTarget = false;
    private bool canAttack;
    [SerializeField]
    private bool followTarget = true;

    


    public override void ActivateAbility(GameObject source, GameObject Target)
    {
        if (!canAttack)
        {
            return;
        }
        base.ActivateAbility(source, Target);
        Movement movement = source.GetComponent<Movement>();
        if (movement == null)
        {
            Debug.LogError($"{this.gameObject} has no attachment Movement component!");
            return;
        }

        if (checkDistance(source.transform,Target.transform))
        {

            Attack(source, Target);

        }
        else if ( followTarget)
        {
            if (!startMoveToTarget)
            {
                StartCoroutine(MoveTotarget(movement, source, Target));
            }
        }
        else
        {
            
            abilityOwner.StopBasicAttack();
        }

    }
    protected IEnumerator MoveTotarget(Movement movement,GameObject source, GameObject Target)
    {
        startMoveToTarget = true;
        while (!checkDistance(source.transform,Target.transform))
        {
            yield return null;
            movement.MoveToTarget(Target.transform);
        }
        startMoveToTarget= false;
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();


    }
    protected virtual void Attack(GameObject source, GameObject target)
    {
        print($"Attack {target} for {damage}");
        //TODO Animations 

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
    private bool checkDistance(Transform source, Transform target)
    {
        float distance = Vector2.Distance(source.position, target.position);

        if (distance <= abilityDistance)
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
