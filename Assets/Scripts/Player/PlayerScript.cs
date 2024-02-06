using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : CharacterScript
{
    public static PlayerScript Instance { get; private set; }
    public List<CharacterScript> Enemylist = new List<CharacterScript>();

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyScript>(out EnemyScript enemy))
        {

            Enemylist.Add(enemy);
           
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyScript>(out EnemyScript enemy))
        {
            if (Enemylist.Contains(enemy))
            {
                Enemylist.Remove(enemy);
            }
            
            
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

