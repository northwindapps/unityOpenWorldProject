using UnityEngine;
using UnityEngine.AI; // This is required for NavMesh

public class EnemyMovement : MonoBehaviour
{
    public Transform player;      // Drag your Player (Soldier_demo) here in Inspector
    private NavMeshAgent agent;
    private Animator anim;

    public float shootRange = 10f;       // 射程距離 overwritten always with inspectors value
    public float shootInterval = 1.0f;  // 次の射撃までの待ち時間（秒）
    private float nextShootTime = 0f;   

    // --- 変数宣言に追加 ---
    [Header("Shooting Setup")]
    public GameObject bulletPrefab; // 弾のプレハブ
    public Transform firePoint;     // 敵の銃口の位置
    public float bulletSpeed = 30f; // 弾の速さ

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= shootRange)
        {
            agent.isStopped = true;

            Vector3 targetPostition = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(targetPostition);

            if (Time.time >= nextShootTime)
            {
                Debug.Log("Enemy Shooting!");
                anim.SetTrigger("Shoot");

                if (bulletPrefab != null && firePoint != null)
                {
                    Vector3 aimTarget = player.position + Vector3.up * 1.0f;
                    Vector3 aimDirection = (aimTarget - firePoint.position).normalized;
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(aimDirection));
                    Rigidbody rb = bullet.GetComponent<Rigidbody>();
                    if (rb) rb.linearVelocity = aimDirection * bulletSpeed;
                    Destroy(bullet, 3f);
                }
                nextShootTime = Time.time + shootInterval;
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }

    // アニメーションの同期
    anim.SetBool("isRunning", agent.velocity.magnitude > 0.1f && !agent.isStopped);
    }

}
