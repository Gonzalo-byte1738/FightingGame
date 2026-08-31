using UnityEngine;

public class KeepUpright : MonoBehaviour
{
    [Header("Upright Force")]
    public float uprightTorque = 500f; 
    public float torqueDamping = 30f;
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Vector cross product gives exact rotational axis and tilt angle
        Vector3 currentUp = transform.up;
        Vector3 targetUp = Vector3.up;

        Vector3 torqueAxis = Vector3.Cross(currentUp, targetUp);
        float angle = Vector3.Angle(currentUp, targetUp);

        if (angle > 0.001f)
        {
            // Apply proportional-derivative torque to upright the hips
            Vector3 targetTorque = torqueAxis.normalized * (angle * Mathf.Deg2Rad * uprightTorque);
            Vector3 damping = rb.angularVelocity * torqueDamping;

            rb.AddTorque(targetTorque - damping, ForceMode.Acceleration);
        }
    }
}