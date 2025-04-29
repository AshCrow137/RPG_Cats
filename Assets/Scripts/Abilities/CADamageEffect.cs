using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CADamageEffect : CABaseEffect
{
    
    private float BaseDamage;
    private float DamageIntervalTime;
    private List<CharacterScript> CharacterScripts;
    private Parameters ActivatorParameters;
    private float ResultDamage = 0;

    private float t1 = 0;
    public CADamageEffect(List<GameObject> targets, EffectExecutionType executionType,float damage,float damageIntervalTime,Parameters activatorParameters) : base(targets, executionType)
    {
        BaseDamage = damage;
        DamageIntervalTime = damageIntervalTime;
        CharacterScripts = new List<CharacterScript>();
        ActivatorParameters = activatorParameters;
        
    }
    public override void SetTargets(List<GameObject> newTargets)
    {
        base.SetTargets(newTargets);
        CharacterScripts.Clear();
        foreach (GameObject target in newTargets)
        {
            CharacterScript script = target?.GetComponent<CharacterScript>();
            if (script != null)
            {
                CharacterScripts.Add(script);
            }
        }
    }
    public override void ActivateEffect()
    {
        t1 = Time.time;
        ResultDamage = (BaseDamage + ActivatorParameters.GetMultuplyer(ParameterToBuff.DamageFlatMultiplyer)) * ((100 + ActivatorParameters.GetMultuplyer(ParameterToBuff.DamagePercentMultiplyer)) / 100);
        if (ResultDamage < 0)
        {
            ResultDamage = 0;
        }
        
        base.ActivateEffect();
    }
    public override void ExecuteEffect()
    {
        base.ExecuteEffect();
        switch(ExecutionType)
        {
            case EffectExecutionType.Instant:
                DealDamage();
                break;
            case EffectExecutionType.Continuous: 
                float t2 = Time.time;
                if(t2-t1>DamageIntervalTime)
                {
                    t1 = t2;
                    DealDamage();
                }
                break;
        }
    }

    private void DealDamage()
    {
        if(CharacterScripts.Count>0)
        {
            foreach (CharacterScript character in CharacterScripts)
            {
                if(character != null)
                {
                    character.TakeDamage(ResultDamage);
                }
                
            }
        }
        
    }
}
