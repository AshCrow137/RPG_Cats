using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManagerScript : MonoBehaviour
{
    [Serializable]
    private struct CombatCharacter
    {
        private CharacterScript character;
        public int initiative { get; private set; }

        public CombatCharacter(CharacterScript c,int i)
        {
            character = c;
            initiative = i;
        }
       
    }

    public static TurnManagerScript Instance;
    private int CurrentRound;

    
    private List<CombatCharacter> CombatCharacterList;
    private List<CombatCharacter> TurnOrderList;
   // private Dictionary<CharacterScript,int> inBattleCharacters;
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
        GlobalEventManager.TurnManagerEvents.Event_StartCombat.AddListener(OnStartCombat);
        GlobalEventManager.TurnManagerEvents.Event_StopCombat.AddListener(OnEndCombat);
        GlobalEventManager.TurnManagerEvents.Event_StartRound.AddListener(OnStartRound);
        //inBattleCharacters = new Dictionary<CharacterScript, int>();
        CombatCharacterList = new List<CombatCharacter>();
    }
    public void AddToCombatants(CharacterScript character, int initiative)
    {
        
        CombatCharacterList.Add(new CombatCharacter(character,initiative));
        //inBattleCharacters.Add(character,initiative);
        //Debug.Log($"{character.gameObject.name} added, initiative: {initiative}");
        //foreach (KeyValuePair<CharacterScript, int> kvp in inBattleCharacters)
        //{
        //    Debug.Log($"{kvp.Key}:{kvp.Value}");
        //}
    }
    private void OnStartCombat()
    {
       
        CurrentRound = 0;
        Debug.Log($"combat start");
    }
    private void OnEndCombat()
    {
        CombatCharacterList.Clear();
    }
    private void OnStartRound()
    {
        CurrentRound++;
       
        TurnOrderList = new List<CombatCharacter>();
        for(int i = 0; i < CombatCharacterList.Count; i++)
        {
            int minInit = i;
            for(int j = i+1; j < CombatCharacterList.Count; j++) 
            {
                if (CombatCharacterList[j].initiative < CombatCharacterList[minInit].initiative)
                {
                    minInit= j;
                }
            }
            CombatCharacterList[i] = CombatCharacterList[minInit];
            CombatCharacterList[minInit] = CombatCharacterList[i];
        }
    }
    #region BUTTONS
    //Только для кнопок, TODO убрать это к чертям
    public static void StartCombat()
    {
        GlobalEventManager.StartCombat();
    }
    public static void StopCombat()
    {
        GlobalEventManager.StopCombat();
    }
    #endregion
}
