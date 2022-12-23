using Enums;
using UnityEngine;

public class AnimatorListener : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerAttack _playerAttack;
    private Health _playerHealth;

    [SerializeField] bool _isPlayer = true;

    private void Awake()
    {
        if (!_isPlayer) return;

        _playerMovement = transform.parent.GetComponent<PlayerMovement>();
        _playerAttack = transform.parent.GetComponent<PlayerAttack>();
        _playerHealth = transform.parent.GetComponent<Health>();
    }

    private void SetPlayerMovement(Movement playerMovement)
    {
        if (!_isPlayer) return;
        _playerMovement.SetMovement(playerMovement);
    }

    private void DestroyEntity()
    {
        Destroy(gameObject);
    }

    void ToggleSpell()
    {
        if (!_isPlayer) return;
        _playerAttack.ToggleSpellVisibility();
    }
    void ToggleWeapon()
    {
        if (!_isPlayer) return;
        _playerAttack.ToggleWeaponCollider();
    }
}
