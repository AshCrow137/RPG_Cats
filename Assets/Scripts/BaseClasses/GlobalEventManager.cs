using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class GlobalEventManager
{
    public class TurnManagerEvents
    {
        public static UnityEvent Event_StartCombat = new UnityEvent();
        public static UnityEvent<List<CombatCharacter>> Event_CombatPrepareFinished = new UnityEvent<List<CombatCharacter>>();
        public static UnityEvent Event_StopCombat = new UnityEvent();
        public static UnityEvent<int> Event_StartRound = new UnityEvent<int>();
        public static UnityEvent<CombatCharacter> Event_StartTurn = new UnityEvent<CombatCharacter>();
        public static UnityEvent<CombatCharacter> Event_EndTurn = new UnityEvent<CombatCharacter>();
        public static void StartRound(int currentRound)
        {
            Event_StartRound.Invoke(currentRound);
        }
        public static void FinishCombatPrepare(List<CombatCharacter> characters)
        {
            Event_CombatPrepareFinished.Invoke( characters);
        }
        public static void StartTurn(CombatCharacter activeCharacter)
        {
            Event_StartTurn.Invoke(activeCharacter);
        }
        public static void EndTurn(CombatCharacter character)
        {
            Event_EndTurn.Invoke(character);
        }
    }
    

    public static void StartCombat()
    {
       TurnManagerEvents.Event_StartCombat.Invoke();
    }
    public static void StopCombat()
    {
      TurnManagerEvents.Event_StopCombat.Invoke();
    }
   
}
  

