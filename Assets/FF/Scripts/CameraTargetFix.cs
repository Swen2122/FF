using UnityEngine;
using Unity.Cinemachine;
public class CameraTargetFix : MonoBehaviour
{
    private CinemachineCamera vcam;

    void Awake()
    {
        vcam = GetComponent<CinemachineCamera>();
        if (vcam == null)
        {
            Debug.LogError("CinemachineCamera component not found on this GameObject.");
            return;
        }
    }
    void OnEnable()
    {
        PlayerEvents.OnPlayerSpawned += OnPlayerSpawned;
    }
    void OnDisable()
    {
        PlayerEvents.OnPlayerSpawned -= OnPlayerSpawned;
    }
    private void OnPlayerSpawned(Transform playerTransform)
    {
        vcam.Follow = playerTransform;
        vcam.LookAt = playerTransform;
        Debug.Log("🎥 Камера підв’язана через подію");
    }
}
