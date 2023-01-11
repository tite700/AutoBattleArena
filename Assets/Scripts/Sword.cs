using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private float _damage;
    private string _tagToSearch;
    
    void Start()
    {
        _tagToSearch = GetComponentInParent<Character>().EnemyTag;
        _damage = GetComponentInParent<Melee>().damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_tagToSearch))
        {
            other.GetComponent<Character>().TakeDamage(_damage);
            Debug.Log("character " + other.name + " took " + _damage + " damage");
        }
    }

    
}