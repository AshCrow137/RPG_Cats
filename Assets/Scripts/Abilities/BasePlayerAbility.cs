using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePlayerAbility : BaseAbility
{
    [SerializeField]
    protected GameObject AbilityDistance;
    [SerializeField]
    protected GameObject AbilityTemplate;
    protected override void Start()
    {
        base.Start();
        AbilityDistance.SetActive(false);
        AbilityTemplate.SetActive(false);
    }
    public void DrawAbilityDistance()
    {
        AbilityDistance.transform.localScale = new Vector3(distance, distance, 1);
        AbilityDistance?.SetActive(true);
    }
    public void StopDrawingAbilityDistance()
    {
        AbilityDistance?.SetActive(false);
    }
    public bool DrawAbilityTemplate(bool draw)
        
    {
        if(hasTarget&&tagertOption==AbilityTargetingOptions.Template)
        {
            AbilityTemplate?.SetActive(draw);
            return true;
        }
        return false;
    }

    
    public void RotateAbilityTemplate(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        AbilityTemplate.transform.eulerAngles = new Vector3(0, 0, -angle);
    }
}
