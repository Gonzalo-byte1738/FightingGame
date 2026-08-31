using UnityEngine;

public class UprightController : MonoBehaviour
{
    [SerializeField] private Transform ghostHips;
    [SerializeField] private float uprightSpring = 500f;
    [SerializeField] private float uprightDamper = 30f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (ghostHips == null) return;

        // Direct error calculation: target * inverse(current) -> change to current target delta
        Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(ghostHips.rotation);
        deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);

        // Normalize angle to [-180, 180] range
        if (angle > 180f) angle -= 360f;

        if (Mathf.Abs(angle) > 0.001f)
        {
            // Calculate rotational spring torque
            Vector3 torque = axis.normalized * (angle * Mathf.Deg2Rad * uprightSpring) - (rb.angularVelocity * uprightDamper);
            
            // Use ForceMode.Force or Acceleration consistently
            rb.AddTorque(-torque, ForceMode.Acceleration); // Note the negative sign to apply corrective force
        }
    }
}