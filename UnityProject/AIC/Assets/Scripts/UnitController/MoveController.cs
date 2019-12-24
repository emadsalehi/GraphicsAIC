using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isMoving ; 
    public bool isRotating;
    public float turnTime ; 
    public bool isMovingAfterRotate;
    
    public Vector3 direction ; 
    public float valueOfRotate;
    public float speed = 1.0f;
    

    void StartMoving(Vector3 direction){
        this.direction = direction / direction.magnitude;
        isMoving = true;
        isRotating = false ; 
        isMovingAfterRotate = false;

    }
    
    void StartMovingAfterRotate(Vector3 direction)
    {
        this.direction = direction / direction.magnitude;
        isMovingAfterRotate = true;
        isRotating = false;
        isMoving = false;     
    }
    
    void StopEveryThing()
    {
        isMoving = false;
        isRotating = false;
        isMovingAfterRotate = false;
    }
    
    void StartRotating(float degree)
    {
        isMoving = false;
        isRotating = true;
        isMovingAfterRotate = false;
        valueOfRotate = degree / (turnTime / 4);
    }

    void Start()
    {

        // Calculate the journey length.
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving){
            transform.position += direction * speed * Time.deltaTime / turnTime;
        }
        if (isRotating){
            transform.Rotate(0, valueOfRotate * Time.deltaTime, 0);
        }  
        if (isMovingAfterRotate)
        {
            transform.position += direction * speed * Time.deltaTime / (turnTime * 3 / 4);
        }      
    }
}
