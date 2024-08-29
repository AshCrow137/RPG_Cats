using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerScript : CharacterScript
{
    public static PlayerScript Instance { get; private set; }
    [SerializeField]
    private GameObject BasicAttackRadiusObject;


    [Inject]
    private void Constructor()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        BasicAttackRadiusObject.SetActive(false);
      

    }
    public void DrawBasicAttackRadius()
    {
        BasicAttackRadiusObject.transform.localScale = new Vector3(baseAttack.GetAbilityDistance(), baseAttack.GetAbilityDistance(), 1);
        BasicAttackRadiusObject.SetActive(true);
    }
    public void StopDrawAttackRadius()
    {
        BasicAttackRadiusObject.SetActive(false);
    }

    protected override void OnStartCombat()
    {
        base.OnStartCombat();
        TurnManagerScript.Instance.AddToCombatants(this, parameters.GetInitiative());
        foreach (CharacterScript character in Enemylist)
        {
            TurnManagerScript.Instance.AddToCombatants(character, character.getCharacterParameters().GetInitiative());
        }
        GlobalEventManager.TurnManagerEvents.StartRound();
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
    public override void AddEnemyToEnemyList(CharacterScript Enemy)
    {
        //if (Enemy.GetType() != typeof(EnemyScript))
        //{
        //    return;
        //}
        base.AddEnemyToEnemyList(Enemy);
    }
    public BaseAbility[] GetAbilityList()
    {
        return abilityArray;
    }
}

