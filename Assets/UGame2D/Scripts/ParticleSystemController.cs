using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UGame2D
{
    /// <summary>
    /// Advanced Particle System Controller demonstrating sophisticated particle effects
    /// and their integration with gameplay mechanics. This script showcases advanced
    /// techniques including sub-emitters, physics interactions, and dynamic effects.
    /// </summary>
    public class ParticleSystemController : MonoBehaviour
    {
        [Header("Advanced Particle Effects")]
        [SerializeField] private ParticleSystem explosionEffect;
        [SerializeField] private ParticleSystem debrisEffect;
        [SerializeField] private ParticleSystem healingAuraEffect;
        [SerializeField] private ParticleSystem magicCircleEffect;
        [SerializeField] private ParticleSystem physicsParticles;
        
        [Header("Weather System")]
        [SerializeField] private ParticleSystem windLeavesEffect;
        [SerializeField] private ParticleSystem dustStormEffect;
        [SerializeField] private ParticleSystem lightRaysEffect;
        
        [Header("Player Integration")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private ParticleSystem playerAuraEffect;
        [SerializeField] private ParticleSystem speedLinesEffect;
        [SerializeField] private ParticleSystem damageEffect;
        
        [Header("Environment Integration")]
        [SerializeField] private ParticleSystem waterSplashEffect;
        [SerializeField] private ParticleSystem steamEffect;
        [SerializeField] private ParticleSystem crystalGlowEffect;
        
        [Header("UI Integration")]
        [SerializeField] private ParticleSystem buttonHoverEffect;
        [SerializeField] private ParticleSystem notificationEffect;
        [SerializeField] private ParticleSystem transitionEffect;
        
        [Header("Physics Integration")]
        [SerializeField] private ParticleSystem collisionEffect;
        [SerializeField] private ParticleSystem gravityParticles;
        [SerializeField] private ParticleSystem magneticFieldEffect;
        
        private Dictionary<string, ParticleSystem> effectPool = new Dictionary<string, ParticleSystem>();
        private Camera mainCamera;
        private Vector3 lastPlayerPosition;
        private float playerSpeed;
        
        public static ParticleSystemController Instance { get; private set; }
        
        void Awake()
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
        
        void Start()
        {
            InitializeAdvancedEffects();
            SetupPlayerIntegration();
            mainCamera = Camera.main ?? FindObjectOfType<Camera>();
            
            if (playerTransform == null)
            {
                // Try to find a player object
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    playerTransform = player.transform;
                }
            }
            
            Debug.Log("Advanced Particle System Controller initialized!");
        }
        
        void Update()
        {
            UpdatePlayerIntegration();
            UpdateDynamicEffects();
            HandleAdvancedInput();
        }
        
        void InitializeAdvancedEffects()
        {
            CreateExplosionEffect();
            CreateDebrisEffect();
            CreateHealingAuraEffect();
            CreateMagicCircleEffect();
            CreatePhysicsParticles();
            CreateWeatherEffects();
            CreateEnvironmentEffects();
            CreateUIEffects();
            CreatePhysicsIntegrationEffects();
        }
        
        void CreateExplosionEffect()
        {
            if (explosionEffect == null)
            {
                GameObject explosionObj = new GameObject("ExplosionEffect");
                explosionObj.transform.SetParent(transform);
                explosionEffect = explosionObj.AddComponent<ParticleSystem>();
            }
            
            var main = explosionEffect.main;
            main.startLifetime = 2f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(5f, 15f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.5f);
            main.startColor = new Color(1f, 0.3f, 0.1f, 1f);
            main.maxParticles = 200;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = explosionEffect.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(0.0f, 100),
                new ParticleSystem.Burst(0.1f, 50),
                new ParticleSystem.Burst(0.2f, 25)
            });
            
            var shape = explosionEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.1f;
            
            var velocityOverLifetime = explosionEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(10f);
            
            var colorOverLifetime = explosionEffect.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient explosionGradient = new Gradient();
            explosionGradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(Color.white, 0.0f), 
                    new GradientColorKey(Color.yellow, 0.2f),
                    new GradientColorKey(Color.red, 0.6f),
                    new GradientColorKey(Color.black, 1.0f)
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(1f, 0.0f), 
                    new GradientAlphaKey(0.8f, 0.3f),
                    new GradientAlphaKey(0.4f, 0.7f),
                    new GradientAlphaKey(0.0f, 1.0f) 
                }
            );
            colorOverLifetime.color = explosionGradient;
            
            var sizeOverLifetime = explosionEffect.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            AnimationCurve explosionSizeCurve = new AnimationCurve();
            explosionSizeCurve.AddKey(0f, 0.2f);
            explosionSizeCurve.AddKey(0.3f, 1f);
            explosionSizeCurve.AddKey(1f, 0.1f);
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, explosionSizeCurve);
            
            explosionEffect.gameObject.SetActive(false);
            effectPool["explosion"] = explosionEffect;
        }
        
        void CreateDebrisEffect()
        {
            if (debrisEffect == null)
            {
                GameObject debrisObj = new GameObject("DebrisEffect");
                debrisObj.transform.SetParent(transform);
                debrisEffect = debrisObj.AddComponent<ParticleSystem>();
            }
            
            var main = debrisEffect.main;
            main.startLifetime = new ParticleSystem.MinMaxCurve(1f, 3f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(3f, 8f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.05f, 0.2f);
            main.startColor = new Color(0.6f, 0.4f, 0.2f, 1f);
            main.maxParticles = 150;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.gravityModifier = 1f; // Enable gravity for realistic debris
            
            var emission = debrisEffect.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(0.0f, 75)
            });
            
            var shape = debrisEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 60f;
            shape.radius = 0.2f;
            
            var velocityOverLifetime = debrisEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(2f, 5f);
            
            var rotationOverLifetime = debrisEffect.rotationOverLifetime;
            rotationOverLifetime.enabled = true;
            rotationOverLifetime.z = new ParticleSystem.MinMaxCurve(-360f, 360f);
            
            debrisEffect.gameObject.SetActive(false);
            effectPool["debris"] = debrisEffect;
        }
        
        void CreateHealingAuraEffect()
        {
            if (healingAuraEffect == null)
            {
                GameObject healingObj = new GameObject("HealingAuraEffect");
                healingObj.transform.SetParent(transform);
                healingAuraEffect = healingObj.AddComponent<ParticleSystem>();
            }
            
            var main = healingAuraEffect.main;
            main.startLifetime = 3f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.5f, 2f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.3f);
            main.startColor = new Color(0.2f, 1f, 0.3f, 0.7f);
            main.maxParticles = 100;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.loop = true;
            
            var emission = healingAuraEffect.emission;
            emission.rateOverTime = 30;
            
            var shape = healingAuraEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 1f;
            
            var velocityOverLifetime = healingAuraEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(1f, 3f);
            
            var colorOverLifetime = healingAuraEffect.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient healingGradient = new Gradient();
            healingGradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(Color.green, 0.0f), 
                    new GradientColorKey(Color.cyan, 0.5f),
                    new GradientColorKey(Color.white, 1.0f)
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(0.0f, 0.0f), 
                    new GradientAlphaKey(0.7f, 0.5f),
                    new GradientAlphaKey(0.0f, 1.0f) 
                }
            );
            colorOverLifetime.color = healingGradient;
            
            var sizeOverLifetime = healingAuraEffect.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            AnimationCurve healingSizeCurve = new AnimationCurve();
            healingSizeCurve.AddKey(0f, 0f);
            healingSizeCurve.AddKey(0.5f, 1f);
            healingSizeCurve.AddKey(1f, 0f);
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, healingSizeCurve);
            
            healingAuraEffect.gameObject.SetActive(false);
            effectPool["healing"] = healingAuraEffect;
        }
        
        void CreateMagicCircleEffect()
        {
            if (magicCircleEffect == null)
            {
                GameObject magicObj = new GameObject("MagicCircleEffect");
                magicObj.transform.SetParent(transform);
                magicCircleEffect = magicObj.AddComponent<ParticleSystem>();
            }
            
            var main = magicCircleEffect.main;
            main.startLifetime = 2f;
            main.startSpeed = 0f;
            main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.2f);
            main.startColor = new Color(0.5f, 0.2f, 1f, 0.8f);
            main.maxParticles = 200;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
            main.loop = true;
            
            var emission = magicCircleEffect.emission;
            emission.rateOverTime = 100;
            
            var shape = magicCircleEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 2f;
            shape.radiusThickness = 0.1f; // Create a ring shape
            
            var velocityOverLifetime = magicCircleEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.orbitalY = new ParticleSystem.MinMaxCurve(45f); // Orbital motion
            
            var colorOverLifetime = magicCircleEffect.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient magicGradient = new Gradient();
            magicGradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(Color.magenta, 0.0f), 
                    new GradientColorKey(Color.blue, 0.5f),
                    new GradientColorKey(Color.cyan, 1.0f)
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(0.8f, 0.0f), 
                    new GradientAlphaKey(0.4f, 0.5f),
                    new GradientAlphaKey(0.8f, 1.0f) 
                }
            );
            colorOverLifetime.color = magicGradient;
            
            magicCircleEffect.gameObject.SetActive(false);
            effectPool["magic"] = magicCircleEffect;
        }
        
        void CreatePhysicsParticles()
        {
            if (physicsParticles == null)
            {
                GameObject physicsObj = new GameObject("PhysicsParticles");
                physicsObj.transform.SetParent(transform);
                physicsParticles = physicsObj.AddComponent<ParticleSystem>();
            }
            
            var main = physicsParticles.main;
            main.startLifetime = 5f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(1f, 3f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.2f);
            main.startColor = Color.white;
            main.maxParticles = 50;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.gravityModifier = 0.5f;
            
            var emission = physicsParticles.emission;
            emission.rateOverTime = 10;
            
            var shape = physicsParticles.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(2, 0.5f, 1);
            
            var collision = physicsParticles.collision;
            collision.enabled = true;
            collision.type = ParticleSystemCollisionType.World;
            collision.mode = ParticleSystemCollisionMode.Collision2D;
            collision.dampen = 0.3f;
            collision.bounce = 0.7f;
            collision.lifetimeLoss = 0.1f;
            
            var velocityOverLifetime = physicsParticles.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(2f, 4f);
            
            physicsParticles.gameObject.SetActive(false);
            effectPool["physics"] = physicsParticles;
        }
        
        void CreateWeatherEffects()
        {
            CreateWindLeavesEffect();
            CreateDustStormEffect();
            CreateLightRaysEffect();
        }
        
        void CreateWindLeavesEffect()
        {
            if (windLeavesEffect == null)
            {
                GameObject leavesObj = new GameObject("WindLeavesEffect");
                leavesObj.transform.SetParent(transform);
                windLeavesEffect = leavesObj.AddComponent<ParticleSystem>();
            }
            
            var main = windLeavesEffect.main;
            main.startLifetime = 8f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(2f, 5f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.2f, 0.4f);
            main.startColor = new Color(0.8f, 0.6f, 0.2f, 0.8f);
            main.maxParticles = 100;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.gravityModifier = 0.1f;
            
            var emission = windLeavesEffect.emission;
            emission.rateOverTime = 12;
            
            var shape = windLeavesEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Rectangle;
            shape.scale = new Vector3(1, 10, 1);
            shape.position = new Vector3(-8, 0, 0);
            
            var velocityOverLifetime = windLeavesEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.World;
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(3f, 6f);
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);
            
            var noise = windLeavesEffect.noise;
            noise.enabled = true;
            noise.strength = 1f;
            noise.frequency = 0.5f;
            noise.scrollSpeed = 2f;
            
            var rotationOverLifetime = windLeavesEffect.rotationOverLifetime;
            rotationOverLifetime.enabled = true;
            rotationOverLifetime.z = new ParticleSystem.MinMaxCurve(-180f, 180f);
            
            windLeavesEffect.gameObject.SetActive(false);
            effectPool["windLeaves"] = windLeavesEffect;
        }
        
        void CreateDustStormEffect()
        {
            if (dustStormEffect == null)
            {
                GameObject dustObj = new GameObject("DustStormEffect");
                dustObj.transform.SetParent(transform);
                dustStormEffect = dustObj.AddComponent<ParticleSystem>();
            }
            
            var main = dustStormEffect.main;
            main.startLifetime = 6f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(4f, 8f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.3f, 0.8f);
            main.startColor = new Color(0.8f, 0.7f, 0.5f, 0.4f);
            main.maxParticles = 300;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = dustStormEffect.emission;
            emission.rateOverTime = 50;
            
            var shape = dustStormEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Rectangle;
            shape.scale = new Vector3(2, 8, 1);
            shape.position = new Vector3(-10, 0, 0);
            
            var velocityOverLifetime = dustStormEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.World;
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(6f, 10f);
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);
            
            var noise = dustStormEffect.noise;
            noise.enabled = true;
            noise.strength = 2f;
            noise.frequency = 0.3f;
            noise.scrollSpeed = 5f;
            
            var sizeOverLifetime = dustStormEffect.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            AnimationCurve dustSizeCurve = new AnimationCurve();
            dustSizeCurve.AddKey(0f, 0.2f);
            dustSizeCurve.AddKey(0.5f, 1f);
            dustSizeCurve.AddKey(1f, 0.1f);
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, dustSizeCurve);
            
            dustStormEffect.gameObject.SetActive(false);
            effectPool["dustStorm"] = dustStormEffect;
        }
        
        void CreateLightRaysEffect()
        {
            if (lightRaysEffect == null)
            {
                GameObject raysObj = new GameObject("LightRaysEffect");
                raysObj.transform.SetParent(transform);
                lightRaysEffect = raysObj.AddComponent<ParticleSystem>();
            }
            
            var main = lightRaysEffect.main;
            main.startLifetime = 4f;
            main.startSpeed = 0f;
            main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.3f);
            main.startColor = new Color(1f, 1f, 0.8f, 0.3f);
            main.maxParticles = 50;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = lightRaysEffect.emission;
            emission.rateOverTime = 12;
            
            var shape = lightRaysEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 15f;
            shape.radius = 0.1f;
            shape.position = new Vector3(0, 5, 0);
            shape.rotation = new Vector3(180, 0, 0);
            
            var velocityOverLifetime = lightRaysEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(-3f, -1f);
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);
            
            var sizeOverLifetime = lightRaysEffect.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            AnimationCurve raysSizeCurve = new AnimationCurve();
            raysSizeCurve.AddKey(0f, 0f);
            raysSizeCurve.AddKey(0.3f, 1f);
            raysSizeCurve.AddKey(1f, 0f);
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, raysSizeCurve);
            
            lightRaysEffect.gameObject.SetActive(false);
            effectPool["lightRays"] = lightRaysEffect;
        }
        
        void CreateEnvironmentEffects()
        {
            // Environment effects would be created here
            // For brevity, I'll create one example
            CreateWaterSplashEffect();
        }
        
        void CreateWaterSplashEffect()
        {
            if (waterSplashEffect == null)
            {
                GameObject splashObj = new GameObject("WaterSplashEffect");
                splashObj.transform.SetParent(transform);
                waterSplashEffect = splashObj.AddComponent<ParticleSystem>();
            }
            
            var main = waterSplashEffect.main;
            main.startLifetime = 1.5f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(3f, 8f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.2f);
            main.startColor = new Color(0.3f, 0.6f, 1f, 0.8f);
            main.maxParticles = 80;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.gravityModifier = 2f;
            
            var emission = waterSplashEffect.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(0.0f, 40)
            });
            
            var shape = waterSplashEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Hemisphere;
            shape.radius = 0.3f;
            
            var velocityOverLifetime = waterSplashEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(3f, 6f);
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(2f, 5f);
            
            waterSplashEffect.gameObject.SetActive(false);
            effectPool["waterSplash"] = waterSplashEffect;
        }
        
        void CreateUIEffects()
        {
            // UI effects implementation
        }
        
        void CreatePhysicsIntegrationEffects()
        {
            // Physics integration effects implementation
        }
        
        void SetupPlayerIntegration()
        {
            if (playerTransform != null)
            {
                lastPlayerPosition = playerTransform.position;
            }
        }
        
        void UpdatePlayerIntegration()
        {
            if (playerTransform != null)
            {
                // Calculate player speed
                Vector3 currentPosition = playerTransform.position;
                playerSpeed = Vector3.Distance(currentPosition, lastPlayerPosition) / Time.deltaTime;
                lastPlayerPosition = currentPosition;
                
                // Update player-related effects based on speed
                UpdatePlayerSpeedEffects();
            }
        }
        
        void UpdatePlayerSpeedEffects()
        {
            // Enable speed lines when player is moving fast
            if (speedLinesEffect != null && playerSpeed > 5f)
            {
                if (!speedLinesEffect.gameObject.activeInHierarchy)
                {
                    speedLinesEffect.transform.position = playerTransform.position;
                    speedLinesEffect.gameObject.SetActive(true);
                }
                speedLinesEffect.transform.position = playerTransform.position;
            }
            else if (speedLinesEffect != null && speedLinesEffect.gameObject.activeInHierarchy)
            {
                speedLinesEffect.gameObject.SetActive(false);
            }
        }
        
        void UpdateDynamicEffects()
        {
            // Update time-based effects
            UpdateWindEffects();
            UpdateLightingEffects();
        }
        
        void UpdateWindEffects()
        {
            // Simulate wind changes
            if (windLeavesEffect != null && windLeavesEffect.gameObject.activeInHierarchy)
            {
                var velocityOverLifetime = windLeavesEffect.velocityOverLifetime;
                float windStrength = 3f + Mathf.Sin(Time.time * 0.5f) * 2f;
                velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(windStrength, windStrength + 3f);
                velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(0f);
                velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);
            }
        }
        
        void UpdateLightingEffects()
        {
            // Simulate changing light conditions
            if (lightRaysEffect != null && lightRaysEffect.gameObject.activeInHierarchy)
            {
                var main = lightRaysEffect.main;
                float alpha = 0.3f + Mathf.Sin(Time.time * 0.3f) * 0.2f;
                main.startColor = new Color(1f, 1f, 0.8f, alpha);
            }
        }
        
        void HandleAdvancedInput()
        {
            // E for explosion
            if (Input.GetKeyDown(KeyCode.E))
            {
                Vector3 mousePos = GetMouseWorldPosition();
                TriggerExplosion(mousePos);
            }
            
            // Q for healing aura
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Vector3 mousePos = GetMouseWorldPosition();
                TriggerHealingAura(mousePos);
            }
            
            // T for magic circle
            if (Input.GetKeyDown(KeyCode.T))
            {
                Vector3 mousePos = GetMouseWorldPosition();
                TriggerMagicCircle(mousePos);
            }
            
            // P for physics particles
            if (Input.GetKeyDown(KeyCode.P))
            {
                TogglePhysicsParticles();
            }
            
            // W for weather effects cycling
            if (Input.GetKeyDown(KeyCode.W))
            {
                CycleWeatherEffects();
            }
            
            // 1-3 for specific weather effects
            if (Input.GetKeyDown(KeyCode.Alpha1)) ToggleEffect("windLeaves");
            if (Input.GetKeyDown(KeyCode.Alpha2)) ToggleEffect("dustStorm");
            if (Input.GetKeyDown(KeyCode.Alpha3)) ToggleEffect("lightRays");
            
            // New advanced effects
            // F for water splash
            if (Input.GetKeyDown(KeyCode.F))
            {
                Vector3 mousePos = GetMouseWorldPosition();
                TriggerWaterSplash(mousePos);
            }
            
            // G for portal effect
            if (Input.GetKeyDown(KeyCode.G))
            {
                Vector3 mousePos = GetMouseWorldPosition();
                CreatePortalEffect(mousePos);
            }
            
            // V for cinematic explosion
            if (Input.GetKeyDown(KeyCode.V))
            {
                Vector3 mousePos = GetMouseWorldPosition();
                CreateCinematicExplosion(mousePos);
            }
            
            // C for lightning strike
            if (Input.GetKeyDown(KeyCode.C))
            {
                Vector3 mousePos = GetMouseWorldPosition();
                CreateLightningStrike(mousePos);
            }
        }
        
        Vector3 GetMouseWorldPosition()
        {
            if (mainCamera != null)
            {
                Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                return mousePos;
            }
            return Vector3.zero;
        }
        
        // Public methods for triggering effects
        public void TriggerExplosion(Vector3 position)
        {
            if (explosionEffect != null)
            {
                explosionEffect.transform.position = position;
                explosionEffect.gameObject.SetActive(true);
                explosionEffect.Play();
                
                // Also trigger debris
                if (debrisEffect != null)
                {
                    debrisEffect.transform.position = position;
                    debrisEffect.gameObject.SetActive(true);
                    debrisEffect.Play();
                    StartCoroutine(DeactivateAfterDuration(debrisEffect.gameObject, 4f));
                }
                
                StartCoroutine(DeactivateAfterDuration(explosionEffect.gameObject, 3f));
                Debug.Log($"Explosion triggered at {position}");
            }
        }
        
        public void TriggerHealingAura(Vector3 position)
        {
            if (healingAuraEffect != null)
            {
                healingAuraEffect.transform.position = position;
                healingAuraEffect.gameObject.SetActive(true);
                healingAuraEffect.Play();
                StartCoroutine(DeactivateAfterDuration(healingAuraEffect.gameObject, 5f));
                Debug.Log($"Healing aura triggered at {position}");
            }
        }
        
        public void TriggerMagicCircle(Vector3 position)
        {
            if (magicCircleEffect != null)
            {
                magicCircleEffect.transform.position = position;
                magicCircleEffect.gameObject.SetActive(true);
                magicCircleEffect.Play();
                StartCoroutine(DeactivateAfterDuration(magicCircleEffect.gameObject, 4f));
                Debug.Log($"Magic circle triggered at {position}");
            }
        }
        
        public void TriggerWaterSplash(Vector3 position)
        {
            if (waterSplashEffect != null)
            {
                waterSplashEffect.transform.position = position;
                waterSplashEffect.gameObject.SetActive(true);
                waterSplashEffect.Play();
                StartCoroutine(DeactivateAfterDuration(waterSplashEffect.gameObject, 2f));
                Debug.Log($"Water splash triggered at {position}");
            }
        }
        
        public void TogglePhysicsParticles()
        {
            if (physicsParticles != null)
            {
                bool isActive = physicsParticles.gameObject.activeInHierarchy;
                physicsParticles.gameObject.SetActive(!isActive);
                
                if (!isActive)
                {
                    physicsParticles.transform.position = GetMouseWorldPosition();
                    physicsParticles.Play();
                }
                
                Debug.Log($"Physics particles: {(!isActive ? "Enabled" : "Disabled")}");
            }
        }
        
        public void CycleWeatherEffects()
        {
            // Disable all weather effects
            ToggleEffect("windLeaves", false);
            ToggleEffect("dustStorm", false);
            ToggleEffect("lightRays", false);
            
            // Enable a random one
            string[] weatherEffects = { "windLeaves", "dustStorm", "lightRays" };
            string selectedEffect = weatherEffects[Random.Range(0, weatherEffects.Length)];
            ToggleEffect(selectedEffect, true);
            
            Debug.Log($"Weather effect switched to: {selectedEffect}");
        }
        
        public void ToggleEffect(string effectName, bool? forceState = null)
        {
            if (effectPool.ContainsKey(effectName))
            {
                ParticleSystem effect = effectPool[effectName];
                bool newState = forceState ?? !effect.gameObject.activeInHierarchy;
                effect.gameObject.SetActive(newState);
                
                if (newState)
                {
                    effect.Play();
                }
                
                Debug.Log($"{effectName} effect: {(newState ? "Enabled" : "Disabled")}");
            }
        }
        
        IEnumerator DeactivateAfterDuration(GameObject obj, float duration)
        {
            yield return new WaitForSeconds(duration);
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        
        void OnDisable()
        {
            StopAllCoroutines();
        }
        
        /// <summary>
        /// Creates a cinematic multi-layered explosion effect
        /// </summary>
        public void CreateCinematicExplosion(Vector3 position)
        {
            // Core blast
            CreateExplosionLayer("CoreBlast", position, Color.white, 0.5f, 12f, 150, 0f);
            
            // Fire ring
            StartCoroutine(DelayedExplosionLayer("FireRing", position, new Color(1f, 0.4f, 0.1f), 0.8f, 8f, 100, 0.1f));
            
            // Smoke plume
            StartCoroutine(DelayedExplosionLayer("SmokePlume", position, new Color(0.3f, 0.3f, 0.3f), 2f, 4f, 80, 0.3f));
            
            // Debris shower
            StartCoroutine(DelayedExplosionLayer("Debris", position, new Color(0.6f, 0.4f, 0.2f), 1.5f, 10f, 60, 0.15f));
            
            // Screen shake effect
            StartCoroutine(CameraShake(0.4f, 0.3f));
            
            Debug.Log($"Cinematic explosion created at {position}");
        }
        
        void CreateExplosionLayer(string name, Vector3 position, Color color, float lifetime, float speed, int particles, float delay)
        {
            GameObject layerObj = new GameObject($"Explosion_{name}");
            layerObj.transform.position = position;
            ParticleSystem layer = layerObj.AddComponent<ParticleSystem>();
            
            var main = layer.main;
            main.startLifetime = lifetime;
            main.startSpeed = new ParticleSystem.MinMaxCurve(speed * 0.5f, speed);
            main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.5f);
            main.startColor = color;
            main.maxParticles = particles;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = layer.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, particles) });
            
            var shape = layer.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.2f;
            
            var velocityOverLifetime = layer.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(speed);
            
            // Auto-destroy after effect
            Destroy(layerObj, lifetime + 1f);
        }
        
        IEnumerator DelayedExplosionLayer(string name, Vector3 position, Color color, float lifetime, float speed, int particles, float delay)
        {
            yield return new WaitForSeconds(delay);
            CreateExplosionLayer(name, position, color, lifetime, speed, particles, delay);
        }
        
        /// <summary>
        /// Creates a magical portal effect with swirling particles
        /// </summary>
        public void CreatePortalEffect(Vector3 position)
        {
            GameObject portalObj = new GameObject("PortalEffect");
            portalObj.transform.position = position;
            ParticleSystem portal = portalObj.AddComponent<ParticleSystem>();
            
            var main = portal.main;
            main.startLifetime = 3f;
            main.startSpeed = 2f;
            main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.3f);
            main.maxParticles = 200;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            // Portal colors - purple to blue gradient
            var colorOverLifetime = portal.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient portalGradient = new Gradient();
            portalGradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(new Color(0.5f, 0.2f, 1f), 0.0f),  // Purple
                    new GradientColorKey(new Color(0.2f, 0.5f, 1f), 0.5f),  // Blue
                    new GradientColorKey(new Color(1f, 1f, 1f), 1.0f)       // White
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(0.8f, 0.0f), 
                    new GradientAlphaKey(1.0f, 0.3f),
                    new GradientAlphaKey(0.0f, 1.0f) 
                }
            );
            colorOverLifetime.color = portalGradient;
            
            // Spiral motion
            var velocityOverLifetime = portal.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.orbitalX = new ParticleSystem.MinMaxCurve(2f);
            velocityOverLifetime.orbitalY = new ParticleSystem.MinMaxCurve(3f);
            velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(-1f, 1f);
            
            // Ring emission shape
            var shape = portal.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 1f;
            shape.radiusThickness = 0.1f;
            
            Destroy(portalObj, 4f);
            Debug.Log($"Portal effect created at {position}");
        }
        
        /// <summary>
        /// Creates a lightning strike effect
        /// </summary>
        public void CreateLightningStrike(Vector3 position)
        {
            GameObject lightningObj = new GameObject("LightningStrike");
            lightningObj.transform.position = position + Vector3.up * 5f; // Start from above
            ParticleSystem lightning = lightningObj.AddComponent<ParticleSystem>();
            
            var main = lightning.main;
            main.startLifetime = 0.3f;
            main.startSpeed = 20f;
            main.startSize = new ParticleSystem.MinMaxCurve(0.05f, 0.2f);
            main.startColor = new Color(0.8f, 0.9f, 1f, 1f); // Electric blue-white
            main.maxParticles = 100;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = lightning.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] { 
                new ParticleSystem.Burst(0f, 50),
                new ParticleSystem.Burst(0.05f, 30),
                new ParticleSystem.Burst(0.1f, 20)
            });
            
            // Downward direction
            var velocityOverLifetime = lightning.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(-15f, -25f);
            
            // Linear emission shape pointing down
            var shape = lightning.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 10f;
            shape.radius = 0.1f;
            
            // Impact effect at ground
            StartCoroutine(CreateLightningImpact(position, 0.2f));
            
            Destroy(lightningObj, 1f);
            Debug.Log($"Lightning strike at {position}");
        }
        
        IEnumerator CreateLightningImpact(Vector3 position, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            GameObject impactObj = new GameObject("LightningImpact");
            impactObj.transform.position = position;
            ParticleSystem impact = impactObj.AddComponent<ParticleSystem>();
            
            var main = impact.main;
            main.startLifetime = 1f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(3f, 8f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.4f);
            main.startColor = new Color(1f, 1f, 0.8f, 1f);
            main.maxParticles = 80;
            
            var emission = impact.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 80) });
            
            var shape = impact.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.3f;
            
            Destroy(impactObj, 2f);
        }
        
        IEnumerator CameraShake(float duration, float magnitude)
        {
            Camera cam = mainCamera ?? Camera.main;
            if (cam == null) yield break;
            
            Vector3 originalPos = cam.transform.position;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;
                
                cam.transform.position = originalPos + new Vector3(x, y, 0);
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            cam.transform.position = originalPos;
        }
        
        void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
} 