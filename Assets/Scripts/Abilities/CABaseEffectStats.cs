using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CABaseEffectStats : ScriptableObject

{
    [SerializeField]
    public List<GameObject> Targets = new List<GameObject>();
    [SerializeField]
    public float EffectDuration;
    [SerializeField]
    public  EffectExecutionType ExecutionType;
}
