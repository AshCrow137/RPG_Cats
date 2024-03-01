using System.Collections;
using UnityEngine;

public class PlayerScript : CharacterScript
{
    public static PlayerScript Instance { get; private set; }
    [SerializeField]
    private GameObject BasicAttackUI;

    protected override void Awake()
    {
        base.Awake();
        BasicAttackUI.SetActive(false);
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }
    public void DrawBasicAttackRadius()
    {
        BasicAttackUI.transform.localScale = new Vector3(baseAttack.GetAbilityDistance(), baseAttack.GetAbilityDistance(), 1);
        BasicAttackUI.SetActive(true);
    }
    public void StopDrawAttackRadius()
    {
        BasicAttackUI.SetActive(false);
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
}

