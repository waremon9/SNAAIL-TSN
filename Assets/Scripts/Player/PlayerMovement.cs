using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    public Animator PlayerAnimator
    {
        get { return _animator; }
    }
    
    [SerializeField] private float _moveSpeed = 2f;
    
    [SerializeField] private GameObject _playerMesh;
    
    [SerializeField] private LayerMask _ground;
    
    private Rigidbody _rigidBody;

    private Animator _animator;

    private Camera _camera;

    private PlayerControls _playerControls;

    private bool _canMove;
    
    public bool CanMove { get => _canMove; set => _canMove = value ; }

    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();
        _playerControls = new PlayerControls();
        _playerControls.Enable();

        _playerControls.Player.Move.performed += OnRun;
        _playerControls.Player.Move.canceled += OnStopRun;
        
        CanMove = true;
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        if (!CanMove)
            return;
        
        Vector2 moveInput = _playerControls.Player.Move.ReadValue<Vector2>();
        Vector3 moveAmount = new Vector3(moveInput.x, 0f, moveInput.y) * (_moveSpeed * Time.fixedDeltaTime);

        _rigidBody.MovePosition(transform.position + moveAmount);
    }
    
    void Rotate()
    {
        if (!CanMove)
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
        if(CanMove)
            _animator.SetBool("Running", true);
    }

    void OnStopRun(InputAction.CallbackContext context)
    {
        _animator.SetBool("Running", false);
    }

}
