using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventManager
{
    public class TurnManagerEvents
    {
        public static UnityEvent Event_StartCombat = new UnityEvent();
        public static UnityEvent Event_StopCombat = new UnityEvent();
        public static UnityEvent Event_StartRound = new UnityEvent();
        public static UnityEvent Event_StartTurn = new UnityEvent();
        public static void StartRound()
        {
            Event_StartRound.Invoke();
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
  

