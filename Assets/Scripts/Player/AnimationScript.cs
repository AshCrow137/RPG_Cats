using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Animator animator;
    

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void UpdateMovement(float horizontal, float vertical, float speed)
    {
        if (animator != null)
        {
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);
            animator.SetFloat("Speed", speed);
           
        }
    }
    public Animator GetAnimator() { return animator; }
}
