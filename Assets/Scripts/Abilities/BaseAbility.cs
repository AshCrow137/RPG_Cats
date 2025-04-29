using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class BaseAbility : MonoBehaviour
{
    //Protected
    [Header("ActivatedAbility targeting options")]
    [SerializeField]
    protected bool hasTarget = true;
    [SerializeField,ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And, nameof(hasTarget))]
    protected AbilityTargetingOptions tagertOption;
    [SerializeField]
    protected OneTargetOptions OneTargetOptions;
    [Header("ActivatedAbility parametres")]
    [SerializeField]
    protected float damage = 1f;
    [SerializeField]
    protected float cooldown;
    [SerializeField]
    protected int cost;
    [SerializeField]
    protected float duration;// instant if duration = 0
    [SerializeField]
    protected float distance;
    [SerializeField]
    protected float castTime;
    [Header("ActivatedAbility type options")]
    [SerializeField]
    protected AbilityExecutionType abilityExecutionType;

    [SerializeField]
    protected bool canBeInterrupted = true;
    [SerializeField]
    protected bool canMoveWhileCastiong = false;
    // bool has target, bool activable, 
    // bool has target, bool activable, 
    // абилки в одну цель, абилки по шаблону (круг, линия, точка в пределах дистанции)

    [Header("ActivatedAbility sound options")]
    [SerializeField]
    protected AudioClip[] abilitySounds;
    [SerializeField]
    protected AudioClip abilityActivationSound;
    [SerializeField]
    protected AudioClip abilityExecutionSound;
    [SerializeField]
    protected AudioClip abilityFinishedSound;
    [Header("ActivatedAbility visual options")]
    [SerializeField]
    protected Sprite AbilityIcon;

    protected CharacterScript abilityOwner;
    protected IEnumerator AbilityExecutionCoroutine;
    protected IEnumerator AbilityCastingCoroutine;

    //Private
    private AbilityState abilityState;
  
    private float RestCooldownTime;

    //Public
    //public  UnityEvent AbilityReadyEvent = new UnityEvent();
    //public  UnityEvent AbilityExecutingEvent = new UnityEvent();
    [HideInInspector]
    public UnityEvent AbilityFinishedEvent = new UnityEvent();
    protected AudioSource audioSource;

    protected Parameters ownerParameters;

    #region UnityMethods
    protected virtual void Start()
    {
        abilityState = AbilityState.Ready;
        abilityOwner = GetComponentInParent<CharacterScript>();
        ownerParameters = abilityOwner.gameObject.GetComponent<Parameters>();
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
        audioSource = GetComponent<AudioSource>();
        if(!audioSource)
        {
            Debug.LogError($"{gameObject} missing AudioSource component");
        }
    }
    protected float CalculateResultDamage()
    {
        float resultDamage = (damage + ownerParameters.GetMultuplyer(ParameterToBuff.DamageFlatMultiplyer)) * ((100 + ownerParameters.GetMultuplyer(ParameterToBuff.DamagePercentMultiplyer)) / 100);
        if (resultDamage < 0 )
        {
            resultDamage = 0;
        }
        return resultDamage;
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
        if(abilityExecutionSound != null)
        {
            audioSource.clip = abilityExecutionSound;
            audioSource.Play();
        }
        //print($"execute {this.name}");
    }
    protected virtual void AbilityReady()
    {
        abilityState = AbilityState.Ready;
        //AbilityReadyEvent.Invoke();
    }
    protected virtual void OnFinishAbility()
    {
        //print($"execute {this.name} finished");
        if(abilityFinishedSound!=null)
        {
            audioSource.clip = abilityFinishedSound;
            audioSource.Play();
        }
        StartCoroutine(AbilityCooldownTimer());
        AbilityFinishedEvent.Invoke();
    }
    #endregion

    public virtual bool ActivateAbility(GameObject source, GameObject Target)
    {
        Parameters activatorParams = source.GetComponent<Parameters>();

        if (activatorParams == null)
        {
            Debug.LogError($"{source.name} has no params");
            return false;
        }
        if (Target == null)
        {
            Debug.LogError($"{this.name} has no target!");
            return false;
        }
        if (activatorParams.getEnergy() >= cost)
        {


            if (CanActavateAbility())
            {
                
                activatorParams.changeEnergy(-cost);
                //AbilityExecutingEvent.Invoke();
                if(castTime>0)
                {
                    if(!canMoveWhileCastiong)
                    {
                        abilityOwner.GetMovementScript().ChangeMovementPossibility(false);
                    }
                    //animations
                    abilityOwner.GetAnimationScript().StartCastAnimation();
                    AbilityCastingCoroutine = AbilityCastingTimer();
                    StartCoroutine(AbilityCastingCoroutine);

                }
                else
                {
                    TryToExecuteAbility();
                }
              
                if(audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                AudioClip audioClip = GetRandomAbilitySound();
                if (true)
                {
                    audioSource.clip = audioClip;
                    audioSource.Play();
                }
                
                return true;
            }
            else
            {
                print("cooldown!");
                return false;
            }
        }
        else
        {
            print("Not enought energy! ");
            return false;
        }

    }
    protected virtual void TryToExecuteAbility()
    {
        //if (abilityExecutionType == AbilityExecutionType.Continuous)
        //{
        //    AbilityExecutionCoroutine = AbilityExecuteTimer();
        //    abilityState = AbilityState.Executing;
        //    StartCoroutine(AbilityExecutionCoroutine);
        //}
        //else if (abilityExecutionType == AbilityExecutionType.Instant)
        //{
        //    print("try to execute instant ability");
        //    ExecuteAbility();
        //    OnFinishAbility();
        //}
        AbilityExecutionCoroutine = AbilityExecuteTimer();
        abilityState = AbilityState.Executing;
        StartCoroutine(AbilityExecutionCoroutine);

    }
    public void InterruptAbility()
    {
        if(canBeInterrupted)
        {
            StopCoroutine(AbilityCastingCoroutine);
            StartCoroutine(AbilityCooldownTimer());
        }
    }

    #region AbilityStats
    public Sprite GetAbilityIcon()
    {
        if (AbilityIcon)
        {
            return AbilityIcon;
        }
        else
        {
            Debug.LogError($"{gameObject.name} ability missing ability icon! ");
            return null;
        }
    }
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
    public float GetAbilityDistance()
    {
        return distance;
    }
    public float GetRestCooldownTime()
    {
        return RestCooldownTime;
    }
    public AudioClip GetRandomAbilitySound()
    {
        if(abilitySounds.Length > 0)
        {
            return abilitySounds[UnityEngine.Random.Range(0, abilitySounds.Length)];
        }
        else return null;
    }
    #endregion
    #region Timers

    protected virtual IEnumerator AbilityExecuteTimer()
    {
       
        float t = 0;
        while (t <= duration)
        {
            ExecuteAbility();
            t += Time.deltaTime;
            yield return null;
        }

        OnFinishAbility();

    }
    protected virtual IEnumerator AbilityCastingTimer()
    {
        abilityState = AbilityState.Casting;
        float t = 0;
        while(t <= castTime)
        {
            t += Time.deltaTime;
            yield return null;
        }
        if (!canMoveWhileCastiong)
        {
            abilityOwner.GetMovementScript().ChangeMovementPossibility(true);
        }
        abilityOwner.GetAnimationScript().StopCastingAnimation();
        TryToExecuteAbility();
        
    }
    protected IEnumerator AbilityCooldownTimer()
    {
        RestCooldownTime = cooldown;
        abilityState = AbilityState.Cooldown;

        while (RestCooldownTime>0)
        {
            //print(RestCooldownTime);
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
 
    #endregion
    private enum AbilityState
    {
        Ready,
        Casting,
        Executing,
        Cooldown,
        Disabled
    }
    protected enum AbilityExecutionType
    {
        Instant,
        Continuous
    }
    protected enum AbilityTargetingOptions
    {
        OneTarget,
        EveryEnemyWithinTemplate,
        Template
    }
}
