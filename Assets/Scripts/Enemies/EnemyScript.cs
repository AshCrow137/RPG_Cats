using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : CharacterScript
{
    protected EnemyMovement enemyMovement;
    protected AI_FSM Fsm;

    private CharacterScript currentEnemy;

    public AI_FSMState currentState;
    public string stateName;
    protected override void Awake()
    {
        base.Awake();
        enemyMovement = GetComponent<EnemyMovement>();
        if (!enemyMovement)
        {
            Debug.LogError($"No EnemyMovement attached to {this}");
        }



    }

    protected override void Start()
    {
        base .Start();
        Fsm = new AI_FSM();
        Fsm.AddState(new AI_FSMState_Idle(Fsm));
        Fsm.AddState(new AI_FSMState_MoveToTarget(Fsm,this));
        Fsm.AddState(new AI_FSMState_Attack(Fsm,this));
        Fsm.SetState<AI_FSMState_Idle>();
        
    }
    private void OnTargetReached()
    {
        Fsm.SetState<AI_FSMState_Attack>();
    }
    protected virtual void Update()
    {
        Fsm.Update();
        currentState = Fsm.GetCurrentState();
        stateName = currentState.GetType().Name;
    }
    protected override void OnAttackCheckDistanceFailed(CharacterScript target)
    {
        base.OnAttackCheckDistanceFailed(target);
        Fsm.SetState<AI_FSMState_MoveToTarget>();
    }
    public override void AddEnemyToEnemyList(CharacterScript Enemy)
    {
        if(Enemy is not PlayerScript)
        {
            return;
        }
        base.AddEnemyToEnemyList(Enemy);
        OnTargerSpotted(Enemy as PlayerScript);
    }
    protected virtual void OnTargerSpotted(PlayerScript target)
    {
        currentEnemy = GetClosestEnemy(Enemylist);
        
        Fsm.SetState<AI_FSMState_MoveToTarget>();
        //gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }
    public override void RemoveEnemyFromEnemyList(CharacterScript Enemy)
    {
        base.RemoveEnemyFromEnemyList(Enemy);

        if(Enemy is not PlayerScript)
        {
           
            return;
        }
       
        if(Enemy == currentEnemy)
        {
            currentEnemy = null;
           
        }
        OnEnemyRemovedFromList(Enemy as PlayerScript);

    }
    protected virtual void OnEnemyRemovedFromList(PlayerScript enemy)
    {
      
    }
    public void StartFollowTarget()
    {
       
        if(currentEnemy)
        {
            enemyMovement.StartMoveToTarget(currentEnemy.transform.position);
        }
        
    }
    public void FollowTarget()
    {
        float distance = Vector3.Distance(transform.position, currentEnemy.transform.position);
        if (distance > baseAttack.GetAbilityDistance())
        {
            enemyMovement.MoveToTarget(currentEnemy.transform.position);
        }
        else
        {
            enemyMovement.InterceptMovement();
            OnTargetReached();
        }
    }
    public void StopFollowTarget()
    {

    }
    

}
