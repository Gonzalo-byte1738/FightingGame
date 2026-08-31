using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsCharacterController : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform; 
    public Transform ghostRig;         
    public Animator ghostAnimator;    
    public Rigidbody hipRigidbody;    

    [Header("Movement Settings")]
    public float maxVelocity = 4f;
    public float acceleration = 25f; // Acceleration rate rather than flat impulse force
    public float turnSpeed = 12f;

    private Vector2 inputVector;

    void Update()
    {
        ReadInput();

        if (ghostAnimator != null)
        {
            ghostAnimator.SetFloat("Speed", inputVector.magnitude, 0.1f, Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        // Keep the ghost synced to physical hips
        ghostRig.position = hipRigidbody.position;

        if (inputVector.magnitude < 0.1f) return;

        // Calculate camera-relative target direction
        float targetAngle = Mathf.Atan2(inputVector.x, inputVector.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        // Rotate ghost rig
        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
        ghostRig.rotation = Quaternion.Slerp(ghostRig.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed);

        // Smoothly accelerate hip velocity towards target velocity
        Vector3 targetVelocity = moveDir * maxVelocity;
        Vector3 currentHorizontalVel = new Vector3(hipRigidbody.linearVelocity.x, 0f, hipRigidbody.linearVelocity.z);
        Vector3 velError = targetVelocity - currentHorizontalVel;

        // Apply controlled acceleration force
        hipRigidbody.AddForce(velError * acceleration, ForceMode.Acceleration);
    }

    private void ReadInput()
    {
        inputVector = Vector2.zero;
        if (Keyboard.current == null) return;

        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.wKey.isPressed) vertical += 1f;
        if (Keyboard.current.sKey.isPressed) vertical -= 1f;
        if (Keyboard.current.aKey.isPressed) horizontal -= 1f;
        if (Keyboard.current.dKey.isPressed) horizontal += 1f;

        inputVector = new Vector2(horizontal, vertical).normalized;
    }
}