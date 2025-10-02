using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    private InputMaster controls;

    private float mouseSensitivity = 100f;
    private Vector2 mouseLook;
    private float xRotation = 0f;
    public Transform playerBody;

    void Awake()
    {
        playerBody = transform.parent;

        controls = new InputMaster();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Look();
    }
    private void Look()
    {
        mouseLook = controls.Player.Look.ReadValue<Vector2>();

        float mouseX = mouseLook.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseLook.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}