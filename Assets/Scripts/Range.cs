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
        yield return new WaitForSeconds(2.5f);
        Instantiate(arrow, transform.position + new Vector3(0,0.925999999f,0.959999979f), Quaternion.identity);

    }

    protected override void Attack()
    {
        _animator.SetTrigger(_hashAttack); 
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
        if (dist < Range)
        {
            _animator.SetBool(_hashWalk, false);
        }
        _animator.SetBool(_hashWalk, true);
    }
    
}
