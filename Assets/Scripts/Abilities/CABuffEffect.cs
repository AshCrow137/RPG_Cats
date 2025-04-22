using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CABuffEffect : CABaseEffect
{
    private List<ParameterPairs> ParametresToBuff;
    private Parameters OwnerParameters;
    private string Source;

    public CABuffEffect(List<GameObject> targets, EffectExecutionType executionType, List<ParameterPairs> parametresToBuff, Parameters ownerParameters, string source) : base(targets,executionType)
    {
        ParametresToBuff = parametresToBuff;
        OwnerParameters = ownerParameters;
        Source = source;
    }

    public override void ActivateEffect()
    {
        base.ActivateEffect();
        foreach (ParameterPairs pair in ParametresToBuff)
        {
            OwnerParameters.AddActiveMultiplyer(pair.parameter, Source, pair.value);
        }
    }
    public override void FinishEffect()
    {
        base.FinishEffect();
        foreach (ParameterPairs pair in ParametresToBuff)
        {
            OwnerParameters.RemovaActiveMultiplyer(pair.parameter, Source);
        }
    }

}
