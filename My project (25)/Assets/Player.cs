using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    //인터페이스에서 조절을 하기때문에 겟터셋터 사용안함
    [Header("스탯")]
    [SerializeField] private int atk = 10;
    [SerializeField] private float hp = 100f;
    [SerializeField] private float maxhp = 100f;

    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f;
    

    [Header("점프 설정")]
    [SerializeField] private float jumpPower = 5f;

    [Header("지면 체크")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero; //처음 속도 0으로 해야함

      
    }

    void Update()
    {
        // 지면 체크
        GroundCheck();

        // 점프 입력 ,땅인지 확인해서 점프 한번만 하기
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
       
        Move();
    }

  
    public void Move()
    {
        // 입력 받기
        float hz = Input.GetAxis("Horizontal"); 
        float vt = Input.GetAxis("Vertical");    

        // 이동 방향 벡터 계산
        Vector3 movement = new Vector3(hz, 0f, vt).normalized;

        // 이동 벡터에 속도를 적용해야지 움직인다.
        Vector3 targetVelocity = movement * moveSpeed;

        // Y축 속도는 유지
        targetVelocity.y = rb.velocity.y;

       
        rb.velocity = targetVelocity;

    }

   
  

    
    public void Jump()
    {
        // 리지드 바디에서 변수 지정한 값만큼 힘으로 점프를 한다.
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        Debug.Log("점프!");
    }

  
    public void GroundCheck()
    {
        // 캐릭터 아래쪽으로 레이케스트
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.5f, groundLayer);
          
    }

    
    void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.down * (groundCheckDistance + 0.5f));
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            hp = 0;
            Die();
        }
        Debug.Log("플레이어 HP: " + hp + "/" + maxhp);
    }

   
    private void Die()
    {
        Debug.Log("플레이어 사망!");
      
    }
}