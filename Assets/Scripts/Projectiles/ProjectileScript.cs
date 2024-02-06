using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProjectileScript : MonoBehaviour
{
    
    
    [SerializeField] private float projectileSpeed = 5;
    [SerializeField] private float angleChangingSpeed = 100;
    private float damage;
    private Rigidbody2D rb;
    private GameObject owner;
    private GameObject projectileTarget;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5);
    }

    public virtual void  Create(GameObject Source, GameObject Target, float damage)
    {
        CharacterScript character = Source.GetComponent<CharacterScript>();
        if (character == null) { Debug.LogError("There is no Character script!"); return; }
        GameObject forwardObject = character.GetForwardObject();
        Quaternion quaternion = forwardObject.transform.rotation;
        GameObject projectile = Instantiate(this.gameObject,forwardObject.transform.position, quaternion);
        ProjectileScript script = projectile.GetComponent<ProjectileScript>();
        script.projectileTarget = Target;
        script.damage = damage;
        script.owner = Source;
    }
    private void FixedUpdate()
    {
        if (projectileTarget != null)
        {
            MoveToTarget();
        }
        else
        {
            //Debug.LogError($"No valid target for projectile!");
            Destroy(this.gameObject);
        }
    }
    private void MoveToTarget()
    {

        Vector2 direction = (Vector2)projectileTarget.transform.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -angleChangingSpeed * rotateAmount;
        rb.velocity = transform.up * projectileSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.TryGetComponent<CharacterScript>(out CharacterScript character))
        {
            if (character.gameObject!= owner) {
                character.TakeDamage(damage);
                Destroy(this.gameObject);
            }
            
        }
    }
}
