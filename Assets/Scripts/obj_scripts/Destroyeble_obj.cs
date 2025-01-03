using UnityEngine;
using UnityEngine.Events;

public class DestroyebleObject : ICanHit
{
    [Header("Durability Settings")]
    [SerializeField] private float maxHits = 3;
    [SerializeField] private float currentHits = 0;

    [Header("Destruction Effects")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip destroySound;
    [SerializeField] private AudioSource audioSource;

    [Header("Visual Feedback")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float colorFadeTime = 0.2f;

    [Header("Destruction Events")]
    [SerializeField] private UnityEvent onDestroyed;
    [SerializeField] private UnityEvent<float> onHit;

    private Color originalColor;

    private void Start()
    {
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public override void TakeHit(float damage, Element element)
    {
        // «б≥льшенн€ л≥чильника пошкоджень
        currentHits += damage;

        onHit?.Invoke(currentHits);

        PlayHitSound();
        ShowHitFeedback();

        if (currentHits >= maxHits)
        {
            DestroyObject();
        }
    }

    public override bool IsDestroyed()
    {
        return currentHits >= maxHits;
    }

    protected override void DestroyObject()
    {
        if (audioSource != null && destroySound != null)
        {
            audioSource.PlayOneShot(destroySound);
        }

        onDestroyed?.Invoke();
        Destroy(gameObject);
    }

    private void PlayHitSound()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    private void ShowHitFeedback()
    {
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashColor());
        }
    }

    private System.Collections.IEnumerator FlashColor()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(colorFadeTime);
        spriteRenderer.color = originalColor;
    }
}
