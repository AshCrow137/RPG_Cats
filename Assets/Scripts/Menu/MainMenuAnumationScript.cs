using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuAnumationScript : MonoBehaviour
{
    [SerializeField]
    private Vector2 targetPosition;
    [SerializeField]
    private float time = 1;

 



    //TODO Change start to constructor
    void Start()
    {
        LeanTween.moveLocal(gameObject, targetPosition, time);
        
    }


}
