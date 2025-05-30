using UnityEngine;
 
public class BowlingBallController : MonoBehaviour
{
    public float speed = 5f;
    public float resetZ = -5f;
    public Transform startPoint;
 
    private bool isRolling = false;
 
    void Update()
    {
        if (isRolling)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
 
        if (Input.GetKeyDown(KeyCode.A) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) // Replace with VR input
        {
            ResetBall();
            isRolling = true;
        }
    }
 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pin"))
        {
            var pin = other.GetComponent<PinController>();
            if (pin != null)
            {
                pin.Knock();
            }
        }
    }
 
    void ResetBall()
    {
        isRolling = false;
        transform.position = startPoint.position;
        transform.rotation = Quaternion.identity;
    }
}