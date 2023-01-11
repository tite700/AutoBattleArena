using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Melee : Character
{

    private Animator _animator;
    private int _hashAttack;

    protected override void Awake()
    {
        base.Awake();
        
        Hp = 100f;
        Damage = 15f;
        Range = 1.5f;
        
        _animator = GetComponent<Animator>();
        _hashAttack = Animator.StringToHash("Attack");
    }

    
    protected override void Attack()
    {
        _animator.SetTrigger(_hashAttack);
    }
    
    
}
