using UnityEngine;
 
[RequireComponent(typeof(OVRGrabbable))]
public class MRBowlingBall : MonoBehaviour
{
    public Transform resetPoint;
    public float resetDelay = 4f;
 
    private OVRGrabbable grabbable;
    private bool wasGrabbed = false;
 
    private bool isRolling = false;
    private Vector3 throwVelocity;
    private Vector3 angularVelocity;
 
    private float deceleration = 0.98f;
 
    void Start()
    {
        grabbable = GetComponent<OVRGrabbable>();
        transform.position = resetPoint.position;
    }
 
    void Update()
    {
        if (grabbable.isGrabbed)
        {
            wasGrabbed = true;
        }
        else if (wasGrabbed)
        {
            // Just released
            wasGrabbed = false;
            isRolling = true;
 
            // Estimate velocity based on controller
            OVRGrabber grabber = grabbable.grabbedBy;
            if (grabber != null)
            {
                throwVelocity = grabber.transform.forward * 6f; // Manual forward force
                angularVelocity = grabber.transform.right * 60f; // Simulate spin
            }
 
            Invoke(nameof(Reset), resetDelay);
        }
 
        if (isRolling)
        {
            // Simulate rolling with simple damping
            transform.position += throwVelocity * Time.deltaTime;
            transform.Rotate(angularVelocity * Time.deltaTime);
 
            // Slow down
            throwVelocity *= deceleration;
            angularVelocity *= deceleration;
        }
    }
 
    void Reset()
    {
        isRolling = false;
        transform.position = resetPoint.position;
        transform.rotation = resetPoint.rotation;
throwVelocity = Vector3.zero;
angularVelocity = Vector3.zero;
    }
}