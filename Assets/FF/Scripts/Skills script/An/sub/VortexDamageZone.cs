using UnityEngine;
using UnityEngine.VFX;
public class VortexDamageZone : AbstractReactionEffect
{
    [SerializeField] private VortexSettings vortexSettings;
    private LayerMask targetLayerMask;
    private AudioSource audioSource;
    private ParticleSystem pullVFX;
    public VisualEffect vfx;
    private float pullTimer;

    public void Initialize(ElementalReaction.ReactionEffect settings, LayerMask targetLayer)
    {
        base.Initialize(settings);
        if (settings == null || vortexSettings == null)
        {
            Debug.LogError("Invalid settings or vortexSettings in VortexDamageZone.");
            return;
        }
        vfx.SetFloat("Radius", settings.radius);
        targetLayerMask = targetLayer;
        SetupAudioAndEffects();
    }

    protected override void Update()
    {
        base.Update();
        if (currentEnergy <= 0)
        {
            return;
        }
        pullTimer -= Time.deltaTime;
        if (pullTimer <= 0f)
        {
            float energyRatio = currentEnergy / maxEnergy;
            ApplyPullForce(energyRatio);
            pullTimer = vortexSettings.pullInterval; // ������� ������
        }
    }

    private void ApplyPullForce(float energyRatio)
    {
        float currentPullRadius = settings.radius;
        float currentPullForce = vortexSettings.pullForce;

        Collider2D[] objectsToPull = Physics2D.OverlapCircleAll(transform.position, currentPullRadius, targetLayerMask);

        foreach (var obj in objectsToPull)
        {
            if (obj.TryGetComponent<Rigidbody2D>(out var rb))
            {
                Vector2 pullDirection = (transform.position - obj.transform.position).normalized;
                rb.AddForce(pullDirection * currentPullForce * Time.fixedDeltaTime, ForceMode2D.Force);
            }

            CheckElementalInteraction(obj);
        }
    }
    private void SetupAudioAndEffects()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        if (settings.particles != null)
        {
            pullVFX = Instantiate(settings.particles, transform);
            pullVFX.Play();
        }

        if (settings.sound != null)
        {
            audioSource.clip = settings.sound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    protected override void OnEnergyTick()
    {
        // ������������ ����� ���� � ������ ��� ����㳿
        float energyRatio = currentEnergy / maxEnergy;
        ApplyDamageInZone(energyRatio);
    }

    private void ApplyDamageInZone(float energyRatio)
    {
        float currentRadius = settings.radius/2;
        float currentDamage = settings.damage * energyRatio;

        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, currentRadius, targetLayerMask);

        foreach (var target in targets)
        {
            if (target.TryGetComponent<ICanHit>(out var canHit))
            {
                canHit.TakeHit(currentDamage, Element.Wind);
            }
        }
    }

    protected override void OnReactionEnd()
    {
        // ���������� ��������
        if (pullVFX != null)
        {
            pullVFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        Destroy(gameObject);
    }

    protected override void OnReactionDisrupted()
    {
        // ������� � ���������, ���� ������� ��������
        if (pullVFX != null)
        {
            pullVFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        Destroy(gameObject);
    }
}
