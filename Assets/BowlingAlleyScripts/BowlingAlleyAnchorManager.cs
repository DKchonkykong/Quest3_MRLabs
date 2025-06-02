using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public enum BowlingMode
{
    Tutorial,
    AnchorPlacement
}
 
public class BowlingAlleyAnchorManager : MonoBehaviour
{
    [Header("References")]
    public GameObject bowlingAlleyPrefab;
    public Transform controllerTransform;
    public Material previewMaterial;
    public LayerMask placementLayer;
 
    [Header("UI")]
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI instructionText;
    public Button modeSwitchButton;
    public GameObject instructionPanel;
    public GameObject tutorialPanel;
    public float tutorialDisplayTime = 6f;
 
    [Header("Placement Settings")]
    public float rotationSpeed = 60f;
 
    private GameObject currentAlley;
    private GameObject previewAlley;
    private BowlingMode currentMode = BowlingMode.AnchorPlacement;
    private bool isPreviewMode = false;
    private float previewRotationY = 0f;
 
    void Start()
    {
        UpdateInstructions();
 
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            Invoke(nameof(HideTutorial), tutorialDisplayTime);
        }
 
        if (modeSwitchButton != null)
        {
            modeSwitchButton.onClick.AddListener(SwitchMode);
        }
    }
 
    void Update()
    {
        if (currentMode != BowlingMode.AnchorPlacement) return;
 
        if (!isPreviewMode && OVRInput.GetDown(OVRInput.Button.One)) // A
        {
            EnterPreviewMode();
        }
 
        if (isPreviewMode)
        {
            UpdatePreviewPosition();
 
            float rotationInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
            previewRotationY += rotationInput * rotationSpeed * Time.deltaTime;
            previewAlley.transform.rotation = Quaternion.Euler(0, previewRotationY, 0);
 
            if (OVRInput.GetDown(OVRInput.Button.Two)) // B
            {
                CancelPreview();
            }
 
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) // Trigger
            {
                ConfirmPlacement();
            }
        }
    }
 
    void SwitchMode()
    {
        if (currentMode == BowlingMode.AnchorPlacement)
            SetToTutorialMode();
        else
            SetToAnchorMode();
    }
 
    void UpdateInstructions()
    {
        if (instructionText == null || instructionPanel == null) return;
 
        instructionPanel.SetActive(true);
 
        if (currentMode == BowlingMode.AnchorPlacement)
        {
            instructionText.text = "🎯 Placement Mode:\nPress A to preview\nRight stick to rotate\nTrigger to place\nB to cancel.";
        }
        else
        {
            instructionText.text = "ℹ️ Tutorial Mode:\nFollow the on-screen instructions.";
        }
    }
 
    void EnterPreviewMode()
    {
        if (previewAlley != null)
            Destroy(previewAlley);
 
        previewAlley = Instantiate(bowlingAlleyPrefab);
        ApplyPreviewMaterial(previewAlley);
        isPreviewMode = true;
        previewRotationY = 0f;
        ShowFeedback("👀 Preview mode started");
    }
 
    void UpdatePreviewPosition()
    {
        Ray ray = new Ray(controllerTransform.position, controllerTransform.forward);
 
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, placementLayer))
        {
            previewAlley.transform.position = hit.point;
        }
    }
 
    void ConfirmPlacement()
    {
        if (currentAlley != null)
            Destroy(currentAlley);
 
        currentAlley = Instantiate(bowlingAlleyPrefab, previewAlley.transform.position, previewAlley.transform.rotation);
        isPreviewMode = false;
 
        if (previewAlley != null)
            Destroy(previewAlley);
 
        Pose pose = new Pose(currentAlley.transform.position, currentAlley.transform.rotation);
 
        OVRAnchor.CreateSpatialAnchorAsync(pose).OnCompleted(anchor =>
        {
            if (anchor == null || anchor == OVRAnchor.Null)
            {
                ShowFeedback("❌ Failed to create anchor.");
                return;
            }
 
            anchor.SaveAsync().OnCompleted(saveResult =>
            {
                ShowFeedback("✅ Anchor saved."); // Assume success — no way to verify without Status
            });
        });
    }
 
    void CancelPreview()
    {
        isPreviewMode = false;
        if (previewAlley != null)
            Destroy(previewAlley);
        ShowFeedback("❌ Placement canceled.");
    }
 
    void ApplyPreviewMaterial(GameObject obj)
    {
        foreach (Renderer r in obj.GetComponentsInChildren<Renderer>())
        {
            r.material = previewMaterial;
        }
    }
 
    void ShowFeedback(string msg)
    {
        if (feedbackText != null)
        {
            feedbackText.text = msg;
            CancelInvoke(nameof(ClearFeedback));
            Invoke(nameof(ClearFeedback), 2.5f);
        }
        Debug.Log("[BowlingAlleyAnchorManager] " + msg);
    }
 
    void ClearFeedback()
    {
        if (feedbackText != null)
            feedbackText.text = "";
    }
 
    void HideTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }
 
    public void SetToTutorialMode()
    {
        currentMode = BowlingMode.Tutorial;
        tutorialPanel.SetActive(true);
        instructionPanel.SetActive(false);
        ShowFeedback("ℹ️ Tutorial mode enabled");
        UpdateInstructions();
    }
 
    public void SetToAnchorMode()
    {
        currentMode = BowlingMode.AnchorPlacement;
        tutorialPanel.SetActive(false);
        instructionPanel.SetActive(true);
        ShowFeedback("📌 Placement mode enabled");
        UpdateInstructions();
    }
}