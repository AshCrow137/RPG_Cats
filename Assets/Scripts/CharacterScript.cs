using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

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

   
    protected IEnumerator AttackCoroutine; 
    protected virtual void Awake()
    {

        parameters = GetComponent<Parameters>();
        movementScript = GetComponent<Movement>();
        if (movementScript == null)
        {
            Debug.LogError($" {this.name} has no attached movement");
        }

    }
    protected virtual int GetCharacterPriority()
    {
        return parameters.GetPriority();
    }
    private void Start()
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
   

    }

    public void TriggerAbility(int number)
    {


        if (number <= abilityArray.Length || number > 0)
        {
            BaseAbility ability = abilityArray[number - 1];
            if (ability != null)
            {

                ability.ActivateAbility(this.gameObject, ability.GetTarget()); //Задаем в качестве цели объект активирующий абилку. Если нужна конкретная цель, переписываем ее в самой абилке
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
    }
    protected virtual CharacterScript GetPossibleEnemy(List<CharacterScript> listOfTargets)
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
                if(baseAttack.checkDistance(transform,newTarget.transform))
                {
                    print($"select {newTarget} as new target");
                    return newTarget;
                }
                else 
                {
                    //startFollow?
                    return null; 
                }
                
            //}

        }
        else
        {
            return null;
        }



    }
    protected virtual CharacterScript GetEnemyWithPriority(CharacterScript potentialEnemy)
    {
        return null;
    }

    public void TakeDamage(float damage)
    {

        parameters.ChangeHealth(-damage);
        float health = parameters.getHealth();
        if (health <= 0)
        {

            CharacterDead();
        }

    }
    public virtual void CharacterDead()
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
    public void RemoveEnemyFromEnemyList(CharacterScript Enemy)
    {
        if (Enemylist.Contains(Enemy))
        {
            Enemylist.Remove(Enemy);
        }
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
            yield return null;
            if (baseAttack.CanActavateAbility()&&baseAttack.CanAttack())
            {             

                if (baseAttack.checkDistance(transform, RepeatAttackTarget.transform))
                {
                    baseAttack.ActivateAbility(this.gameObject, RepeatAttackTarget.gameObject);
                }
                else
                {
                    StopBasicAttack(true);
                }
                RepeatAttackTarget = GetPossibleEnemy(Enemylist);
            }
            
        }
        print("end attack");
        isAttacking = false;
        CheckForTarget();

    }
    protected void CheckForTarget()
    {
        CharacterScript attackTarget = GetPossibleEnemy(Enemylist);
        if (attackTarget != null)
        {
            BasicAttack(attackTarget);
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
    public Movement GetMovementScript()
    {
        return movementScript;
    }
}
