using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI ;
using UnityEngine.Events;
using Unity.Netcode;

public abstract class AEnemy : NetworkBehaviour 
{
    [SerializeField]
    protected float speedDefault;
    protected float speedCurrent;


    [SerializeField]
    protected float damageDefault;
    protected float damageCurrent;

    [SerializeField]
    public GameObject target;    
    
    
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
        if(IsOwner==false) return;
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
        if (IsOwner == false) yield break;
        if (_movement == Movement.Free)
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
