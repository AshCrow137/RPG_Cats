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
    private void Start()
    {
        //AddActiveMultiplyer(ParameterToBuff.DamageFlatMultiplyer, "buff1", 9);
        //AddActiveMultiplyer(ParameterToBuff.DamageFlatMultiplyer, "buff2", 1);
        //AddActiveMultiplyer(ParameterToBuff.DamageFlatMultiplyer, "buff3", 5);
        //RemovaActiveMultiplyer(ParameterToBuff.DamageFlatMultiplyer, "buff3");
         //AddActiveMultiplyer(ParameterToBuff.MovementSpeedPercentMultiplyer, "buff3", 500);
    }
}
