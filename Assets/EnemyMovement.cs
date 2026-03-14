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
            // 1. Tell the AI where to go
            agent.SetDestination(player.position);

            // 2. Sync the Animation with the AI movement speed
            // If the agent is moving, the animator will play the run/walk animation
            float speed = agent.velocity.magnitude;
            anim.SetFloat("Speed", speed); 
            
            // Note: Change "Speed" to whatever the float parameter 
            // is called in your Soldier's Animator Controller.
        }
    }
}
