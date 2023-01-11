using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : Character
{
    // Start is called before the first frame update
    void Start()
    {
        Hp = 70f;
        Damage = 10f;
        Range = 5f;
        Cooldown = 1f; //temps de l'animation
    }

    
}
