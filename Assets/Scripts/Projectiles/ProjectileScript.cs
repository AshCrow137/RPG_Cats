using System.Collections;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{


    [SerializeField] 
    private float projectileSpeed = 5;
    [SerializeField]
    protected AudioClip[] ProjectileHitSounds;
    private AudioSource audioSource;
    //[SerializeField] private float angleChangingSpeed = 100;
    private float damage;
    private Rigidbody2D rb;
    private GameObject owner;
    private GameObject projectileTarget;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, 5);
    }

    public virtual void Create(GameObject Source, GameObject Target, float damage)
    {
        CharacterScript character = Source.GetComponent<CharacterScript>();
        if (character == null) { Debug.LogError("There is no Character script!"); return; }
        GameObject forwardObject = character.GetForwardObject();
        Quaternion quaternion = forwardObject.transform.rotation;
        GameObject projectile = Instantiate(this.gameObject, forwardObject.transform.position, quaternion);
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
        //else
        //{
        //    //Debug.LogError($"No valid target for projectile!");
        //    Destroy(this.gameObject);
        //}
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, projectileTarget.transform.position, projectileSpeed * Time.deltaTime);
        Vector3 targetDirection = projectileTarget.transform.position - transform.position;
        transform.up = targetDirection.normalized;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.TryGetComponent<CharacterScript>(out CharacterScript character))
        {
            if (character.gameObject == projectileTarget)
            {
                character.TakeDamage(damage);
                audioSource.clip = GetRandomProjectileSound();
                audioSource.Play();
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(SoundCoroutine());
            }

        }
    }
    private IEnumerator SoundCoroutine()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        Destroy(this.gameObject);

    }
    private AudioClip GetRandomProjectileSound()
    {
        return ProjectileHitSounds[Random.Range(0, ProjectileHitSounds.Length)];
    }
}
