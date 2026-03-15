using UnityEngine;
using UnityEngine.AI; // This is required for NavMesh

public class EnemyMovement : MonoBehaviour
{
    public Transform player;      // Drag your Player (Soldier_demo) here in Inspector
    private NavMeshAgent agent;
    private Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
{
    if (player != null)
    {
        // 1. 目的地をセット
        agent.SetDestination(player.position);

        // 2. あなたのAnimatorの "isRunning" スイッチを入れる
        // 速度が 0.1 より大きければ走る(true)、そうでなければ止まる(false)
        bool moving = agent.velocity.magnitude > 0.1f;
        anim.SetBool("isRunning", moving); 
    }
}
}
