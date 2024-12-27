using UnityEngine;
using UnityEngine.Events;

public class Destroyeble_obj : MonoBehaviour, ICanHit
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
    [SerializeField] private float pressure;
    [SerializeField] private float amount;
    [SerializeField] private int maxstep;
    [SerializeField] private UnityEvent onDestroyed;
    [SerializeField] private UnityEvent<float> onHit;
    private Color originalColor;

    private void Start()
    {
        // ���������� ������������ �������
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeHit(float damage)
    {
        damage = 1;
        // ��������� ��������� ����������
        currentHits += damage;

        // ������ ��䳿 ��� ����������
        onHit?.Invoke(currentHits);

        // �������� �����
        PlayHitSound();

        // ³�������� ����� �����������
        ShowHitFeedback();

        // �������� ����������
        if (currentHits >= maxHits)
        {
            Destroy();
        }
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
            // �������� ��������
            StartCoroutine(FlashColor());
        }
    }

    private System.Collections.IEnumerator FlashColor()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(colorFadeTime);
        spriteRenderer.color = originalColor;
    }

    private void Destroy()
    {
        // ���� ����������
        if (audioSource != null && destroySound != null)
        {
            audioSource.PlayOneShot(destroySound);
        }

        // ������ ��䳿 ����������
        onDestroyed?.Invoke();
        WaterSpreadManager.Instance.SpreadWater(transform.position, amount, pressure, maxstep);
        // Գ����� �������� ��'����
        Destroy(gameObject);
    }

    // �������� ������ ��� ���������� ������
    public void ResetDamage()
    {
        currentHits = 0;
    }

    public float GetCurrentHits()
    {
        return currentHits;
    }

    public bool IsDestroyed()
    {
        return currentHits >= maxHits;
    }
}