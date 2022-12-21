using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kid : AEnemy
{
    private void Start()
    {
        nav.SetDestination(new Vector3(10,0,10)); 
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Damage(5f);
        }
    }
}
