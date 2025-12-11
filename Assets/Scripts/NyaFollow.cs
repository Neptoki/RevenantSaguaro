using UnityEngine;

public class BoneLookAt : MonoBehaviour
{
    [Header("Look At")]
    public Transform target;
    public Vector3 upAxis = Vector3.up;
    public bool onlyYRotation = true;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] clips;
    public float minInterval = 5f;
    public float maxInterval = 15f;
    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;

    private float nextSoundTime;

    void Start()
    {
        if (audioSource != null)
            audioSource.spatialBlend = 0f;
        
        ScheduleNextSound();
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;

        if (onlyYRotation)
        {
            direction.y = 0;
            if (direction.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(direction, upAxis);
        }
        else
        {
            if (direction.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(direction, upAxis);
        }

        if (audioSource != null && clips.Length > 0 && Time.time >= nextSoundTime)
        {
            PlayRandomClip();
            ScheduleNextSound();
        }
    }

    void PlayRandomClip()
    {
        int index = Random.Range(0, clips.Length);
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(clips[index]);
    }

    void ScheduleNextSound()
    {
        nextSoundTime = Time.time + Random.Range(minInterval, maxInterval);
    }
}