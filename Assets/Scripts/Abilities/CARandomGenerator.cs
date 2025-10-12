
using UnityEngine;
using static BaseAbility;

public class CARandomGenerator : MonoBehaviour
{


    public AbilityTargetingOptions GetRandomAbilityTargetingOptions()
    {
        return (AbilityTargetingOptions)Random.Range(0,System.Enum.GetValues(typeof(AbilityTargetingOptions)).Length);
    }
    public OneTargetOptions GetRandomOneTargetOptions()
    {
        return (OneTargetOptions)Random.Range(0,System.Enum.GetValues(typeof (OneTargetOptions)).Length);
    }
  
    public EffectTypes GetRandomEffectType()
    {
        return (EffectTypes)Random.Range(0, System.Enum.GetValues(typeof(EffectTypes)).Length);
    }

   
    public void GenerateRandomAbility()
    {
        AbilityTargetingOptions abilityTargetingOptions = GetRandomAbilityTargetingOptions();

        if (abilityTargetingOptions == AbilityTargetingOptions.OneTarget)
        {
            OneTargetOptions oneTargetOptions = GetRandomOneTargetOptions();
        }
        //EffectTypes abilityEffectType = GetRandomEffectType();
        EffectTypes abilityEffectType = EffectTypes.Damage;
        AbilityStruct randomAbility = new AbilityStruct();
        CABaseEffect abilityEffect = new CABaseEffect(null, EffectExecutionType.Instant);
        randomAbility.Stats = ScriptableObject.CreateInstance<CABaseEffectStats>();


        randomAbility.AbilityEffect = abilityEffect;
        randomAbility.EffectTypes = abilityEffectType;

        Debug.Log(randomAbility.EffectTypes);
        //PlayerScript.Instance.ChangeAbility(0,)
    }

    private void Start()
    {

        GlobalEventManager.EventOpenChest.AddListener(OnOpenChest);

    }

    private void OnOpenChest(bool repetative)
    {
        GenerateRandomAbility();
    }
}

