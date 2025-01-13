using UnityEngine;

public class PlayerParameters : Parameters
{
    [SerializeField]
    private bool followTarget = true;

    public bool GetFollowTarget()
    {
        return followTarget;
    }
    public override void ChangeHealth(float amount)
    {
        base.ChangeHealth(amount);
        GlobalEventManager.InvokeHealthChangeEvent(health, maxHealth);
    }
    public override void changeEnergy(float amount)
    {
        base.changeEnergy(amount);
        GlobalEventManager.InvokeEnergyChangeEvent(energy, maxEnergy);
    }

}
