using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
public class Health : ICanHit
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private HealthStat healthStat;
    public float maxHP = 100;
    public HealthState healthState = HealthState.alive;
    public float currentHP;
    public Image HP_bar;
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red;
    public float colorFadeTime = 0.2f;
    private Color originalColor;
    private bool LateHPbar = false;
    [SerializeField] private UnityEvent onDeath;

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
    public override void AddInternalEnergy(float energy, Element elementType)
    {
      base.AddInternalEnergy(energy, elementType);
    }
    public override float GetEnergy(Element element)
    {
        return base.GetEnergy(element);
    }
    public override void TakeHit(float damage, Element elementType)
    {
        AddEnergy(damage, elementType);
        CheckReactions();
        ShowHitFeedback();
        float finalDamage = CalculateDamage(damage, elementType);
        currentHP -= finalDamage;

        if (healthStat != null && healthStat.hit_audio != null && audioSource != null)
        {
            audioSource.PlayOneShot(healthStat.hit_audio);
        }
        if(currentHP < 25 && currentHP > 0)
        {
            healthState = HealthState.crit;
        }
        else healthState = HealthState.alive;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHPBar();

        if (currentHP <= 0)
        {
            DestroyObject();
        }
    }
    private void Update()
    {
        if(healthState == HealthState.corpse)WorldUI.Instance.TryShowJaw(transform, 10f);
        if(HP_bar == null)
        {
            LateHPbar = true;
            WorldUI.Instance.TryShowHealthBar(transform, 5f);
        }
        if (HP_bar != null && LateHPbar == true)
        {
            HP_bar.transform.position = transform.position + new Vector3(0, 0.75f, 0);
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
        onDeath?.Invoke();
        if (healthStat != null && healthStat.deathAudio != null && audioSource != null)
        {
            audioSource.PlayOneShot(healthStat.deathAudio);
        }
        healthState = HealthState.corpse;
        if(gameObject.TryGetComponent<BaseEnemyAI>(out BaseEnemyAI enemyAI))
        {
            enemyAI.DisableAI();
        }
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
        Destroy(gameObject, 10f);
    }
    private void UpdateHPBar()
    {
        if (HP_bar != null)
        {
            HP_bar.DOFillAmount(currentHP / maxHP, 0.2f).SetEase(Ease.InOutSine);
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

public enum HealthState{ corpse, alive, crit}