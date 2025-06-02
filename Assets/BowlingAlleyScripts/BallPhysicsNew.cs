using UnityEngine;
using Oculus.Interaction;

[RequireComponent(typeof(Rigidbody), typeof(Grabbable))]
public class MetaBallThrow : MonoBehaviour
{
    private Rigidbody rb;
    private Grabbable grabbable;
    private Vector3 startPos;
    private Quaternion startRot;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabbable = GetComponent<Grabbable>();

        startPos = transform.position;
        startRot = transform.rotation;

        rb.isKinematic = true;
        rb.useGravity = false;

        grabbable.WhenPointerEventRaised += OnPointerEvent;
    }

    private void OnPointerEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Select)
        {
            OnGrab();
        }
        else if (evt.Type == PointerEventType.Unselect)
        {
            OnRelease();
        }
    }

    private void OnGrab()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void OnRelease()
    {
        rb.isKinematic = false;
        rb.useGravity = true;

        // Let the SDK apply throw velocity (via grab pose)
        Invoke(nameof(ResetBall), 5f);
    }

    private void ResetBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;

        transform.position = startPos;
        transform.rotation = startRot;
    }
}
