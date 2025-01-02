using UnityEngine;

public class VortexDamageZone : AbstractReactionEffect
{
    [SerializeField] private VortexSettings vortexSettings;
    private LayerMask targetLayerMask;
    private AudioSource audioSource;
    private ParticleSystem pullVFX;

    private float pullTimer; // Таймер для контролю частоти затягування

    public void Initialize(ElementalReaction.ReactionEffect settings, LayerMask targetLayer)
    {
        base.Initialize(settings);

        if (settings == null || vortexSettings == null)
        {
            Debug.LogError("Invalid settings or vortexSettings in VortexDamageZone.");
            return;
        }

        targetLayerMask = targetLayer;

        // Ініціалізація аудіо і візуальних ефектів
        SetupAudioAndEffects();
    }

    protected override void Update()
    {
        base.Update(); // Виконуємо загальні оновлення (енергія, тік тощо)

        if (currentEnergy <= 0)
        {
            return; // Вортекс завершено, нічого не робимо
        }

        // Оновлення таймера затягування
        pullTimer -= Time.deltaTime;
        if (pullTimer <= 0f)
        {
            float energyRatio = currentEnergy / maxEnergy;
            ApplyPullForce(energyRatio);
            pullTimer = vortexSettings.pullInterval; // Скидаємо таймер
        }
    }

    private void ApplyPullForce(float energyRatio)
    {
        float currentPullRadius = vortexSettings.pullRadius * energyRatio;
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
        // Застосування шкоди лише в момент тіку енергії
        float energyRatio = currentEnergy / maxEnergy;
        ApplyDamageInZone(energyRatio);
    }

    private void ApplyDamageInZone(float energyRatio)
    {
        float currentRadius = settings.radius * energyRatio;
        float currentDamage = settings.damage * energyRatio;

        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, currentRadius, targetLayerMask);

        foreach (var target in targets)
        {
            if (target.TryGetComponent<ICanHit>(out var canHit))
            {
                canHit.TakeHit(currentDamage);
            }
        }
    }

    protected override void OnReactionEnd()
    {
        // Завершення вортекса
        if (pullVFX != null)
        {
            pullVFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        Destroy(gameObject);
    }

    protected override void OnReactionDisrupted()
    {
        // Зупинка і видалення, якщо вортекс зупинено
        if (pullVFX != null)
        {
            pullVFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        Destroy(gameObject);
    }
}
