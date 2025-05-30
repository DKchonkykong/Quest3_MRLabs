using UnityEngine;
using Meta.XR.MRUtilityKit;
 
public class SmartAlleyPlacer : MonoBehaviour
{
    public GameObject bowlingAlleyPrefab;
    public float alleyLength = 10f;
    public float alleyWidth = 1.5f;
    public float padding = 0.5f;
    public LayerMask obstacleLayer;
 
    private bool sceneReady = false;
    private bool hasPlaced = false;
 
    void Update()
    {
        if (!sceneReady && MRUK.Instance != null && MRUK.Instance.Rooms.Count > 0)
        {
            sceneReady = true;
            Debug.Log("MRUK rooms loaded.");
        }
 
        if (!hasPlaced && sceneReady && OVRInput.GetDown(OVRInput.Button.One))
        {
            TryPlaceAlley();
        }
    }
 
    void TryPlaceAlley()
    {
        MRUKRoom bestRoom = null;
        float bestArea = 0f;

        foreach (var room in MRUK.Instance.Rooms)
        {
            if (room.FloorAnchor == null || !room.FloorAnchor.VolumeBounds.HasValue) continue;

            Bounds bounds = room.FloorAnchor.VolumeBounds.Value;
            if (bounds.size.x < alleyWidth + padding || bounds.size.z < alleyLength + padding) continue;

            if (Physics.CheckBox(bounds.center, new Vector3(alleyWidth / 2, 1, alleyLength / 2), Quaternion.identity, obstacleLayer))
                continue;

            float area = bounds.size.x * bounds.size.z;
            if (area > bestArea)
            {
                bestArea = area;
                bestRoom = room;
            }
        }

        if (bestRoom != null)
        {
            Bounds roomBounds = bestRoom.FloorAnchor.VolumeBounds.Value;
            Vector3 centerPos = roomBounds.center;

            Quaternion rot = roomBounds.size.z >= roomBounds.size.x ? Quaternion.identity : Quaternion.Euler(0, 90, 0);
            float scaleFactor = Mathf.Clamp(roomBounds.size.z / alleyLength, 0.25f, 0.8f);

            Vector3 alleyOffset = rot * new Vector3(0, 0, -alleyLength / 2f);
            Vector3 adjustedPos = centerPos + alleyOffset;

            GameObject alley = Instantiate(bowlingAlleyPrefab, adjustedPos, rot);
            alley.transform.localScale = Vector3.one * scaleFactor;

            hasPlaced = true;
            Debug.Log("Bowling alley placed at center of room.");
        }
        else
        {
            Debug.LogWarning("No valid room or space found.");
        }
    }
}