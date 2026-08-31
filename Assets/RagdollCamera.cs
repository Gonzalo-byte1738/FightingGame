using UnityEngine;
using UnityEngine.InputSystem; // Uses the New Input System package

public class RagdollCamera : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform target; // Drag your Ragdoll's HIPS or SPINE here

    [Header("Camera Settings")]
    public float sensitivity = 0.2f;
    public float distance = 4f;
    public Vector2 pitchLimits = new Vector2(-15f, 60f); // How far up/down you can look
    
    private float yaw;
    private float pitch;

    void Start()
    {
        // Lock and hide the mouse cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get mouse delta from the New Input System
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            yaw += mouseDelta.x * sensitivity;
            pitch -= mouseDelta.y * sensitivity;

            // Clamp pitch so the camera doesn't flip upside down
            pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate camera rotation
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            
            // Calculate camera position offset backward from target
            Vector3 position = target.position - (rotation * Vector3.forward * distance);

            // Apply position and rotation
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}