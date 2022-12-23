using UnityEngine;

public class Kid : AEnemy
{
    private void Start()
    {
        nav.SetDestination(target.transform.position); 
    }
}
