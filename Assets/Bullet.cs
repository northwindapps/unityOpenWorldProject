using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
