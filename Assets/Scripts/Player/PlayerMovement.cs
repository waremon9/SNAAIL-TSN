using System.Collections;
using UnityEngine;

public enum Movement
{
    Free,
    MoveOnly,
    RotateOnly,
    Blocked
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 2;
    [SerializeField]
    private float _rotationSpeed = 2;
    [SerializeField]
    private GameObject _attackVFX = null;

    private Animator _animator;
    public Animator PlayerAnimator
    {
        get { return _animator; }
    }

    private Movement _movement = Movement.Free;

    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Move();
        Roll();


        if (Input.GetKeyDown(KeyCode.F))
        {
            _movement = Movement.Blocked;
            _animator.SetTrigger("Died");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            _animator.SetTrigger("Revived");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _movement = Movement.Blocked;
            _animator.SetTrigger("Hit");
        }
    }

    private void Move()
    {
        if (_movement == Movement.Free || _movement == Movement.MoveOnly)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                _animator.SetBool("Running", isMoving);
                transform.position += transform.forward * _moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                _animator.SetBool("Running", isMoving);
                transform.position -= transform.forward * _moveSpeed * Time.deltaTime;
            }
        }
    }

    private void Rotate()
    {
        if (_movement == Movement.Free || _movement == Movement.RotateOnly)
        {
            if (_animator.GetBool("Casting"))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    transform.LookAt(hit.point);
                }
            }

            if (Input.GetKey(KeyCode.Q))
            {
                isMoving = true;
                transform.Rotate(transform.up, -Time.deltaTime * _rotationSpeed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                isMoving = true;
                transform.Rotate(transform.up, Time.deltaTime * _rotationSpeed);
            }
        }
    }

    private void Roll()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            _movement = Movement.Blocked;
            _animator.SetBool("Running", false);
            _animator.SetTrigger("Roll");
            //StartCoroutine(Rolling(transform.position.z));
        }
    }

    private IEnumerator Rolling(float pos)
    {
        while(Mathf.Abs(pos - transform.position.z) > 1)
        {
            transform.position += transform.forward * Time.deltaTime * _moveSpeed;
            yield return null;
        }
        _movement = Movement.Free;
    }

    private void ToggleAttackVFX()
    {
        if (_attackVFX.activeSelf)
        {
            _attackVFX.SetActive(false);
        } else
        {
            _attackVFX.SetActive(true);
        }
    }

    public void SetMovement(Movement movement)
    {
        _movement = movement;
    }
}
