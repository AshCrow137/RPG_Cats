using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CADamageEffect : CABaseEffect
{
    
    private float Damage;
    private float DamageIntervalTime;
    private List<CharacterScript> CharacterScripts;
    private Parameters ActivatorParameters;


    private float t1 = 0;
    public CADamageEffect(List<GameObject> targets, EffectExecutionType executionType,float damage,float damageIntervalTime,Parameters activatorParameters) : base(targets, executionType)
    {
        Damage = damage;
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
            CharacterScript script = target.GetComponent<CharacterScript>();
            if (script != null)
            {
                CharacterScripts.Add(script);
            }
        }
    }
    public override void ActivateEffect()
    {
        t1 = Time.time;
        float resultDamage = (Damage + ActivatorParameters.GetMultuplyer(ParameterToBuff.DamageFlatMultiplyer)) * ((100 + ActivatorParameters.GetMultuplyer(ParameterToBuff.DamagePercentMultiplyer)) / 100);
        if (resultDamage < 0)
        {
            resultDamage = 0;
        }
        Damage = resultDamage;
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

                character.TakeDamage(Damage);
            }
        }
        
    }
}
