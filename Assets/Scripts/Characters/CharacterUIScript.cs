using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterUIScript : MonoBehaviour
{
    private Canvas canvas;
    private TMP_Text damageTakenText;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        damageTakenText = canvas.GetComponentInChildren<TMP_Text>();
        damageTakenText.gameObject.SetActive(false);
    }

    public void ShowTakenDamage(float damage)
    {
        damageTakenText.text = damage.ToString();
        damageTakenText.gameObject.SetActive(true);
        StartCoroutine(DamageTextAnimation());

    }
    private IEnumerator DamageTextAnimation()
    {
        float t1 = Time.time;
        float t2 = Time.time;
         
        while (t2-t1<1)
        {
            t2 += Time.deltaTime;
            yield return null;
            damageTakenText.transform.Translate(Vector3.up * Time.deltaTime) ;

        }
        damageTakenText.gameObject.SetActive(false) ;
    }


}
