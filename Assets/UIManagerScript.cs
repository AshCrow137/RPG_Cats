using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{
    public static UIManagerScript Instance;
    [SerializeField]
    private GameObject TurnOrderPanel;
    [SerializeField]
    private GameObject TurnOrderContentPanel;
    [SerializeField]
    private GameObject CharacterImageObject;
    [SerializeField]
    private TMP_Text RoundText;

    
    private CombatCharacter ActiveCharacter;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);

        }
        else
        {
            Instance = this;
        }
        if (!TurnOrderPanel)
        {
            Debug.LogError("Turn order panel does not attached to UI Manager");
        }
        TurnOrderPanel.SetActive(false);
        GlobalEventManager.TurnManagerEvents.Event_CombatPrepareFinished.AddListener(SetupTurnOrderPanel);
        GlobalEventManager.TurnManagerEvents.Event_StopCombat.AddListener(ClearTurnOrderPanel);
        GlobalEventManager.TurnManagerEvents.Event_StartRound.AddListener(ChangeRound);
        GlobalEventManager.TurnManagerEvents.Event_StartTurn.AddListener(OnStartTurn);
        GlobalEventManager.TurnManagerEvents.Event_EndTurn.AddListener(OnEndTurn);
    }
    private void SetupTurnOrderPanel(List<CombatCharacter> inCombatCharacter)
    {
        print($"turn orde panel characters counT: {inCombatCharacter.Count}");

        int i = 0;
        foreach(CombatCharacter character in inCombatCharacter)
        {
            character.TurnOrderIconObject.transform.SetSiblingIndex(i);
            //GameObject clone = Instantiate(CharacterImageObject, TurnOrderContentPanel.transform);
            //clone.name = $"{character.character.name} " ;
            //clone.GetComponent<Image>().sprite = character.character.GetCharacterIcon();

            //turnOrdericonsList.Add(TurnOrderContentPanel.transform.GetChild(i).gameObject);
            i++;

        }
        
        TurnOrderPanel.SetActive(true);
    }
    public GameObject CreateTurnOrderIconObject(CharacterScript c)
    {
        GameObject clone = Instantiate(CharacterImageObject, TurnOrderContentPanel.transform);
        clone.name = $"{c.name}IconObject";
        clone.GetComponent<Image>().sprite = c.GetCharacterIcon();
        return clone;
    }
    private void ClearTurnOrderPanel()
    {
        TurnOrderPanel.SetActive(false);
        foreach (Transform obj in TurnOrderContentPanel.transform) 
        {
            Destroy(obj.gameObject);
        }

    }
    public void EndCurrentTurn()
    {
        GlobalEventManager.TurnManagerEvents.EndTurn(ActiveCharacter);
     
    }
    private void OnEndTurn(CombatCharacter character)
    {
       
        character.TurnOrderIconObject.transform.SetSiblingIndex(TurnManagerScript.Instance.GetTurnOrderList().Count);


    }
    private void ChangeRound(int newRound)
    {
        RoundText.text =  newRound.ToString();
    }
    private void OnStartTurn(CombatCharacter character)
    {
        ActiveCharacter = character;

    }
}
