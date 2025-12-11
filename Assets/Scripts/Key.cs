using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 100, 0);
    [SerializeField] private float floatAmplitude = 0.25f;
    [SerializeField] private float floatFrequency = 2f;
    private Vector3 startPos;

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
