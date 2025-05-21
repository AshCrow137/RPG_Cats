using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterUIScript : MonoBehaviour
{
    private Canvas canvas;
    private TMP_Text damageTakenText;
    [SerializeField]
    private GameObject damageTextPrefab;

    private GameObject[] damageTexts = new GameObject[8];

    private int index = 0;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        damageTakenText = canvas.GetComponentInChildren<TMP_Text>();
        damageTakenText.gameObject.SetActive(false);
        for(int i = 0; i < damageTexts.Length; i++)
        {
            damageTexts[i] = Instantiate(damageTextPrefab,canvas.transform);
        }
    }

    public void ShowTakenDamage(float damage)
    {
        TMP_Text dText = damageTexts[index].GetComponent<TMP_Text>();
       
        dText.text = damage.ToString();
        dText.gameObject.SetActive(true);
        dText.transform.localPosition = Vector3.right * Random.Range(-0.3f, 0.3f);
        StartCoroutine(DamageTextAnimation(dText));
        index++;
        if (index > 7)
        {
            index = 0;
        }
    }
    private IEnumerator DamageTextAnimation(TMP_Text damageText)
    {
        float t1 = Time.time;
        float t2 = Time.time;
         
        while (t2-t1<1)
        {
            t2 += Time.deltaTime;
            yield return null;
            damageText.transform.Translate(Vector3.up * Time.deltaTime) ;

        }
        damageText.gameObject.SetActive(false) ;
        
    }


}
