using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public float pickupForce = 150f;
    public LayerMask pickupLayer;

    [Header("References")]
    public Transform holdPoint;
    public InputActionReference Pickup;

    private Rigidbody heldObject;

    private void OnEnable()
    {
        Pickup.action.Enable();
        Pickup.action.started += TryPickup;
        Pickup.action.canceled += DropObject;
    }

    private void OnDisable()
    {
        Pickup.action.started -= TryPickup;
        Pickup.action.canceled -= DropObject;
        Pickup.action.Disable();
    }


    private void Update()
    {
        if (heldObject != null)
        {
            Vector3 moveDir = (holdPoint.position - heldObject.position);
            heldObject.AddForce(moveDir * pickupForce);

            heldObject.angularVelocity = Vector3.zero;
        }
    }

    private void TryPickup(InputAction.CallbackContext context)
    {
        Debug.Log("Interact pressed");

        if (heldObject != null)
            return;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                heldObject = rb;
                heldObject.useGravity = false;
                heldObject.linearDamping = 10f;
            }
        }
    }

    private void DropObject(InputAction.CallbackContext context)
    {
        if (heldObject == null)
            return;

        heldObject.useGravity = true;
        heldObject.linearDamping = 0f;
        heldObject = null;
    }
}