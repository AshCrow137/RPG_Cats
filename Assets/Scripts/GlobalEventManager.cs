
using UnityEngine.Events;

public class GlobalEventManager 
{
    public static UnityEvent<float,float> EventPlayerHealthChanged = new UnityEvent<float, float>();
    public static UnityEvent<float,float> EventPlayerEnergyChanged = new UnityEvent<float, float>();

    public static void InvokeHealthChangeEvent(float newHealth,float maxHealth)
    {
        EventPlayerHealthChanged.Invoke(newHealth,maxHealth);
    }
    public static void InvokeEnergyChangeEvent(float newEnergy,float maxEnergy)
    {
        EventPlayerEnergyChanged.Invoke(newEnergy,maxEnergy);
    }
}
