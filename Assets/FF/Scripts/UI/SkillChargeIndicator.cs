using UnityEngine;
using UnityEngine.UI;
public class SkillChargeIndicator : MonoBehaviour
{
    [SerializeField] private Image chargeImage;
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, 0f);
    private Transform followTarget;
    private RectTransform rectTransform;
    private Camera mainCamera;
    private Canvas canvas;
    private RectTransform canvasRect;
    private void Awake()
    {
        followTarget = PlayerUtility.PlayerTransform;
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }
    private void LateUpdate()
    {
        if (followTarget == null) return;

        Vector3 targetPosition = followTarget.position + offset;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);

        if (screenPosition.z < 0)
        {
            gameObject.SetActive(false);
            return;
        }

        // Конвертуємо screenPosition в локальні координати Canvas
        Vector2 viewportPosition = mainCamera.WorldToViewportPoint(targetPosition);
        Vector2 worldObjectScreenPosition = new Vector2(
            ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

        rectTransform.anchoredPosition = worldObjectScreenPosition;
    }
    public void ShowIndicator(bool show)
    {
        gameObject.SetActive(show);
    }
    public void UpdateCharge(float chargePrecent)
    {
        chargeImage.fillAmount = chargePrecent;
    }
    public void SetTarget(Transform target)
    {
        followTarget = target;
    }
}
