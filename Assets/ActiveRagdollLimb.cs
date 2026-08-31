using UnityEngine;

public class ActiveRagdollLimb : MonoBehaviour
{
    [SerializeField] private Transform ghostBone; // Matching bone on ghostRig
    private ConfigurableJoint joint;
    private Quaternion initialJointLocalRotation;
    private Quaternion initialGhostLocalRotation;

    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        
        // Lock initial rest orientations at Start
        initialJointLocalRotation = transform.localRotation;
        if (ghostBone != null)
        {
            initialGhostLocalRotation = ghostBone.localRotation;
        }
    }

    void FixedUpdate()
    {
        if (ghostBone == null || joint == null) return;

        // Calculate local rotation delta relative to bind pose
        Quaternion targetDelta = Quaternion.Inverse(initialGhostLocalRotation) * ghostBone.localRotation;
        
        // Convert target delta to joint space
        joint.targetRotation = Quaternion.Inverse(targetDelta);
    }
}