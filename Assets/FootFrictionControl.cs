using UnityEngine;

public class FootFrictionControl : MonoBehaviour
{
    [SerializeField] private Collider footCollider;
    [SerializeField] private Collider toeCollider;
    
    [SerializeField] private PhysicsMaterial highFrictionMat;
    [SerializeField] private PhysicsMaterial lowFrictionMat;
    [SerializeField] private float rayDistance = 0.15f;
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance);
        
        Vector3 worldVel = rb.linearVelocity;
        bool isMoving = new Vector3(worldVel.x, 0, worldVel.z).sqrMagnitude > 0.05f;

        PhysicsMaterial targetMat = (!isGrounded || isMoving) ? lowFrictionMat : highFrictionMat;

        if (footCollider != null) footCollider.material = targetMat;
        if (toeCollider != null) toeCollider.material = targetMat;
    }
}