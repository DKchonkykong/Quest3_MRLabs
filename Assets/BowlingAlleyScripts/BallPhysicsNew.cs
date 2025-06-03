using UnityEngine;
 
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OVRGrabbable))]
public class OVRBallThrow : MonoBehaviour
{
    private Rigidbody rb;
    private OVRGrabbable grabbable;
 
    public Transform resetPoint;
    public float resetDelay = 5f;
 
    private bool wasGrabbed = false;
 
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabbable = GetComponent<OVRGrabbable>();
    }
 
    void Update()
    {
        else if (wasGrabbed)
{
    wasGrabbed = false;
    rb.isKinematic = false;
    rb.useGravity = true;
 
    var grabber = grabbable.grabbedBy;
    if (grabber != null)
    {
        Rigidbody grabberRB = grabber.GetComponent<Rigidbody>();
        if (grabberRB != null)
        {
            rb.velocity = grabberRB.velocity;
            rb.angularVelocity = grabberRB.angularVelocity;
        }
        else
        {
            Debug.LogWarning("⚠ Grabber Rigidbody not found — can't apply velocity.");
        }
    }
 
    Invoke(nameof(ResetBall), resetDelay);
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