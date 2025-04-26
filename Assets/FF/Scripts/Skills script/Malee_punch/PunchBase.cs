using UnityEngine;
using System.Collections.Generic;
public abstract class PunchBase : BaseSkills
{
    [SerializeField] protected float damage;
    [SerializeField] protected float knockback;
    [SerializeField] protected Collider2D hitBox;
    [SerializeField] protected Element element;
    [SerializeField] protected LayerMask targetLayer;
    [Header("Hit Effects")]
    [SerializeField] protected Animator anim;
    [SerializeField] protected float hitStopDuration = 0.05f;
    [SerializeField] protected float hitStopScale = 0.0f;
    [SerializeField] protected bool isForward;
    protected HashSet<GameObject> _hitEnemies = new HashSet<GameObject>();
    [SerializeField] protected AnimationClip clip;
    protected override void UseSkill()
    {
        if (hitBox == null) return;
        PerformPunch();
    }
    protected virtual void PerformPunch()
    {
        _hitEnemies.Clear();
        _hitEnemies = FindEnemies();
        Vector2 punchDirection = (transform.position - mainCamera.ScreenToWorldPoint(Input.mousePosition)).normalized;
        ApplyRecoil(punchDirection, 10f, isForward);
        if (_hitEnemies.Count == 0) return;
        ApplyHitEffects();
        ApplyDamageToEnemies();
        ApplyKnockbackToEnemies();
        OnPunchComplete();
    }

    protected virtual HashSet<GameObject> FindEnemies()
    {
        var enemies = new HashSet<GameObject>();
        var hitTargets = Physics2D.OverlapBoxAll(hitBox.bounds.center, hitBox.bounds.size, 0f);

        foreach (var hitTarget in hitTargets)
        {
            if (((1 << hitTarget.gameObject.layer) & targetLayer) != 0)
            {
                var canHit = hitTarget.GetComponent<ICanHit>();
                var reactionItem = hitTarget.GetComponent<ReactionItem>();
                if(reactionItem)reactionItem.StartReaction(element, hitTarget.gameObject, hitBox.bounds.center);
                if (canHit) enemies.Add(hitTarget.gameObject);
            }
        }

        return enemies;
    }
    protected void PlayClip(AnimationClip clip)
    {
        anim.Play(clip.name, - 1, 0f);
    }
    protected virtual void ApplyHitEffects()
    {
        HitStop.TriggerStop(hitStopDuration, hitStopScale);
    }

    protected virtual void ApplyDamageToEnemies()
    {
        Damage.ApplyDamage(new List<GameObject>(_hitEnemies).ToArray(), damage, element);
    }

    protected virtual void ApplyKnockbackToEnemies()
    {
        foreach (var enemy in _hitEnemies)
        {
            if (enemy.TryGetComponent<Rigidbody2D>(out var enemyRb) && enemyRb.bodyType != RigidbodyType2D.Static)
            {
                PushUtility.Push(enemyRb, transform.position, knockback);
            }
        }
    }

    protected virtual void OnPunchComplete()
    {
        // Hook ��� ��������� ����� ���� �����
    }
    protected void AddClipToAnimator(AnimationClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("Clip is null!");
            return;
        }

        // ������ �������� �� Animator Controller (��������)
        var controller = anim.runtimeAnimatorController as AnimatorOverrideController;
        if (controller == null)
        {
            controller = new AnimatorOverrideController(anim.runtimeAnimatorController);
            anim.runtimeAnimatorController = controller;
        }

        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(controller.overridesCount);
        controller.GetOverrides(overrides);

        // ������������ ��� ������ ���� ��������
        for (int i = 0; i < overrides.Count; i++)
        {
            if (overrides[i].Key.name == clip.name)
            {
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, clip);
                break;
            }
        }

        controller.ApplyOverrides(overrides);
    }
}