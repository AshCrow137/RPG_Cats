using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : CharacterScript
{
    public static PlayerScript Instance { get; private set; }
    

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    private void Update()
    {
        
        if (movementScript.GetCurrentSpeed()<=0.01)
        {
            
            BasicAttack();
        }
    }

    public override void BasicAttack()
    {
        if (baseAttack != null)
        {
            if (baseAttack.CanActavateAbility()&& baseAttack.IsAttacking())
            {



                attackTarget = GetPossibleEnemyList(Enemylist);
                if (attackTarget != null)
                {
                    baseAttack.ActivateAbility(this.gameObject, attackTarget.gameObject);
                    //baseAttack.ChangeIsAttacking(true);
                }
                else
                {
                    print("No valid targets!");
                    baseAttack.ChangeIsAttacking(false);

                }


            }


            
        }
        else
        {
            Debug.LogError($"there is no basic attack attached to {this.name}");
        }

    }
    public void TryToAttack()
    {
        if (!baseAttack)
        {
            Debug.LogError($"there is no basic attack attached to {this.name}");
            return;
        }
        baseAttack.ChangeIsAttacking(true);

    }
}

