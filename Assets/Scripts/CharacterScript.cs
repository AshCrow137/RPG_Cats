using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterScript : MonoBehaviour
{

    [SerializeField]
    protected BaseAttack baseAttack;

    [SerializeField]
    protected BaseAbility[] abilityArray = new BaseAbility[3];

    protected GameObject forwardObject;
    //protected CharacterScript attackTarget = null;
    protected Movement movementScript;
    protected bool isAttacking = false;

    protected Parameters parameters;
    protected List<CharacterScript> Enemylist = new List<CharacterScript>();

    protected AnimationScript animationScript;
    protected IEnumerator AttackCoroutine; 

   
    protected virtual void Awake()
    {

        parameters = GetComponent<Parameters>();
        movementScript = GetComponent<Movement>();
        if (movementScript == null)
        {
            Debug.LogError($" {this.name} has no attached movement");
        }
        animationScript = GetComponent<AnimationScript>();
        if (!animationScript)
        {
            Debug.LogError("Missing AnimationScript");
        }


    }
    protected virtual int GetCharacterPriority()
    {
        return parameters.GetPriority();
    }
    protected virtual void  Start()
    {
        forwardObject = GetComponentInChildren<ForwardObjectScript>().gameObject;
        if (!forwardObject)
        {
            Debug.LogError($"there is no forward object attached to {this.name}");
        }
        if (!baseAttack)
        {
            Debug.LogError($"there is no basic attack attached to {this.name}");
           
        }


    }
    public GameObject GetForwardObject()
    {
        return forwardObject;
    }
    public virtual void BasicAttack(CharacterScript target)
    {

        if (baseAttack != null)
        {
            if (!isAttacking)
            {
                AttackCoroutine = RepeatAttack(target);
                StartCoroutine(AttackCoroutine);
                
            }
        }
        else
        {
            Debug.LogError($"there is no basic attack attached to {this.name}");
        }
    }

    public bool  TriggerAbility(int number)
    {
        Debug.Log("Activate ability");

        if (number <= abilityArray.Length || number > 0)
        {
            BaseAbility ability = abilityArray[number - 1];
            if (ability != null)
            {

                if(ability.ActivateAbility(this.gameObject, ability.GetTarget()))
                {
                    return true;
                }
                
                //Задаем в качестве цели объект активирующий абилку. Если нужна конкретная цель, переписываем ее в самой абилке

            }
            else
            {
                Debug.LogError($"There is no such ability in {this.name} ability array");
            }
        }
        else
        {
            Debug.LogError("Invalid ability number");
        }
        return false;
    }
    protected virtual CharacterScript GetClosestEnemy(List<CharacterScript> listOfTargets)
    {
        if (listOfTargets.Count != 0)
        {
            //if (listOfTargets.Contains(attackTarget))
            //{
            //    return attackTarget;
            //}
            //else
            //{
            //    int cPriority = 0;
                float minDistance = Mathf.Infinity;
                //CharacterScript newTarget = listOfTargets[0];//TODO get closest target
                CharacterScript newTarget = null;
                foreach (CharacterScript potentialTarget in listOfTargets)
                {
                    //if (potentialTarget.GetCharacterPriority() > cPriority)
                    //{
                    //    cPriority = potentialTarget.GetCharacterPriority();
                    //    newTarget = potentialTarget;
                    //}
                    float distance = Vector2.Distance(transform.position,potentialTarget.transform.position);
                    if(distance <minDistance)
                    {
                        minDistance = distance;
                        newTarget = potentialTarget;
                    }
                }
                return newTarget;
 
                
            //}

        }
        else
        {
            return null;
        }
    }
    protected virtual void OnAttackCheckDistanceFailed(CharacterScript target)
    {

    }
    protected virtual CharacterScript GetEnemyWithPriority(CharacterScript potentialEnemy)
    {
        return null;
    }

    public void TakeDamage(float damage)
    {
        
        float resultDamage = (damage - parameters.GetMultuplyer(ParameterToBuff.IncomingDamageFlatMultiplyer)) *
            ((100 - parameters.GetMultuplyer(ParameterToBuff.IncomingDamagePercentMultiplyer)) / 100);
        if (resultDamage > 0)
        {
            OnTakeDamage();
            parameters.ChangeHealth(-resultDamage);
            Debug.Log($"{gameObject} take {resultDamage} damage");
            float health = parameters.GetHealth();
            if (health <= 0)
            {

                KillCharacter();
            }
        }
        else
        {
            Debug.Log($"{gameObject} take no damage, result damage < 0 ");
        }
        

    }
    protected virtual void OnTakeDamage()
    {

    }
    public virtual void KillCharacter()
    {
        Destroy(this.gameObject);
    }
    public virtual void AddEnemyToEnemyList(CharacterScript Enemy)
    {
        if (Enemy == gameObject)// игнорим сами себя
        {
            return;
        }
        Enemylist.Add(Enemy);
    }
    public virtual void RemoveEnemyFromEnemyList(CharacterScript Enemy)
    {
        if (Enemylist.Contains(Enemy))
        {
            Enemylist.Remove(Enemy);
        }
        else
        {
            return;
        }
    }
    public AnimationScript GetAnimationScript()
    {
        return animationScript;
    }
    public void TryToAttack()
    {

        CheckForTarget();

    }
    protected IEnumerator RepeatAttack(CharacterScript RepeatAttackTarget)
    {
        isAttacking = true;
        
        while (RepeatAttackTarget != null)
        {
            movementScript.LookToTarget(RepeatAttackTarget.gameObject.transform.position);
            yield return null;
            if (baseAttack.CanActavateAbility()&&baseAttack.CanAttack())
            {

                print("can attack");
                if (baseAttack.checkDistance(transform, RepeatAttackTarget.transform))
                {
                    print("activate attack");
                    baseAttack.ActivateAbility(this.gameObject, RepeatAttackTarget.gameObject);
                    
                }
                else
                {
                    StopBasicAttack(true);
                }
                RepeatAttackTarget = GetClosestEnemy(Enemylist);
            }
            
        }
        print("end attack");
        isAttacking = false;
        CheckForTarget();

    }
    protected void CheckForTarget()
    {
        CharacterScript attackTarget = GetClosestEnemy(Enemylist);
        if (attackTarget != null)
        {
            if (baseAttack.checkDistance(transform, attackTarget.transform))
            {
                print($"select {attackTarget} as new target");
                BasicAttack(attackTarget);
            }
            else
            {
                OnAttackCheckDistanceFailed(attackTarget);
               
            }
            
        }
        else
        {
            print("No valid targets!");
        }

    }
    public void StopBasicAttack(bool searchForNewEnemy)
    {
        print("stop BA");
        StopCoroutine(AttackCoroutine);
        isAttacking=false;
        if (searchForNewEnemy)
        {
            TryToAttack();
        }

    }
    public void RunAttackAnimation (float animationAngle)
    {
        animationScript.runAttackAnimation(animationAngle);
    }
    public void StopAttackAnimation ()
    {
        animationScript.StopAttackAnimation();
    }
    public Movement GetMovementScript()
    {
        return movementScript;
    }
}
