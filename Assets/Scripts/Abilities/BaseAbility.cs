using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float startAbilityTime;
    private float startCooldownTime;

    //Public


    ////Voids

    //Private
    private void Start()
    {
        abilityState = AbilityState.Ready;
    }

    //Protected
    protected virtual void executeAbility()
    {

    }
    protected virtual void ReadyAbility()
    {

    }
    protected virtual void OnFinishAbility()
    {

    }
    protected void SwitchAbilityState()
    {
        switch (abilityState)
        {

            case AbilityState.Ready:
                break;
            case AbilityState.Executing:
                //print($"try execute {this.name} in case");
                float ctime = Time.time;
                if (startAbilityTime + executeTime > ctime)
                {
                    //print($"execute {this.name}");
                    executeAbility();


                }
                else
                {
                    //print($"start cooldown {this.name}");
                    OnFinishAbility();
                    abilityState = AbilityState.Cooldown;
                    startCooldownTime = Time.time;
                }
                break;
            case AbilityState.Cooldown:
                ctime = Time.time;

                if (startCooldownTime + cooldown > ctime)
                {

                    break;
                }
                else
                {
                    abilityState = AbilityState.Ready;
                }

                break;

        }
    }
    protected virtual void FixedUpdate()
    {
        SwitchAbilityState();
        
    }

    //Public
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
    public float getCooldown(){ return cooldown; }
    public int getCost() { return cost; }
    public void changeCost(int value)
    {
        cost += value;
    }
    public void changeCooldown(float value)
    {
        cooldown += value;
    }

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

            
            if (abilityState == AbilityState.Ready)
            {
                print($"activate {this.name} with target {Target}");
                //print($"Try to execute {this.name}");
                activatorParams.changeEnergy(-cost);
                abilityState = AbilityState.Executing;
                startAbilityTime = Time.time;
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


    public virtual GameObject GetTarget()
    {
        return this.gameObject;
    }
    private enum AbilityState
    {
        Ready,
        Executing,
        Cooldown
    }
}
