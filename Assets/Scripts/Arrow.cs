using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    //private float _arrowSpeed = 10f;
    public string enemyTag;
    private Rigidbody _rb;
    
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Parabole(GameObject target)
    {
        
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        //Instantiate(bloodSplash, this.transform.position, new Quaternion());
        if (collision.gameObject.CompareTag(enemyTag))
        {
            collision.gameObject.GetComponent<Character>().TakeDamage(10);
            Debug.Log("Hit");
        }
        
        Destroy(gameObject,0.01f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
