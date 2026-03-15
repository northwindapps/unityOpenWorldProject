using UnityEngine;
using UnityEngine.AI; // This is required for NavMesh

public class EnemyMovement : MonoBehaviour
{
    public Transform player;      // Drag your Player (Soldier_demo) here in Inspector
    private NavMeshAgent agent;
    private Animator anim;

    public float shootRange = 1.0f;     // 射程距離 overwritten always with inspectors value
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
    agent.SetDestination(player.position);

    if (distance <= shootRange)
    {
        // 1. まず足を止める
        agent.isStopped = true;

        // 2. プレイヤーの方を「じわっと」ではなく「即座に」向かせる
        // (y軸だけ固定して、変な傾きを防ぐ)
        Vector3 targetPostition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPostition);

        // 3. 射撃タイマーの判定（ここには視界判定を入れない方が確実です）
        if (Time.time >= nextShootTime)
        {
            // ログを出して動いているか確認
            Debug.Log("Enemy Shooting!"); 
            
            anim.SetTrigger("Shoot");

            if (bulletPrefab != null && firePoint != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null) rb.linearVelocity = firePoint.forward * bulletSpeed;
                Destroy(bullet, 3f);
            }
            nextShootTime = Time.time + shootInterval;
        }
    }
    else
    {
        // 射程外なら追いかける
        agent.isStopped = false;
    }

    // アニメーションの同期
    anim.SetBool("isRunning", agent.velocity.magnitude > 0.1f && !agent.isStopped);
    }

}
