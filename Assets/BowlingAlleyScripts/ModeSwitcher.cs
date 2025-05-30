using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSwitcher : MonoBehaviour
{
    public GameObject AnchorUI;
    public GameObject TutorialUI;

    public void SwitchToTutorialMode()
    {
        TutorialUI.SetActive(true);
        AnchorUI.SetActive(false);
    }

    public void SwitchToAnchorMode()
    {
        TutorialUI.SetActive(false);
        AnchorUI.SetActive(true);
    }
}
