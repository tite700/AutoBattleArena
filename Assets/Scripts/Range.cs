using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Range : Character
{
    [SerializeField] private GameObject arrow;
    
    private Animator _animator;
    private int _hashAttack;
    private int _hashWalk;
    private int _hashDeath;
    private Character _character;
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
        _hashDeath = Animator.StringToHash("Dies");
        _character = GetComponent<Character>();
    }
    
    IEnumerator TirDeFleche()
    {
        _animator.SetTrigger(_hashAttack);
        yield return new WaitForSeconds(2.5f);
        var transform1 = transform;
        for (int i = 0; i < 2; i++)
        {
            var position = transform1.position;
            var temp = Instantiate(arrow, position + new Vector3(0, 0.925999999f, 0.959999979f),
                transform1.rotation);
            temp.tag = tag;
            temp.GetComponent<Arrow>().enemyTag = enemyTag;
            Rigidbody rb = temp.GetComponent<Rigidbody>();
            Vector3 direction = _character.GetClosestEnemyPos() - position;
            direction.y = 3f;
            rb.AddForce(direction * 2f, ForceMode.Impulse);
            temp.GetComponent<Arrow>().Parabole(_character.GetClosestEnemy());


            yield return new WaitForSeconds(0.5f);
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
