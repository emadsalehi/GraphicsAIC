﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private bool _isDeploy; 
    private Animator _animator;
    private float _speed;
    private float _turnTime;
    private static readonly int Deploy1 = Animator.StringToHash("Deploy");
    private static readonly int Stop = Animator.StringToHash("Stop");
    private static readonly int Rotate1 = Animator.StringToHash("Rotate");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Die1 = Animator.StringToHash("Die");
    private static readonly int Attack = Animator.StringToHash("DeployAttack");
    private static readonly int DeployDie1 = Animator.StringToHash("DeployDie");

    void Start()
    {
        _animator = GetComponent<Animator>();
        _speed = 2 / _turnTime;
        _animator.speed = 2 / _turnTime;
    }

    public void SetTurnTime(float turnTime){
        _turnTime = turnTime; 
        _speed  = 2 / turnTime;
        _animator.speed = _speed;
    }

    public void Deploy()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.SetBool(Deploy1 , true);
    }

    public void StopMove(){
        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.SetBool(Stop, true);
    }

    public void Rotate(){
        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.SetBool(Rotate1 , true);
    }

    public void StartMoving(){
        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.SetBool(Move , true);
    }

    public void Die(){
        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.SetBool(Die1 , true);
    }

    public void DeployAttack()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.SetBool(Attack , true);
    }
    
    public void DeployDie()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.SetBool(DeployDie1 , true);
    }

    public void Restart(){
        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.SetBool(Stop , false);
        _animator.SetBool(Rotate1 , false);
        _animator.SetBool(Move , false);
        _animator.SetBool(Die1 , false);
    }
}
