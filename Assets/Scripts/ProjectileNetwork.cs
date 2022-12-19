using Unity.Netcode;
using UnityEngine;
public class ProjectileNetwork : NetworkBehaviour {
    private const float MOVE_SPEED = 12;
    public NetworkObject networkObject { get; private set; }

    private void Awake() {
        this.networkObject = this.GetComponent<NetworkObject>();
    }

    private void Update() {
        if (this.IsServer == false) {
            return;
        }

        this.transform.position += this.transform.forward * (MOVE_SPEED * Time.deltaTime);
        Debug.Log(networkObject.IsSpawned);
    }

    private void OnTriggerEnter(Collider other) {
        if (this.IsServer == false) {
            return;
        }

        if (other.CompareTag("Player")) {
            return;
        }

        Debug.Log("HIT!");
        this.networkObject.Despawn(false);
    }
}