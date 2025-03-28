using UnityEngine;

public abstract class BaseSkills : MonoBehaviour
{
    [Header("Base Skill Settings")]
    [SerializeField] protected float cooldown;
    [SerializeField] protected AudioClip skillSound;
    [SerializeField] protected string animationTrigger;
    [SerializeField] protected Rigidbody2D rb;
    protected float lastUseTime;
    protected Animator animator;
    protected AudioSource audioSource;
    [SerializeField] protected float maxPich = 1.15f;
    [SerializeField] protected float minPich = 0.8f;
    protected Camera mainCamera;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        mainCamera = Camera.main;
    }

    public bool CanUseSkill()
    {
        return Time.time >= lastUseTime + cooldown;
    }
    public virtual void TryUseSkill()
    {
        if (!CanUseSkill()) return;

        lastUseTime = Time.time;
        UseSkill();
        PlaySkillEffects();
    }
    protected abstract void UseSkill();

    protected virtual void PlaySkillEffects()
    {
        if (skillSound != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(minPich, maxPich);
            audioSource.PlayOneShot(skillSound);
        }

        if (animator != null && !string.IsNullOrEmpty(animationTrigger))
        {
            animator.SetTrigger(animationTrigger);
        }
    }
    protected virtual void ApplyRecoil(Vector2 punchDirectoin, float forse, bool isForward)
    {
        rb = GetComponentInParent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 recoilDirectoin = punchDirectoin.normalized;
            if (!isForward) forse *= -1;
            rb.AddForce(-recoilDirectoin * forse, ForceMode2D.Impulse);
        }
    }
}
