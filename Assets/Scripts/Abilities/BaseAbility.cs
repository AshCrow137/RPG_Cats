using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class BaseAbility : MonoBehaviour
{
    //Protected
    [SerializeField]
    protected float damage = 1f;
    [SerializeField]
    protected float cooldown;
    private float RestCooldownTime;
    [SerializeField]
    protected int cost;
    [SerializeField]
    private AbilityExecutionType abilityExecutionType;
    [SerializeField]
    protected float executeTime;
    [SerializeField]
    protected float abilityDistance;
    [SerializeField]
    protected bool canBeInterrupted = true;

    protected CharacterScript abilityOwner;

    //Private
    private AbilityState abilityState;
    protected IEnumerator AbilityExecutionCoroutine;


    //Public
    //public  UnityEvent AbilityReadyEvent = new UnityEvent();
    //public  UnityEvent AbilityExecutingEvent = new UnityEvent();
    //public UnityEvent AbilityFinishedEvent = new UnityEvent();


    #region UnityMethods
    private void Start()
    {
        abilityState = AbilityState.Ready;
        abilityOwner = GetComponentInParent<CharacterScript>();
        if (!abilityOwner)
        {
            Debug.LogError($"{this.name} ability has no ability owner!");
        }
        else
        {
            Movement movement = abilityOwner.GetMovementScript();
            if (movement != null) 
            {
                
                movement.OnMove.AddListener(OnCharacterMove);
                movement.OnMovementFinished.AddListener(OnCharacterStopMove);
            }
          
      

        }
    }
    protected virtual void OnCharacterMove()
    {
        

    }
    protected virtual void OnCharacterStopMove()
    {
        
    }
    protected virtual void FixedUpdate()
    {


    }
    #endregion
    #region AbilityStates
    protected virtual void ExecuteAbility()
    {
        print($"execute {this.name}");
    }
    protected virtual void AbilityReady()
    {
        abilityState = AbilityState.Ready;
        //AbilityReadyEvent.Invoke();
    }
    protected virtual void OnFinishAbility()
    {
        print($"execute {this.name} finished");
        //AbilityFinishedEvent.Invoke();
        StartCoroutine(AbilityCooldownTimer());
    }
    #endregion

    public virtual void ActivateAbility(GameObject source, GameObject Target)
    {
        Parameters activatorParams = source.GetComponent<Parameters>();

        if (activatorParams == null)
        {
            Debug.LogError($"{source.name} has no params");
            return;
        }
        if (Target == null)
        {
            Debug.LogError($"{this.name} has no target!");
        }
        if (activatorParams.getEnergy() >= cost)
        {


            if (CanActavateAbility())
            {
                
                activatorParams.changeEnergy(-cost);
                //AbilityExecutingEvent.Invoke();
                if (abilityExecutionType == AbilityExecutionType.Continuous)
                {
                    AbilityExecutionCoroutine = AbilityExecuteTimer();
                    StartCoroutine(AbilityExecutionCoroutine);
                }
                else if( abilityExecutionType == AbilityExecutionType.Single ) 
                { 
                    ExecuteAbility();
                    OnFinishAbility();
                }
                

            }
            else
            {
                print("cooldown!");
            }
        }
        else
        {
            print("Not enought energy! ");
            return;
        }

    }
    #region AbilityStats
    public float getCooldown() { return cooldown; }
    public int getCost() { return cost; }
    public void changeCost(int value)
    {
        cost += value;
    }
    public void changeCooldown(float value)
    {
        cooldown += value;
    }
    public virtual bool CanActavateAbility()
    {
        if (abilityState == AbilityState.Ready)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public virtual GameObject GetTarget()
    {
        return this.gameObject;
    }
    #endregion
    #region Timers

    protected IEnumerator AbilityExecuteTimer()
    {
        abilityState = AbilityState.Executing;
        float t = 0;
        while (t <= executeTime)
        {
            ExecuteAbility();
            t += Time.deltaTime;
            yield return null;
        }

        OnFinishAbility();

    }
    protected IEnumerator AbilityCooldownTimer()
    {
        RestCooldownTime = cooldown;
        abilityState = AbilityState.Cooldown;
        while (RestCooldownTime>0)
        {
            RestCooldownTime -= Time.deltaTime;
            yield return null;
        }
        
        AbilityReady();
    }

    public void StopExecutingAbility()
    {
        StopCoroutine(AbilityExecutionCoroutine);
        abilityState = AbilityState.Cooldown;
        OnFinishAbility();
    }
    public void DisableAbility()
    {
        if (abilityState == AbilityState.Ready)
        {
            abilityState = AbilityState.Disabled;
        }
        
    }
    public void EnableAbility()
    {
        if (abilityState == AbilityState.Disabled)
        {
            abilityState = AbilityState.Ready;
        }
    }
    public float GetAbilityDistance()
    {
        return abilityDistance;
    }
    public float GetRestCooldownTime()
    {
        return RestCooldownTime;
    }
    #endregion
    private enum AbilityState
    {
        Ready,
        Executing,
        Cooldown,
        Disabled
    }
    private enum AbilityExecutionType
    {
        Single,
        Continuous
    }
}
