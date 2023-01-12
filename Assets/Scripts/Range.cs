using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : Character
{
    [SerializeField] private GameObject arrow;
    
    private Animator _animator;
    private int _hashAttack;
    private int _hashWalk;
    private float _latestShotTime;
    private float _sub = 10000f;
    

    protected override void Awake()
    {
        base.Awake();
        
        Hp = 70f;
        Damage = 10f;
        Range = 5f;
        Cooldown = 5f;
        
        _animator = GetComponent<Animator>();
        _hashAttack = Animator.StringToHash("Attack");
        _hashWalk = Animator.StringToHash("Moving");
    }
    
    IEnumerator TirDeFleche()
    {
        _animator.SetTrigger(_hashAttack);
        yield return new WaitForSeconds(2.5f);
        var transform1 = transform;
        var temp = Instantiate(arrow, transform1.position + new Vector3(0,0.925999999f,0.959999979f), transform1.rotation);
        temp.tag = tag;
        temp.GetComponent<Arrow>().enemyTag = enemyTag;

    }

    protected override void Attack()
    {
        if (Time.time > Cooldown)
        {
            _sub = Time.time - _latestShotTime;
        }
        if ( _sub > Cooldown)
        {
            _latestShotTime = Time.time;
            StartCoroutine(TirDeFleche());
            _sub = 0;
            
        }

        
    }
    
    protected override void MoveToPosition(Vector3 destination)
    {
        base.MoveToPosition(destination);
        float dist = Vector3.Distance(destination, transform.position);
        if (dist <= Range)
        {
            _animator.SetBool(_hashWalk, false);
            
        }
        else
        {
            _animator.SetBool(_hashWalk, true);
        }
    }
    
}
