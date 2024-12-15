using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_FSMState_Attack : AI_FSMState
{
    EnemyScript Owner;
    public AI_FSMState_Attack(AI_FSM fsm,EnemyScript owner) : base(fsm)
    {
        Owner = owner;
    }
    public override void Enter()
    {
        base.Enter();
        Owner.TryToAttack();

    }
    public override void Exit()
    { 
        base.Exit();
        Owner.StopAttackAnimation();
    }

    public override void Update()
    {
        base.Update();
    }


}
