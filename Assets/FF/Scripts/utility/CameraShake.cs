using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;
    public static CameraShake Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (impulseSource == null)
            impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void Shake(Vector3 direction, float intensity = 0.3f)
    {
        var dir = direction.normalized * -1;
        impulseSource.GenerateImpulse(dir * intensity);
    }
}