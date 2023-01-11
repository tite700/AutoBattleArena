using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Character
{
    // Start is called before the first frame update
    void Start()
    {
        Hp = 100f;
        Damage = 15f;
        Range = 1.5f;
        Cooldown = 1f; // temps de l'animation
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
