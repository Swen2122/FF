using UnityEngine;
using System.Collections;
public class PlayerTracker : MonoBehaviour
{
    public static PlayerTracker Instance { get; private set; }

    public bool IsDashing { get; private set; } 
    public bool IsUseSkill {get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetDash()
    {
        IsDashing = true;
        Debug.Log("Dashing true");
        StartCoroutine(DashCooldown(0.3f));
    }

    private IEnumerator DashCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        Debug.Log("Dashing false");
        IsDashing = false;
    }
    public void SetUseSkill() => StartCoroutine(UseSkillCooldown(0.3f));
    private IEnumerator UseSkillCooldown(float duration)
    {
        IsUseSkill = true;
        yield return new WaitForSeconds(duration);
        IsUseSkill = false;
    }
}
