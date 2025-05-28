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
        if (!sceneReady)
        {
            if (MRUK.Instance != null && MRUK.Instance.Rooms.Count > 0) // Fixed the Rooms > 0 error
            {
                sceneReady = true;
                Debug.Log("MRUK ready.");
            }
            return;
        }

        if (!hasPlaced && OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("[A] pressed. Attempting alley placement.");
            TryPlaceAlley();
        }
    }

    void TryPlaceAlley()
    {
        MRUKRoom bestRoom = null;
        float bestArea = 0f;

        // Iterate through rooms
        foreach (var room in MRUK.Instance.Rooms)
        {
            Bounds? bounds = CalculateRoomBounds(room);
            if (!bounds.HasValue)
                continue;

            Vector3 size = bounds.Value.size;

            if (size.x < alleyWidth + padding || size.z < alleyLength + padding)
                continue;

            // Optional physical collision check
            if (Physics.CheckBox(bounds.Value.center, new Vector3(alleyWidth / 2, 1, alleyLength / 2), Quaternion.identity, obstacleLayer))
                continue;

            float area = size.x * size.z;
            if (area > bestArea)
            {
                bestRoom = room;
                bestArea = area;
            }
        }

        if (bestRoom != null)
        {
            Bounds bounds = CalculateRoomBounds(bestRoom).Value;
            Vector3 position = bounds.center;
            Quaternion rotation = bounds.size.z >= bounds.size.x ? Quaternion.identity : Quaternion.Euler(0, 90, 0);

            GameObject alley = Instantiate(bowlingAlleyPrefab, position, rotation);
            alley.transform.localScale = Vector3.one * Mathf.Clamp(bounds.size.z / alleyLength, 0.5f, 2f);

            hasPlaced = true;
            Debug.Log("Bowling alley placed.");
        }
        else
        {
            Debug.LogError("No suitable room found. Alley not placed.");
        }
    }

    Bounds? CalculateRoomBounds(MRUKRoom room)
    {
        if (room.Anchors == null || room.Anchors.Count == 0)
            return null;

        Vector3 min = Vector3.positiveInfinity;
        Vector3 max = Vector3.negativeInfinity;

        foreach (var anchor in room.Anchors)
        {
            if (anchor.TryGetComponent(out Renderer renderer))
            {
                min = Vector3.Min(min, renderer.bounds.min);
                max = Vector3.Max(max, renderer.bounds.max);
            }
        }

        if (min == Vector3.positiveInfinity || max == Vector3.negativeInfinity)
            return null;

        return new Bounds((min + max) / 2, max - min);
    }
}
