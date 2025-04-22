using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CATemplateScript : MonoBehaviour
{
    private List<GameObject> charactersInArea = new List<GameObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<CharacterScript>(out CharacterScript enemy))
        {
            charactersInArea.Add(enemy.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<CharacterScript>(out CharacterScript enemy))
        {
            charactersInArea.Remove(enemy.gameObject);
        }
    }
    public List<GameObject> GetCharactersInTemplate()
    {
        return charactersInArea;
    }
}
