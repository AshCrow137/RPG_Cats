using System.Collections;
using System.Collections.Generic;
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

    public float getHealth() 
    {
        return health; 
    }
    public void ChangeHealth(float amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    public void changeEnergy(float amount) { 
        energy += amount; 
        if(energy > maxEnergy)
        {
            energy = maxEnergy;
        }
    }
    private void EnergyRegeneration()
    {
        if (energy<maxEnergy)
        {
            changeEnergy( energyRegen*Time.deltaTime );

        }
    }
    public int GetPriority()
    {
        return priority;
    }
}
