using Enums;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Health _playerHealth;
    private PlayerMovement _playerMovement;
    private PlayerAttack _playerAttack;

    private float tickDelay = 0.5f;
    private float lastTick = 0f;

    private void Awake()
    {
        _playerHealth = GetComponent<Health>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAttack = GetComponent<PlayerAttack>();
    }

    private void Start()
    {
        StartCoroutine(SetupActions());
            }

    private IEnumerator SetupActions()
    {
        yield return null;
        foreach (var spellCollider in FindObjectsOfType<LineProjectileCollisionBehaviour>())
        {
            Debug.Log("spell found");
            spellCollider.THATSALOTOFDAMAGE += ContinuousDamage;
            spellCollider.THATSNODAMAGEATALL += StopDamage;
        }
        
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
    private void ContinuousDamage(int damage)
    {
        Debug.Log("ALOTOFDAMAGE");
        StartCoroutine(DamageCoroutine(damage));
    }

    private IEnumerator DamageCoroutine(int damage)
    {
        Debug.Log("DAMAGE");
        if (Time.time - lastTick < tickDelay)
        {
            yield return null;
        } else
        {
            lastTick = Time.time;
            _playerHealth.TakeDamage(_playerAttack.Spell.Damage, false);
        }
    }
    private void StopDamage(int damage)
    {
        Debug.Log("NOMOREDAMAGE");
        StopCoroutine(DamageCoroutine(damage));
        lastTick = 0;
    }
}
