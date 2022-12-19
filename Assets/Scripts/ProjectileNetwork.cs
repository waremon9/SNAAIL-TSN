using Unity.Netcode;
using UnityEngine;
public class ProjectileNetwork : NetworkBehaviour {
    private const float MOVE_SPEED = 12;
    private bool _hitOnce;

    private void Update() {
        this.transform.position += this.transform.forward * (MOVE_SPEED * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (this.IsServer == false) {
            return;
        }

        if (other.CompareTag("Player")) {
            return;
        }

        if (this._hitOnce) {
            return;
        }
        this._hitOnce = true;

        this.DespawnProjectileServerRpc();
    }

    [ServerRpc]
    private void DespawnProjectileServerRpc() {
        Debug.Log("HIT");
        this.DespawnProjectileClientRpc();
    }

    [ClientRpc]
    private void DespawnProjectileClientRpc() {
        Destroy(this.gameObject);
    }
}