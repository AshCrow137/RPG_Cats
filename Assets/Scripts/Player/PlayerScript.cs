using System.Collections;
using UnityEngine;

public class PlayerScript : CharacterScript
{
    public static PlayerScript Instance { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }


    public override void BasicAttack(CharacterScript target)
    {
        if (baseAttack != null)
        {
            if (!isAttacking)
            {
                AttackCoroutine = RepeatAttack(target);
                StartCoroutine(AttackCoroutine);
            }
        }
        else
        {
            Debug.LogError($"there is no basic attack attached to {this.name}");
        }
    }

}

