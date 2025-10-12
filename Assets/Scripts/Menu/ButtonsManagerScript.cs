using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEditor.Playables;

public class ButtonsManagerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] abilityButtons = new GameObject[3];
    [SerializeField]
    private GameObject basicAttackButton;
    private BaseAbility[] _abilities;

    private delegate void StartAbilityTimerDelegate(Image cdImage, TextMeshProUGUI cdText, BaseAbility ability);
    StartAbilityTimerDelegate AbilityDelegate;
    private BasePlayerAbility selectedAbility;

    private bool cancelAbility = false;

    private Joystick abilityJoystick;
    private UnityAction abilityFinishedDelegate;
    private UnityAction abilityCastingFinishedDelegate;
   
    //public ButtonsManagerScript(BaseAbility[] abilities) 
    //{
    //    print("Constuctor");


    //}

    public void ActivateAbility(int AbilityNumber)
    {
        
        GameObject abilityButton = abilityButtons[AbilityNumber-1];
        Image CooldownImage = null;
        TextMeshProUGUI CooldownText = null;
        foreach (Transform t in abilityButton.transform)
        {
            if(t.gameObject.TryGetComponent<Image>(out Image image)&& t.gameObject.tag == "CooldownImage")
           
            
                {
                    CooldownImage = image;
                }
            if (t.gameObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
            {
                CooldownText = text;
            }

        }
        BasePlayerAbility activatedAbility = _abilities[AbilityNumber - 1] as BasePlayerAbility;

        abilityCastingFinishedDelegate = delegate { StartCastingTimer(CooldownImage, CooldownText, activatedAbility); };
        activatedAbility.AbilityCastFinishedEvent.AddListener(abilityCastingFinishedDelegate);
        if (PlayerScript.Instance.TriggerAbility(AbilityNumber,activatedAbility.SelectTarget()))
        {
            CooldownText.text = "casting";//TODO add button animations here
            CooldownText.gameObject.SetActive(true);
        
        }
        else
        {
            activatedAbility.AbilityCastFinishedEvent.RemoveListener(abilityCastingFinishedDelegate);
        }

    }
    private void StartCastingTimer(Image cdImage, TextMeshProUGUI cdText, BasePlayerAbility ability)
    {
        
        ability.AbilityCastFinishedEvent.RemoveListener(abilityCastingFinishedDelegate);
        
        abilityFinishedDelegate = delegate { StartAbilityTimer(cdImage, cdText, ability); };
        ability.AbilityFinishedEvent.AddListener(abilityFinishedDelegate);  
        
    }

   
    public void OnDragDelegate(BaseEventData data )
    {
        if(abilityJoystick.isActiveAndEnabled)
        {
            
            abilityJoystick.OnDrag(data as PointerEventData);
            Vector2 direction = abilityJoystick.Direction;
            selectedAbility.RotateAbilityTemplate(direction);
        }
        
    }
    public void SelectAbility(int abilityNumber)
    {

        cancelAbility = false;
        selectedAbility = _abilities[abilityNumber - 1] as BasePlayerAbility;
        
        CharacterScript charc = selectedAbility.SelectTarget().GetComponent<CharacterScript>();
        if( charc != null )
        {
            charc.SelectCharacter();
        }
        DrawAbilityDistance();
        
        
    }
    public void DeselectAbility() 
    {
        CharacterScript charc = selectedAbility.SelectTarget().GetComponent<CharacterScript>();
        if (charc != null)
        {
            charc.DeselectCharacter();
        }

        StopDrawAbilityDistance();
        
    }

    public void DrawAbilityTemplate(Joystick selectedAbilityJoystick)
    {
     
        abilityJoystick = selectedAbilityJoystick;
        if(selectedAbility.DrawAbilityTemplate(true))
        {
            abilityJoystick.gameObject.SetActive(true);
        }
        
    }
    public void StopDrawingAbilityTemplate(int abilityNumber)
    {
        if (selectedAbility.DrawAbilityTemplate(false))
        {
            abilityJoystick.gameObject.SetActive(false);
        }
        if (!cancelAbility)
        {
            ActivateAbility(abilityNumber);
            selectedAbility.DrawAbilityTemplate(false);
        }
    }
    public void CancelAbility()
    {
        cancelAbility=true;
    }
    public void DecancelAbility()
    {
        cancelAbility=false;
    }
    private void Awake()
    {

        PlayerScript player = PlayerScript.Instance;
        if (!player)
        {
            Debug.LogError("No player!");
            return;
        }
        _abilities = player.GetAbilityList();
        Debug.Log(_abilities);
        int index = 0;
        foreach (GameObject abilityButton in abilityButtons)
        {
            UpdateAbilityButtonGUI(index);
            index++;

        }
    }
    private void UpdateAbilityButtonGUI(int index)
    {
        GameObject abilityButton = abilityButtons[index];
        Image ButtonAbilityImage = abilityButton.GetComponent<Image>();
        BasePlayerAbility ability = _abilities[index] as BasePlayerAbility;
        
        if (ability != null)
        {
            Sprite AbilityIcon = ability.GetAbilityIcon();
            ButtonAbilityImage.sprite = AbilityIcon;
            
        }
    }
    private void StartAbilityTimer(Image cdImage, TextMeshProUGUI cdText, BaseAbility ability)
    {
        cdImage.fillAmount = 1;
        StartCoroutine(StartButtonCooldownCoroutine(cdImage, cdText, ability)); 
       
       
    }

    private IEnumerator StartButtonCooldownCoroutine(Image cdImage, TextMeshProUGUI cdText, BaseAbility ability)
    {
        float restTime = ability.GetRestCooldownTime();
        float totalTime = ability.getCooldown();
        while (restTime > 0)
        {   
            restTime = ability.GetRestCooldownTime();
            cdImage.fillAmount = restTime / totalTime;
            cdText.text = MathF.Round(restTime,1).ToString();
            yield return null;
        }
        cdText.gameObject.SetActive(false);
        ability.AbilityFinishedEvent.RemoveListener(abilityFinishedDelegate);
    }

    public void ActivateBasicAttack()
    {
        PlayerScript.Instance.TryToAttack();
    }
    private void DrawAbilityDistance( )
    {
        selectedAbility?.DrawAbilityDistance();
    }
    private void StopDrawAbilityDistance()
    {
        selectedAbility?.StopDrawingAbilityDistance ();
    }
    public void ShowElement(GameObject UIElement)
    {
        UIElement.SetActive(true);
    }
    public void HideElement(GameObject UIElement)
    {
        UIElement.SetActive(false);
    }
    public void ExitPressed()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
