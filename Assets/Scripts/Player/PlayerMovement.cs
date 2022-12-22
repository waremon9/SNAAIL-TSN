using System.Collections;
using UnityEngine;
using Enums;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 2;
    [SerializeField]
    private float _rotationSpeed = 2;

    private Plane _raycastPlane;

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
        _raycastPlane = new Plane(Vector3.up, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;
        Rotate();
        Move();
        Roll();

        _animator.SetBool("Running", isMoving);
    }

    private void Move()
    {
        if (_movement == Movement.Free || _movement == Movement.MoveOnly)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                isMoving = true;
                transform.position += transform.forward * _moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                isMoving = true;
                transform.position -= transform.forward * _moveSpeed * Time.deltaTime;
            }
        }
    }

    private void Rotate()
    {
        if (_movement == Movement.Free || _movement == Movement.RotateOnly)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hit;

            if (_raycastPlane.Raycast(ray, out hit))
            {
                if(Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(ray.GetPoint(hit).x, ray.GetPoint(hit).z)) > 0.1f)
                {
                    transform.LookAt(ray.GetPoint(hit));
                }
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

    public void SetMovement(Movement movement)
    {
        _movement = movement;
    }

    private void Revive()
    {
        PlayerAnimator.SetTrigger("Revive");
    }
}
