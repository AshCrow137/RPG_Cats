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

    protected Parameters parameters;
    private void Awake()
    {

        parameters = GetComponent<Parameters>();
        

    }
    protected virtual int GetCharacterPriority()
    {
        return parameters.GetPriority();
    }
    private void Start()
    {
        forwardObject = GetComponentInChildren<ForwardObjectScript>().gameObject;
        if (forwardObject == null)
        {
            Debug.LogError($"there is no forward object attached to {this.name}");
        }
        movementScript = GetComponent<Movement>();
        if (movementScript == null)
        {
            Debug.LogError($" {this.name} has no attached movement");
        }

    }
    public GameObject GetForwardObject()
    {
        return forwardObject;
    }
    public virtual void BasicAttack()
    {
        if (baseAttack != null)
        {
            baseAttack.ActivateAbility(this.gameObject,baseAttack.GetTarget());//TODO Не целиться в себя
        }
        else
        {
            Debug.LogError($"there is no basic attack attached to {this.name}");
        }
    }

    public void TriggerAbility(int number)
    {


        if (number <= abilityArray.Length || number > 0)
        {
            BaseAbility ability = abilityArray[number - 1];
            if (ability != null)
            {
                ability.ActivateAbility(this.gameObject,ability.GetTarget()); //Задаем в качестве цели объект активирующий абилку. Если нужна конкретная цель, переписываем ее в самой абилке
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
        if(listOfTargets.Count != 0)
        {
            if(listOfTargets.Contains(attackTarget))
            {
                return attackTarget;
            }
            else
            {
                int cPriority = 0;
                //CharacterScript newTarget = listOfTargets[0];//TODO get closest target
                CharacterScript newTarget = null;
                foreach ( CharacterScript potentialTarget in listOfTargets)
                {
                    if (potentialTarget.GetCharacterPriority()>cPriority)
                    {
                        cPriority = potentialTarget.GetCharacterPriority();
                        newTarget = potentialTarget;
                    }
                }
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
}
