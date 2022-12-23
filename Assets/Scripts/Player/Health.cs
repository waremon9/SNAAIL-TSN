using UnityEngine;
using Enums;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Character))]
public class Health : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private int _maxHealth ;
    
    private int _currentHealth;

    private bool _isDead = false;
    public bool IsDead { get { return _isDead; } }

    public int MaxHealth { get { return _maxHealth; } }
    public int CurrentHealth { get { return _currentHealth; } }

    protected SkinnedMeshRenderer meshRenderer;

    [SerializeField]
    protected float colorSpeed;

    [SerializeField]
    UnityEvent playAudio, onHit, onDeath;

    private void Start()
    {
        _currentHealth = _maxHealth;
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void TakeDamage(int damage, bool withHit = true)
    {
        if (_isDead) return;

        _currentHealth -= damage;

        if(withHit) Hit();

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    private void Hit()
    {
        playAudio.Invoke();
        onHit.Invoke();
        StopAllCoroutines();
        StartCoroutine(DamageVisual());
        _animator.SetTrigger("Hit");
    }

    IEnumerator DamageVisual()
    {
        float t = 0;
        Color color;
        while (t < 1)
        {

            t += Time.deltaTime * colorSpeed;
            color = Color.Lerp(Color.white, Color.red, t);
            meshRenderer.material.color = color;
            yield return new WaitForSeconds(0.05f);
        }
        while (t > 0)
        {
            t -= Time.deltaTime * colorSpeed;
            color = Color.Lerp(Color.red, Color.white, t);
            meshRenderer.material.color = color;
            yield return new WaitForSeconds(0.05f);
        }

    }

    private void Die()
    {
        
        onDeath.Invoke();
        _isDead = true;
        _animator.SetTrigger("Died");
    }
    private void DestroyCharacter()
    {
        Destroy(gameObject);
    }
}
