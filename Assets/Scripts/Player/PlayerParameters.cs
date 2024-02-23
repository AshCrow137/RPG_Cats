using UnityEngine;

public class PlayerParameters : Parameters
{
    [SerializeField]
    private bool followTarget = true;

    public bool GetFollowTarget()
    {
        return followTarget;
    }
}
