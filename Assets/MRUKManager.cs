using System.Collections.Generic;
using UnityEngine;

public class MRUKManager: MonoBehaviour
{
    // Dictionary to store AnchorID and their corresponding world poses
    private Dictionary<string, Vector3> anchorWorldPoses = new Dictionary<string, Vector3>();

    // Reference to the AnchorTutorialUIManager
    [SerializeField]
    private NewBehaviourScript anchorTutorialUIManager;

    private void Awake()
    {
        if (anchorTutorialUIManager == null)
        {
            Debug.LogError("AnchorTutorialUIManager reference is missing. Please assign it in the inspector.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("MRUKManager initialized.");
    }

    // Update is called once per frame
    void Update()
    {
        // Automatically save anchors created by AnchorTutorialUIManager
        SaveAnchorsFromTutorialManager();
    }

    // Save anchors created by AnchorTutorialUIManager
    private void SaveAnchorsFromTutorialManager()
    {
        foreach (var anchor in anchorTutorialUIManager.GetActiveAnchors())
        {
            string anchorID = anchor.Uuid.ToString();
            Vector3 worldPose = anchor.transform.position;

            if (!anchorWorldPoses.ContainsKey(anchorID))
            {
                anchorWorldPoses.Add(anchorID, worldPose);
                Debug.Log($"Anchor saved: ID = {anchorID}, Position = {worldPose}");
            }
        }
    }

    // Method to get the saved world pose by AnchorID
    public Vector3? GetWorldPose(string anchorID)
    {
        if (anchorWorldPoses.TryGetValue(anchorID, out Vector3 worldPose))
        {
            return worldPose;
        }
        else
        {
            Debug.LogWarning($"No world pose found for AnchorID: {anchorID}");
            return null;
        }
    }

    // Method to save a specific anchor pose
    public void SaveAnchor(string anchorID, Vector3 worldPose)
    {
        if (!anchorWorldPoses.ContainsKey(anchorID))
        {
            anchorWorldPoses.Add(anchorID, worldPose);
            Debug.Log($"Anchor saved: ID = {anchorID}, Position = {worldPose}");
        }
        else
        {
            Debug.LogWarning($"Anchor with ID {anchorID} already exists.");
        }
    }
}
