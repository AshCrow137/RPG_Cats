using System.Collections.Generic;
using UnityEngine;

public class Parameters : MonoBehaviour
{


    [Header("Health")]
    [SerializeField]

    protected float health;
    [SerializeField]
    protected float maxHealth;

    [Header("Energy")]
    [SerializeField]
    protected float energy;

    [SerializeField]
    protected int maxEnergy;
    [SerializeField]
    protected float energyRegen;

    [Header("Targeting")]
    [SerializeField]
    protected int priority = 1;

    [Header("BaseDamage")]
    [SerializeField]
    protected float damageFlatMultiplyer;
    [SerializeField]
    protected float damagePercentMultiplyer = 0;//(100+PDM)/100
    [SerializeField]
    protected float incomingDamagePercentMultiplyer = 0;
    [SerializeField]
    protected float incomingDamageFlatMultiplyer = 0;
    [Header("Movement")]
    [SerializeField]
    protected float movementSpeedFlatMultiplyer;
    [SerializeField]
    protected float movementSpeedPercentMultiplyer;

    private Dictionary<string, float> ActiveDamageFlatMultiplyer = new Dictionary<string, float>();
    private Dictionary<string, float> ActiveDamagePercentMultiplyer = new Dictionary<string, float>();
    private Dictionary<string, float> ActiveIncomingDamageFlatMultiplyer = new Dictionary<string, float>();
    private Dictionary<string, float> ActiveIncomingDamagePercentMultiplyer = new Dictionary<string, float>();
    private Dictionary<string, float> ActiveMovementSpeedFlatMultiplyer = new Dictionary<string, float>();
    private Dictionary<string, float> ActiveMovementSpeedPercentMultiplyer = new Dictionary<string, float>();
    private void Awake()
    {
        health = maxHealth;
        energy = maxEnergy;
    }
    private void FixedUpdate()
    {
        EnergyRegeneration();
    }

    public float  GetHealth()
    {
        return health;
    }
   /// <summary>
   /// minus is damage, plus is heal
   /// </summary>
   /// <param name="amount"></param>
    public virtual void ChangeHealth(float amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if(health<0)
        {
            health = 0;
        }
    }
    public virtual void changeEnergy(float amount)
    {
        energy += amount;
        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }
    }
    public void changeEnergyRegen(float value)
    { 
        energyRegen += value;
    }
    public float getEnergy() { return energy; }
    private void EnergyRegeneration()
    {
        if (energy < maxEnergy)
        {
            changeEnergy(energyRegen * Time.deltaTime);

        }
    }
    public int GetPriority()
    {
        return priority;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
   


    private (Dictionary<string,float>, float) ChooseDictionary(ParameterToBuff type)
    {
        Dictionary<string, float> activeDict = new Dictionary<string, float>();
        float resultMultiplyer = 0;
        switch (type)
        {
            case ParameterToBuff.DamageFlatMultiplyer:
                activeDict = ActiveDamageFlatMultiplyer;
                resultMultiplyer = damageFlatMultiplyer;
                break;
            case ParameterToBuff.DamagePercentMultiplyer:
                activeDict = ActiveDamagePercentMultiplyer;
                resultMultiplyer = damagePercentMultiplyer;
                break;
            case ParameterToBuff.IncomingDamageFlatMultiplyer:
                activeDict = ActiveIncomingDamageFlatMultiplyer;
                resultMultiplyer = incomingDamageFlatMultiplyer;
                break;
            case ParameterToBuff.IncomingDamagePercentMultiplyer:
                activeDict = ActiveIncomingDamagePercentMultiplyer;
                resultMultiplyer = incomingDamagePercentMultiplyer;
                break;
            case ParameterToBuff.MovementSpeedFlatMultiplyer:
                activeDict = ActiveMovementSpeedFlatMultiplyer;
                resultMultiplyer = movementSpeedFlatMultiplyer;
                break;
            case ParameterToBuff.MovementSpeedPercentMultiplyer:
                activeDict = ActiveMovementSpeedPercentMultiplyer;
                resultMultiplyer = movementSpeedPercentMultiplyer;
                break;

        }
        return (activeDict,resultMultiplyer);
    }
    public float GetMultuplyer(ParameterToBuff type)
    {
        (Dictionary<string, float> activeDict,float resultMultiplyer) = ChooseDictionary(type);
        if (activeDict.Count > 0)
        {
            resultMultiplyer = 0;
            foreach (KeyValuePair<string, float> pair in activeDict)
            {
                resultMultiplyer += pair.Value;
            }
        }
        return resultMultiplyer;


    }
    public void AddActiveMultiplyer(ParameterToBuff type ,string source, float value)
    {
        (Dictionary<string, float> activeDict, float resultMultiplyer) = ChooseDictionary(type);
        switch (type)
        {
            case ParameterToBuff.DamageFlatMultiplyer:
                activeDict = ActiveDamageFlatMultiplyer;
                break;
            case ParameterToBuff.DamagePercentMultiplyer:
                activeDict = ActiveDamagePercentMultiplyer;
                break;
            case ParameterToBuff.IncomingDamageFlatMultiplyer:
                activeDict = ActiveIncomingDamageFlatMultiplyer;
                break;
            case ParameterToBuff.IncomingDamagePercentMultiplyer:
                activeDict = ActiveIncomingDamagePercentMultiplyer;
                break;

                
        }
        if (activeDict.ContainsKey(source))
        {
            activeDict[source] = value;

        }
        else
        {
            activeDict.Add(source, value);
        }
    }
    public void RemovaActiveMultiplyer(ParameterToBuff type, string source)
    {
        (Dictionary<string, float> activeDict, float resultMultiplyer) = ChooseDictionary(type);
        if (activeDict.ContainsKey(source))
        {
            activeDict.Remove(source);

        }
    }
}
