using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPlacement : MonoBehaviour
{
    public GameObject acnhorPrefab;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            CreateSpatialAnchor();
        }        
    }

    public void CreateSpatialAnchor()
    {
        GameObject prefab = Instantiate(
            acnhorPrefab, 
            OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), 
            Quaternion.identity // Use Quaternion.identity for no rotation
        );
        prefab.AddComponent<OVRSpatialAnchor>();
    }
}