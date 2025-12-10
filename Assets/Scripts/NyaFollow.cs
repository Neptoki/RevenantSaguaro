using UnityEngine;

public class BoneLookAt : MonoBehaviour
{
    public Transform target;
    public Vector3 upAxis = Vector3.up;
    public bool onlyYRotation = true;

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
    }
}