using UnityEngine;

public class Parameters : MonoBehaviour
{


    // Health params
    [SerializeField]

    protected float health;
    [SerializeField]
    protected float maxHealth;

    //Energy params
    [SerializeField]
    protected float energy;
    public float getEnergy() { return energy; }
    [SerializeField]
    protected int maxEnergy;
    [SerializeField]
    protected float energyRegen; public void changeEnergyRegen(float value) { energyRegen += value; }


    [SerializeField]
    protected int priority = 1;

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

}
