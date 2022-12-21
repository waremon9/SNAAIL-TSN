using System;
using Unity.Netcode;
using UnityEngine;
public class PlayerNetworkTest : NetworkBehaviour {
    public float moveSpeed = 8;
    private Camera _camera;
    private Plane _plane = new(Vector3.up, Vector3.zero);
    private Ray _ray;

    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += AttachCameraToPlayer;
    }

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
        Vector3 position = this.transform.position + Vector3.up + this.transform.forward * .3f; //replace by a spawn origin transform
        Vector3 forward = this.transform.forward;
        this.ShootProjectileClientRpc(position, forward);
    }

    [ClientRpc]
    private void ShootProjectileClientRpc(Vector3 position, Vector3 forward) {
        ProjectileNetwork projectile = Instantiate(ResourceManager.Instance.projectile);
        projectile.transform.position = position;
        projectile.transform.forward = forward;
    }

    private Vector3 GetPointOnGround() {
        this._ray = this._camera.ScreenPointToRay(Input.mousePosition);
        return this._plane.Raycast(this._ray, out float enter) ? this._ray.GetPoint(enter) : Vector3.zero;
    }

    void AttachCameraToPlayer(ulong obj)
    {
        transform.SetParent(_camera.transform);
        _camera.transform.localPosition = new Vector3(0, 20f, 0);
        _camera.transform.LookAt(gameObject.transform);
    }
    
}