using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Melee : Character
{

    private Animator _animator;
    private int _hashAttack;
    private int _hashWalk;
    private float _attackTime;
    
    

    protected override void Awake()
    {
        base.Awake();
        
        Hp = 100f;
        Damage = 15f;
        Range = 1.5f;
        Cooldown = 1f;
        
        _animator = GetComponent<Animator>();
        _hashAttack = Animator.StringToHash("Attack");
        _hashWalk = Animator.StringToHash("Moving");

    }

    
    protected override void Attack()
    {
        _animator.SetTrigger(_hashAttack);
    }
    
    protected override void MoveToPosition(Vector3 destination)
    {
        base.MoveToPosition(destination);
        float dist = Vector3.Distance(destination, transform.position);
        if (dist < Range)
        {
            _animator.SetBool(_hashWalk, false);
            if (Time.time - _attackTime > Cooldown)
            {
                _animator.SetTrigger(_hashAttack);
            }
        }
        _animator.SetBool(_hashWalk, true);
    }
}
