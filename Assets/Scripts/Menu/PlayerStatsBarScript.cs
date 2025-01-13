using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsBarScript : MonoBehaviour
{
    [SerializeField]
    private Image healthForegroundImage;
    [SerializeField]
    private Image energyForegroundImage;

    private void Start()
    {
        if (!healthForegroundImage)
        {
            Debug.LogError("Missing health foreground image!");
        }
        if (!energyForegroundImage)
        {
            Debug.LogError("Missing energy foreground image!");
        }
        GlobalEventManager.EventPlayerEnergyChanged.AddListener(OnPlayerEnergyChanged);
        GlobalEventManager.EventPlayerHealthChanged.AddListener(OnPlayerHealthChanged);
    }
    private void OnPlayerHealthChanged(float newHealth,float maxHealth)
    {
        healthForegroundImage.fillAmount = newHealth / maxHealth;
    }
    private void OnPlayerEnergyChanged(float newEnergy, float maxEnergy)
    {
        energyForegroundImage.fillAmount = newEnergy / maxEnergy;
    }
}
