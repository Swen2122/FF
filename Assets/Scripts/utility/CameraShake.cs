using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;

    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shake();
        }
    }

    public void Shake(float intensity = 1f)
    {
        impulseSource.GenerateImpulse(Vector3.right * intensity);
    }
}