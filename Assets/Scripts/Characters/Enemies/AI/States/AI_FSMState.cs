using UnityEngine;
public abstract class AI_FSMState
{

    protected readonly AI_FSM Fsm;

    public AI_FSMState(AI_FSM fsm)
    {
        Fsm = fsm;
    }
    public virtual void Enter() 
    {
        Debug.Log($"Enter {this} state");
    }
    public virtual void Exit()
    {
        Debug.Log($"Exit {this} state");
    }
    public virtual void Update() { }
}
