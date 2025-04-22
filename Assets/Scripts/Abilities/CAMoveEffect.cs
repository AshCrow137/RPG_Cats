using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CAMoveEffect : CABaseEffect
{
    private MoveEffectTypes MoveEffectType;
    private CATemplateScript TargetPointTemplate;
    private float Speed;
    private float Distance;
    private List<Rigidbody2D> rbArray = new List<Rigidbody2D>();
   
    //public override void ExecuteEffect(GameObject targets, MoveEffectTypes moveType,Vector3 targetPoint,float speed)
    //{
    //    Debug.Log($"effect targets: {targets}, movement type: {moveType}, destination point: {targetPoint}, speed: {speed}");
    //}
    public CAMoveEffect(List<GameObject> targets, EffectExecutionType executionType, MoveEffectTypes moveType,CATemplateScript targetPointTemplate, float speed,float distance) : base(targets,executionType)
    {
        MoveEffectType = moveType;
        TargetPointTemplate = targetPointTemplate;
        Speed = speed;
        Distance = distance;
        foreach(GameObject target in targets )
        {
            rbArray.Add(target.GetComponent<Rigidbody2D>());
        }
        if(moveType == MoveEffectTypes.Teleport)
        {
            ExecutionType = EffectExecutionType.Instant;
        }
    }
    public override void ExecuteEffect()
    {
        base.ExecuteEffect();

        Vector2 targetPoint = CalculateTemplateTargetPoint(TargetPointTemplate.gameObject);
       // Debug.Log($"move {Targets}  to {targetPoint} by {MoveEffectType} with speed {Speed}");
        switch ( MoveEffectType )
        {
            case MoveEffectTypes.MoveToTarget:
                //float rad = (AbilityTemplate.transform.eulerAngles.z + 90) * Mathf.Deg2Rad;

                //Vector2 targets = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                foreach (Rigidbody2D rb in rbArray )
                {
                   rb.MovePosition(rb.position + targetPoint * Speed * Time.fixedDeltaTime);
                  //  Debug.Log($"move {rb.gameObject}  to {targetPoint} by {MoveEffectType} with speed {Speed}");
                }
                
               
                //Targets.transform.position = Vector3.MoveTowards(Targets.transform.position, CalculateTemplateTargetPoint(TargetPointTemplate, Distance), Speed * Time.deltaTime);
                break;
            case MoveEffectTypes.Teleport:

                foreach(GameObject target in Targets)
                    target.transform.position = new Vector3(targetPoint.x, targetPoint.y, 0) * Distance + target.transform.position;
                break;

        }
    }
    public override void SetTargets(List<GameObject> newTarget)
    {
        base.SetTargets(newTarget);
        rbArray.Clear();
        foreach (GameObject target in Targets)
        {
            rbArray.Add(target.GetComponent<Rigidbody2D>());
        }
        
    }
    public Vector2 CalculateTemplateTargetPoint(GameObject template)
    {
        float rad = (template.transform.eulerAngles.z +90) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad) , Mathf.Sin(rad) ).normalized; 


    }
    public override void ActivateEffect()
    {
        
        foreach(GameObject target in Targets)
        {
            target.GetComponent<Movement>()?.ChangeMovementPossibility(false);
        }
        base.ActivateEffect();
    }
    public override void FinishEffect()
    {
        
        foreach (GameObject target in Targets)
        {
            target.GetComponent<Movement>()?.ChangeMovementPossibility(true);
        }
        base.FinishEffect();
    }
}


