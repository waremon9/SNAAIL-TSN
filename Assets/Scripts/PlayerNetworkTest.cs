using System;
using Unity.Netcode;
using UnityEngine;
public class PlayerNetworkTest : NetworkBehaviour {
    public float moveSpeed = 8;
    private Camera _camera;
    private Plane _plane = new(Vector3.up, Vector3.zero);
    private Ray _ray;

    private void Awake() {
        this._camera = Camera.main;
    }
    private void Update() {
        if (this.IsOwner == false) {
            return;
        }

        this.Rotate();

        if (Input.GetMouseButtonDown(0)) {
            this.ShootProjectileServerRpc();
        }
    }

    private void FixedUpdate() {
        if (this.IsOwner == false) {
            return;
        }
        
        this.Move();
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

        this.transform.position += moveDirection.normalized * (Time.deltaTime * this.moveSpeed);
    }

    private void Rotate() {
        Vector3 lookAtPoint = this.GetPointOnGround();
        lookAtPoint.y = this.transform.position.y;
        this.transform.LookAt(lookAtPoint);
    }

    [ServerRpc]
    private void ShootProjectileServerRpc() {
        ProjectileNetwork projectile = Instantiate(ResourceManager.Instance.projectile);
        
        Debug.Log(projectile.networkObject.IsSpawned);
        projectile.networkObject.Spawn(true);
        Debug.Log(projectile.networkObject.IsSpawned);
        
        projectile.transform.position = this.transform.position + Vector3.up;
        projectile.transform.forward = this.transform.forward;
    }

    private Vector3 GetPointOnGround() {
        this._ray = this._camera.ScreenPointToRay(Input.mousePosition);
        return this._plane.Raycast(this._ray, out float enter) ? this._ray.GetPoint(enter) : Vector3.zero;
    }
}