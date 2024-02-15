using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private PlayerScript playerScript;
    public static SceneManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);

        }
        else
        {
            Instance = this;
        }
    }
    private void Start()

    {

        playerScript = PlayerScript.Instance;


    }



}
