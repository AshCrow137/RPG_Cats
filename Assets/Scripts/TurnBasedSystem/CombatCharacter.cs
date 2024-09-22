using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CombatCharacter
{
    public CharacterScript character { get; private set; }
    public int initiative { get; private set; }
    public GameObject TurnOrderIconObject { get; private set; }

    public void SetTurnorderImageObject(GameObject obj)
    {
        TurnOrderIconObject = obj;
    }
    public CombatCharacter(CharacterScript c, int i,GameObject iconObject)
    {
        character = c;
        initiative = i;
        TurnOrderIconObject = iconObject;
    }
    public void StartCharacterTurn()
    {
        character.OnCharacterTurnStart();
    }
}

