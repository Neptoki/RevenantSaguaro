using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
 
public class PlayerInteraction : MonoBehaviour
{
    public Camera mainCam;
    public float interactionDistance = 2f;
 
    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;

    public InputActionReference interact;
    public Transform cam;
    public float interactRange = 3f;
 
 
    private void Update() {
        InteractionRay();
    }
 
    void InteractionRay() {
        Ray ray = mainCam.ViewportPointToRay(Vector3.one/2f);
        RaycastHit hit;
 
        bool hitSomething = false;
 
        if (Physics.Raycast(ray, out hit, interactionDistance)) {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
 
            if (interactable != null) {
                hitSomething = true;
                interactionText.text = interactable.GetDescription();
 
                if (Input.GetKeyDown(KeyCode.E)) {
                    interactable.Interact();
                }
            }
        }
 
        interactionUI.SetActive(hitSomething);
    }

    private void OnEnable()
    {
        interact.action.Enable();
        interact.action.performed += OnInteract;
    }

    private void OnDisable()
    {
        interact.action.performed -= OnInteract;
        interact.action.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, interactRange))
        {
            DoorInteraction door = hit.transform.GetComponent<DoorInteraction>();
            if (door != null)
            {
                door.Interact(gameObject);
            }
        }
    }
}