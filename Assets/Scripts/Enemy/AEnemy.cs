using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI ;
using UnityEngine.Events;

public abstract class AEnemy : MonoBehaviour
{
    [SerializeField]
    protected float speedDefault;
    protected float speedCurrent;


    [SerializeField]
    protected float damageDefault;
    protected float damageCurrent;

    [SerializeField]
    protected GameObject target;    
    
    
    protected NavMeshAgent nav;

    private Movement _movement = Movement.Free;


    private void Awake()
    {
        speedCurrent = speedDefault;
        damageCurrent = damageDefault;
        nav = GetComponent<NavMeshAgent>();
        nav.speed = speedCurrent;
    }

    public virtual void Move()
    {
        if(_movement == Movement.Free)
        {
            nav.SetDestination(target.transform.position);
        }
    }

    public void CallPauseAgent()
    {
        StartCoroutine(PauseAgent());
    }

    IEnumerator PauseAgent()
    {
        if(_movement == Movement.Free)
        {
            nav.isStopped = true;
            yield return new WaitForSeconds(1.05f);
            nav.isStopped = false;
        }
    }

    private void SetMovement(Movement movement)
    {
        _movement = movement;
    }
}
