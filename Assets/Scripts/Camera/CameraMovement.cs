using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private static GameObject player;

    private void Start()
    {
        player = PlayerScript.Instance.gameObject;

        if (player == null)
        {
            Debug.LogError("player not found on camera!");
        }
    }
    private void FixedUpdate()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        }

    }
}
