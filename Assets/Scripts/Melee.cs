using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Melee : Character
{

    private Animator _animator;

    public Animator Animator
    {
        get => _animator;
        set => _animator = value;
    }

    private int _hashAttack;
    private int _hashWalk;
    private int _hashDeath;
    private float _attackTime;
    private Character _character;
    
    
    

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
        _hashDeath = Animator.StringToHash("Dies");
        
        _character = GetComponent<Character>();
    }

    
    protected override void Attack()
    {
        if (Time.time - _attackTime > Cooldown)
        {
            _animator.SetTrigger(_hashAttack);
        }
    }
    
    protected internal override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (_character.Hp <= 0)
        {
            _animator.SetTrigger(_hashDeath);
            Destroy(gameObject, 1.5f);
        }
        
    }
    
    protected override void MoveToPosition(Vector3 destination)
    {
        base.MoveToPosition(destination);
        float dist = Vector3.Distance(destination, transform.position);
        if (dist >= Range)
        {
            base.MoveToPosition(destination);
            _animator.SetBool(_hashWalk, true);
        }
        else
        {
            _animator.SetBool(_hashWalk, false);
        }
    }
}
