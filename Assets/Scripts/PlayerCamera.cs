using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public float sensX;
    public float sensY;
    public Transform orientation;
    public InputActionReference look;
    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void OnEnable()
    {
        if (look != null && look.action != null)
        {
            look.action.Enable();
            look.action.performed += OnLookPerformed;
        }
    }

    private void OnDisable()
    {
        if (look != null && look.action != null)
        {
            look.action.Disable();
            look.action.performed -= OnLookPerformed;
        }
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (PauseMenu.isPaused)
            return;

        if (PlayerStats.isDead) 
        return;

        Vector2 lookDelta = Vector2.zero;
        bool isMouseInput = false;

        //mouse input first
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            if (mouseDelta.sqrMagnitude > 0.0001f)
            {
                lookDelta = mouseDelta;
                isMouseInput = true;
            }
        }

        //then gamepad
        if (!isMouseInput)
        {
            if (look != null && look.action != null)
            {
                lookDelta = look.action.ReadValue<Vector2>();
            }
        }

        //sens
        float mouseX, mouseY;

        if (isMouseInput)
        {
            mouseX = lookDelta.x * sensX * 0.025f;
            mouseY = lookDelta.y * sensY * 0.025f;
        }
        else
        {
            //gamepad
            mouseX = lookDelta.x * Time.deltaTime * sensX * 100f;
            mouseY = lookDelta.y * Time.deltaTime * sensY * 100f;
        }

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Camera rotation + orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}