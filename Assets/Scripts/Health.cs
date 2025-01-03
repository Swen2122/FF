using UnityEngine.UI;
using UnityEngine;
public class Health : ICanHit
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private HealthStat healthStat;
    public float maxHP = 100;
    public float currentHP;
    public Image HP_bar;

    private void Start()
    {
        if (healthStat != null)
        {
            maxHP = healthStat.maxHealth;
        }
        currentHP = maxHP;
        UpdateHPBar();
    }

    public override void TakeHit(float damage, Element elementType)
    {
        //обчисленн€ шкоди з урахуванн€м резист≥в
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
        float resistance = resist != null ? resist.resistance : 1f; // якщо нема резисту, то звичайний урон

        // ‘ормула: Ўкода * (1 - –езист)
        float finalDamage = baseDamage * (resistance);

        Debug.Log($"Base Damage: {baseDamage}, Resistance: {resistance}, Final Damage: {finalDamage}");
        return finalDamage; // Ўкода не може бути меншою за 0
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
}