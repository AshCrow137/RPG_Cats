using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{

    [SerializeField]
    protected BaseAttack baseAttack;

    [SerializeField]
    protected BaseAbility[] abilityArray = new BaseAbility[3];

    protected GameObject forwardObject;
    protected CharacterScript attackTarget = null;
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
    protected virtual CharacterScript GetPossibleEnemyList(List<CharacterScript> listOfTargets)
    {
        if (listOfTargets.Count != 0)
        {
            if (listOfTargets.Contains(attackTarget))
            {
                return attackTarget;
            }
            else
            {
                int cPriority = 0;
                //CharacterScript newTarget = listOfTargets[0];//TODO get closest target
                CharacterScript newTarget = null;
                foreach (CharacterScript potentialTarget in listOfTargets)
                {
                    if (potentialTarget.GetCharacterPriority() > cPriority)
                    {
                        cPriority = potentialTarget.GetCharacterPriority();
                        newTarget = potentialTarget;
                    }
                }
                attackTarget = newTarget;
                print($"select {newTarget} as new target. Priority {cPriority}");
                return newTarget;
            }

        }
        else
        {
            return null;
        }



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
    public void AddEnemyToEnemyList(CharacterScript Enemy)
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
    protected virtual void TryToAttack()
    {

        CheckForTarget();

    }
    protected IEnumerator RepeatAttack(CharacterScript RepeatAttackTarget)
    {
        isAttacking = true;
        while (RepeatAttackTarget != null)
        {
            yield return null;
            if (baseAttack.CanActavateAbility())
            {
                baseAttack.ActivateAbility(this.gameObject, RepeatAttackTarget.gameObject);
            }
        }
        print("end attack");
        isAttacking = false;
        CheckForTarget();

    }
    protected void CheckForTarget()
    {
        CharacterScript attackTarget = GetPossibleEnemyList(Enemylist);
        if (attackTarget != null)
        {
            //baseAttack.ActivateAbility(this.gameObject, attackTarget.gameObject);
            //baseAttack.ChangeIsAttacking(true);
            BasicAttack(attackTarget);

        }
        else
        {
            print("No valid targets!");
        }

    }
    public void StopBasicAttack()
    {
        print("stop BA");
        StopCoroutine(AttackCoroutine);
        isAttacking=false;
        

    }
    public Movement GetMovementScript()
    {
        return movementScript;
    }
}
