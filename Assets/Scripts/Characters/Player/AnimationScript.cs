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
    public void runAttackAnimation(float animationAngle)
    {
        //animator.SetBool("IsAttacking", true);
        print(animationAngle);
        animator.SetTrigger("AttackTrigger");
        animator.SetFloat("AttackAngle", animationAngle);
        
    }
    public void StopAttackAnimation()
    {
        //animator.SetBool("IsAttacking", false);

    }
    public void StartCastAnimation()
    {
        animator.SetTrigger("StartCastingAnimation");
    }
    public void StopCastingAnimation()
    {
        animator.SetTrigger("StopCastingAnimation");
    }
    public Animator GetAnimator() { return animator; }
}
