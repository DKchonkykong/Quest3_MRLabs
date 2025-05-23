using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;

public class BowlingAlleyLoader : MonoBehaviour
{
    public GameObject Bowling_Alley;
    public float distanceFromUser = 3.0f;
    public float heightOffset = -0.5f;

    // Start is called before the first frame update
    void Start()
    {
        LoadAlleyRelativeToUser();
    }

    void LoadAlleyRelativeToUser()
    {
        Transform cameraTransform = Camera.main.transform;

        // Corrected 'vector3' to 'Vector3'
        Vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * distanceFromUser;
        spawnPosition.y += heightOffset;

        // Corrected 'Quarternion' to 'Quaternion'
        Quaternion spawnRotation = Quaternion.LookRotation(-cameraTransform.forward);

        // Instantiate the bowling alley
        GameObject alley = Instantiate(Bowling_Alley, spawnPosition, spawnRotation);

        // Set the name of the instantiated object
        alley.name = "DynamicBowlingAlley";
    }
}
