using UnityEngine;
using Enums;

[RequireComponent(typeof(Player))]
public class Health : MonoBehaviour
{
    private PlayerMovement _player;

    private int _maxHealth = 100;
    [SerializeField]
    private int _currentHealth;

    public int MaxHealth { get { return _maxHealth; } }
    public int CurrentHealth { get { return _currentHealth; } }

    private void Awake()
    {
        _player = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage, bool withHit = true)
    {
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
        _player.SetMovement(Movement.Blocked);
        _player.PlayerAnimator.SetTrigger("Hit");
    }

    private void Die()
    {
        _player.SetMovement(Movement.Blocked);
        _player.PlayerAnimator.SetTrigger("Died");
    }
    private void Revive()
    {
        _player.PlayerAnimator.SetTrigger("Revive");
    }
}
