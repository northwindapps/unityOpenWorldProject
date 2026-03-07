using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float rotationSpeed = 10f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Animator anim;
    
    // 追加：カメラの向きを参照するための変数
    private Transform cam;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        // メインカメラの情報を取得
        cam = Camera.main.transform;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 1. カメラの向きに基づいた移動方向を計算
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0f; // 地面と水平に動くように
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = (forward * z + right * x).normalized;

        if (move.magnitude >= 0.1f)
        {
            // 2. 移動方向にスムーズに回転
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

            // 3. 移動実行
            controller.Move(move * speed * Time.deltaTime);
        }

        // アニメーションの更新
        bool isMoving = (move.magnitude > 0);
        if (anim != null) 
        {
            anim.SetBool("isRunning", isMoving);
        }

        // ジャンプ処理
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (Input.GetButton("Fire1")) 
        {
            Debug.Log("Bang!");
            anim.SetTrigger("Shoot");
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
