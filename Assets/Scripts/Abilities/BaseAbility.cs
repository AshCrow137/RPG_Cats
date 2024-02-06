using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseAbility : MonoBehaviour
{
    //Protected
    [SerializeField]
    protected float damage = 1f;
    [SerializeField]
    protected float cooldown;
    [SerializeField]
    protected int cost;
    [SerializeField]
    protected float executeTime;
    [SerializeField]
    protected float abilityDistance;
    //Private
    private AbilityState abilityState;


    //Public
    public static UnityEvent AbilityReadyEvent = new UnityEvent();
    public static UnityEvent AbilityExecutingEvent = new UnityEvent();
    public static UnityEvent AbilityFinishedEvent = new UnityEvent();


    #region UnityMethods
    private void Start()
    {
        abilityState = AbilityState.Ready;
    }
    protected virtual void FixedUpdate()
    {


    }
    #endregion
    #region AbilityStates
    protected virtual void ExecuteAbility()
    {
        
    }
    protected virtual void AbilityReady()
    {
        abilityState = AbilityState.Ready;
        AbilityReadyEvent.Invoke();
    }
    protected virtual void OnFinishAbility()
    {
        Debug.Log($"{gameObject} ability finished");
        AbilityFinishedEvent.Invoke();
        StartCoroutine(AbilityCooldownTimer());
    }
    #endregion

    public virtual void ActivateAbility(GameObject source,GameObject Target)
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
        if (activatorParams.getEnergy() >= cost  )
        {

            
            if (CanActavateAbility())
            {
                print($"activate {this.name} with target {Target}");
                //print($"Try to execute {this.name}");
                activatorParams.changeEnergy(-cost);
                AbilityExecutingEvent.Invoke();
                StartCoroutine(AbilityExecuteTimer());
                
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
        while (t<=executeTime)
        {
            ExecuteAbility();
            t+= Time.deltaTime; 
            yield return null;
        }

        OnFinishAbility();

    }
    protected IEnumerator AbilityCooldownTimer()
    {
        abilityState = AbilityState.Cooldown;
        yield return new WaitForSeconds(cooldown);
        AbilityReady();
    }
    #endregion
    private enum AbilityState
    {
        Ready,
        Executing,
        Cooldown
    }
}
