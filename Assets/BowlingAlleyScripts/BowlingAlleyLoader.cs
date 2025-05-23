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
    vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * distanceFromUser;

    Quarternion spawnRotation = Quarternion.LookRotation(-cameraTransform.forward); 

    GameObject alley = Instantiate(Bowling_Alley, spawnPosition, spawnRotation);

alley.name = "DynamicBowlingAlley";

}
}
