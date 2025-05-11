using UnityEngine;
using System.Collections.Generic;
public class ComboPunchs : PunchBase
{
    [SerializeField] private ComboDatabase combo;
    [SerializeField] private float comboResetTime = 1.5f;
    private ComboDatabase.Combo currentCombo;
    private int currentComboIndex;
    private int maxComboIndex;
    private float lastPunchTime;
    private Element currentElement;
    protected override void PerformPunch()
    {
        currentCombo = combo.punchComboList.Find(x => x.damageType == element.currentElement);
        if (currentCombo != null)
        {
            maxComboIndex = currentCombo.punchCombo.ComboSegments.Count;
        }
        if (currentCombo == null || currentComboIndex >= maxComboIndex) return;

        var currentSegment = currentCombo.punchCombo.ComboSegments[currentComboIndex];
        cooldown = currentCombo.comboDelay;
        damage = currentSegment.damage;
        skillSound = currentSegment.hitSound;
        knockback = currentSegment.knockback;
        elementCombo = currentCombo.damageType;
        if (currentSegment.animation != null)
        {
            AddClipToAnimator(currentSegment.animation);
            PlayClip(currentSegment.animation);
        }

        base.PerformPunch();

        lastPunchTime = Time.time;
        currentComboIndex = (currentComboIndex + 1) % maxComboIndex;
    }
    public override void TryUseSkill()
    {
        if (Time.time - lastPunchTime > comboResetTime)
        {
            currentComboIndex = 0;
        }
        base.TryUseSkill();
    }
}
