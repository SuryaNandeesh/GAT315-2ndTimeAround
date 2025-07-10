using UnityEngine;
using System.Collections;

namespace UGame2D
{
    /// <summary>
    /// Advanced Particle Visual Enhancement System
    /// This script demonstrates professional techniques for creating visually stunning particle effects
    /// </summary>
    public class ParticleVisualEnhancer : MonoBehaviour
    {
        [Header("Visual Enhancement Settings")]
        [SerializeField] private bool enableAdvancedBlending = true;
        [SerializeField] private bool enableDistanceFading = true;
        [SerializeField] private bool enableLightIntegration = true;
        [SerializeField] private bool enableScreenSpaceEffects = true;
        
        [Header("Lighting Integration")]
        [SerializeField] private Light dynamicLight;
        [SerializeField] private Color lightColorMultiplier = Color.white;
        [SerializeField] private float lightIntensityMultiplier = 1f;
        
        void Start()
        {
            DemonstrateVisualTechniques();
        }
        
        /// <summary>
        /// Demonstrates professional particle effect visual techniques
        /// </summary>
        void DemonstrateVisualTechniques()
        {
            Debug.Log("=== PARTICLE VISUAL ENHANCEMENT GUIDE ===");
            Debug.Log("1. COLOR GRADIENTS: Use rich, multi-color gradients for depth");
            Debug.Log("2. SIZE CURVES: Animate size over lifetime for dynamic growth/shrink");
            Debug.Log("3. VELOCITY CURVES: Create natural motion with velocity over lifetime");
            Debug.Log("4. NOISE: Add subtle noise for organic, non-uniform motion");
            Debug.Log("5. SUB-EMITTERS: Create complex effects with particle-spawned particles");
            Debug.Log("6. SHAPE VARIETY: Use different emission shapes for unique patterns");
            Debug.Log("7. TEXTURE SHEETS: Animate sprite sequences for complex visuals");
            Debug.Log("8. LIGHTING: Integrate with Unity's lighting system");
            Debug.Log("9. BLENDING MODES: Use Additive/Alpha blending strategically");
            Debug.Log("10. SORTING & DEPTH: Layer effects properly for visual hierarchy");
        }
        
        /// <summary>
        /// Creates a professional-quality fire effect with multiple layers
        /// </summary>
        public ParticleSystem CreateAdvancedFireEffect()
        {
            GameObject fireObj = new GameObject("AdvancedFire");
            fireObj.transform.SetParent(transform);
            ParticleSystem fire = fireObj.AddComponent<ParticleSystem>();
            
            // Main fire particles
            var main = fire.main;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.5f, 1.5f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(2f, 5f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.3f, 0.8f);
            main.maxParticles = 100;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            // Rich color gradient for realistic fire
            var colorOverLifetime = fire.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient fireGradient = new Gradient();
            fireGradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(new Color(1f, 1f, 0.8f), 0.0f),    // Hot white
                    new GradientColorKey(new Color(1f, 0.6f, 0.1f), 0.3f),  // Orange
                    new GradientColorKey(new Color(1f, 0.2f, 0.1f), 0.7f),  // Red
                    new GradientColorKey(new Color(0.2f, 0.1f, 0.1f), 1.0f) // Dark red/black
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(0.8f, 0.0f), 
                    new GradientAlphaKey(1.0f, 0.2f),
                    new GradientAlphaKey(0.6f, 0.8f),
                    new GradientAlphaKey(0.0f, 1.0f) 
                }
            );
            colorOverLifetime.color = fireGradient;
            
