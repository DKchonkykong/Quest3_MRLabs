using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;
using Meta.XR.MRUtilityKit; // Add this line

public class BowlingAlleyLoader : MonoBehaviour
{
    public GameObject Bowling_Alley;
    public string anchorID = "BowlingAlleyAnchor";
    public float distanceFromUser = 3.0f;
    public LayerMask placementLayer;
    public float maxDistance = 10.0f; // Define maxDistance
    public Quaternion rotation = Quaternion.identity; // Default rotation

    public bool placed = false;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find(anchorID))
        {
            placed = true;
        }
    }

    void Update() // Corrected method name
    {
        if (!placed && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            PlaceLookableAlley();
        }
    }

    void PlaceLookableAlley()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, placementLayer))
        {
            GameObject alley = Instantiate(Bowling_Alley, hit.point, rotation);
            alley.name = anchorID;

            // Add the MRUKAnchor component without setting non-existent properties
            var anchor = alley.AddComponent<MRUKAnchor>();

            // If additional functionality is needed, implement it here
            // Example: Log or handle the anchor in some way
            Debug.Log("MRUKAnchor added to the Bowling Alley object.");

            placed = true;
        }
        else
        {
            Debug.Log("No Valid Surface found.");
        }
    }
}
