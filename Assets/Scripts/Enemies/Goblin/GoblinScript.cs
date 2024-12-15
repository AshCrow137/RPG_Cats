using UnityEngine;

public class GoblinScript : EnemyScript
{
    [SerializeField]
    private float RandomMovementDistance = 5;
    [SerializeField]
    private float WaitTime = 3;
    [SerializeField]
    private float InterruptTime = 10;

    protected override void Start()
    {
        base.Start();
        
        Fsm.AddState(new AI_FSMState_RandomMovement(Fsm,RandomMovementDistance,WaitTime,InterruptTime,enemyMovement));
        Fsm.SetState<AI_FSMState_RandomMovement>();

    }
    public override void RemoveEnemyFromEnemyList(CharacterScript Enemy)
    {
        base.RemoveEnemyFromEnemyList(Enemy);

    }
    protected override void OnEnemyRemovedFromList(PlayerScript enemy)
    {
        base.OnEnemyRemovedFromList(enemy);
        Fsm.SetState<AI_FSMState_RandomMovement>();
       // gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

}
