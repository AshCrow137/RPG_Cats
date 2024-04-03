using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePlayerAbility : BaseAbility
{
    [SerializeField]
    protected Sprite AbilityIcon;

    public Sprite GetAbilityIcon()
    { return AbilityIcon; }
}
