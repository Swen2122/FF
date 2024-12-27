using UnityEngine;
using System.Collections.Generic;

public class VortexDamageZone : MonoBehaviour
{
    private ConvergenceSkillData settings;
    private LayerMask targetLayerMask;
    private Element currentElement;
    private float nextDamageTime;
    private AudioSource audioSource;
    private float lifetime;
    public void Initialize(ConvergenceSkillData data, LayerMask targetLayer, Element element)
    {
        settings = data;
        targetLayerMask = targetLayer;
        currentElement = element;
        lifetime = settings.vortexLifetime;
        SetupAudioAndEffects();
    }
    private void Update()
    {
        if (settings == null) return;

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
            return;
        }

        ApplyPullForce();
        if (Time.time >= nextDamageTime)
        {
            ApplyDamageInZone();
            nextDamageTime = Time.time + settings.damageInterval;
        }
    }
    private void SetupAudioAndEffects()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        if (settings.pullEffect != null)
        {
            var pullVFX = Instantiate(settings.pullEffect, transform);
            pullVFX.Play();
        }

        if (settings.pullSound != null)
        {
            audioSource.clip = settings.pullSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    private void ApplyPullForce()
    {
        Collider2D[] objectsToPull = Physics2D.OverlapCircleAll(transform.position,
            settings.pullRadius, targetLayerMask);

        foreach (var obj in objectsToPull)
        {
            if (obj.TryGetComponent<Rigidbody2D>(out var rb))
            {
                Vector2 pullDirection = (transform.position - obj.transform.position).normalized;
                rb.AddForce(pullDirection * settings.pullForce * Time.fixedDeltaTime,
                    ForceMode2D.Force);
            }
        }
    }

    private void ApplyDamageInZone()
    {
        Collider2D[] enemiesInZone = Physics2D.OverlapCircleAll(transform.position,
            settings.damageRadius, targetLayerMask);

        List<GameObject> enemyObjects = new List<GameObject>();

        foreach (var enemy in enemiesInZone)
        {
            enemyObjects.Add(enemy.gameObject);
        }

        if (enemyObjects.Count > 0)
        {
            HitStop.TriggerStop(0.05f, 0.0f);
            ApplyElementalDamage(enemyObjects.ToArray(), settings.damageAmount);
        }
    }

    private void ApplyElementalDamage(GameObject[] targets, int damage)
    {
        switch (currentElement)
        {
            case Element.Water:
                Damage.Water(targets, damage);
                break;
            case Element.Fire:
                Damage.Fire(targets, damage);
                break;
            case Element.Earth:
                Damage.Earth(targets, damage);
                break;
            case Element.Wind:
                Damage.Wind(targets, damage);
                break;
            case Element.Electro:
                Damage.Wind(targets, damage);
                break;
            case Element.Ice:
                Damage.Wind(targets, damage);
                break;
        }
    }
    private void OnDestroy()
    {
        if (settings.pullEffect != null)
        {
            var pullVFX = GetComponentInChildren<ParticleSystem>();
            if (pullVFX != null) pullVFX.Stop();
        }

        if (settings.destroySound != null && audioSource != null)
        {
            audioSource.loop = false;
            audioSource.PlayOneShot(settings.destroySound);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (settings == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, settings.damageRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, settings.pullRadius);
    }
}