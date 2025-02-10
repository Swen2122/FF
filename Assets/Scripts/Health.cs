using UnityEngine.UI;
using UnityEngine;
public class Health : ICanHit
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private HealthStat healthStat;
    public float maxHP = 100;
    public float currentHP;
    public Image HP_bar;
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red;
    public float colorFadeTime = 0.2f;
    private Color originalColor;

    private void Start()
    {
        if (healthStat != null)
        {
            maxHP = healthStat.maxHealth;
        }
        if(spriteRenderer)originalColor = spriteRenderer.color;
        currentHP = maxHP;
        UpdateHPBar();
    }

    public override void TakeHit(float damage, Element elementType)
    {
        ShowHitFeedback();
        float finalDamage = CalculateDamage(damage, elementType);
        currentHP -= finalDamage;

        if (healthStat != null && healthStat.hit_audio != null && audioSource != null)
        {
            audioSource.PlayOneShot(healthStat.hit_audio);
        }

        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHPBar();

        if (currentHP <= 0)
        {
            DestroyObject();
        }
    }


    public float CalculateDamage(float baseDamage, Element elementType)
    {
        if (healthStat == null)
            return baseDamage;

        var resist = healthStat.resistStat.Find(r => r.elementType == elementType);
        float resistance = resist != null ? resist.resistance : 1f; 

        float finalDamage = baseDamage * (resistance);

        Debug.Log($"Base Damage: {baseDamage}, Resistance: {resistance}, Final Damage: {finalDamage}");
        return finalDamage; 
    }
    public override bool IsDestroyed()
    {
        return currentHP <= 0;
    }
    protected override void DestroyObject()
    {
        if (healthStat != null && healthStat.deathAudio != null && audioSource != null)
        {
            audioSource.PlayOneShot(healthStat.deathAudio);
        }
        Destroy(gameObject);
    }
    private void UpdateHPBar()
    {
        if (HP_bar != null)
        {
            HP_bar.fillAmount = currentHP / maxHP;
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