using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 2;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        bool isMoving = false;
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.Z))
        {
            isMoving = true;
            moveDirection.z += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            isMoving = true;
            moveDirection.z -= 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            isMoving = true;
            moveDirection.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            isMoving = true;
            moveDirection.x += 1;
        }
        if(isMoving)
        {
            _animator.SetBool("Running", true);
        } else
        {
            _animator.SetBool("Running", false);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            _animator.SetTrigger("Died");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            _animator.SetTrigger("Revived");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _animator.SetTrigger("MeleeAttack");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _animator.SetTrigger("RangeAttack");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _animator.SetTrigger("Hit");
        }

        this.transform.position += moveDirection.normalized * (Time.deltaTime * this._moveSpeed);
    }
}
