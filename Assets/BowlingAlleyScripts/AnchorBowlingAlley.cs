using UnityEngine;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;

public class AnchorBowlingAlley : MonoBehaviour
{
    public GameObject Bowling_Alley;
    public float spawnDistance = 3f;
    public float heightOffset = -0.5f;

    private IMixedRealitySpatialAwarenessMeshObserver meshObserver;

    // Start is called before the first frame update
    void Start()
    {
        TryLoadAnchorOrSpawnNew();
    }

    void TryLoadAnchorOrSpawnNew()
    {
        var spatialAnchors = FindObjectsOfType<GameObject>();
        foreach (var anchor in spatialAnchors)
        {
            if (anchor.name == "BowlingAlleyAnchor")
            {
                Instantiate(Bowling_Alley, anchor.transform.position, anchor.transform.rotation, anchor.transform);
                Debug.Log("Loaded saved anchor.");
                return;
            }
        }
        SpawnAndAnchorNewAlley();
    }

    void SpawnAndAnchorNewAlley()
    {
        Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * spawnDistance;
        spawnPosition.y += heightOffset;

        Quaternion spawnRotation = Quaternion.LookRotation(-Camera.main.transform.forward);

        GameObject alley = Instantiate(Bowling_Alley, spawnPosition, spawnRotation);
        alley.name = "DynamicAlley";

        // Add MRUK spatial anchor component
        var spatialAnchor = alley.AddComponent<SpatialAnchor>();
        spatialAnchor.name = "BowlingAlleyAnchor";

        // Save the anchor
        spatialAnchor.SaveAnchor(success =>
        {
            if (success)
                Debug.Log("Anchor Saved.");
            else
                Debug.LogError("Failed to save anchor.");
        });
    }
}
