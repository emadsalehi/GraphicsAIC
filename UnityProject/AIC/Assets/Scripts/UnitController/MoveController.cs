using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isMoving; 
    private bool isRotating;
    private bool isMovingAfterRotate;
    private Vector3 direction;
    private float valueOfRotate;
    private float speed = 1.0f;

    public float turnTime;

    public void StartMoving(){
        isMoving = true;
        isRotating = false; 
        isMovingAfterRotate = false;

    }
    
    public void StartMovingAfterRotate(Vector3 direction)
    {
        this.direction = direction / direction.magnitude;
        isMovingAfterRotate = true;
        isRotating = false;
        isMoving = false;     
    }
    
    public void StopEveryThing()
    {
        isMoving = false;
        isRotating = false;
        isMovingAfterRotate = false;
    }
    
    public void StartRotating(float degree)
    {
        isMoving = false;
        isRotating = true;
        isMovingAfterRotate = false;
        valueOfRotate = degree / (turnTime * 4.0f / 14.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving){
            transform.position += direction * (speed * Time.deltaTime) / turnTime;
        }
        if (isRotating)
        {
            transform.Rotate(0, valueOfRotate * Time.deltaTime, 0);
        }  
        if (isMovingAfterRotate)
        {
            transform.position += direction * (speed * Time.deltaTime) / (turnTime * 10.0f / 14.0f);
        }      
    }
}
