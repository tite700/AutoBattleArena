using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private float _damage;
    private string _tagToSearch;
    private float _lastAttackTime = 0f;
    
    void Start()
    {
        _tagToSearch = GetComponentInParent<Character>().EnemyTag;
        _damage = GetComponentInParent<Melee>().damage;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (GetComponentInParent<Character>().Hp > 0)
        {
            if (other.CompareTag(_tagToSearch) && Time.time - _lastAttackTime > 0.7f)
            {
                var melee = GetComponentInParent<Melee>();
                if (melee)
                {
                    var hashWalk = Animator.StringToHash("Moving");
                    var moving = GetComponentInParent<Melee>().Animator.GetBool(hashWalk);
                    if (moving) return;
                }

                other.GetComponent<Character>().TakeDamage(_damage);
                _lastAttackTime = Time.time;
            }
        }
    }

}