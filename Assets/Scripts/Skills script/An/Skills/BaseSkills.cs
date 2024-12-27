using UnityEngine;

public abstract class BaseSkills : MonoBehaviour
{
    [Header("Base Skill Settings")]
    [SerializeField] protected float cooldown;
    [SerializeField] protected AudioClip skillSound;
    [SerializeField] protected string animationTrigger;

    protected float lastUseTime;
    protected Animator animator;
    protected AudioSource audioSource;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Перевірка кулдауна
    protected bool CanUseSkill()
    {
        return Time.time >= lastUseTime + cooldown;
    }

    // Основний метод активації скіла
    public virtual void TryUseSkill()
    {
        if (!CanUseSkill()) return;

        lastUseTime = Time.time;
        UseSkill();
        PlaySkillEffects();
    }

    // Абстрактний метод для реалізації логіки скіла
    protected abstract void UseSkill();

    // Ефекти скіла (звук, анімація)
    protected virtual void PlaySkillEffects()
    {
        if (skillSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(skillSound);
        }

        if (animator != null && !string.IsNullOrEmpty(animationTrigger))
        {
            animator.SetTrigger(animationTrigger);
        }
    }
}
