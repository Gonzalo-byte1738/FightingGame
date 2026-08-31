using UnityEngine;

public class BalanceController : MonoBehaviour
{
    public Rigidbody hipRigidbody;
    public float uprightSpring = 1200f;
    public float uprightDamper = 120f;

    void FixedUpdate()
    {
        // 1. Calculate orientation error
        Quaternion currentRotation = hipRigidbody.transform.rotation;
        Quaternion targetRotation = Quaternion.FromToRotation(hipRigidbody.transform.up, Vector3.up) * currentRotation;
        
        Quaternion deltaRotation = targetRotation * Quaternion.Inverse(currentRotation);
        deltaRotation.ToAngleAxis(out float angleInDegrees, out Vector3 rotationAxis);

        if (angleInDegrees > 180f) angleInDegrees -= 360f;
        if (Mathf.Abs(angleInDegrees) < 0.05f) return;

        // 2. Compute proportional-derivative torque
        Vector3 torque = rotationAxis.normalized * (angleInDegrees * uprightSpring);
        Vector3 damping = hipRigidbody.angularVelocity * uprightDamper;

        // 3. Apply in world space
        hipRigidbody.AddTorque(torque - damping, ForceMode.Force);
    }
}