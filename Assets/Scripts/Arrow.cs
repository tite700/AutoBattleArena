using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private const float ArrowSpeed = 10f;
    public string enemyTag;
    private GameObject _target;

    public GameObject Target
    {
        get => _target;
        set => _target = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(enemyTag))
        {
            other.gameObject.GetComponent<Character>().TakeDamage(10);
        }

        Destroy(gameObject, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Target.transform.position + Vector3.up);
        transform.Translate(new Vector3(0f, 0f, 1f * ArrowSpeed) * Time.deltaTime);
    }
}