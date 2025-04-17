using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

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
            if(t.gameObject.TryGetComponent<Image>(out Image image))
            {
                CooldownImage = image;
            }

            if (t.gameObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
            {
                CooldownText = text;
            }

        }
        BasePlayerAbility activatedAbility = _abilities[AbilityNumber - 1] as BasePlayerAbility;
        
        if (PlayerScript.Instance.TriggerAbility(AbilityNumber))
        {
            abilityFinishedDelegate = delegate { StartAbilityTimer(CooldownImage, CooldownText, activatedAbility); };
            activatedAbility.AbilityFinishedEvent.AddListener(abilityFinishedDelegate);
            CooldownText.gameObject.SetActive(true);
            CooldownImage.fillAmount = 1;
            
  
        }
        
        //AbilityDelegate= StartAbilityTimer(CooldownImage, CooldownText, activatedAbility);
        


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
        DrawAbilityDistance();
        
        
    }
    public void DeselectAbility() 
    { 
        
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
            UpdateAbilityButtonGUI(abilityButton, index);
            index++;

        }
    }
    private void UpdateAbilityButtonGUI(GameObject abilityButton,int index)
    {

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
