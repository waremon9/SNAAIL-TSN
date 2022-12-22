using UnityEngine;

public class Player : MonoBehaviour
{
    private Health _playerHealth;

    private void Awake()
    {
        _playerHealth = GetComponent<Health>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if(other.transform.CompareTag("Weapon"))
        {
            Debug.Log("Triggered with weapon");
            _playerHealth.TakeDamage(other.gameObject.GetComponent<Weapon>().Damage);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        
        Debug.Log("TriggerParticle");
        
    }
}
