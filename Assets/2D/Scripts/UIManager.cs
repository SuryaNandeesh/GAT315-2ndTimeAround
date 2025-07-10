using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider healthSlider;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI debugText;

    private void Start()
    {
        // Find UI elements if not assigned
        if (healthSlider == null)
            healthSlider = GameObject.Find("HealthSlider")?.GetComponent<Slider>();
        
        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        
        if (debugText == null)
            debugText = GameObject.Find("DebugText")?.GetComponent<TextMeshProUGUI>();

        // Find player and assign UI references
        AssignUIToPlayer();
    }

    private void AssignUIToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.healthBar = healthSlider;
                playerController.scoreText = scoreText;
                playerController.debugText = debugText;
                
                Debug.Log("UI elements successfully assigned to PlayerController!");
            }
            else
            {
                Debug.LogError("PlayerController not found on player object!");
            }
        }
        else
        {
            Debug.LogError("Player object not found! Make sure it has the 'Player' tag.");
        }
    }

    // Method to manually reassign UI (can be called from inspector)
    [ContextMenu("Reassign UI")]
    public void ReassignUI()
    {
        AssignUIToPlayer();
    }
} 