using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Character
{

    private BoxCollider _swordbox;
    // Start is called before the first frame update
    void Start()
    {
        Hp = 100f;
        Damage = 15f;
        Range = 1.5f;
        Cooldown = 1f; // temps de l'animation
    }

    new void Awake()
    {
        base.Awake();
        _swordbox = GetComponent<BoxCollider>();
    }
    
    protected override void Attack()
    {
        Collider[] results = new Collider[] { };
        var transform1 = _swordbox.transform;
        var size = Physics.OverlapBoxNonAlloc(transform1.position, transform1.localScale / 2f, results, transform1.rotation);
        Debug.Log("size" + size);

        foreach (var enemy in results)
        {
            if (enemy.gameObject.CompareTag(enemyTag))
            {
                enemy.GetComponent<Character>().TakeDamage(Damage);
                Debug.Log("enemy hp : " + enemy.GetComponent<Character>().Hp);
            }
            
        }
        
        //Animation et cooldown
    }
}
