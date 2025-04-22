using UnityEngine;
using UnityEngine.VFX;
public class BurstParticleEffectManager : MonoBehaviour
{
    public ParticleSystem ps;
    public VisualEffect vfx;
    public static BurstParticleEffectManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayEffect(Vector2 position, Gradient color, float duration)
    {
        /*
        ps.transform.position = position;
        var main = ps.main;
        main.startColor = color;
        main.startLifetime = duration;
        var ep = new ParticleSystem.EmitParams {
            position = position,
            startColor = color,
            startLifetime = duration,
        };
        ps.Emit(ep, 20); // Викликаємо один burst
        */
        vfx.SetVector3("position", (Vector3)position + new Vector3(0, 0, 0));
        vfx.SetGradient("color", color);
        vfx.Play();
    }
       
}
