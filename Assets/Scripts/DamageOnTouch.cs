using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private string playerTag = "Player";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            DealDamage(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            DealDamage(other.gameObject);
        }
    }

    private void DealDamage(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(damageAmount);
        }
    }
}