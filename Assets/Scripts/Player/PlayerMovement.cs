using UnityEngine;
using UnityEngine.InputSystem;
using Enums;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public Animator PlayerAnimator
    {
        get { return _animator; }
    }
    
    [SerializeField] private float _moveSpeed = 2f;
    
    [SerializeField] public GameObject _playerMesh;
    
    [SerializeField] private LayerMask _ground;
    
    private Rigidbody _rigidBody;

    private Animator _animator;

    private Camera _camera;

    private PlayerControls _playerControls;

    private Movement _playerMovement = Movement.Free;   
    
    
    // Start is called before the first frame update
    void Start()
    {
        if(IsOwner==false){return;}
        
        _animator = GetComponentInChildren<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();
        _playerControls = new PlayerControls();
        _playerControls.Enable();

        _playerControls.Player.Move.performed += OnRun;
        _playerControls.Player.Move.canceled += OnStopRun;
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Update()
    {
        if(IsOwner==false){return;}
        Move();
        Rotate();
    }

    private void Move()
    {
        if (_playerMovement == Movement.Blocked || _playerMovement == Movement.RotateOnly) return;
        
        Vector2 moveInput = _playerControls.Player.Move.ReadValue<Vector2>();
        Vector3 moveAmount = new Vector3(moveInput.x, 0f, moveInput.y) * (_moveSpeed * Time.fixedDeltaTime);

        _rigidBody.MovePosition(transform.position + moveAmount);
    }
    
    void Rotate()
    {
        if (_playerMovement == Movement.Blocked || _playerMovement == Movement.MoveOnly)
            return;

        Vector3 mousePosition = GetMousePosition();

        Ray ray = _camera.ScreenPointToRay(mousePosition);

        Physics.Raycast(ray, out RaycastHit hit, _ground);
        
        Vector3 facingDirection = hit.point;

        facingDirection.y = _playerMesh.transform.position.y;

        _playerMesh.transform.LookAt(facingDirection);
    }

    public Vector2 GetMousePosition()
    {
        return _playerControls.Player.Look.ReadValue<Vector2>();
    }

    void OnRun(InputAction.CallbackContext context)
    {
        if (_playerMovement == Movement.Free || _playerMovement == Movement.MoveOnly)
            _animator.SetBool("Running", true);
    }

    void OnStopRun(InputAction.CallbackContext context)
    {
        _animator.SetBool("Running", false);
    }

    public void SetMovement(Movement movement)
    {
        _playerMovement = movement;
    }

}
