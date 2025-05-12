
public class AI_FSMState_RandomMovement : AI_FSMState
{
    private float RandomMovementDistance;
    private float WaitTime;
    private float InterruptTime;
    private EnemyMovement MovementScript;

    public AI_FSMState_RandomMovement(AI_FSM fsm, float randomMovementDistance, float waitTime, float interruptTime,EnemyMovement movementScript) : base(fsm)
    {
        RandomMovementDistance = randomMovementDistance;
        WaitTime = waitTime;
        InterruptTime = interruptTime;
        MovementScript = movementScript;
    }
    public override void Enter()
    {
        base.Enter();
        MovementScript.SetRandomMovementStartPoint(MovementScript.transform.position);
        
        MovementScript.StartRandomMovement(RandomMovementDistance, WaitTime);
        //start random movement
    }
    public override void Exit() 
    {
        base.Exit();
        MovementScript.StopRandomMovement();

    }
    public override void Update()
    {
        base.Update();
        if(MovementScript.movementTime>=InterruptTime)
        {
            Fsm.SetState<AI_FSMState_Idle>();
            
        }
    }

}
