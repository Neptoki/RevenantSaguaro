using UnityEngine;
public class HealingItem : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 100, 0);
    [SerializeField] private float floatAmplitude = 0.25f;
    [SerializeField] private float floatFrequency = 2f;
    public float healAmount = 5f;
    private Vector3 startPos;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().HealPlayer(healAmount);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        //spin and float
        transform.Rotate(rotationSpeed * Time.deltaTime);
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}