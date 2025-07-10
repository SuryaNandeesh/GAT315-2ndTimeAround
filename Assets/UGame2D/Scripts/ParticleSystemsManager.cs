using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace UGame2D
{
    /// <summary>
    /// Comprehensive Particle Systems Manager for demonstrating various particle effects
    /// in a 2D Unity game environment. This script showcases different applications
    /// of Unity's particle system technology.
    /// 
    /// Features demonstrated:
    /// - Environmental effects (rain, snow, floating particles)
    /// - Player feedback effects (jump dust, landing particles, movement trails)
    /// - Interactive effects (click/touch particles, collection bursts)
    /// - UI feedback effects (damage numbers, floating text)
    /// - Hazard effects (fire, smoke, sparks)
    /// - Background ambiance effects
    /// </summary>
    public class ParticleSystemsManager : MonoBehaviour
    {
        [Header("Environmental Effects")]
        [SerializeField] private ParticleSystem rainEffect;
        [SerializeField] private ParticleSystem snowEffect;
        [SerializeField] private ParticleSystem floatingParticles;
        
        [Header("Player Feedback Effects")]
        [SerializeField] private ParticleSystem jumpDustEffect;
        [SerializeField] private ParticleSystem landingEffect;
        [SerializeField] private ParticleSystem movementTrailEffect;
        
        [Header("Interactive Effects")]
        [SerializeField] private ParticleSystem clickBurstEffect;
        [SerializeField] private ParticleSystem collectionBurstEffect;
        
        [Header("Hazard Effects")]
        [SerializeField] private ParticleSystem fireEffect;
        [SerializeField] private ParticleSystem smokeEffect;
        [SerializeField] private ParticleSystem sparksEffect;
        
        [Header("UI and Feedback")]
        [SerializeField] private Canvas uiCanvas;
        [SerializeField] private Text instructionsText;
        [SerializeField] private Text particleCountText;
        
        [Header("Demo Controls")]
        [SerializeField] private bool enableRain = false;
        [SerializeField] private bool enableSnow = false;
        [SerializeField] private bool enableFloatingParticles = true;
        [SerializeField] private bool enableHazardEffects = true;
        [SerializeField] private float effectSwitchInterval = 5f;
        
        private Camera mainCamera;
        private float nextEffectSwitch;
        private int currentEnvironmentalEffect = 0;
        private bool isDemoRunning = true;
        
        // Pool for reusable particle systems
        private Dictionary<string, ParticleSystem> particlePool = new Dictionary<string, ParticleSystem>();
        
        // Color variations for distinct particles
        private Color[] randomColors = new Color[]
        {
            Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan,
            new Color(1f, 0.5f, 0f), // Orange
            new Color(0.5f, 0f, 1f), // Purple
            new Color(1f, 0.75f, 0.8f), // Pink
            new Color(0.5f, 1f, 0.5f), // Light Green
            new Color(0.8f, 0.8f, 0.2f), // Olive
            new Color(0.2f, 0.8f, 0.8f), // Turquoise
        };
        
        private float lastColorChangeTime;
        
        void Start()
        {
            InitializeParticleSystems();
            SetupUI();
            SetupCamera();
            
            nextEffectSwitch = Time.time + effectSwitchInterval;
            
            Debug.Log("Particle Systems Demo Started!");
            Debug.Log("Left Click: Interactive burst effect");
            Debug.Log("Right Click: Collection burst effect");
            Debug.Log("Space: Toggle environmental effects");
            Debug.Log("H: Toggle hazard effects");
        }
        
        /// <summary>
        /// Get a random color from the predefined color array
        /// </summary>
        Color GetRandomColor()
        {
            return randomColors[Random.Range(0, randomColors.Length)];
        }
        
        /// <summary>
        /// Get a random color with specified alpha
        /// </summary>
        Color GetRandomColor(float alpha)
        {
            Color color = GetRandomColor();
            color.a = alpha;
            return color;
        }
        
        void Update()
        {
            HandleInput();
            UpdateEffectSwitching();
            UpdateUI();
            UpdateParticleEffects();
        }
        
        /// <summary>
        /// Initialize all particle systems with appropriate settings for 2D gameplay
        /// </summary>
        void InitializeParticleSystems()
        {
            // Create environmental effects
            CreateRainEffect();
            CreateSnowEffect();
            CreateFloatingParticlesEffect();
            
            // Create player feedback effects
            CreateJumpDustEffect();
            CreateLandingEffect();
            CreateMovementTrailEffect();
            
            // Create interactive effects
            CreateClickBurstEffect();
            CreateCollectionBurstEffect();
            
            // Create hazard effects
            CreateFireEffect();
            CreateSmokeEffect();
            CreateSparksEffect();
        }
        
        void CreateRainEffect()
        {
            if (rainEffect == null)
            {
                GameObject rainObj = new GameObject("RainEffect");
                rainObj.transform.SetParent(transform);
                rainObj.transform.position = new Vector3(0, 10, 0);
                rainEffect = rainObj.AddComponent<ParticleSystem>();
            }
            
            var main = rainEffect.main;
            main.startLifetime = 2f;
            main.startSpeed = 8f;
            main.startSize = 0.1f;
            main.startColor = new Color(0.7f, 0.8f, 1f, 0.8f);
            main.maxParticles = 1000;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = rainEffect.emission;
            emission.rateOverTime = 500;
            
            var shape = rainEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Rectangle;
            shape.scale = new Vector3(20, 1, 1);
            
            var velocityOverLifetime = rainEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.World;
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(-10f);
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);
            
            var sizeOverLifetime = rainEffect.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            AnimationCurve sizeCurve = new AnimationCurve();
            sizeCurve.AddKey(0f, 1f);
            sizeCurve.AddKey(1f, 0.5f);
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);
            
            rainEffect.gameObject.SetActive(enableRain);
        }
        
        void CreateSnowEffect()
        {
            if (snowEffect == null)
            {
                GameObject snowObj = new GameObject("SnowEffect");
                snowObj.transform.SetParent(transform);
                snowObj.transform.position = new Vector3(0, 10, 0);
                snowEffect = snowObj.AddComponent<ParticleSystem>();
            }
            
            var main = snowEffect.main;
            main.startLifetime = 5f;
            main.startSpeed = 2f;
            main.startSize = new ParticleSystem.MinMaxCurve(0.05f, 0.2f);
            main.startColor = Color.white;
            main.maxParticles = 500;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = snowEffect.emission;
            emission.rateOverTime = 100;
            
            var shape = snowEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Rectangle;
            shape.scale = new Vector3(25, 1, 1);
            
            var velocityOverLifetime = snowEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.World;
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(-0.5f, 0.5f);
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(-2f, -1f);
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);
            
            var noise = snowEffect.noise;
            noise.enabled = true;
            noise.strength = 0.1f;
            noise.frequency = 1f;
            
            snowEffect.gameObject.SetActive(enableSnow);
        }
        
        void CreateFloatingParticlesEffect()
        {
            if (floatingParticles == null)
            {
                GameObject floatingObj = new GameObject("FloatingParticles");
                floatingObj.transform.SetParent(transform);
                floatingParticles = floatingObj.AddComponent<ParticleSystem>();
            }
            
            var main = floatingParticles.main;
            main.startLifetime = 10f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.5f, 2f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.3f);
            // Use random colors with reduced saturation for magical floating effect
            Color magicalColor = GetRandomColor(0.6f);
            magicalColor = Color.Lerp(magicalColor, Color.white, 0.3f);
            main.startColor = magicalColor;
            main.maxParticles = 200;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = floatingParticles.emission;
            emission.rateOverTime = 20;
            
            var shape = floatingParticles.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(15, 10, 1);
            
            var velocityOverLifetime = floatingParticles.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(0.5f, 1.5f);
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);
            
            var limitVelocityOverLifetime = floatingParticles.limitVelocityOverLifetime;
            limitVelocityOverLifetime.enabled = true;
            limitVelocityOverLifetime.limit = 2f;
            limitVelocityOverLifetime.dampen = 0.5f;
            
            var colorOverLifetime = floatingParticles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.yellow, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(0.6f, 0.5f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            colorOverLifetime.color = gradient;
            
            floatingParticles.gameObject.SetActive(enableFloatingParticles);
        }
        
        void CreateJumpDustEffect()
        {
            if (jumpDustEffect == null)
            {
                GameObject jumpDustObj = new GameObject("JumpDustEffect");
                jumpDustObj.transform.SetParent(transform);
                jumpDustEffect = jumpDustObj.AddComponent<ParticleSystem>();
            }
            
            var main = jumpDustEffect.main;
            main.startLifetime = 0.5f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(2f, 5f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.2f);
            main.startColor = new Color(0.8f, 0.7f, 0.5f, 0.8f);
            main.maxParticles = 50;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = jumpDustEffect.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(0.0f, 15)
            });
            
            var shape = jumpDustEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Hemisphere;
            shape.radius = 0.5f;
            
            var velocityOverLifetime = jumpDustEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(3f);
            
            jumpDustEffect.gameObject.SetActive(false);
        }
        
        void CreateLandingEffect()
        {
            if (landingEffect == null)
            {
                GameObject landingObj = new GameObject("LandingEffect");
                landingObj.transform.SetParent(transform);
                landingEffect = landingObj.AddComponent<ParticleSystem>();
            }
            
            var main = landingEffect.main;
            main.startLifetime = 1f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(1f, 3f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.3f);
            main.startColor = new Color(0.7f, 0.6f, 0.4f, 0.9f);
            main.maxParticles = 30;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = landingEffect.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(0.0f, 20)
            });
            
            var shape = landingEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.3f;
            
            var velocityOverLifetime = landingEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(1f, 2f);
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(2f);
            
            landingEffect.gameObject.SetActive(false);
        }
        
        void CreateMovementTrailEffect()
        {
            if (movementTrailEffect == null)
            {
                GameObject trailObj = new GameObject("MovementTrailEffect");
                trailObj.transform.SetParent(transform);
                movementTrailEffect = trailObj.AddComponent<ParticleSystem>();
            }
            
            var main = movementTrailEffect.main;
            main.startLifetime = 0.8f;
            main.startSpeed = 0f;
            main.startSize = new ParticleSystem.MinMaxCurve(0.05f, 0.1f);
            main.startColor = new Color(1f, 0.8f, 0.2f, 0.7f);
            main.maxParticles = 100;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = movementTrailEffect.emission;
            emission.rateOverTime = 50;
            
            var colorOverLifetime = movementTrailEffect.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient trailGradient = new Gradient();
            trailGradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.red, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(0.7f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            colorOverLifetime.color = trailGradient;
            
            movementTrailEffect.gameObject.SetActive(false);
        }
        
        void CreateClickBurstEffect()
        {
            if (clickBurstEffect == null)
            {
                GameObject clickObj = new GameObject("ClickBurstEffect");
                clickObj.transform.SetParent(transform);
                clickBurstEffect = clickObj.AddComponent<ParticleSystem>();
            }
            
            var main = clickBurstEffect.main;
            main.startLifetime = 0.8f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(3f, 8f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.2f);
            main.startColor = GetRandomColor(0.9f);
            main.maxParticles = 50;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = clickBurstEffect.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(0.0f, 25)
            });
            
            var shape = clickBurstEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.1f;
            
            var velocityOverLifetime = clickBurstEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(5f);
            
            clickBurstEffect.gameObject.SetActive(false);
        }
        
        void CreateCollectionBurstEffect()
        {
            if (collectionBurstEffect == null)
            {
                GameObject collectionObj = new GameObject("CollectionBurstEffect");
                collectionObj.transform.SetParent(transform);
                collectionBurstEffect = collectionObj.AddComponent<ParticleSystem>();
            }
            
            var main = collectionBurstEffect.main;
            main.startLifetime = 1.2f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(2f, 6f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.25f);
            main.startColor = new Color(1f, 0.8f, 0.2f, 1f);
            main.maxParticles = 40;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = collectionBurstEffect.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(0.0f, 30)
            });
            
            var shape = collectionBurstEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.2f;
            
            var velocityOverLifetime = collectionBurstEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(2f, 4f);
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(3f);
            
            var sizeOverLifetime = collectionBurstEffect.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            AnimationCurve collectionSizeCurve = new AnimationCurve();
            collectionSizeCurve.AddKey(0f, 0.5f);
            collectionSizeCurve.AddKey(0.3f, 1f);
            collectionSizeCurve.AddKey(1f, 0f);
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, collectionSizeCurve);
            
            collectionBurstEffect.gameObject.SetActive(false);
        }
        
        void CreateFireEffect()
        {
            if (fireEffect == null)
            {
                GameObject fireObj = new GameObject("FireEffect");
                fireObj.transform.SetParent(transform);
                fireObj.transform.position = new Vector3(-3, -2, 0);
                fireEffect = fireObj.AddComponent<ParticleSystem>();
            }
            
            var main = fireEffect.main;
            main.startLifetime = 1.5f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(1f, 3f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.2f, 0.4f);
            main.startColor = new Color(1f, 0.4f, 0.1f, 0.8f);
            main.maxParticles = 100;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = fireEffect.emission;
            emission.rateOverTime = 50;
            
            var shape = fireEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.3f;
            
            var velocityOverLifetime = fireEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(2f, 4f);
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);
            
            var colorOverLifetime = fireEffect.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient fireGradient = new Gradient();
            fireGradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(Color.yellow, 0.0f), 
                    new GradientColorKey(Color.red, 0.5f),
                    new GradientColorKey(new Color(0.2f, 0.1f, 0.1f), 1.0f)
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(0.8f, 0.0f), 
                    new GradientAlphaKey(0.6f, 0.5f),
                    new GradientAlphaKey(0.0f, 1.0f) 
                }
            );
            colorOverLifetime.color = fireGradient;
            
            fireEffect.gameObject.SetActive(enableHazardEffects);
        }
        
        void CreateSmokeEffect()
        {
            if (smokeEffect == null)
            {
                GameObject smokeObj = new GameObject("SmokeEffect");
                smokeObj.transform.SetParent(transform);
                smokeObj.transform.position = new Vector3(0, -2, 0);
                smokeEffect = smokeObj.AddComponent<ParticleSystem>();
            }
            
            var main = smokeEffect.main;
            main.startLifetime = 3f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.5f, 2f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.3f, 0.6f);
            main.startColor = new Color(0.5f, 0.5f, 0.5f, 0.6f);
            main.maxParticles = 75;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = smokeEffect.emission;
            emission.rateOverTime = 25;
            
            var shape = smokeEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.2f;
            
            var velocityOverLifetime = smokeEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(1f, 2f);
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);
            
            var sizeOverLifetime = smokeEffect.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            AnimationCurve smokeSizeCurve = new AnimationCurve();
            smokeSizeCurve.AddKey(0f, 0.3f);
            smokeSizeCurve.AddKey(1f, 1f);
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, smokeSizeCurve);
            
            var colorOverLifetime = smokeEffect.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient smokeGradient = new Gradient();
            smokeGradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(new Color(0.7f, 0.7f, 0.7f), 0.0f), 
                    new GradientColorKey(new Color(0.3f, 0.3f, 0.3f), 1.0f)
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(0.6f, 0.0f), 
                    new GradientAlphaKey(0.0f, 1.0f) 
                }
            );
            colorOverLifetime.color = smokeGradient;
            
            smokeEffect.gameObject.SetActive(enableHazardEffects);
        }
        
        void CreateSparksEffect()
        {
            if (sparksEffect == null)
            {
                GameObject sparksObj = new GameObject("SparksEffect");
                sparksObj.transform.SetParent(transform);
                sparksObj.transform.position = new Vector3(3, -2, 0);
                sparksEffect = sparksObj.AddComponent<ParticleSystem>();
            }
            
            var main = sparksEffect.main;
            main.startLifetime = 1f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(3f, 8f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.05f, 0.1f);
            main.startColor = new Color(1f, 0.8f, 0.2f, 1f);
            main.maxParticles = 60;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = sparksEffect.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(0.0f, 20),
                new ParticleSystem.Burst(0.5f, 15),
                new ParticleSystem.Burst(1.0f, 10)
            });
            
            var shape = sparksEffect.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 45f;
            shape.radius = 0.1f;
            
            var velocityOverLifetime = sparksEffect.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.World;
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(0f);
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(-5f);
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);
            
            var colorOverLifetime = sparksEffect.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient sparksGradient = new Gradient();
            sparksGradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(Color.white, 0.0f), 
                    new GradientColorKey(Color.yellow, 0.3f),
                    new GradientColorKey(Color.red, 1.0f)
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(1f, 0.0f), 
                    new GradientAlphaKey(0.0f, 1.0f) 
                }
            );
            colorOverLifetime.color = sparksGradient;
            
            StartCoroutine(PeriodicSparks());
        }
        
        IEnumerator PeriodicSparks()
        {
            while (enableHazardEffects && sparksEffect != null)
            {
                yield return new WaitForSeconds(Random.Range(2f, 4f));
                if (sparksEffect.gameObject.activeInHierarchy)
                {
                    sparksEffect.Play();
                }
            }
        }
        
        void SetupUI()
        {
            // Create UI Canvas if it doesn't exist
            if (uiCanvas == null)
            {
                GameObject canvasObj = new GameObject("ParticleSystemsUI");
                uiCanvas = canvasObj.AddComponent<Canvas>();
                uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
                canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            }
            
            // Create instructions text
            if (instructionsText == null)
            {
                GameObject instructionsObj = new GameObject("InstructionsText");
                instructionsObj.transform.SetParent(uiCanvas.transform, false);
                instructionsText = instructionsObj.AddComponent<Text>();
                instructionsText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                instructionsText.fontSize = 14;
                instructionsText.color = Color.white;
                
                RectTransform instructionsRect = instructionsText.GetComponent<RectTransform>();
                instructionsRect.anchorMin = new Vector2(0, 1);
                instructionsRect.anchorMax = new Vector2(0, 1);
                instructionsRect.pivot = new Vector2(0, 1);
                instructionsRect.anchoredPosition = new Vector2(10, -10);
                instructionsRect.sizeDelta = new Vector2(400, 150);
                
                instructionsText.text = "PARTICLE SYSTEMS DEMO\n\n" +
                                       "Left Click: Interactive Burst\n" +
                                       "Right Click: Collection Burst\n" +
                                       "Space: Toggle Weather\n" +
                                       "H: Toggle Hazard Effects\n" +
                                       "Effects auto-cycle every 5 seconds";
            }
            
            // Create particle count text
            if (particleCountText == null)
            {
                GameObject countObj = new GameObject("ParticleCountText");
                countObj.transform.SetParent(uiCanvas.transform, false);
                particleCountText = countObj.AddComponent<Text>();
                particleCountText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                particleCountText.fontSize = 12;
                particleCountText.color = Color.yellow;
                
                RectTransform countRect = particleCountText.GetComponent<RectTransform>();
                countRect.anchorMin = new Vector2(1, 1);
                countRect.anchorMax = new Vector2(1, 1);
                countRect.pivot = new Vector2(1, 1);
                countRect.anchoredPosition = new Vector2(-10, -10);
                countRect.sizeDelta = new Vector2(200, 50);
            }
        }
        
        void SetupCamera()
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                mainCamera = FindObjectOfType<Camera>();
            }
        }
        
        void HandleInput()
        {
            // Left click for interactive burst
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                TriggerClickBurst(mousePos);
            }
            
            // Right click for collection burst
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                TriggerCollectionBurst(mousePos);
            }
            
            // Space to toggle environmental effects
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ToggleEnvironmentalEffects();
            }
            
            // H to toggle hazard effects
            if (Input.GetKeyDown(KeyCode.H))
            {
                ToggleHazardEffects();
            }
            
            // R to trigger player feedback effects (simulated)
            if (Input.GetKeyDown(KeyCode.R))
            {
                SimulatePlayerFeedback();
            }
        }
        
        void UpdateEffectSwitching()
        {
            if (Time.time >= nextEffectSwitch && isDemoRunning)
            {
                CycleEnvironmentalEffects();
                nextEffectSwitch = Time.time + effectSwitchInterval;
            }
        }
        
        void UpdateUI()
        {
            if (particleCountText != null)
            {
                int totalParticles = GetTotalActiveParticles();
                particleCountText.text = $"Active Particles: {totalParticles}";
            }
        }
        
        void UpdateParticleEffects()
        {
            // Update movement trail effect position (simulate player movement)
            if (movementTrailEffect != null && movementTrailEffect.gameObject.activeInHierarchy)
            {
                Vector3 trailPos = new Vector3(
                    Mathf.Sin(Time.time * 2f) * 3f,
                    Mathf.Sin(Time.time * 1.5f) * 2f,
                    0
                );
                movementTrailEffect.transform.position = trailPos;
            }
            
            // Change floating particle colors periodically for visual variety
            if (Time.time - lastColorChangeTime > 3f && floatingParticles != null && floatingParticles.gameObject.activeInHierarchy)
            {
                var main = floatingParticles.main;
                Color magicalColor = GetRandomColor(0.6f);
                magicalColor = Color.Lerp(magicalColor, Color.white, 0.3f);
                main.startColor = magicalColor;
                lastColorChangeTime = Time.time;
            }
        }
        
        public void TriggerClickBurst(Vector3 position)
        {
            if (clickBurstEffect != null)
            {
                // Set a new random color each time
                var main = clickBurstEffect.main;
                main.startColor = GetRandomColor(0.9f);
                
                clickBurstEffect.transform.position = position;
                clickBurstEffect.gameObject.SetActive(true);
                clickBurstEffect.Play();
                StartCoroutine(DeactivateAfterDuration(clickBurstEffect.gameObject, 2f));
            }
        }
        
        public void TriggerCollectionBurst(Vector3 position)
        {
            if (collectionBurstEffect != null)
            {
                // Set a new random color each time
                var main = collectionBurstEffect.main;
                main.startColor = GetRandomColor(1f);
                
                collectionBurstEffect.transform.position = position;
                collectionBurstEffect.gameObject.SetActive(true);
                collectionBurstEffect.Play();
                StartCoroutine(DeactivateAfterDuration(collectionBurstEffect.gameObject, 3f));
            }
        }
        
        public void SimulatePlayerFeedback()
        {
            // Simulate jump dust
            if (jumpDustEffect != null)
            {
                // Set a new random earthy color each time
                var main = jumpDustEffect.main;
                Color earthyColor = GetRandomColor(0.8f);
                // Modify to be more dust-like (brown/tan tones)
                earthyColor = Color.Lerp(earthyColor, new Color(0.8f, 0.7f, 0.5f), 0.5f);
                main.startColor = earthyColor;
                
                jumpDustEffect.transform.position = new Vector3(Random.Range(-2f, 2f), -3f, 0);
                jumpDustEffect.gameObject.SetActive(true);
                jumpDustEffect.Play();
                StartCoroutine(DeactivateAfterDuration(jumpDustEffect.gameObject, 1f));
            }
            
            // Simulate landing effect shortly after
            StartCoroutine(SimulateLandingEffect());
            
            // Toggle movement trail
            if (movementTrailEffect != null)
            {
                movementTrailEffect.gameObject.SetActive(!movementTrailEffect.gameObject.activeInHierarchy);
            }
        }
        
        IEnumerator SimulateLandingEffect()
        {
            yield return new WaitForSeconds(0.8f);
            
            if (landingEffect != null)
            {
                // Set a new random earthy color each time
                var main = landingEffect.main;
                Color landingColor = GetRandomColor(0.9f);
                // Modify to be more dust-like (brown/tan tones)
                landingColor = Color.Lerp(landingColor, new Color(0.7f, 0.6f, 0.4f), 0.6f);
                main.startColor = landingColor;
                
                landingEffect.transform.position = new Vector3(Random.Range(-2f, 2f), -3f, 0);
                landingEffect.gameObject.SetActive(true);
                landingEffect.Play();
                StartCoroutine(DeactivateAfterDuration(landingEffect.gameObject, 2f));
            }
        }
        
        public void ToggleEnvironmentalEffects()
        {
            currentEnvironmentalEffect = (currentEnvironmentalEffect + 1) % 3;
            CycleEnvironmentalEffects();
        }
        
        void CycleEnvironmentalEffects()
        {
            // Disable all environmental effects first
            if (rainEffect != null) rainEffect.gameObject.SetActive(false);
            if (snowEffect != null) snowEffect.gameObject.SetActive(false);
            if (floatingParticles != null) floatingParticles.gameObject.SetActive(false);
            
            // Enable the current effect
            switch (currentEnvironmentalEffect)
            {
                case 0:
                    if (rainEffect != null) rainEffect.gameObject.SetActive(true);
                    Debug.Log("Environmental Effect: Rain");
                    break;
                case 1:
                    if (snowEffect != null) snowEffect.gameObject.SetActive(true);
                    Debug.Log("Environmental Effect: Snow");
                    break;
                case 2:
                    if (floatingParticles != null) floatingParticles.gameObject.SetActive(true);
                    Debug.Log("Environmental Effect: Floating Particles");
                    break;
            }
        }
        
        public void ToggleHazardEffects()
        {
            enableHazardEffects = !enableHazardEffects;
            
            if (fireEffect != null) fireEffect.gameObject.SetActive(enableHazardEffects);
            if (smokeEffect != null) smokeEffect.gameObject.SetActive(enableHazardEffects);
            if (sparksEffect != null) sparksEffect.gameObject.SetActive(enableHazardEffects);
            
            Debug.Log($"Hazard Effects: {(enableHazardEffects ? "Enabled" : "Disabled")}");
        }
        
        int GetTotalActiveParticles()
        {
            int total = 0;
            ParticleSystem[] allParticleSystems = FindObjectsOfType<ParticleSystem>();
            
            foreach (ParticleSystem ps in allParticleSystems)
            {
                if (ps.gameObject.activeInHierarchy)
                {
                    total += ps.particleCount;
                }
            }
            
            return total;
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
        
        void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
} 