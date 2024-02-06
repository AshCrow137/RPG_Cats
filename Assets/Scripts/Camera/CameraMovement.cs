using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private static GameObject player;

    private void Start()
    {
        player = PlayerScript.Instance.gameObject;

        if (player == null )
        {
            Debug.Log("player not found on camera!");
        }
    }
    private void FixedUpdate()
    {
        if ( player != null )
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        }

    }
}
