using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public CharacterController controller;
    public Transform cam;
    public Animator animator;

    public enum State
    {
        Free,
        Busy
    }

    public State movementState;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    void Start()
    {
        animator = GetComponent<Animator>();
        movementState = State.Free;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (movementState == State.Free)
        {
            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Moving", true);
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Moving", false);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Attack()
    {
        movementState = State.Busy;
        animator.SetBool("Attacking", true);
        animator.SetBool("Moving", false);
    }

    void Warp()
    {
        transform.position += transform.forward * 10;
    }

    void FinishWarp()
    {
        movementState = State.Free;
        animator.SetBool("Attacking", false);
    }
}
