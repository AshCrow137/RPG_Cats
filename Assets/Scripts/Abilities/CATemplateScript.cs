using Pathfinding.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CATemplateScript : MonoBehaviour
{
    private List<GameObject> charactersInArea = new List<GameObject>();
    private bool isActive = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<CharacterScript>(out CharacterScript enemy)&&isActive)
        {
            enemy.SelectCharacter();
            charactersInArea.Add(enemy.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<CharacterScript>(out CharacterScript enemy)&&isActive)
        {
            enemy.DeselectCharacter();
            charactersInArea.Remove(enemy.gameObject);
        }
    }
    public List<GameObject> GetCharactersInTemplate()
    {
        return charactersInArea;
    }
    public void ActivateTemplate(bool state)
        
    {
       
        isActive = state;
        GetComponentInChildren<SpriteRenderer>().enabled = state;
       
    }
    public void DeactivateTemplate()
    {
        isActive = false;
        foreach (GameObject charc in charactersInArea)
        {
            charc.GetComponent<CharacterScript>().DeselectCharacter();
        }
        charactersInArea.Clear();
    }
}
