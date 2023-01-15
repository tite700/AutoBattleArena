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

    /*public void Parabole(float angle, float speed)
    {
        float gravity = Physics.gravity.magnitude;
        float radianAngle = angle * Mathf.Deg2Rad;
        float Vx = Mathf.Sqrt(speed) * Mathf.Cos(radianAngle);
        float Vy = Mathf.Sqrt(speed) * Mathf.Sin(radianAngle);
        float flightDuration = (Vy + Mathf.Sqrt(Vy * Vy + 2 * gravity * transform.position.y)) / gravity;
        transform.rotation = Quaternion.LookRotation(new Vector3(Vx, Vy - (gravity * flightDuration) / 2.0f, flightDuration));
        StartCoroutine(ParaboleMovement(new Vector3(Vx, Vy, flightDuration)));
    }
    
    private IEnumerator ParaboleMovement(Vector3 velocity)
    {
        float elapseTime = 0;
        while (elapseTime < velocity.z)
        {
            transform.Translate(0, (velocity.y - (Physics.gravity.magnitude * elapseTime)) * Time.deltaTime, velocity.x * Time.deltaTime);
            elapseTime += Time.deltaTime;
            yield return null;
        }
    }*/
    
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
