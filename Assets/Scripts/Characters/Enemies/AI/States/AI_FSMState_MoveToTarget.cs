using UnityEngine;

public class AI_FSMState_MoveToTarget : AI_FSMState
{
    private EnemyScript OwnerScript;
    public AI_FSMState_MoveToTarget(AI_FSM fsm,EnemyScript ownerScript) : base(fsm)
    {
        OwnerScript = ownerScript;   
    }
    public override void Enter()
    {
        base.Enter();
        OwnerScript.StartFollowTarget();
    }
    public override void Exit() 
    { 
        base.Exit();
        OwnerScript.StopFollowTarget();
    }
    public override void Update()
    {
        base.Update();
        OwnerScript.FollowTarget();
    }
}
