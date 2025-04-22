using UnityEngine;

public enum ParameterToBuff
{
    DamageFlatMultiplyer,
    DamagePercentMultiplyer,
    IncomingDamageFlatMultiplyer,
    IncomingDamagePercentMultiplyer,
    MovementSpeedFlatMultiplyer, 
    MovementSpeedPercentMultiplyer
}
[System.Serializable]
public struct ParameterPairs
{
    [SerializeField]
    public ParameterToBuff parameter;
    [SerializeField]
    public float value;
}