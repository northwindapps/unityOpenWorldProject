using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float rotationSpeed = 10f;

    [Header("Shooting")]
    // これを追加：弾のプレハブをインスペクターから入れるため
    public GameObject bulletPrefab; 
    // これを追加：銃口の位置を指定するため
    public Transform firePoint;     
    public float bulletSpeed = 20f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Animator anim;
    private Transform cam;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
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

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = (forward * z + right * x).normalized;

        float targetAngle = Mathf.Atan2(cam.forward.x, cam.forward.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        if (move.magnitude >= 0.1f)
        {
            controller.Move(move * speed * Time.deltaTime);
        }

        bool isMoving = (move.magnitude > 0);
        if (anim != null) 
        {
            anim.SetBool("isRunning", isMoving);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 射撃処理を修正
        if (Input.GetButtonDown("Fire1")) 
        {
            if (anim != null) anim.SetTrigger("Shoot");

            // 弾を生成して飛ばす処理を追加
            if (bulletPrefab != null && firePoint != null)
            {
                // 1. 弾を生成
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                // 2. 弾を前に飛ばす
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null) rb.linearVelocity = firePoint.forward * bulletSpeed;
                // 3. 3秒後に弾を消す
                Destroy(bullet, 3f);
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
