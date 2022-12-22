using Enums;
using UnityEngine;

public class AnimatorListener : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerAttack _playerAttack;
    private Health _playerHealth;

    private void Awake()
    {
        _playerMovement = transform.parent.GetComponent<PlayerMovement>();
        _playerAttack = transform.parent.GetComponent<PlayerAttack>();
        _playerHealth = transform.parent.GetComponent<Health>();
    }

    private void SetPlayerMovement(Movement playerMovement)
    {
        _playerMovement.SetMovement(playerMovement);
    }

    private void DestroyEntity()
    {
        Debug.Log("Destroy entity : " + gameObject.name);
        Destroy(gameObject);
    }

    void ToggleSpell()
    {
        _playerAttack.ToggleSpellVisibility();
    }
    void ToggleWeapon()
    {
        _playerAttack.ToggleWeaponCollider();
    }
}
