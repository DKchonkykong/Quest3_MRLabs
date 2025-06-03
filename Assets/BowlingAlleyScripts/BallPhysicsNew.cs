using UnityEngine;
 
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OVRGrabbable))]
public class OVRBallThrow : MonoBehaviour
{
    private Rigidbody rb;
    private OVRGrabbable grabbable;
 
    public Transform resetPoint;
    public float resetDelay = 5f;
    public float velocityMultiplier = 0.5f; // Reduce throw velocity
    public float maxVelocity = 10f; // Cap maximum velocity
    public PhysicMaterial ballPhysicsMaterial; // Assign in inspector
    public LayerMask boundaryLayer = -1; // Layer for boundary detection
    public float boundaryCheckDistance = 1f;
 
    private bool wasGrabbed = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabbable = GetComponent<OVRGrabbable>();
        
        // Apply physics material if assigned
        if (ballPhysicsMaterial != null)
        {
            Collider collider = GetComponent<Collider>();
            if (collider != null)
                collider.material = ballPhysicsMaterial;
        }
    }

    void Start()
    {
        // Ensure the ball starts at the reset point and doesn't fall due to gravity
        ResetBall();
    }
 
    void Update()
    {
        if (grabbable.isGrabbed)
        {
            wasGrabbed = true;
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        else if (wasGrabbed)
        {
            // Just released
            wasGrabbed = false;
            rb.isKinematic = false;
            rb.useGravity = true;

            var grabber = grabbable.grabbedBy;
            if (grabber != null)
            {
                Rigidbody grabberRB = grabber.GetComponent<Rigidbody>();
                if (grabberRB != null)
                {
                    // Apply dampened velocity
                    Vector3 throwVelocity = grabberRB.velocity * velocityMultiplier;
                    Vector3 throwAngularVelocity = grabberRB.angularVelocity * velocityMultiplier;
                    
                    // Cap the velocity
                    if (throwVelocity.magnitude > maxVelocity)
                    {
                        throwVelocity = throwVelocity.normalized * maxVelocity;
                    }
                    
                    rb.velocity = throwVelocity;
                    rb.angularVelocity = throwAngularVelocity;
                }
                else
                {
                    Debug.LogWarning("⚠ Grabber Rigidbody not found — can't apply velocity.");
                }
            }

            Invoke(nameof(ResetBall), resetDelay);
        }
    }
 
    void FixedUpdate()
    {
        // Check if ball is about to fall off the alley
        if (!rb.isKinematic && rb.useGravity)
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, Vector3.down, out hit, boundaryCheckDistance, boundaryLayer))
            {
                // Ball is about to fall off - stop it or reset it
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                ResetBall();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Directly notify pin objects when struck so they can update the scoreboard
        Pin pin = collision.gameObject.GetComponent<Pin>();
        if (pin != null)
        {
            pin.Knock();
        }
    }
 
    void ResetBall()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
 
        if (resetPoint != null)
        {
            transform.position = resetPoint.position;
            transform.rotation = resetPoint.rotation;
        }
    }
}