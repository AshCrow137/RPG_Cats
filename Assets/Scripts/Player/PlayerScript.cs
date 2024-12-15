using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerScript : CharacterScript
{
    public static PlayerScript Instance { get; private set; }
    [SerializeField]
    private GameObject BasicAttackRadiusObject;

    private IEnumerator damageCoroutine;

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


    public override void BasicAttack(CharacterScript target)
    {
       base.BasicAttack(target);
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
    public override void KillCharacter()
    {

        print("Player is dead ");
        transform.position = Vector3.zero;
        parameters.ChangeHealth(parameters.GetMaxHealth());
    }
    protected override void OnTakeDamage()
    {
        if(damageCoroutine!=null)
        {
            return;
        }

        base.OnTakeDamage();
        damageCoroutine = TakeDamageCoroutine();
        StartCoroutine(damageCoroutine);
    }
    private IEnumerator TakeDamageCoroutine()
    {
        for(int i = 0;i<5; i++)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        damageCoroutine = null;
    }
}

