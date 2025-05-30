using UnityEngine;
 
public class PinController : MonoBehaviour
{
    private Quaternion initialRotation;
    private Vector3 initialPosition;
    private bool isKnocked = false;
 
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
 
    public void Knock()
    {
        if (isKnocked) return;
        isKnocked = true;
 
        // Simulate falling by tilting the pin forward
        transform.rotation = Quaternion.Euler(60f, transform.rotation.eulerAngles.y, 0f);
        // Optionally move the pin slightly backward
        transform.position += transform.forward * 0.1f;
    }
 
    public void ResetPin()
    {
        isKnocked = false;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
 
    public bool IsKnocked()
    {
        return isKnocked;
    }
}