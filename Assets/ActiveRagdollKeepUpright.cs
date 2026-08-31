using UnityEngine;

public class ActiveRagdollKeepUpright : MonoBehaviour
{
    [Header("Target Tracking")]
    [SerializeField] private Transform ghostHips;
    [SerializeField] private Transform ghostFoot;
    [SerializeField] private Transform leftFoot;
    [SerializeField] private Transform rightFoot;

    [Header("Hover / Height Settings")]
    [SerializeField] private float heightSpring = 1500f;
    [SerializeField] private float heightDamper = 120f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Center of Mass Balancing")]
    [SerializeField] private float comBalanceForce = 150f; // Reduced strength to stop dragging
    [SerializeField] private float comDamper = 15f;
    [SerializeField] private float maxCoMForce = 300f;     // Hard cap on balance correction

    private Rigidbody hipRb;

    void Start()
    {
        hipRb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (hipRb == null || ghostHips == null || ghostFoot == null) return;

        // 1. DYNAMIC HEIGHT LIFT
        float dynamicTargetHeight = ghostHips.position.y - ghostFoot.position.y;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, dynamicTargetHeight * 2f, groundLayer))
        {
            float currentHeight = hit.distance;
            float heightError = dynamicTargetHeight - currentHeight;

            float liftForce = (heightError * heightSpring) - (hipRb.linearVelocity.y * heightDamper);
            if (liftForce > 0)
            {
                hipRb.AddForce(Vector3.up * liftForce, ForceMode.Force);
            }
        }

        // 2. CENTER OF MASS (CoM) FOOT CORRECTION
        if (leftFoot != null && rightFoot != null)
        {
            Vector3 feetCenter = (leftFoot.position + rightFoot.position) * 0.5f;
            
            // Vector pointing FROM Hips TO Feet Center (Target vector)
            Vector3 balanceError = feetCenter - transform.position;
            balanceError.y = 0; // Evaluate strictly on the horizontal X/Z plane

            // Apply corrective pull toward feet center, damped by current horizontal velocity
            Vector3 horizontalVel = new Vector3(hipRb.linearVelocity.x, 0f, hipRb.linearVelocity.z);
            Vector3 correctiveForce = (balanceError * comBalanceForce) - (horizontalVel * comDamper);

            // Clamp force to prevent sudden violent yanks during walking/kicking
            correctiveForce = Vector3.ClampMagnitude(correctiveForce, maxCoMForce);

            hipRb.AddForce(correctiveForce, ForceMode.Force);
        }
    }
}