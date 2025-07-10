using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game State")]
    public bool isGamePaused = false;
    public bool gameWon = false;
    public bool gameOver = false;
    
    [Header("Player Progression")]
    public bool hasDoubleJump = false;
    public bool hasDash = false;
    public bool hasWallJump = false;
    public int energyTanks = 0;
    public int missileCount = 0;
    public int maxMissiles = 0;
    
    [Header("Game Stats")]
    public int score = 0;
    public int itemsCollected = 0;
    public int enemiesDefeated = 0;
    public float gameTime = 0f;
    
    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip backgroundMusic;
    public AudioClip victoryMusic;
    public AudioClip gameOverMusic;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        InitializeGame();
    }
    
    private void Update()
    {
        if (!gameOver && !gameWon && !isGamePaused)
        {
            gameTime += Time.deltaTime;
        }
        
        HandleInput();
    }
    
    void InitializeGame()
    {
        Time.timeScale = 1f;
        if (musicSource && backgroundMusic)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
    
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        
        if (Input.GetKeyDown(KeyCode.R) && (gameOver || gameWon))
        {
            RestartGame();
        }
    }
    
    public void TogglePause()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;
    }
    
    public void AddScore(int points)
    {
        score += points;
    }
    
    public void CollectItem()
    {
        itemsCollected++;
        AddScore(100);
    }
    
    public void EnemyDefeated()
    {
        enemiesDefeated++;
        AddScore(50);
    }
    
    public void CollectEnergyTank()
    {
        energyTanks++;
        PlayerHealth.Instance?.IncreaseMaxHealth(25);
        CollectItem();
    }
    
    public void CollectMissileExpansion()
    {
        maxMissiles += 5;
        missileCount = maxMissiles;
        CollectItem();
    }
    
    public void UnlockAbility(string abilityName)
    {
        switch (abilityName.ToLower())
        {
            case "doublejump":
                hasDoubleJump = true;
                break;
            case "dash":
                hasDash = true;
                break;
            case "walljump":
                hasWallJump = true;
                break;
        }
        
        AddScore(500);
        PlaySFX("AbilityUnlock");
    }
    
    public void GameWin()
    {
        if (gameWon) return;
        
        gameWon = true;
        Time.timeScale = 0f;
        
        if (musicSource && victoryMusic)
        {
            musicSource.clip = victoryMusic;
            musicSource.Play();
        }
    }
    
    public void GameOver()
    {
        if (gameOver) return;
        
        gameOver = true;
        Time.timeScale = 0f;
        
        if (musicSource && gameOverMusic)
        {
            musicSource.clip = gameOverMusic;
            musicSource.Play();
        }
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void PlaySFX(string clipName)
    {
        // This would normally load from Resources or be assigned
        if (sfxSource)
        {
            // sfxSource.PlayOneShot(GetSFXClip(clipName));
        }
    }
    
    public bool HasAbility(string abilityName)
    {
        switch (abilityName.ToLower())
        {
            case "doublejump": return hasDoubleJump;
            case "dash": return hasDash;
            case "walljump": return hasWallJump;
            default: return false;
        }
    }
} 