using System;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start, Update 유니티 이벤트 함수의 같은 이름이 있는지 조사
    // 같은 이름이 있으면 유니티에서 정해놓은 실행 시간에 그 함수를 실행

    // FPS : Frame Per Second; 초당 몇 프레임
    // 프레임 : 이미지 1장 게임엔진이 이미지 1장을 생성하는데 걸리는 시간
    // ex). 60FPS : 1초당 60프레임 생성

    // 속도, 방향
    [Header("Move")]
    public float moveSpeed = 3f;  // 캐릭터의 이동 속도
    public float jumpForce = 5f;  // 플레이어의 점프
    private float moveInput;  // 플레이어의 방향 및 인풋 데이터 저장

    public Transform startTransform;  // 캐릭터가 시작할 위치를 저장하는 변수
    public Rigidbody2D rigidbody2D;

    [Header("jump")]
    public bool isGrounded;  // true : 점프가능, false : 점프불가능
    public float groundDistance = 2f;
    public LayerMask groundLayer;

    [Header("Flip")]
    public SpriteRenderer spriteRenderer;
    private bool facingRight = true;
    private int facingDirection = 1;

    public Animator anim;
    private bool isMove;

    // Start is called before the first frame update
    // 첫 프레임이 불러지기 전에 (한번) 시작한다
    void Start()
    {

        anim = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        // 현재 내 위치 <= 새로운 x, y 저장하는 데이터 타입 (현재 x좌표, 10, y좌표)
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
    // 1 프레임에 한번 호출된다
    void Update()
    {
        //함수 이름 앞에 마우스 커서를 누르고 ctrl + R + R 을 누르면 함수 이름을 한번에 바꿀 수 있음

        HandleAnimation();
        CollisionCheck();
        HandleInput();
        HandleFlip();
        Move();

        FallDownCheck();

    }

    //y의 높이가 특정 지점보다 낮을 때 낙사한 것으로 간주한다 => 충돌 체크 대체
    private void FallDownCheck()
    {
        if (transform.position.y < -11)
        {
            InitializePlayerStatus();
        }
    }

    private void HandleAnimation()
    {
        //rigidbody.velocity : 현재 rigidbody 속도 = 0 움직이지 않는 상태, 속도 = 1 움직이는 상태
        isMove = rigidbody2D.velocity.x != 0;

        anim.SetBool("isMove", isMove);
        anim.SetBool("isGrounded", isGrounded);

        //SetFloat 함수에 의해서 y최대 일 때 1로 변환.. y최소일 때 -1로 변환
        //점프키를 누르면 순간적으로 y 높이 증가, 중력에 의해서 점점 y 속도 -까지 내려감
        anim.SetFloat("yVelocity", rigidbody2D.velocity.y);
    }

    /// <summary>
    /// 점프를 할 때 땅인지 아닌지 체크하는 기능 -> Collider Check
    /// </summary>
    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundLayer);
    }

    /// <summary>
    /// 플레이어의 입력 값을 받아와야 합니다. a,d 키보드 좌 우 키를 눌렀을 때 -1 ~ 1 반환하는 클래스
    /// 플레이어의 입력을 받아오는 코드
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
