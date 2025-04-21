using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasePlayerAbility : BaseAbility
{
    
    public List<AbilityStruct> abilityStructs = new List<AbilityStruct>();
    [SerializeField]
    protected GameObject AbilityDistance;
    [SerializeField]
    protected CATemplateScript AbilityTemplate;
    [HideInInspector]
    public UnityEvent AbilityCastFinishedEvent = new UnityEvent();
    protected override void Start()
    {
        base.Start();
        AbilityDistance.GetComponent<SpriteRenderer>().enabled = false;
        AbilityTemplate.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false ;
        SetupAbility();
    }
    protected override void ExecuteAbility()
    {
        base.ExecuteAbility();
        //foreach (AbilityStruct ability in abilityStructs)
        //{
        //    ability.AbilityEffect.ExecuteEffect();
        //}
        
    }

    public void SetupAbility()
    {
        for (int i=0; i<abilityStructs.Count; i++)
        {
            AbilityStruct ability = abilityStructs[i];
            EffectTypes type = ability.EffectTypes;
            switch (type)
            {
                case EffectTypes.Move:
                    CAMoveEffectStats moveStats = ability.Stats as CAMoveEffectStats;
                    moveStats.TargetPointTemplate = AbilityTemplate;
                    List<GameObject> newtargets = new List<GameObject> { abilityOwner.gameObject };

                        ability.AbilityEffect = new CAMoveEffect(moveStats.Targets.Count > 0 ? moveStats.Targets : newtargets,
                        moveStats.ExecutionType,
                        moveStats.MoveEffectType,
                        moveStats.TargetPointTemplate,
                        distance / duration,
                        distance);

                    break;
                case EffectTypes.Buff:
                    CABuffEffectStats buffStats = ability.Stats as CABuffEffectStats;
                    ability.AbilityEffect = new CABuffEffect(buffStats.Targets, buffStats.ExecutionType, buffStats.parametresToBuff, ownerParameters, gameObject.name);
                    break;

                

            }
            abilityStructs[i] = ability;
        }
    }
    public void DrawAbilityDistance()
    {
        if (hasTarget)
        {
            AbilityDistance.transform.localScale = new Vector3(distance, distance, 1);
            AbilityDistance.GetComponent<SpriteRenderer>().enabled = true;
        }
       
    }
    public void StopDrawingAbilityDistance()
    {
        AbilityDistance.GetComponent<SpriteRenderer>().enabled = false;
    }
    public bool DrawAbilityTemplate(bool draw)
        
    {
        if(hasTarget&&(tagertOption==AbilityTargetingOptions.Template|| tagertOption == AbilityTargetingOptions.EveryEnemyWithinTemplate))
        {
            AbilityTemplate.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = draw; ;
            return true;
        }
        return false;
    }

    
    public void RotateAbilityTemplate(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        AbilityTemplate.transform.eulerAngles = new Vector3(0, 0, -angle);
    }

    public override bool ActivateAbility(GameObject source, GameObject Target)
    {
        foreach (AbilityStruct ability in abilityStructs)
        {
            switch (tagertOption)
            {
                case AbilityTargetingOptions.Template:
                    ability.AbilityEffect.SetTargets(new List<GameObject>() { abilityOwner.gameObject });
                    break;
                case AbilityTargetingOptions.EveryEnemyWithinTemplate:
                    List<GameObject> targets = AbilityTemplate.GetCharactersInTemplate();
                    ability.AbilityEffect.SetTargets(targets);
                    break;
                case AbilityTargetingOptions.OneTarget://TODO Получать цель в абилке одним из способов: ближайшая, по выбору, рандомная

                    ability.AbilityEffect.SetTargets(new List<GameObject>() { Target });
                    break;

            }
            
            
        }
        return base.ActivateAbility(source, Target);
    }
    protected override void OnFinishAbility()
    {
        base.OnFinishAbility();


    }
    protected override void TryToExecuteAbility()
    {
        print("invoke execute event");
        AbilityCastFinishedEvent.Invoke();
        base.TryToExecuteAbility();
    }

    protected override IEnumerator AbilityExecuteTimer()
    {
       
        foreach(AbilityStruct ability in abilityStructs)
        {
            ability.AbilityEffect.ActivateEffect();
            switch (ability.AbilityEffect.GetEffectExecutionType())
            {
                case EffectExecutionType.Instant:
                    ExecuteAbility();
                    ability.AbilityEffect.ExecuteEffect();
                    break;
                case EffectExecutionType.Continuous:
                    float t = 0;
                    while (t <= ability.Stats.EffectDuration)
                    {
                        ExecuteAbility();
                        ability.AbilityEffect.ExecuteEffect();
                        t += Time.deltaTime;
                        yield return null;
                    }
                    break;
            }

            ability.AbilityEffect.FinishEffect();
            
            
        }
        OnFinishAbility();

    }
}

[Serializable]
public struct AbilityStruct
{
    public EffectTypes EffectTypes;
    public CABaseEffect AbilityEffect;
    public CABaseEffectStats Stats;
    
}
public enum EffectTypes
{
    Move,
    Damage,
    Summon,
    Buff,
    Debuff

}

