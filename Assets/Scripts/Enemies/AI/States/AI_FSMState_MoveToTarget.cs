using UnityEngine;

public class AI_FSMState_MoveToTarget : AI_FSMState
{
    private EnemyScript EnemyScript;
    public AI_FSMState_MoveToTarget(AI_FSM fsm,EnemyScript enemyScript) : base(fsm)
    {
        EnemyScript = enemyScript;   
    }
    public override void Enter()
    {
        base.Enter();
        EnemyScript.StartFollowTarget();
    }
    public override void Exit() 
    { 
        base.Exit();
        EnemyScript.StopFollowTarget();
    }
    public override void Update()
    {
        base.Update();
        EnemyScript.FollowTarget();
    }
}
