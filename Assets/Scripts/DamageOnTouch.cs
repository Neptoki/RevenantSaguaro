using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class DamageOnTouch : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private Volume postProcessVolume;
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private float flashAmount = 0.5f;

    private Vignette vignette;
    private float originalIntensity;

    private void Start()
    {
        if (postProcessVolume != null && postProcessVolume.profile.TryGet<Vignette>(out var vig))
        {
            vignette = vig;
            originalIntensity = vignette.intensity.value;
        }
    }

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

        if (vignette != null)
        {
            StartCoroutine(FlashVignette());
        }
    }

    private IEnumerator FlashVignette()
    {
        float elapsed = 0f;
        float targetIntensity = originalIntensity + flashAmount;

        while (elapsed < flashDuration)
        {
            vignette.intensity.value = Mathf.Lerp(targetIntensity, originalIntensity, elapsed / flashDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        vignette.intensity.value = originalIntensity;
    }
}