using Unity.Netcode;
using UnityEngine;
public class PlayerNetworkTest : NetworkBehaviour {
    public float moveSpeed = 8;

    private void Update() {
        if (this.IsOwner == false) {
            return;
        }

        this.Move();
    }

    public override void OnNetworkSpawn() {
        if (this.IsOwner == false) {
            return;
        }

        base.OnNetworkSpawn();
    }

    private void Move() {
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.Z)) {
            moveDirection.z += 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveDirection.z -= 1;
        }
        if (Input.GetKey(KeyCode.Q)) {
            moveDirection.x -= 1;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveDirection.x += 1;
        }

        transform.position += moveDirection.normalized * (Time.deltaTime * this.moveSpeed);
    }
}