using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GutterDeadzone : MonoBehaviour
{
    public Material targetMaterial; // Assign the specific material in the Inspector
    public Transform respawnAnchor; // Assign the anchor point in the Inspector

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object has a Renderer and its material matches the target material
        Renderer renderer = collision.gameObject.GetComponent<Renderer>();
        if (renderer != null && renderer.material == targetMaterial)
        {
            // Reset the position of the bowling ball to the anchor point
            collision.gameObject.transform.position = respawnAnchor.position;
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero; // Reset velocity
        }
    }
}
