using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private bool isDeploy; 
    private Animator animator;
    private float speed;

    public float turnTime;

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

    public void Deploy(){
        animator.speed = speed;
        animator.SetBool("Deploy" , true);
    }

    public void StopMove(){
        animator.speed = speed;
        animator.SetBool("Stop", true);
    }

    public void Rotate(){
        animator.speed = speed * 4;
        animator.SetBool("Rotate" , true);
    }

    public void MoveAfterRotate() {
        animator.speed = speed * 4 / 3;
    }

    public void StartMoving(){
        animator.speed = speed;
        animator.SetBool("StartMove" , true);
    }

    public void Die(){
        animator.speed = speed;
        animator.SetBool("Die" , true);
    }

    public void Restart(){
        isDeploy = false;
        animator.SetBool("isDeploy" , isDeploy);
        animator.SetBool("Stop" , isDeploy);
        animator.SetBool("Rotate" , isDeploy);
        animator.SetBool("StartMove" , isDeploy);
        animator.SetBool("Die" , isDeploy);
        
    }
}
