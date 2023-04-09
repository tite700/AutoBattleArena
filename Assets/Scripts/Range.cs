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

    IEnumerator TirDeFleche(GameObject closestEnemy)
    {
        _animator.SetTrigger(_hashAttack);
        yield return new WaitForSeconds(2.5f);
        var transform1 = transform;
        for (int i = 0; i < 2; i++)
        {
            if (_character.Hp <= 0) break;
            var position = transform1.position;
            var temp = Instantiate(arrow, position + new Vector3(0, 1f, 1f),
                transform1.rotation);
            Arrow script = temp.GetComponent<Arrow>();
            temp.tag = tag;
            script.enemyTag = enemyTag;
            script.Target = closestEnemy;

            yield return new WaitForSeconds(0.5f);
        }
    }

    protected internal override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (_character.Hp <= 0 && isDead == false)
        {
            if (tag == "Enemy")
            {
                gameManager.Gold += goldGiven;
                goldCoin.SetActive(true);
                Destroy(goldCoin, 0.5f);
                isDead = true;
            }
            _animator.SetTrigger(_hashDeath);
            Destroy(gameObject, 1.5f);
        }
    }

    protected override void Attack(GameObject closestEnemy)
    {
        base.Attack(closestEnemy);
        if (Time.time > Cooldown)
        {
            _sub = Time.time - _latestShotTime;
        }

        if (_sub > Cooldown)
        {
            _latestShotTime = Time.time;
            StartCoroutine(TirDeFleche(closestEnemy));
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

        if (destination == Vector3.zero)
        {
            _animator.SetBool(_hashWalk, false);
        }
    }
}