using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CABaseEffect 
{
    protected List<GameObject> Targets = new List<GameObject> ();
    
    protected EffectExecutionType ExecutionType;
    public CABaseEffect(List<GameObject> targets,EffectExecutionType executionType )
    {
        Targets = targets;
        ExecutionType = executionType;
    }


    public virtual void ExecuteEffect()
    {

    }
    public virtual void SetTargets(List<GameObject> newTargets)
    {
        Targets.Clear();
        Targets = new List<GameObject>(newTargets);
    }
    public virtual void ActivateEffect()
    {

    }
    public virtual void FinishEffect()
    {
        Targets.Clear();
    }
    public EffectExecutionType GetEffectExecutionType()
    {
        return ExecutionType;
    }

}
public enum EffectExecutionType
{
    Instant,
    Continuous
}

