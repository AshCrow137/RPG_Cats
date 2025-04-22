using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
[CreateAssetMenu(fileName = "BuffEffectStats", menuName = "EffectStats/BuffEffectStats")]
public class CABuffEffectStats : CABaseEffectStats
{
    public List<ParameterPairs> parametresToBuff = new List<ParameterPairs>();

}

