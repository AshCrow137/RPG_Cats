using UnityEngine;

public class BaseMeleeAttack : BaseAttack
{
    protected override void Attack(GameObject source, GameObject target)
    {
        base.Attack(source, target);

        print("Attack");
    }
}
