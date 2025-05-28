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
            if (MRUK.Instance != null && MRUK.Instance.SceneAnchors.Count > 0)
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
        MRUKAnchor bestAnchor = null;
        float bestArea = 0f;

        foreach (var anchor in MRUK.Instance.SceneAnchors)
        {
            if (!anchor.HasAnyLabel(MRUKAnchor.SceneLabels.FLOOR) || !anchor.VolumeBounds.HasValue)
                continue;

            Bounds bounds = anchor.VolumeBounds.Value;
            Vector3 size = bounds.size;

            if (size.x < alleyWidth + padding || size.z < alleyLength + padding)
                continue;

            // Optional physical collision check
            if (Physics.CheckBox(bounds.center, new Vector3(alleyWidth / 2, 1, alleyLength / 2), Quaternion.identity, obstacleLayer))
                continue;

            float area = size.x * size.z;
            if (area > bestArea)
            {
                bestAnchor = anchor;
                bestArea = area;
            }
        }

        if (bestAnchor != null)
        {
            Bounds bounds = bestAnchor.VolumeBounds.Value;
            Vector3 position = bestAnchor.GetAnchorCenter();
            Quaternion rotation = bounds.size.z >= bounds.size.x ? Quaternion.identity : Quaternion.Euler(0, 90, 0);

            GameObject alley = Instantiate(bowlingAlleyPrefab, position, rotation);
            alley.transform.localScale = Vector3.one * Mathf.Clamp(bounds.size.z / alleyLength, 0.5f, 2f);

            hasPlaced = true;
            Debug.Log("Bowling alley placed.");
        }
        else
        {
            Debug.LogError("No suitable floor found. Alley not placed.");
        }
    }
}
