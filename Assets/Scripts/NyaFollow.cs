using UnityEngine;

public class BoneLookAt : MonoBehaviour
{
    public Transform target;      // Assign the player
    public Vector3 upAxis = Vector3.up; // Optional: adjust if bone is rotated strangely
    public bool onlyYRotation = true;  // Look only on Y axis (common for heads)

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;

        if (onlyYRotation)
        {
            // Keep the bone level; ignore vertical difference
            direction.y = 0;
            if (direction.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(direction, upAxis);
        }
        else
        {
            if (direction.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(direction, upAxis);
        }
    }
}
