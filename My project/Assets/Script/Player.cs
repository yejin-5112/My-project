using System;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start, Update ����Ƽ �̺�Ʈ �Լ��� ���� �̸��� �ִ��� ����
    // ���� �̸��� ������ ����Ƽ���� ���س��� ���� �ð��� �� �Լ��� ����

    // FPS : Frame Per Second; �ʴ� �� ������
    // ������ : �̹��� 1�� ���ӿ����� �̹��� 1���� �����ϴµ� �ɸ��� �ð�
    // ex). 60FPS : 1�ʴ� 60������ ����

    // �ӵ�, ����
    [Header("Move")]
    public float moveSpeed = 3f;  // ĳ������ �̵� �ӵ�
    public float jumpForce = 5f;  // �÷��̾��� ����
    private float moveInput;  // �÷��̾��� ���� �� ��ǲ ������ ����

    public Transform startTransform;  // ĳ���Ͱ� ������ ��ġ�� �����ϴ� ����
    public Rigidbody2D rigidbody2D;

    [Header("jump")]
    public bool isGrounded;  // true : ��������, false : �����Ұ���
    public float groundDistance = 2f;
    public LayerMask groundLayer;

    [Header("Flip")]
    public SpriteRenderer spriteRenderer;
    private bool facingRight = true;
    private int facingDirection = 1;

    public Animator anim;
    private bool isMove;

    // Start is called before the first frame update
    // ù �������� �ҷ����� ���� (�ѹ�) �����Ѵ�
    void Start()
    {

        anim = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        // ���� �� ��ġ <= ���ο� x, y �����ϴ� ������ Ÿ�� (���� x��ǥ, 10, y��ǥ)
        // transform.position = new Vector2(transform.position.x, 10);

        InitializePlayerStatus();
    }

    void InitializePlayerStatus()
    {
        transform.position = startTransform.position;
        rigidbody2D.velocity = Vector2.zero;
        facingRight = true;
        spriteRenderer.flipX = false;
    }

    // Update is called once per frame
    // 1 �����ӿ� �ѹ� ȣ��ȴ�
    void Update()
    {
        //�Լ� �̸� �տ� ���콺 Ŀ���� ������ ctrl + R + R �� ������ �Լ� �̸��� �ѹ��� �ٲ� �� ����

        HandleAnimation();
        CollisionCheck();
        HandleInput();
        HandleFlip();
        Move();

        FallDownCheck();

    }

    //y�� ���̰� Ư�� �������� ���� �� ������ ������ �����Ѵ� => �浹 üũ ��ü
    private void FallDownCheck()
    {
        if (transform.position.y < -11)
        {
            InitializePlayerStatus();
        }
    }

    private void HandleAnimation()
    {
        //rigidbody.velocity : ���� rigidbody �ӵ� = 0 �������� �ʴ� ����, �ӵ� = 1 �����̴� ����
        isMove = rigidbody2D.velocity.x != 0;

        anim.SetBool("isMove", isMove);
        anim.SetBool("isGrounded", isGrounded);

        //SetFloat �Լ��� ���ؼ� y�ִ� �� �� 1�� ��ȯ.. y�ּ��� �� -1�� ��ȯ
        //����Ű�� ������ ���������� y ���� ����, �߷¿� ���ؼ� ���� y �ӵ� -���� ������
        anim.SetFloat("yVelocity", rigidbody2D.velocity.y);
    }

    /// <summary>
    /// ������ �� �� ������ �ƴ��� üũ�ϴ� ��� -> Collider Check
    /// </summary>
    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundLayer);
    }

    /// <summary>
    /// �÷��̾��� �Է� ���� �޾ƿ;� �մϴ�. a,d Ű���� �� �� Ű�� ������ �� -1 ~ 1 ��ȯ�ϴ� Ŭ����
    /// �÷��̾��� �Է��� �޾ƿ��� �ڵ�
    /// </summary>
    private void HandleInput()
    {
        moveInput = Input.GetAxis("Horizontal");

        JumpButten();
    }

    private void HandleFlip()
    {
        if (facingRight && moveInput < 0)
        {
            Flip();
        }
        else if (!facingRight && moveInput > 0)
        {
            Flip();
        }
    }
    private void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        spriteRenderer.flipX = !facingRight;
    }

    private void JumpButten()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            Jump();
        }
    }

    private void Move()
    {
        rigidbody2D.velocity = new Vector2(moveSpeed * moveInput, rigidbody2D.velocity.y);
    }

    private void Jump()
    {
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundDistance));
    }
}
