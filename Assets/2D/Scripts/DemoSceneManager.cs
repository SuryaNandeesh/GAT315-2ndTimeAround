using UnityEngine;
using UnityEngine.UI;

public class DemoSceneManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Canvas demoUI;
    [SerializeField] private Text instructionsText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text collectedText;
    
    [Header("Demo Settings")]
    [SerializeField] private bool showInstructions = true;
    [SerializeField] private float instructionDisplayTime = 5f;
    
    private Health playerHealth;
    private int itemsCollected = 0;
    private float instructionTimer;
    
    private void Start()
    {
        SetupUI();
        FindPlayerComponents();
        
        if (showInstructions)
        {
            instructionTimer = instructionDisplayTime;
        }
    }
    
    private void Update()
    {
        UpdateUI();
        HandleInstructionTimer();
    }
    
    private void SetupUI()
    {
        if (demoUI == null)
        {
            CreateSimpleUI();
        }
    }
    
    private void CreateSimpleUI()
    {
        // Create a simple canvas with basic UI elements
        GameObject canvasGO = new GameObject("Demo UI");
        demoUI = canvasGO.AddComponent<Canvas>();
        demoUI.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();
        
        // Create instructions text
        CreateUIText("Instructions", "WASD/Arrow Keys to move, Space to jump\nCollect coins, avoid spikes, find health!", 
                    new Vector2(10, -10), TextAnchor.UpperLeft, 16, ref instructionsText);
        
        // Create health display
        CreateUIText("Health", "Health: 100", new Vector2(-10, -10), TextAnchor.UpperRight, 18, ref healthText);
        
        // Create collected items display
        CreateUIText("Collected", "Collected: 0", new Vector2(-10, -50), TextAnchor.UpperRight, 18, ref collectedText);
    }
    
    private void CreateUIText(string name, string content, Vector2 anchoredPosition, TextAnchor anchor, int fontSize, ref Text textRef)
    {
        GameObject textGO = new GameObject(name);
        textGO.transform.SetParent(demoUI.transform, false);
        
        RectTransform rectTransform = textGO.AddComponent<RectTransform>();
        rectTransform.anchorMin = anchor == TextAnchor.UpperLeft ? Vector2.up : Vector2.one;
        rectTransform.anchorMax = anchor == TextAnchor.UpperLeft ? Vector2.up : Vector2.one;
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(300, 60);
        
        Text text = textGO.AddComponent<Text>();
        text.text = content;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.color = Color.white;
        text.alignment = anchor;
        
        // Add outline for better readability
        Outline outline = textGO.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(1, 1);
        
        textRef = text;
    }
    
    private void FindPlayerComponents()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
        }
    }
    
    private void UpdateUI()
    {
        if (playerHealth != null && healthText != null)
        {
            float currentHealth = playerHealth.CurrentHealth;
            healthText.text = $"Health: {currentHealth:F0}";
            
            // Change color based on health percentage
            float healthPercent = playerHealth.HealthPercentage;
            if (healthPercent > 0.6f)
                healthText.color = Color.green;
            else if (healthPercent > 0.3f)
                healthText.color = Color.yellow;
            else
                healthText.color = Color.red;
        }
        
        if (collectedText != null)
        {
            collectedText.text = $"Collected: {itemsCollected}";
        }
    }
    
    private void HandleInstructionTimer()
    {
        if (instructionTimer > 0)
        {
            instructionTimer -= Time.deltaTime;
            if (instructionTimer <= 0 && instructionsText != null)
            {
                instructionsText.gameObject.SetActive(false);
            }
        }
    }
    
    // Public method to be called when items are collected
    public void OnItemCollected()
    {
        itemsCollected++;
    }
    
    // Public method to show completion message
    public void ShowCompletionMessage()
    {
        if (instructionsText != null)
        {
            instructionsText.text = "Level Complete! Great job exploring the 2D world!";
            instructionsText.color = Color.yellow;
            instructionsText.gameObject.SetActive(true);
        }
    }
    
    // Reset demo statistics
    [ContextMenu("Reset Demo")]
    public void ResetDemo()
    {
        itemsCollected = 0;
        instructionTimer = instructionDisplayTime;
        
        if (instructionsText != null)
        {
            instructionsText.text = "WASD/Arrow Keys to move, Space to jump\nCollect coins, avoid spikes, find health!";
            instructionsText.color = Color.white;
            instructionsText.gameObject.SetActive(showInstructions);
        }
    }
    
    private void OnEnable()
    {
        // Subscribe to collectible events if available
        // This could be expanded to listen for collection events
    }
    
    private void OnDisable()
    {
        // Unsubscribe from events
    }
} 