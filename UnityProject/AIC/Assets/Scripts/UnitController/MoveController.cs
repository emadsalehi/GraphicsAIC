using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private bool _isMoving; 
    private bool _isRotating;
    private bool _isMovingAfterRotate;
    private bool _isAttacking;
    private Vector3 _direction;
    private float _valueOfRotate;
    private float speed = 1.0f;
    private GameObject _target;

    public float turnTime;

    public void StartMoving(){
        _isMoving = true;
        _isRotating = false; 
        _isMovingAfterRotate = false;
        _isAttacking = false;
    }
    
    public void StartMovingAfterRotate(Vector3 direction)
    {
        this._direction = direction / direction.magnitude;
        _isMovingAfterRotate = true;
        _isRotating = false;
        _isMoving = false;
        _isAttacking = false;
    }
    
    public void StopEveryThing()
    {
        _isMoving = false;
        _isRotating = false;
        _isMovingAfterRotate = false;
        _isAttacking = false;
    }
    
    public void StartRotating(float degree)
    {
        _isMoving = false;
        _isRotating = true;
        _isMovingAfterRotate = false;
        _isAttacking = false;
        _valueOfRotate = degree / (turnTime / 3.5f);
    }

    
    public void Attack(GameObject target)
    {
        this._target = target;
        _isMoving = false;
        _isRotating = false;
        _isMovingAfterRotate = false;
        _isAttacking = true;
    }
    
    void Update()
    {
        if (_isMoving){
            transform.position += _direction * (speed * Time.deltaTime) / turnTime;
        }
        else if (_isRotating)
        {
            transform.Rotate(0.0f, _valueOfRotate * Time.deltaTime, 0.0f);
        }  
        else if (_isMovingAfterRotate)
        {
            transform.position += _direction * (speed * Time.deltaTime) / (turnTime / 1.4f);
        } 
        else if (_isAttacking)
        {
            Transform transform1;
            (transform1 = transform).LookAt(_target.transform);
            transform1.eulerAngles = new Vector3(0.0f, transform1.eulerAngles.y, 0.0f);
        }
    }
}
