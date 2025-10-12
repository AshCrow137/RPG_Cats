using UnityEngine;

public class ChestsCollisionScript : MonoBehaviour
{
    [SerializeField]
    private bool Repetative = true;
    private void OpenChest()
    {
        GlobalEventManager.InvokeOpenChestEvent(Repetative);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject.GetComponent<PlayerScript>()) 
        {
            OpenChest();
        } 
    }
}
