using UnityEngine;
using Oculus.MRUtilityKit;

public class AnchorPlacement : MonoBehaviour
{
    public GameObject anchorPrefab;
    public string anchorId = "BowlingAlleyAnchor";

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            CreateMRUKAnchor();
        }
    }

    void CreateMRUKAnchor()
    {
        Vector3 spawnPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Quaternion spawnRotation = Quaternion.identity;

        GameObject instance = Instantiate(anchorPrefab, spawnPosition, spawnRotation);
        instance.name = anchorId;

        MRUKAnchorComponent anchor = instance.AddComponent<MRUKAnchorComponent>();
        anchor.anchorId = anchorId;
        anchor.saveAnchor = true;
        anchor.restoreAnchorPose = true;
    }
}
