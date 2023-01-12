using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float _arrowSpeed = 10f;
    public string enemyTag;
    
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        //Instantiate(bloodSplash, this.transform.position, new Quaternion());
        
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Character>().TakeDamage(10); 
        }
        
        Destroy(gameObject,0.01f);
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 0, _arrowSpeed * Time.deltaTime));
    }
}
