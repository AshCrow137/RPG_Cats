using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimationScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private float OnPointerEnterNewScale = 1.3f;
    [SerializeField]
    private float TransitionTime = 0.3f;
    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.scale(gameObject, Vector3.one * OnPointerEnterNewScale, TransitionTime);
        print($"{this.name} selected");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.scale(gameObject, Vector3.one, TransitionTime);
    }
}
