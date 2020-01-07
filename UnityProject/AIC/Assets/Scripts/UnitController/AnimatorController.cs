using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private bool isDeploy; 
    private Animator animator;
    private float speed;
    private float turnTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        speed = 2 / turnTime;
        animator.speed = 2 / turnTime;
    }

    public void SetTurnTime(float turnTime){
        this.turnTime = turnTime; 
        speed  = 2 / turnTime;
        animator.speed = speed;
    }

    public void Deploy()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("Deploy" , true);
    }

    public void StopMove(){
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("Stop", true);
    }

    public void Rotate(){
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("Rotate" , true);
    }

    public void MoveAfterRotate() {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void StartMoving(){
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("Move" , true);
    }

    public void Die(){
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("Die" , true);
    }

    public void DeployAttack()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("DeployAttack" , true);
    }

    public void Restart(){
        if (animator == null)
            animator = GetComponent<Animator>();
        isDeploy = false;
        animator.SetBool("Stop" , isDeploy);
        animator.SetBool("Rotate" , isDeploy);
        animator.SetBool("Move" , isDeploy);
        animator.SetBool("Die" , isDeploy);
        
    }
}
