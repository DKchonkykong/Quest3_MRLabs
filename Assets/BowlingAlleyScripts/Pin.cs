using UnityEngine;

public class Pin : MonoBehaviour
{
    private bool isKnockedDown = false;
    private UpdateScoreBoard scoreBoardManager;

    public void Initialize(UpdateScoreBoard manager)
    {
        scoreBoardManager = manager;
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