using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMesh Pro namespace

public class UpdateScoreBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // Reference to the TextMesh Pro component
    [SerializeField] private List<Pin> pins = new List<Pin>(); // List of Pin objects
    private int currentRound = 1;
    private int totalPinsHit = 0;

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

        foreach (Pin pin in pins)
        {
            if (pin.transform.position.y < 0.70f)
            {
                pinsHit++;
            }
        }

        totalPinsHit += pinsHit;

        if (pinsHit == 10)
        {
            scoreText.text = $"Round {currentRound}: Strike!\nTotal Pins: {totalPinsHit}";
        }
        else if (pinsHit == 0)
        {
            scoreText.text = $"Round {currentRound}: No pins hit.\nTotal Pins: {totalPinsHit}";
        }
        else
        {
            scoreText.text = $"Round {currentRound}: {pinsHit} pin(s) fell.\nTotal Pins: {totalPinsHit}";
        }

        currentRound++;
    }
}
