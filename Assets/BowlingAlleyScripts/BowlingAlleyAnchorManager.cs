using System;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Platform;
using Oculus.Platform.Models;
 
public class BowlingAlleyAnchorSpawner : MonoBehaviour
{
    public GameObject bowlingAlleyPrefab;
    public Transform controllerTransform;
    public LayerMask placementLayer;
    public string anchorSaveKey = "BowlingAlley_UUID";
 
    private OVRSpatialAnchor spawnedAnchor;
    private GameObject spawnedAlley;
 
    private void Start()
    {
        if (PlayerPrefs.HasKey(anchorSaveKey))
        {
            Guid uuid = Guid.Parse(PlayerPrefs.GetString(anchorSaveKey));
            OVRSpatialAnchor.LoadUnboundAnchor(uuid, unbound =>
            {
                if (unbound != null)
                {
                    GameObject alley = Instantiate(bowlingAlleyPrefab);
                    spawnedAlley = alley;
 
                    var anchor = alley.GetComponent<OVRSpatialAnchor>();
                    anchor.UnboundAnchor = unbound;
                    anchor.enabled = true;
 
                    Debug.Log("📍 Loaded alley from saved anchor.");
                }
                else
                {
                    Debug.Log("⚠ No saved anchor found. Ready for placement.");
                }
            });
        }
        else
        {
            Debug.Log("🆕 No saved anchor key found. Wait for user to place.");
        }
    }
 
    private void Update()
    {
        // If not placed yet, allow user to spawn it using A and place
        if (spawnedAlley == null && OVRInput.GetDown(OVRInput.Button.One)) // A button
        {
            TryPlaceAlley();
        }
    }
 
    private void TryPlaceAlley()
    {
        Ray ray = new Ray(controllerTransform.position, controllerTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, placementLayer))
        {
            Vector3 placePos = hit.point;
            Quaternion placeRot = Quaternion.LookRotation(Vector3.forward);
 
            spawnedAlley = Instantiate(bowlingAlleyPrefab, placePos, placeRot);
 
            var anchor = spawnedAlley.GetComponent<OVRSpatialAnchor>();
            anchor.enabled = true;
 
            anchor.Save((savedAnchor, success) =>
            {
                if (success)
                {
                    PlayerPrefs.SetString(anchorSaveKey, savedAnchor.Uuid.ToString());
                    PlayerPrefs.Save();
                    Debug.Log("✅ Bowling alley anchor saved.");
                }
                else
                {
                    Debug.LogError("❌ Failed to save bowling alley anchor.");
                }
            });
        }
    }
}