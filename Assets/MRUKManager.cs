using System.Collections.Generic;
using UnityEngine;

public class MRUKManager : MonoBehaviour
{
    private Dictionary<string, Vector3> anchorWorldPoses = new Dictionary<string, Vector3>();

    void Start()
    {
        Debug.Log("MRUKManager initialized.");
    }

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
