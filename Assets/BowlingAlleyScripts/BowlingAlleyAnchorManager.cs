using UnityEngine;
using Meta.XR.MRUtilityKit;
using UnityEngine.UI;
 
public enum BowlingMode
{
    Tutorial,
    AnchorPlacement
}
 
public class BowlingAlleyAnchorManager : MonoBehaviour
{
    [Header("Assign your alley prefab here")]
    public GameObject bowlingAlleyPrefab;
 
    [Header("UI Elements")]
    public Text feedbackText;
    public Text instructionText;
    public Toggle anchorModeToggle;
    public GameObject instructionPanel;
    public GameObject tutorialPanel;
    public float tutorialDisplayTime = 6f;
 
    [Header("Mode Control")]
    public BowlingMode currentMode = BowlingMode.AnchorPlacement;
    public GameObject previewAnchorVisual;
 
    private GameObject currentAlley;
    private MRUKAnchor currentAnchor;
    private bool useSavedAnchorMode = false;
 
    void Start()
    {
        useSavedAnchorMode = anchorModeToggle != null && anchorModeToggle.isOn;
        UpdateInstructions();
        PlaceAlley();
 
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            Invoke(nameof(HideTutorial), tutorialDisplayTime);
        }
    }
 
    void Update()
    {
        if (currentMode != BowlingMode.AnchorPlacement) return;
 
        if (OVRInput.GetDown(OVRInput.Button.One)) // A button
        {
            if (!useSavedAnchorMode)
                PlaceAlley();
        }
 
        if (OVRInput.GetDown(OVRInput.Button.Two)) // B button
        {
            DeleteAlleyAndAnchor();
        }
    }
 
    public void OnToggleAnchorModeChanged()
    {
        useSavedAnchorMode = anchorModeToggle.isOn;
        UpdateInstructions();
        DeleteAlleyAndAnchor();
        PlaceAlley();
    }
 
    private void UpdateInstructions()
    {
        if (instructionText == null || instructionPanel == null) return;
 
        instructionPanel.SetActive(true);
 
        if (useSavedAnchorMode)
            instructionText.text = "\ud83d\udccc Saved Anchor Mode:\nPress B to clear.\nToggle to switch to manual placement.";
        else
            instructionText.text = "\ud83c\udfaf Manual Placement Mode:\nPress A to place at room center.\nPress B to remove.";
    }
 
    private void PlaceAlley()
    {
        DeleteAlleyAndAnchor();

        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        if (room == null)
        {
            ShowFeedback("\u274c No valid MRUK room found");
            return;
        }

        Bounds bounds = room.GetRoomBounds();
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
        Vector3 center = (min + max) / 2f;
        Vector3 size = max - min;

        bool isLongAlongZ = size.z > size.x;
        Quaternion rotation = isLongAlongZ ? Quaternion.identity : Quaternion.Euler(0, 90, 0);

        float forwardOffset = 0.5f;
        Vector3 forwardDir = rotation * Vector3.forward;
        center += forwardDir * forwardOffset;

        currentAlley = Instantiate(bowlingAlleyPrefab, center, rotation);
        currentAnchor = room.FloorAnchor != null ? room.FloorAnchor.GetComponent<MRUKAnchor>() : null;

        if (currentAnchor != null)
        {
            ShowFeedback("\ud83d\udd39 Alley anchored to floor");
        }
        else
        {
            ShowFeedback("\u26a0\ufe0f Anchor component missing");
        }
    }
 
    private void DeleteAlleyAndAnchor()
    {
        if (currentAlley != null)
        {
            Destroy(currentAlley);
            currentAlley = null;
        }

        if (currentAnchor != null)
        {
            MRUKRoom room = MRUK.Instance.GetCurrentRoom();
            if (room != null)
            {
                room.RemoveAndDestroyAnchor(currentAnchor);
            }
            currentAnchor = null;
        }

        ShowFeedback("\ud83d\uddd1\ufe0f Alley and anchor removed");
    }
 
    private void ShowFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            CancelInvoke(nameof(ClearFeedback));
            Invoke(nameof(ClearFeedback), 2.5f);
        }
 
        Debug.Log("[BowlingAlleyAnchorManager] " + message);
    }
 
    private void ClearFeedback()
    {
        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
    }
 
    public void ToggleTutorialVisibility()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(!tutorialPanel.activeSelf);
    }
 
    private void HideTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }
 
    public void SetToTutorialMode()
    {
        currentMode = BowlingMode.Tutorial;
        tutorialPanel.SetActive(true);
        instructionPanel.SetActive(false);
        if (previewAnchorVisual != null) previewAnchorVisual.SetActive(false);
        ShowFeedback("\u2139\ufe0f Tutorial mode enabled");
    }
 
    public void SetToAnchorMode()
    {
        currentMode = BowlingMode.AnchorPlacement;
        tutorialPanel.SetActive(false);
        instructionPanel.SetActive(true);
        if (previewAnchorVisual != null) previewAnchorVisual.SetActive(true);
        ShowFeedback("\ud83d\udccc Anchor placement mode enabled");
    }
}