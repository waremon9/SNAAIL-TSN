using Enums;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Health _playerHealth;

    private float _tickDelay = 0.2f;
    private float _lastTick = 0f;
    bool _takeDamage = false;

    private void Awake()
    {
        _playerHealth = GetComponent<Health>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Weapon"))
        {
            _playerHealth.TakeDamage(other.gameObject.GetComponent<Weapon>().Damage);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        PlayerAttack SpellOwner = other.GetComponent<Weapon>().Owner;
        if (SpellOwner.Spell.SpellType == SpellType.Point)
        {
            var particleCollision = other.GetComponent<ParticleSystem>().collision;
            particleCollision.sendCollisionMessages = false;

            _playerHealth.TakeDamage(SpellOwner.Spell.Damage);
        }
    }

    public void ContinuousDamage(int damage)
    {
        StartCoroutine(DamageCoroutine(damage));
    }

    private IEnumerator DamageCoroutine(int damage)
    {
        _takeDamage = true;
        do
        {
            if (Time.time - _lastTick < _tickDelay)
            {
                yield return null;
            }
            else
            {
                _lastTick = Time.time;
                _playerHealth.TakeDamage(damage, false);
            }
        } while (_takeDamage);
    }

    public void StopDamage()
    {
        _takeDamage = false;
        _lastTick = 0;
    }

}
