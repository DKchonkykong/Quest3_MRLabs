using UnityEngine;

public class Pin : MonoBehaviour
{
    private bool isKnockedDown = false;
    private UpdateScoreBoard scoreBoardManager;

    public void ResetPosition()
    {
        // Corrected reset position with float values
        transform.position = new Vector3(0.307f, 0.007f, 0.799f); // Example reset position
        transform.rotation = Quaternion.identity; // Reset rotation
    }

    public void Initialize(UpdateScoreBoard manager)
    {
        // Initialization logic
    }

    public void SetKnockedDown(bool knockedDown)
    {
        if (isKnockedDown != knockedDown)
        {
            isKnockedDown = knockedDown;
            scoreBoardManager.UpdatePinState(this, isKnockedDown);
        }
    }

    public bool IsKnockedDown()
    {
        return isKnockedDown;
    }
}