            // Dynamic size changes
            var sizeOverLifetime = fire.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            AnimationCurve sizeCurve = new AnimationCurve();
            sizeCurve.AddKey(0f, 0.3f);
            sizeCurve.AddKey(0.2f, 1f);
            sizeCurve.AddKey(0.8f, 1.2f);
            sizeCurve.AddKey(1f, 0.1f);
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);
            
            // Upward velocity with turbulence
            var velocityOverLifetime = fire.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(1f, 3f);
            
            // Add noise for organic movement
            var noise = fire.noise;
            noise.enabled = true;
            noise.strength = 0.5f;
            noise.frequency = 0.8f;
            noise.damping = true;
            
            // Emission shape
            var shape = fire.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.2f;
            
            return fire;
        }
        
        /// <summary>
        /// Creates a magical sparkle effect with sub-emitters
        /// </summary>
        public ParticleSystem CreateMagicalSparkleEffect()
        {
            GameObject sparkleObj = new GameObject("MagicalSparkles");
            sparkleObj.transform.SetParent(transform);
            ParticleSystem sparkles = sparkleObj.AddComponent<ParticleSystem>();
            
            var main = sparkles.main;
            main.startLifetime = new ParticleSystem.MinMaxCurve(2f, 4f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(1f, 3f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.3f);
            main.maxParticles = 50;
            
            // Magical color scheme
            var colorOverLifetime = sparkles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient magicGradient = new Gradient();
            magicGradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(new Color(0.8f, 0.4f, 1f), 0.0f),  // Purple
                    new GradientColorKey(new Color(0.4f, 0.8f, 1f), 0.3f),  // Blue
                    new GradientColorKey(new Color(1f, 0.8f, 0.4f), 0.6f),  // Gold
                    new GradientColorKey(new Color(1f, 1f, 1f), 1.0f)       // White
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(0.0f, 0.0f), 
                    new GradientAlphaKey(1.0f, 0.3f),
                    new GradientAlphaKey(0.8f, 0.7f),
                    new GradientAlphaKey(0.0f, 1.0f) 
                }
            );
            colorOverLifetime.color = magicGradient;
            
            // Twinkling size effect
            var sizeOverLifetime = sparkles.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            AnimationCurve twinkleCurve = new AnimationCurve();
            twinkleCurve.AddKey(0f, 0f);
            twinkleCurve.AddKey(0.1f, 1f);
            twinkleCurve.AddKey(0.3f, 0.3f);
            twinkleCurve.AddKey(0.5f, 1f);
            twinkleCurve.AddKey(0.7f, 0.2f);
            twinkleCurve.AddKey(0.9f, 0.8f);
            twinkleCurve.AddKey(1f, 0f);
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, twinkleCurve);
            
            // Orbital motion
            var velocityOverLifetime = sparkles.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.orbitalX = new ParticleSystem.MinMaxCurve(0.5f);
            velocityOverLifetime.orbitalY = new ParticleSystem.MinMaxCurve(0.8f);
            
            return sparkles;
        }
        
        /// <summary>
        /// Creates a cinematic explosion with multiple layers
        /// </summary>
        public void CreateCinematicExplosion(Vector3 position)
        {
            // Core blast
            CreateExplosionLayer("CoreBlast", position, Color.white, 0.5f, 8f, 100);
            
            // Fire ring
            StartCoroutine(DelayedExplosionLayer("FireRing", position, new Color(1f, 0.4f, 0.1f), 0.2f, 6f, 80, 0.1f));
            
            // Smoke plume
            StartCoroutine(DelayedExplosionLayer("SmokePlume", position, new Color(0.3f, 0.3f, 0.3f), 2f, 3f, 60, 0.3f));
            
            // Debris shower
            StartCoroutine(DelayedExplosionLayer("Debris", position, new Color(0.6f, 0.4f, 0.2f), 1.5f, 10f, 40, 0.15f));
            
            // Add camera shake if available
            StartCoroutine(CameraShake(0.3f, 0.2f));
        }
        
        void CreateExplosionLayer(string name, Vector3 position, Color color, float lifetime, float speed, int particles)
        {
            GameObject layerObj = new GameObject($"Explosion_{name}");
            layerObj.transform.position = position;
            ParticleSystem layer = layerObj.AddComponent<ParticleSystem>();
            
            var main = layer.main;
            main.startLifetime = lifetime;
            main.startSpeed = speed;
            main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.4f);
            main.startColor = color;
            main.maxParticles = particles;
            
            var emission = layer.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, particles) });
            
            // Auto-destroy after effect
            Destroy(layerObj, lifetime + 1f);
        }
        
        IEnumerator DelayedExplosionLayer(string name, Vector3 position, Color color, float lifetime, float speed, int particles, float delay)
        {
            yield return new WaitForSeconds(delay);
            CreateExplosionLayer(name, position, color, lifetime, speed, particles);
        }
        
        IEnumerator CameraShake(float duration, float magnitude)
        {
            Camera cam = Camera.main;
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
        
        /// <summary>
        /// Professional tips for particle optimization
        /// </summary>
        public void OptimizationTips()
        {
            Debug.Log("=== PARTICLE OPTIMIZATION TIPS ===");
            Debug.Log("• Use Prewarm for looping effects");
            Debug.Log("• Limit Max Particles to reasonable numbers");
            Debug.Log("• Use object pooling for frequently spawned effects");
            Debug.Log("• Disable unnecessary modules to save performance");
            Debug.Log("• Use LOD (Level of Detail) for distance-based quality");
            Debug.Log("• Consider Mesh particles for complex shapes");
            Debug.Log("• Use GPU-friendly texture atlases");
            Debug.Log("• Profile effects with Unity Profiler");
        }
    }
} 