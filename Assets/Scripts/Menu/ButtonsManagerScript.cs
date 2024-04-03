using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ButtonsManagerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] abilityButtons = new GameObject[3];
    private BaseAbility[] abilities;
 
    public void ActivateAbility(int AbilityNumber)
    {
        
        GameObject abilityButton = abilityButtons[AbilityNumber-1];
        Image CooldownImage = null;
        TextMeshProUGUI CooldownText = null;
        foreach (Transform t in abilityButton.transform)
        {
            if(t.gameObject.TryGetComponent<Image>(out Image image))
            {
                CooldownImage = image;
            }

            if (t.gameObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
            {
                CooldownText = text;
            }

        }
        CooldownText.gameObject.SetActive(true);
        CooldownImage.fillAmount = 1;


        PlayerScript.Instance.TriggerAbility(AbilityNumber);
        BaseAbility activatedAbility = abilities[AbilityNumber-1];
    
        
    }


    private void Awake()
    {
        abilities = PlayerScript.Instance.GetAbilityList();
        
        int index = 0;
        foreach(GameObject abilityButton in abilityButtons)
        {
            UpdateAbilityButtonGUI(abilityButton, index);
            index++;

        }
    }
    private void UpdateAbilityButtonGUI(GameObject abilityButton,int index)
    {

        Image ButtonAbilityImage = abilityButton.GetComponent<Image>();
        BasePlayerAbility ability = abilities[index] as BasePlayerAbility;
        
        if (ability != null)
        {
            Sprite AbilityIcon = ability.GetAbilityIcon();
            ButtonAbilityImage.sprite = AbilityIcon;
        }
    }
    private IEnumerator StartButtonCooldownCoroutine(Image cdImage, TextMeshProUGUI cdText, BaseAbility ability)
    {
        float restTime = ability.GetRestCooldownTime();
        float totalTime = ability.getCooldown();
        print(restTime);
        print(totalTime);
        while (restTime > 0)
        {
            restTime = ability.GetRestCooldownTime();
            cdImage.fillAmount = restTime / totalTime;
            cdText.text = restTime.ToString();
            print(restTime);
            yield return null;
        }
        cdText.gameObject.SetActive(false);
    }
}
