using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMesh Pro namespace

public class UpdateScoreBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // Reference to the TextMesh Pro component
    [SerializeField] private List<Pin> pins = new List<Pin>(); // List of Pin objects

    // Start is called before the first frame update
    void Start()
    {
        // Initialize each pin and assign this manager
        foreach (Pin pin in pins)
        {
            pin.Initialize(this);
        }

        UpdateScoreText(); // Initialize the scoreboard
    }

    // Method to update the state of a specific pin
    public void UpdatePinState(Pin pin, bool isKnockedDown)
    {
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        int pinsHit = 0;

        // Count the number of pins that have fallen (based on their Y-axis position)
        foreach (Pin pin in pins)
        {
            if (pin.transform.position.y < 0.70) // Replace 'someThreshold' with the Y-axis value that indicates a fallen pin
            {
                pinsHit++;
            }
        }

        // Update the scoreboard text
        if (pinsHit == 10)
        {
            scoreText.text = "Strike!";
        }
        else if (pinsHit == 0)
        {
            scoreText.text = "No pins hit.";
        }
        else
        {
            scoreText.text = $"{pinsHit} pin(s) fell.";
        }
    }
}
