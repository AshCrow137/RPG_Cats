using System.Collections;

using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]

public class EnemyMovement : Movement
{

    private float RandomMovementDistance = 10;
    private float WaitTime = 3;
    private Vector3 destination;

    private Vector3 startPoint;
    private IEnumerator movementCoroutine;

    private Seeker seeker;
    private Path path;
    private float nextWaypointDistance = 3;
    private int CurrentWaypoint = 0;
    private bool bReachedEndOfPath;
    public float movementTime{ get; private set; }

    private Vector3 previousTargetPosition;
    protected override void Awake()
    {
        base.Awake();
        seeker = GetComponent<Seeker>();
        if(!seeker)
        {
            Debug.LogError($"There is no seeker component attached to {this.gameObject}");
        }
        startPoint = transform.position;
    }


    public override void MoveTo(Vector3 target)
    {
        base.MoveTo(target);
    }
    private void CreatePath(Vector3 target)
    {
        seeker.StartPath(transform.position, target, OnPathComplete);

    }
    private void OnPathComplete(Path p)
    {
       // Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        if (!p.error)
        {
            path = p;
            CurrentWaypoint = 0;
        }
    }
    public void StartMoveToTarget(Vector3 targetPosition)
    {
        StopMove();
        CreatePath(targetPosition);
        previousTargetPosition = targetPosition;
    }
    public void MoveToTarget(Vector3 targetPosition)
    {
        if(path==null)
        {
            return;
        }

        if(previousTargetPosition!=targetPosition)
        {
            CreatePath(targetPosition);
        } 

        MovementCycle();
        MoveTo(path.vectorPath[CurrentWaypoint]);
            

        
    }
    private void MovementCycle()
    {
        float distanceToWaypoint;
        while (true)
        {
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[CurrentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (CurrentWaypoint + 1 < path.vectorPath.Count)
                {
                    CurrentWaypoint++;
                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    bReachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }
        
    }
    public void SetRandomMovementStartPoint(Vector3 value)
    {
        startPoint = value;
    }
    public void StartRandomMovement(float randomMovementDistance,float waitTime)
    {
        
        
        RandomMovementDistance = randomMovementDistance;
        WaitTime = waitTime;
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        
        movementCoroutine = RandomMovementTimer();
        StartCoroutine(movementCoroutine);
    }
    private IEnumerator RandomMovementTimer()
    {
        movementTime = 0;
        destination = new Vector3(Random.Range(startPoint.x - RandomMovementDistance, startPoint.x + RandomMovementDistance), Random.Range(startPoint.y - RandomMovementDistance, startPoint.y + RandomMovementDistance), 0);
        bReachedEndOfPath = false;
        CreatePath(destination);
        while(path==null)
        {
            yield return null;
        }
        while (transform.position != destination)
        {

            MovementCycle();
            movementTime += Time.deltaTime;
            MoveTo(path.vectorPath[CurrentWaypoint]);
            yield return null;
        }
        yield return new WaitForSeconds(WaitTime);
        StartRandomMovement(RandomMovementDistance, WaitTime);
    }
    public void StopRandomMovement()
    {
        StopCoroutine(movementCoroutine);
        //StopAllCoroutines();
        movementTime = 0;
        StopMove();
    }
    protected override void FixedUpdate()
    {
        
    }
    protected override void Update()
    {
       
    }
}
