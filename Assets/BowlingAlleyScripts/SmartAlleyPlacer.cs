using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class SmartAlleyPlacer : MonoBehaviour
{

    public GameObject Bowling_Alley;
    public float alleyLength = 10f;
    public float alleyWidth = 1.5f;
    public float padding = 0.5f;

    private bool sceneReady = false;
    private bool hasPlaced = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Waiting for anchors to load...");
    }

    // Update is called once per frame
    void Update()
    {
        // Check if anchors are loaded
        if (!sceneReady && MRUK.Instance.Rooms.Count > 0 && MRUK.Instance.Rooms[0].Anchors.Count > 0)
        {
            sceneReady = true;
            Debug.Log("Anchors loaded. Ready to place the bowling alley.");
        }

        // If scene is not ready or alley is already placed, return early
        if (!sceneReady || hasPlaced)
            return;

        // Check for user input to place the alley
        if (OVRInput.GetDown(OVRInput.Button.One)) // [A] button on controller
        {
            TryPlaceAlley();
        }
    }

    void TryPlaceAlley()
    {
        MRUKAnchor bestAnchor = null;
        float bestLength = 0f;

        foreach (var room in MRUK.Instance.Rooms)
        {
            foreach (var anchor in room.Anchors)
            {
                if (!anchor.HasAnyLabel(MRUKAnchor.SceneLabels.FLOOR) || !anchor.VolumeBounds.HasValue)
                    continue;

                Vector3 size = anchor.VolumeBounds.Value.size;
                float usableLength = Mathf.Max(size.x, size.z);

                if (usableLength > bestLength && size.x >= alleyWidth + padding && size.z >= alleyLength + padding)
                {
                    bestLength = usableLength;
                    bestAnchor = anchor;
                }
            }
        }

        if (bestAnchor != null)
        {
            Vector3 center = bestAnchor.GetAnchorCenter();
            Vector3 size = bestAnchor.VolumeBounds.Value.size;
            float scaleFactor = Mathf.Clamp(size.z / alleyLength, 0.5f, 2f);

            bool longIsZ = size.z >= size.x;
            Quaternion rotation = longIsZ ? Quaternion.identity : Quaternion.Euler(0, 90, 0);

            GameObject alley = Instantiate(Bowling_Alley, center, rotation);
            alley.transform.localScale = new Vector3(scaleFactor, 1f, scaleFactor);
            hasPlaced = true;
            Debug.Log("Bowling Alley Placed and Scaled on floor.");
        }
        else
        {
            Debug.LogError("Failed to place bowling alley: no suitable place anchor found.");
        }
    }
}
