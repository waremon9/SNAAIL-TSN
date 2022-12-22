using UnityEngine;

public class Kid : AEnemy
{
    private void Start()
    {
        nav.SetDestination(new Vector3(10,0,10)); 
    }
}
