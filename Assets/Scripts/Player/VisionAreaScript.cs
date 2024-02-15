using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class VisionAreaScript : MonoBehaviour
{
    private CharacterScript _characterScript;

    private void Start()
    {
        _characterScript = GetComponentInParent<CharacterScript>();
        if (!_characterScript)
        {
            Debug.LogError($"{gameObject} vidion area does not attached to character!");
            return;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyScript>(out EnemyScript enemy))
        {
            _characterScript.AddEnemyToEnemyList(enemy);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyScript>(out EnemyScript enemy))
        {
            _characterScript.RemoveEnemyFromEnemyList(enemy);
        }
    }
}
