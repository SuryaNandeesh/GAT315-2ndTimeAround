using UnityEngine;

namespace UGame2D
{
    /// <summary>
    /// Simple tester script to verify particle systems functionality
    /// and demonstrate random color variations
    /// </summary>
    public class ParticleSystemsTester : MonoBehaviour
    {
        [Header("Test Settings")]
        [SerializeField] private bool enableAutoTesting = true;
        [SerializeField] private float testInterval = 2f;
        
        private ParticleSystemsManager particleManager;
        private ParticleSystemController particleController;
        private float lastTestTime;
        
        void Start()
        {
            // Find the particle managers
            particleManager = FindObjectOfType<ParticleSystemsManager>();
            particleController = FindObjectOfType<ParticleSystemController>();
            
            if (particleManager == null)
            {
                Debug.LogWarning("ParticleSystemsManager not found! Make sure it's in the scene.");
            }
            
            if (particleController == null)
            {
                Debug.LogWarning("ParticleSystemController not found!");
            }
            
            Debug.Log("Particle Systems Tester started. Press T to run manual tests.");
        }
        
        void Update()
        {
            // Manual testing
            if (Input.GetKeyDown(KeyCode.T))
            {
                RunManualTest();
            }
            
            // Auto testing
            if (enableAutoTesting && Time.time - lastTestTime > testInterval)
            {
                RunAutoTest();
                lastTestTime = Time.time;
            }
        }
        
        void RunManualTest()
        {
            Debug.Log("Running manual particle systems test...");
            
            // Test random burst effects
            Vector3 testPos = new Vector3(Random.Range(-3f, 3f), Random.Range(-2f, 2f), 0);
            
            if (particleManager != null)
            {
                // Test click burst with random color
                particleManager.TriggerClickBurst(testPos);
                Debug.Log($"Triggered click burst at {testPos}");
                
                // Test collection burst
                Vector3 testPos2 = new Vector3(Random.Range(-3f, 3f), Random.Range(-2f, 2f), 0);
                particleManager.TriggerCollectionBurst(testPos2);
                Debug.Log($"Triggered collection burst at {testPos2}");
                
                // Test player feedback
                particleManager.SimulatePlayerFeedback();
                Debug.Log("Triggered player feedback effects");
            }
            
            if (particleController != null)
            {
                // Test explosion effect
                Vector3 explosionPos = new Vector3(Random.Range(-3f, 3f), Random.Range(-2f, 2f), 0);
                particleController.TriggerExplosion(explosionPos);
                Debug.Log($"Triggered explosion at {explosionPos}");
            }
        }
        
        void RunAutoTest()
        {
            if (particleManager == null) return;
            
            // Randomly trigger different effects
            int testType = Random.Range(0, 4);
            Vector3 randomPos = new Vector3(Random.Range(-4f, 4f), Random.Range(-3f, 3f), 0);
            
            switch (testType)
            {
                case 0:
                    particleManager.TriggerClickBurst(randomPos);
                    Debug.Log($"Auto test: Click burst at {randomPos}");
                    break;
                case 1:
                    particleManager.TriggerCollectionBurst(randomPos);
                    Debug.Log($"Auto test: Collection burst at {randomPos}");
                    break;
                case 2:
                    particleManager.SimulatePlayerFeedback();
                    Debug.Log("Auto test: Player feedback");
                    break;
                case 3:
                    if (particleController != null)
                    {
                        particleController.TriggerExplosion(randomPos);
                        Debug.Log($"Auto test: Explosion at {randomPos}");
                    }
                    break;
            }
        }
        
        void OnGUI()
        {
            GUI.Box(new Rect(10, 100, 300, 120), "Particle Systems Tester");
            
            if (GUI.Button(new Rect(20, 130, 100, 25), "Manual Test"))
            {
                RunManualTest();
            }
            
            if (GUI.Button(new Rect(130, 130, 100, 25), "Toggle Auto"))
            {
                enableAutoTesting = !enableAutoTesting;
            }
            
            if (GUI.Button(new Rect(20, 160, 100, 25), "Test All"))
            {
                TestAllEffects();
            }
            
            GUI.Label(new Rect(20, 190, 280, 20), $"Auto Testing: {(enableAutoTesting ? "ON" : "OFF")}");
        }
        
        void TestAllEffects()
        {
            Debug.Log("Testing all particle effects...");
            
            if (particleManager != null)
            {
                // Test environmental effects
                particleManager.ToggleEnvironmentalEffects();
                
                // Test hazard effects
                particleManager.ToggleHazardEffects();
                
                // Test interactive effects
                for (int i = 0; i < 3; i++)
                {
                    Vector3 pos = new Vector3(Random.Range(-3f, 3f), Random.Range(-2f, 2f), 0);
                    particleManager.TriggerClickBurst(pos);
                    particleManager.TriggerCollectionBurst(pos + Vector3.right);
                }
                
                // Test player feedback
                particleManager.SimulatePlayerFeedback();
            }
            
            if (particleController != null)
            {
                // Test advanced effects
                for (int i = 0; i < 2; i++)
                {
                    Vector3 pos = new Vector3(Random.Range(-3f, 3f), Random.Range(-2f, 2f), 0);
                    particleController.TriggerExplosion(pos);
                    particleController.TriggerHealingAura(pos + Vector3.up);
                    particleController.TriggerMagicCircle(pos + Vector3.down);
                }
                
                // Test weather effects
                particleController.CycleWeatherEffects();
            }
            
            Debug.Log("All effects test completed!");
        }
    }
} 