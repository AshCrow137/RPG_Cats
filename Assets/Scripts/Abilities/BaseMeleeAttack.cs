using UnityEngine;

public class BaseMeleeAttack : BaseAttack
{
    protected override void Attack(GameObject source, GameObject target)
    {
        base.Attack(source, target);

        print($"Attack {target}");
        CharacterScript targetCharacter = target.GetComponent<CharacterScript>();
        if(!targetCharacter)
        {
            Debug.LogError($"{target} has no CharacterScript attached to it!");
            return;
        }
        targetCharacter.TakeDamage(CalculateResultDamage());
    }
}
