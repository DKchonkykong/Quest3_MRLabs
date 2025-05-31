using UnityEngine;

public class Pin : MonoBehaviour
{
    private Quaternion initialRotation;
    private Vector3 initialPosition;
    private bool isKnocked = false;

    private UpdateScoreBoard scoreBoardManager;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        float tilt = Quaternion.Angle(transform.rotation, Quaternion.identity);
        if (tilt > 30f && !isKnocked)
        {
            Knock();
        }
    }

    public void Knock()
    {
        if (isKnocked) return;
        isKnocked = true;

        transform.rotation = Quaternion.Euler(60f, transform.rotation.eulerAngles.y, 0f);
        transform.position += transform.forward * 0.1f;

        if (scoreBoardManager != null)
            scoreBoardManager.UpdatePinState(this, true);
    }

    public void ResetPin()
    {
        isKnocked = false;
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        if (scoreBoardManager != null)
            scoreBoardManager.UpdatePinState(this, false);
    }

    public bool IsKnocked()
    {
        return isKnocked;
    }

    public void Initialize(UpdateScoreBoard manager)
    {
        scoreBoardManager = manager;
    }
}
