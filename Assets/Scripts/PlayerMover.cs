using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer render;
    private Vector2 inputDir;
    private Vector2 dashDir;
    private float jumpTime;
    private bool isJump;
    private bool isGround;
    private bool isDash;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Move();

        Debug.Log(isDash);
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void Move()
    {
        if (inputDir.x > 0)
            transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
        else if (inputDir.x < 0)
            transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
    }

    private void OnMove(InputValue value)
    {
        if (isDash)
            return;

        inputDir = value.Get<Vector2>();
        
        if(inputDir != new Vector2(0, 0))
        {
            dashDir = value.Get<Vector2>();
        }

        animator.SetFloat("Move", Mathf.Abs(inputDir.x));

        if (inputDir.x > 0)
            render.flipX = false;
        else if (inputDir.x < 0)
            render.flipX = true;
    }

    Coroutine jumpRoutine;
    IEnumerator JumpRoutine()
    {
        Debug.Log("������ƾ ����");

        while (isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpTime += Time.deltaTime;

            if (jumpTime > 1f)
            {
                Debug.Log("��� ��");
                break;
            }

            yield return null;
        }
        Debug.Log("������ƾ ��");
    }

    private void OnJump(InputValue value)
    {        
        isJump = value.isPressed;

        if (isJump) 
            jumpTime = 0f;

        if (isJump && isGround)
        {
            jumpRoutine = StartCoroutine(JumpRoutine());
        }

        if (!isJump)
        {
            StopCoroutine(jumpRoutine);
        }
    }

    Coroutine dashRoutine;
    IEnumerator DashRoutine()
    {
        Debug.Log("��÷�ƾ ����");

        Vector2 startPosition = transform.position;
        isDash = true;
        animator.SetTrigger("Dash");

        while (true)
        {
            if (dashDir.x > 0)
                transform.Translate(new Vector2(dashSpeed * Time.deltaTime, 0));
            else if (dashDir.x < 0)
                transform.Translate(new Vector2(-dashSpeed * Time.deltaTime, 0));            

            if (Mathf.Abs(startPosition.x - transform.position.x) > 10)
            {
                inputDir = new Vector2(0, 0);
                isDash = false;
                break;
            }
            yield return null;
        }
    }

    private void OnDash(InputValue value)
    {
        if(!isDash)
            dashRoutine = StartCoroutine(DashRoutine());
    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2.05f, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * 2.05f, Color.red);

        if (hit.collider != null)
        {
            isGround = true;
            animator.SetBool("IsGround", true);
        }
        else
        {
            isGround = false;
            animator.SetBool("IsGround", false);
        }
    }
}
