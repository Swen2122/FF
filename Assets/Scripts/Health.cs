using UnityEngine.UI;
using UnityEngine;
public class Health : MonoBehaviour, ICanHit
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip death_audio;
    [SerializeField] private AudioClip hit_audio;
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

    public void TakeHit(float damage, Element elementType)
    {
        // ќбчисленн€ шкоди з урахуванн€м резист≥в
        float finalDamage = CalculateDamage(damage, elementType);
        currentHP -= finalDamage;

        if (hit_audio != null)
            audioSource.PlayOneShot(hit_audio);

        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHPBar();

        Debug.Log($"Current HP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private float CalculateDamage(float baseDamage, Element elementType)
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

    private void UpdateHPBar()
    {
        if (HP_bar != null)
        {
            HP_bar.fillAmount = currentHP / maxHP;
        }
    }

    private void Die()
    {
        if (death_audio != null)
            audioSource.PlayOneShot(death_audio);

        Destroy(gameObject);
    }
}