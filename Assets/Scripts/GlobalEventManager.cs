
using UnityEngine.Events;

public class GlobalEventManager 
{
    public static UnityEvent<float,float> EventPlayerHealthChanged = new UnityEvent<float, float>();
    public static UnityEvent<float,float> EventPlayerEnergyChanged = new UnityEvent<float, float>();
    
    public static UnityEvent<bool> EventOpenChest = new UnityEvent<bool>();

    public static UnityEvent<int> EventPlayerAbilityChanged = new UnityEvent<int>();

    public static void InvokePlayerAbilityChangedEvent(int abilityIndex)
    {
        EventPlayerAbilityChanged.Invoke(abilityIndex);
    }

    public static void InvokeHealthChangeEvent(float newHealth,float maxHealth)
    {
        EventPlayerHealthChanged.Invoke(newHealth,maxHealth);
    }
    public static void InvokeEnergyChangeEvent(float newEnergy,float maxEnergy)
    {
        EventPlayerEnergyChanged.Invoke(newEnergy,maxEnergy);
    }

    public static void InvokeOpenChestEvent(bool repetative)
    {
        EventOpenChest.Invoke(repetative);
    }
}
