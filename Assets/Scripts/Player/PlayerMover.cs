using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] LayerMask groundLayer;

    public enum UpDown { None, Up, Down }

    private PlayerAttacker playerAttacker;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer render;
    private Vector2 inputDir;
    private Vector2 lookDir = new Vector2(1, 0);
    private float lookUpDownTime;
    private float jumpTime;
    private float dashTime;
    private bool isLook;
    private bool isJump;
    private bool isGround;
    private bool isDash;
    private bool isCameraMove;
    private UpDown upDown;

    private void Awake()
    {
        playerAttacker = GetComponent<PlayerAttacker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();

        DefaultLook();
    }

    private void Update()
    {
        if (!isDash) 
            Move();

        Debug.Log(playerAttacker.IsAttack());
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void Move()
    {
        if (inputDir.x > 0)
        {
            transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
            render.flipX = false;
        }
        else if (inputDir.x < 0)
        {
            transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
            render.flipX = true;
        }
    }

    Coroutine lookingRoutine;
    IEnumerator LookingRoutine()
    {
        while (isLook)
        {
            if (playerAttacker.IsAttack())
            {
                DefaultLook();

                yield return new WaitUntil(() => playerAttacker.IsAttack() == false);
            }

            lookUpDownTime += Time.deltaTime;

            if (inputDir.x != 0 || inputDir.y == 0)
            {
                DefaultLook();
                isLook = false;
                animator.SetFloat("LookUpDown", inputDir.y);
                break;
            }
            else if (inputDir.y > 0)
            { 
                animator.SetBool("isLook", true);
                animator.SetFloat("LookUpDown", inputDir.y);

                if (inputDir.x == 0)
                {
                    if(lookUpDownTime > 0.7f)
                    {
                        upDown = UpDown.Up;
                        isCameraMove = true;
                    }
                }
            }
            else if (inputDir.y < 0)
            {
                animator.SetBool("isLook", true);
                animator.SetFloat("LookUpDown", inputDir.y);

                if (inputDir.x == 0)
                {
                    if (lookUpDownTime > 0.7f)
                    {
                        upDown = UpDown.Down;
                        isCameraMove = true;
                    }
                }
            }
            yield return null;
        }
    }

    private void DefaultLook()
    {
        lookUpDownTime = 0;
        animator.SetBool("isLook", false);
        animator.Play("Idle");
        upDown = UpDown.None;
        isCameraMove = false;
    }

    //TODO : ��, �Ʒ� ����Ű ������ ���� �Ĵٺ�, ī�޶� �̵� and �ٶ󺸴� ���⿡ ���� ���� �ٸ� ��ų ���, ��ų �� �� Move �ȵǰ�
    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
        
        if(inputDir != new Vector2(0, 0) && !isDash)    // ����Ű�� ������ �ʰ� ����� �� �������� �ʴ°� ����, ��� �� ���� �ٲ�°� ����
            lookDir = value.Get<Vector2>();

        if(inputDir.y != 0)
        {
            isLook = true;
            lookingRoutine = StartCoroutine(LookingRoutine());
        }

        animator.SetFloat("Move", Mathf.Abs(inputDir.x));
    }

    Coroutine jumpRoutine;
    IEnumerator JumpRoutine()
    {
        while (isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpTime += Time.deltaTime;

            if (jumpTime > 1f)
            {
                break;
            }

            yield return null;
        }
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
        isDash = true;
        dashTime = 0f;  // TODO : ��� �ִϸ��̼ǰ� �ð� ����ȭ �ʿ�, ������ ��� ������ �����鼭 ��ø� �ϸ� ��� ���� �� �����ϴ� ���� ���� �ʿ�
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation; // ��� �� �߷� ����޾� �������°� ����
        animator.SetTrigger("Dash");

        while (true)
        {
            if (lookDir.x > 0)
                transform.Translate(new Vector2(dashSpeed * Time.deltaTime, 0));
            else if (lookDir.x < 0)
                transform.Translate(new Vector2(-dashSpeed * Time.deltaTime, 0));

            dashTime += Time.deltaTime;

            if(dashTime > 0.3f)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                isDash = false;
                break;
            }
            yield return null;
        }
    }

    private void OnDash(InputValue value)
    {
        if (!isDash)    // ��� �� �ٽ� ����ϴ°� ����
            dashRoutine = StartCoroutine(DashRoutine());
    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 5f, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * 5f, Color.red);

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

    public UpDown LookingUpDown()
    {
        return upDown;
    }

    public Vector2 LookDir()
    {
        return lookDir;
    }

    public Vector2 InputDir()
    {
        return inputDir;
    }

    public bool IsLook()
    {
        return isLook;
    }

    public bool IsDash()
    {
        return isDash;
    }

    public bool IsCameraMove()
    {
        return isCameraMove;
    }
}
