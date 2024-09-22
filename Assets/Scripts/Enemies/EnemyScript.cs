using System.Collections;
using UnityEngine;

public class EnemyScript : CharacterScript
{
    EnemyMovement enemyMovement;

    protected override void Awake()
    {
        base.Awake();
        enemyMovement = GetComponent<EnemyMovement>();
        if (!enemyMovement )
        {
            Debug.LogError("Cast failed");
        }
        else
        {
            enemyMovement.StartRandomMovement();
        }
        GlobalEventManager.TurnManagerEvents.Event_StartCombat.AddListener(StopRandomMovement);
       
    }
    private void StopRandomMovement()

    {
        
        enemyMovement.StopRandomMovement();
    }
}
