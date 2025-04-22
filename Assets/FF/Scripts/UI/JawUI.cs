using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
public class JawUI : MonoBehaviour
{
    public Image jawTop;
    public Image jawBottom;
    public Vector3 jawTopStartPos;
    public Vector3 jawBottomStartPos;
    public Ease easeType = Ease.OutBack;
    public float moveAmount = 20f;
    public float animDuration = 0.3f;
    private Transform target;
    private ObjectPool<JawUI> pool;

    private void Awake()
    {
        jawTopStartPos = jawTop.transform.localPosition;
        jawBottomStartPos = jawBottom.transform.localPosition;
    }
    public void SetTarget(Transform target, ObjectPool<JawUI> pool)
    {
        this.target = target;
        this.pool = pool;
        StartAnim();
    }
    private void LateUpdate()
    {
        if (target != null && target.GetComponent<Health>().healthState == HealthState.corpse)
        
        {
            transform.position = target.position + Vector3.up;
        }else pool.Release(this); 
    }
    public void StartAnim()
    {
        StartCoroutine(JawLoop(1.0f)); 
    }

    private IEnumerator JawLoop(float intervalSeconds)
    {
        while (true)
        {
            yield return JawSequence();
            yield return new WaitForSeconds(intervalSeconds);
        }
    }

    public void OpenJaw()
    {
        jawTop.transform.DOLocalMoveY(jawTopStartPos.y + moveAmount, animDuration).SetEase(easeType);
        jawBottom.transform.DOLocalMoveY(jawBottomStartPos.y - moveAmount, animDuration).SetEase(easeType);
    }

    public void CloseJaw()
    {
        jawTop.transform.DOLocalMoveY(jawTopStartPos.y, animDuration).SetEase(easeType);
        jawBottom.transform.DOLocalMoveY(jawBottomStartPos.y, animDuration).SetEase(easeType);
    }
    public IEnumerator JawSequence()
    {
        OpenJaw();
        yield return new WaitForSeconds(0.5f);
        CloseJaw();
    }
}
