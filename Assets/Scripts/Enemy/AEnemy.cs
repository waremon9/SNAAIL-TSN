using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI ;
using UnityEngine.Events;

public abstract class AEnemy : MonoBehaviour
{
    [SerializeField]
    protected float hpMax;
    protected float hpCurrent;


    [SerializeField]
    protected float speedDefault;
    protected float speedCurrent;


    [SerializeField]
    protected float damageDefault;
    protected float damageCurrent;


    [SerializeField]
    protected GameObject target;    
    
    
    protected NavMeshAgent nav;
    protected SkinnedMeshRenderer meshRenderer;

  

    [SerializeField]
    protected UnityEvent onDeath, onDamage, onCollision;

    [SerializeField]
    protected Color damageColor, baseColor;

    [SerializeField]
    protected float colorSpeed;

    //[SerializeField]
    //protected GameObject Drop;

    //[SerializeField]
    //protected GameObject spawnPointGemme;


    [SerializeField]
    UnityEvent playAudio;


    private void Awake()
    {
        
        hpCurrent = hpMax;
        speedCurrent = speedDefault;
        damageCurrent = damageDefault;
        nav = GetComponent<NavMeshAgent>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        nav.speed = speedCurrent;
    }

    public virtual void SetTarget()
    {

    }
    public virtual void Move()
    {
        nav.SetDestination(target.transform.position);
    }
    public void Damage(float damage)
    {

        playAudio.Invoke();

        hpCurrent -= damage;
        StartCoroutine(DamageVisual());
        if (hpCurrent <= 0)
        {
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        StopCoroutine(DamageVisual());
        onDeath.Invoke();
        //Instantiate(Drop, spawnPointGemme.transform.position, Quaternion.identity);
        Destroy(gameObject);

    }



    public virtual void OnDamage()
    {
        onDamage.Invoke();
    }

    IEnumerator DamageVisual()
    {
        float t = 0;
        Color color;
        while (t < 1)
        {

            t += Time.deltaTime * colorSpeed;
            color = Color.Lerp(baseColor, damageColor, t);
            meshRenderer.material.color = color;
            yield return new WaitForSeconds(0.05f);
        }
        while (t > 0)
        {
            t -= Time.deltaTime * colorSpeed;
            color = Color.Lerp(baseColor, damageColor, t);
            meshRenderer.material.color = color;
            yield return new WaitForSeconds(0.05f);
        }

    }
}
