using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class TurnManagerScript : MonoBehaviour
{
    public static TurnManagerScript Instance;
    private int CurrentRound;

    
    //private List<CombatCharacter> CombatCharacterList;
    private List<CombatCharacter> TurnOrderList;
    public List<CombatCharacter> GetTurnOrderList() { return TurnOrderList; }

    private bool bNextCharacter = false;

    CancellationTokenSource cancellationTokenSource;
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
        GlobalEventManager.TurnManagerEvents.Event_EndTurn.AddListener(OnEndTurn);
        //GlobalEventManager.TurnManagerEvents.Event_StartRound.AddListener(OnStartRound);

    }
    public void AddToCombatants(CharacterScript character, int initiative)
    {
        
        TurnOrderList.Add(new CombatCharacter(character,initiative,UIManagerScript.Instance.CreateTurnOrderIconObject(character)));
    }
    public void InitiateCombat(CharacterScript initiator, List<CharacterScript> CombatCharacters)
    {
        TurnOrderList = new List<CombatCharacter>();
        AddToCombatants(initiator,initiator.getCharacterParameters().GetInitiative());
        CreateCombatCharacterList(CombatCharacters);
        PrepareTurnOreder();
        GlobalEventManager.TurnManagerEvents.FinishCombatPrepare(TurnOrderList);
        StartNewRound();
    }
    public void CreateCombatCharacterList(List<CharacterScript> CombatCharacters)
    {
        
        foreach (CharacterScript character in CombatCharacters)
        {
            AddToCombatants(character, character.getCharacterParameters().GetInitiative());
        }

    }

    public void RemoveFromCombatants(CharacterScript character)
    {
        //TurnOrderList.Remove(character);
    }
    private void OnStartCombat()
    {
       
        CurrentRound = 0;
        Debug.Log($"combat start");
    }
    private void OnEndCombat()
    {
        
        TurnOrderList.Clear();
    }
    private async void StartNewRound()
    {
        CurrentRound++;
        GlobalEventManager.TurnManagerEvents.StartRound(CurrentRound);
        cancellationTokenSource = new CancellationTokenSource();
        try
        {
            for(int i = 0;i< TurnOrderList.Count;i++)
            {
                CombatCharacter character = TurnOrderList[0];
                StartNewTurn(character);
                while (!bNextCharacter)
                {
                    await Task.Yield();
                }
                bNextCharacter = false;
            }
                
            
                if(TurnOrderList.Count > 0)
                {
                    StartNewRound();
                }
        }
        catch (Exception ex) 
        {
            Debug.Log($"Task cancelled {ex}");
            return;
        }
        finally
        { 
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }

       
    }
    private void OnEndTurn(CombatCharacter character)
    {
        bNextCharacter= true;
        //CombatCharacter c = TurnOrderList[0];
        TurnOrderList.Insert(TurnOrderList.Count,character);
        TurnOrderList.RemoveAt(0);

    }
    private void StartNewTurn(CombatCharacter activeCharacter)
    {
        print($"Active character: {activeCharacter.character.name}");
        GlobalEventManager.TurnManagerEvents.StartTurn(activeCharacter);
        activeCharacter.StartCharacterTurn();
    }
    
    private void PrepareTurnOreder()
    {
        
        TurnOrderList = TurnOrderList.OrderByDescending((val) => val.initiative).ToList();
    }

    private void OnDestroy()
    {
        cancellationTokenSource?.Cancel();
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
