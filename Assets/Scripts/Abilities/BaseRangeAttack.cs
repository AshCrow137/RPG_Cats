using UnityEngine;

public class BaseRangeAttack : BaseAttack
{
    [SerializeField] private ProjectileScript projectile;

    protected override void Attack(GameObject source, GameObject target)
    {
        base.Attack(source, target);

        GameObject forwardObject = source.GetComponent<CharacterScript>().GetForwardObject();
        if (forwardObject != null)
        {
            Vector3 direction = target.transform.position - source.transform.position;// rotate source forward object to target 
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            forwardObject.transform.eulerAngles = new Vector3(0, 0, -angle);
            projectile.Create(source, target, damage);
        }
        else
        {
            Debug.LogError($"There is no forward object attached to {source.gameObject}");
        }
    }
}
