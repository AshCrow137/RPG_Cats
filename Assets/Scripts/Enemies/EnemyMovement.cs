using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Movement
{
    [SerializeField]
    private float RandomMovementDistance = 10;
    private float WaitForNextMoveTime = 3;
    private Vector3 destination;

    private Vector3 startPoint;

    private IEnumerator MovementCoroutine;
    public override void MoveToTarget(Vector3 target)
    {
        base.MoveToTarget(target);
    }
    public void StartRandomMovement()
    {
        startPoint = transform.position;
        MovementCoroutine = RandomMovementTimer();
        StartCoroutine(MovementCoroutine);
    }
    private IEnumerator RandomMovementTimer()
    {

        destination = new Vector3(Random.Range(startPoint.x - RandomMovementDistance, startPoint.x + RandomMovementDistance), Random.Range(startPoint.y - RandomMovementDistance, startPoint.y + RandomMovementDistance), 0);
        while (destination != transform.position)
        {
            yield return null;

            MoveToTarget(destination);
        }
        yield return new WaitForSeconds(WaitForNextMoveTime);
        MovementCoroutine = RandomMovementTimer();
        StartCoroutine(MovementCoroutine);
    }
    public void StopRandomMovement()
    {
        StopCoroutine(MovementCoroutine);
        
    }
    protected override void FixedUpdate()
    {
        
    }
    protected override void Update()
    {
       
    }
}
